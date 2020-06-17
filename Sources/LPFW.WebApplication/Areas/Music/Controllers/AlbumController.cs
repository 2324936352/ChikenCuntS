using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.EntitiyModels.MusicEntity;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels.MusicViewModel;
using LPFW.ViewModelServices;
using LPFW.ViewModelServices.Extensions.Music;
using LPFW.WebApplication.BaseTemplateControllers;
using Microsoft.AspNetCore.Mvc;

namespace LPFW.WebApplication.Areas.Music.Controllers
{
    [Area("Music")]
    /// <summary>
    /// 用于管理音乐专辑的控制器
    /// 1.前置条件:已实现Music、Album
    /// 2.只有管理员用户才能访问这个控制器
    /// 3.通过用户识别用户管理员ID
    /// 4.参考DemoEntityPaginationController控制器

    /// </summary>
    public class AlbumController : BasePaginationTemplateController<Album, AlbumViewModel>
    {
        /// <summary>
        /// 构造函数，负责注入数据访问服务，和初始化一些视图参数
        /// </summary>
        /// <param name="service"></param>
        public AlbumController(IViewModelService<Album, AlbumViewModel> service) : base(service)
        {
            ModuleName = "音乐管理";
            FunctionName = "MusicEntity";
            // 3. 设置列表视图参数
            ListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="专辑名称", Width=0, IsSort =false, SortDesc=""  },
                //new TableListItem() { PropertyName = "SortCode", DsipalyName="编码", Width=150, IsSort = true, SortDesc=""},   // 如果将 SortCode 设置为自动生成，一般就不在列表中显示了
                new TableListItem() { PropertyName = "SingerName", DsipalyName = "歌手", IsSort = false, SortDesc = "", Width =50},
                new TableListItem() { PropertyName = "IssueTime", DsipalyName = "发行时间", IsSort = false, SortDesc = "", Width = 100 },
                new TableListItem() { PropertyName = "Price", DsipalyName = "价格", IsSort = false, SortDesc = "", Width = 100 },

            };

            // 4. 设置编辑和新建数据视图参数
            CreateOrEditItems = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "SingerName", TipsString="", DataType = ViewModelDataType.单行文本 },
                //new CreateOrEditItem() { PropertyName = "MusicTypeId", TipsString = "", DataType = ViewModelDataType.层次下拉单选一},
                new CreateOrEditItem() { PropertyName = "IssueTime", TipsString="", DataType = ViewModelDataType.日期},   // 如果将 SortCode 设置为自动生成，这个隐含的数据是必须的
                new CreateOrEditItem() { PropertyName = "Price", TipsString="", DataType = ViewModelDataType.单行文本 },
                //new CreateOrEditItem() { PropertyName = "Price", TipsString="", DataType = ViewModelDataType.单行文本},
            };

            // 5. 设置明细数据视图参数
            DetailItems = new List<DetailItem>()
            {
                new DetailItem() { PropertyName = "Name", DataType = ViewModelDataType.单行文本  },
                new DetailItem() { PropertyName = "SingerName", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "IssueTime", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Price", DataType = ViewModelDataType.单行文本 },
            };
        }
        [HttpGet]
        public override async Task<IActionResult> CreateOrEdit(Guid id)
        {
            var boVM = await ViewModelService.GetBoVMAsyn(id);
            // 设置关联属性
            await ViewModelService.GetboVMRelevanceData(boVM);
            // 设置附件相关的属性
            await ViewModelService.SetAttachmentFileItem(boVM);

            boVM.IsSaved = false;

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + "：编辑数据";
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }
        //保存选中的职位类型下拉选项后的所有数据
        [HttpPost]
        public override async Task<IActionResult> CreateOrEdit(AlbumViewModel boVM)
        {
            boVM.IsSaved = false;
            if (ModelState.IsValid)
            {
                // 使用自定义的扩展方法保存数据
                var saveStatusModel = await ViewModelService.SaveBoWithRelevanceDataAsyn(boVM);

                // 如果这个值是 ture ，页面将调转回到列表页
                boVM.IsSaved = saveStatusModel.SaveSatus;
            }

            // 设置附件相关的属性
            await ViewModelService.SetAttachmentFileItem(boVM);
            // 设置关联属性
            await ViewModelService.GetboVMRelevanceData(boVM);

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + "：编辑数据";
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }
        //明细数据职位类型的Name
        [HttpGet]
        public override async Task<IActionResult> Detail(Guid id)
        {
            var boVM = await ViewModelService.GetBoVMAsyn(id);
            // 设置关联属性
            await ViewModelService.GetboVMRelevanceData(boVM);

            ViewData["DetailItems"] = DetailItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + "：明细数据";
            return PartialView(DetailPartialViewPath, boVM);
        }


    }
}

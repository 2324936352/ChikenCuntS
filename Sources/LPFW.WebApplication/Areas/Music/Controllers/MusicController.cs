using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.MusicEntity;
using LPFW.ViewModels.MusicViewModel;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModelServices;
using LPFW.ViewModelServices.Extensions.Music;
using Microsoft.AspNetCore.Mvc;
using LPFW.WebApplication.BaseTemplateControllers;

/// <summary>
/// 用于管理音乐
///   1.前置条件:已实现Music01与MusicViewModel
///   2.音乐类型以下拉形式展开
///   3.只有管理员可以访问这个控制器
///   4.参考BasePaginationTemplateController控制器进行分页
/// </summary>

namespace LPFW.WebApplication.Areas.Music
{
    [Area("Music")]
    public class MusicController : BasePaginationTemplateController<MusicEntity, MusicViewModel>
    {
        public MusicController(IViewModelService<MusicEntity, MusicViewModel> service) : base(service)
        {
            ModuleName = "音乐管理";
            FunctionName = "MusicEntity";
            // 3. 设置列表视图参数
            ListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="音乐名称", Width=0, IsSort =false, SortDesc=""  },
                //new TableListItem() { PropertyName = "SortCode", DsipalyName="编码", Width=150, IsSort = true, SortDesc=""},   // 如果将 SortCode 设置为自动生成，一般就不在列表中显示了
                new TableListItem() { PropertyName = "SingerName", DsipalyName = "歌手", IsSort = false, SortDesc = "", Width =50},
                //new TableListItem() { PropertyName = "Price", DsipalyName = "价格", IsSort = false, SortDesc = "", Width = 100 },

            };

            // 4. 设置编辑和新建数据视图参数
            CreateOrEditItems = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "SingerName", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "MusicTypeId", TipsString = "", DataType = ViewModelDataType.层次下拉单选一},
                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.隐藏},   // 如果将 SortCode 设置为自动生成，这个隐含的数据是必须的
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 },
                //new CreateOrEditItem() { PropertyName = "Price", TipsString="", DataType = ViewModelDataType.单行文本},
            };

            // 5. 设置明细数据视图参数
            DetailItems = new List<DetailItem>()
            {
                new DetailItem() { PropertyName = "Name", DataType = ViewModelDataType.单行文本  },
                new DetailItem() { PropertyName = "SingerName", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "MusicTypeName", DataType = ViewModelDataType.单行文本 },
                //new DetailItem() { PropertyName = "Price", DataType = ViewModelDataType.单行文本 },
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
        public override async Task<IActionResult> CreateOrEdit(MusicViewModel boVM)
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
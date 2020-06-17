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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

/// <summary>
/// 1.音乐类型
/// </summary>
/// /// <typeparam name="BookType"></typeparam>
/// <typeparam name="BookTypeVM"></typeparam>

namespace LPFW.WebApplication.Areas.BookInfo.Controller
{
    [Area("Music")]
    public class MusicTypeController : BaseTemplateController<MusicTypeEntity, MusicTypeViewModel>
    {
        public MusicTypeController(IViewModelService<MusicTypeEntity, MusicTypeViewModel> service) : base(service)
        {
            ModuleName = "音乐类型";
            FunctionName = "MusicType";

            // 1.视图路径
            //IndexViewPath = "../../Views/BaseTemplate/IndexWithModal";
            //ListPartialViewPath = "_CommonListWithModal";
            //CreateOrEditPartialViewPath = "_CommonCreateOrEditWithModal";
            //DetailPartialViewPath = "_CommonDetailWithModal";

            // 2.列表元素
            ListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="音乐名称", Width=150, IsSort = false, SortDesc=""  },
                new TableListItem() { PropertyName = "Description", DsipalyName = "简要说明", IsSort = false, SortDesc = "", Width = 0 }
            };

            // 3. 设置编辑和新建数据视图参数
            CreateOrEditItems = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "MusicTypeId", TipsString="如果选择的上级菜单为空值，则默认为是根节点菜单", DataType = ViewModelDataType.层次下拉单选一 },
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 }
            };

            // 4. 设置明细数据视图参数
            DetailItems = new List<DetailItem>()
            {
                new DetailItem() { PropertyName = "MusicTypeName", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Name", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Description", DataType = ViewModelDataType.多行文本 }
            };
        }
        //编辑页面显示职位类型下拉选项
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
        /// <summary>
        /// 校验与保存数据数据
        /// </summary>
        /// <param name="boVM"></param>
        /// <returns></returns>
        [HttpPost]
        public override async Task<IActionResult> CreateOrEdit(MusicTypeViewModel boVM)
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
        public override async Task<IActionResult> Index()
        {
            // 放回实体名称按照层级进行缩进处理的视图模型对象集合
            var boVMCollection = await ViewModelService.GetBoVMCollectionWithHierarchicalStyleAsyn(NoPaginateListPageParameter);

            ViewData["ListSinglePageParameter"] = NoPaginateListPageParameter;
            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + ": " + NoPaginateListPageParameter.TypeName;

            // 所有的 Index 或者不是返回局部页的 Action 都应该包含下面两个传输数据
            ViewData["ApplicationMenuGroupId"] = GetApplicationMenuGroupId();
            ViewData["ApplicationMenuItemActiveId"] = GetApplicationMenuItemActiveId();

            return View(IndexViewPath, boVMCollection);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.EntitiyModels.ApplicationCommon.AppPathAndMenu;
using LPFW.EntitiyModels.TeachingBusiness;
using LPFW.ViewModels.ApplicationCommon.AppPathAndMenu;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels.TeachingBusiness;
using LPFW.ViewModelServices;
using LPFW.WebApplication.BaseTemplateControllers;
using Microsoft.AspNetCore.Mvc;

namespace LPFW.WebApplication.Areas.TeachingBusiness.Controllers
{
    /// <summary>
    /// 课程类别管理
    /// </summary>
    [Area("TeachingBusiness")]
    public class CourseContainerController : BaseTemplateController<CourseContainer, CourseContainerVM>
    {
        public CourseContainerController(IViewModelService<CourseContainer, CourseContainerVM> service) : base(service)
        {
            ModuleName = "组织和教学资源管理";
            FunctionName = "课程分类管理";

            // 1. 设置视图路径
            IndexViewPath               = "../../Views/BaseTemplate/IndexWithModal";
            ListPartialViewPath         = "_CommonListWithModal";
            CreateOrEditPartialViewPath = "_CommonCreateOrEditWithModal";
            DetailPartialViewPath       = "_CommonDetailWithModal";

            // 3. 设置列表视图参数
            ListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="名称", Width=250, IsSort = false, SortDesc=""  },
                new TableListItem() { PropertyName = "SortCode", DsipalyName="编码", Width=150, IsSort = false, SortDesc="Ascend"},
                new TableListItem() { PropertyName = "Description", DsipalyName="简要说明", Width=0, IsSort = false  }
            };

            // 4. 设置编辑和新建数据视图参数
            CreateOrEditItems = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem(){ PropertyName="Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem(){ PropertyName="SortCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem(){ PropertyName="Description", TipsString="", DataType = ViewModelDataType.多行文本 },
            };

            // 5. 设置明细数据视图参数
            DetailItems = new List<DetailItem>()
            {
                new DetailItem(){ PropertyName="Name", DataType = ViewModelDataType.单行文本 },
                new DetailItem(){ PropertyName="SortCode", DataType = ViewModelDataType.单行文本 },
                new DetailItem(){ PropertyName="Description", DataType = ViewModelDataType.多行文本 },
            };

        }

        public override async Task<IActionResult> Index()
        {
            NoPaginateListPageParameter.TypeName = TypeName;
            NoPaginateListPageParameter.SortProperty = "SortCode";
            NoPaginateListPageParameter.SortDesc = "Ascend"; 

            var boVMCollection = await ViewModelService.GetBoVMCollectionAsyn(NoPaginateListPageParameter);

            ViewData["ListSinglePageParameter"] = NoPaginateListPageParameter;
            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + ": 所有数据";

            // 以下两个与系统菜单有关，所有的 Index 方法都需要的
            var appMenuGroup = await ViewModelService.GetOtherBoVMCollection<ApplicationMenuGroup, ApplicationMenuGroupVM>(x => x.Name == "用户");
            ViewData["ApplicationMenuGroupId"] = appMenuGroup.FirstOrDefault().Id.ToString();

            var appMenuItem = await ViewModelService.GetOtherBoVMCollection<ApplicationMenuItem, ApplicationMenuItemVM>(x => x.Name == "分页列表");
            ViewData["ApplicationMenuItemActiveId"] = appMenuItem.FirstOrDefault().Id.ToString();

            return View(IndexViewPath, boVMCollection);
        }

    }
}

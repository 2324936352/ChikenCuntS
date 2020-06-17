using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.EntitiyModels.ApplicationCommon.AppPathAndMenu;
using LPFW.EntitiyModels.Demo;
using LPFW.ViewModels.ApplicationCommon.AppPathAndMenu;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModelServices;
using LPFW.WebApplication.BaseTemplateControllers;
using Microsoft.AspNetCore.Mvc;

namespace LPFW.WebApplication.Areas.Admin.Controllers.ApplicationCommon.AppPathAndMenu
{
    /// <summary>
    /// 导航分区基础数据管理，简单实现数据的增删查改即可。
    /// </summary>
    [Area("Admin")]
    public class ApplicationMenuGroupController : BaseTemplateController<ApplicationMenuGroup, ApplicationMenuGroupVM>
    {
        public ApplicationMenuGroupController(IViewModelService<ApplicationMenuGroup,ApplicationMenuGroupVM> service) : base(service)
        {
            ModuleName = "应用菜单分组管理";
            FunctionName = "分组";

            // 1. 设置视图路径
            IndexViewPath               = "../../Views/BaseTemplate/IndexWithModal";
            ListPartialViewPath         = "_CommonListWithModal";
            CreateOrEditPartialViewPath = "_CommonCreateOrEditWithModal";
            DetailPartialViewPath       = "_CommonDetailWithModal";

            // 3. 设置列表视图参数
            ListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="名称", Width=250, IsSort = true, SortDesc=""  },
                new TableListItem() { PropertyName = "SortCode", DsipalyName="编码", Width=150, IsSort = true, SortDesc="Ascend"},
                new TableListItem() { PropertyName = "PortalUrl", DsipalyName="分区路径", Width=0, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Description", DsipalyName="简要说明", Width=0, IsSort = false  }
            };

            // 4. 设置编辑和新建数据视图参数
            CreateOrEditItems = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem(){ PropertyName="Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem(){ PropertyName="SortCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem(){ PropertyName="PortalUrl", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem(){ PropertyName="Description", TipsString="", DataType = ViewModelDataType.多行文本 },
            };

            // 5. 设置明细数据视图参数
            DetailItems = new List<DetailItem>()
            {
                new DetailItem(){ PropertyName="Name", DataType = ViewModelDataType.单行文本 },
                new DetailItem(){ PropertyName="SortCode", DataType = ViewModelDataType.单行文本 },
                new DetailItem(){ PropertyName="PortalUrl", DataType = ViewModelDataType.单行文本 },
                new DetailItem(){ PropertyName="Description", DataType = ViewModelDataType.多行文本 },
            };

        }
    }
}
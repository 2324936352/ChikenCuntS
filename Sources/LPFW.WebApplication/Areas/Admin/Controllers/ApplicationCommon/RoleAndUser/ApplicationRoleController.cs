using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.ViewModels.ApplicationCommon.RoleAndUser;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModelServices;
using LPFW.ViewModelServices.Extensions.ApplicationCommon.RoleAndUser;
using LPFW.WebApplication.BaseTemplateControllers;
using Microsoft.AspNetCore.Mvc;

namespace LPFW.WebApplication.Areas.Admin.Controllers.ApplicationCommon.RoleAndUser
{
    [Area("Admin")]
    public class ApplicationRoleController : BaseTemplateController<ApplicationRole, ApplicationRoleVM>
    {
        public ApplicationRoleController(IViewModelService<ApplicationRole, ApplicationRoleVM> service) : base(service)
        {
            ModuleName = "系统用户角色组管理";
            FunctionName = "角色组";

            // 1.视图路径
            IndexViewPath = "../../Views/BaseTemplate/IndexWithModal";
            ListPartialViewPath = "_CommonListWithModal";
            CreateOrEditPartialViewPath = "_CommonCreateOrEditWithModal";
            DetailPartialViewPath = "_CommonDetailWithModal";

            // 2.列表元素
            ListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="名称", Width=260, IsSort = false, SortDesc=""  },
                new TableListItem() { PropertyName = "SortCode", DsipalyName="编码", Width=250, IsSort = false, SortDesc=""},
                new TableListItem() { PropertyName = "Description", DsipalyName = "简要说明", IsSort = false, SortDesc = "", Width = 0 }
            };

            // 3. 设置编辑和新建数据视图参数
            CreateOrEditItems = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "ParentRoleId", TipsString="如果选择的上级角色为空值，则默认为是根节点角色组", DataType = ViewModelDataType.层次下拉单选一 },
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 }
            };

            // 4. 设置明细数据视图参数
            DetailItems = new List<DetailItem>()
            {
                new DetailItem() { PropertyName = "ParentRoleName", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Name", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "SortCode", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Description", DataType = ViewModelDataType.多行文本 }
            };
        }

        /// <summary>
        /// 考虑需要将显示的数据按照层次方式，处理，重写 Index 方法
        /// </summary>
        /// <returns></returns>
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

        public override async Task<IActionResult> List(string listPageParaJson)
        {
            NoPaginateListPageParameter = Newtonsoft.Json.JsonConvert.DeserializeObject<ListSinglePageParameter>(listPageParaJson);
            var boVMCollection = await ViewModelService.GetBoVMCollectionWithHierarchicalStyleAsyn(NoPaginateListPageParameter);

            ViewData["ListSinglePageParameter"] = NoPaginateListPageParameter;
            ViewData["ListItems"] = ListItems;

            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + ": " + NoPaginateListPageParameter.TypeName; ;
            return PartialView(ListPartialViewPath, boVMCollection);
        }

        [HttpGet]
        public override async Task<IActionResult> CreateOrEdit(Guid id)
        {
            var boVM = await ViewModelService.GetApplicationRoleVMForCreateOrEdit(id);
            // 设置关联属性
            await ViewModelService.GetApplicationRoleVMRelevanceData(boVM);

            boVM.IsSaved = false;

            var boVMName = "";
            if (!String.IsNullOrEmpty(boVM.Name))
                boVMName = "：" + boVM.Name;

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = "编辑" + FunctionName + boVMName;
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }

        /// <summary>
        /// 校验与保存数据数据
        /// </summary>
        /// <param name="boVM"></param>
        /// <returns></returns>
        [HttpPost]
        public override async Task<IActionResult> CreateOrEdit(ApplicationRoleVM boVM)
        {
            boVM.IsSaved = false;
            if (ModelState.IsValid)
            {
                if (boVM.IsNew)
                {
                    // 先检查名称唯一性
                    var isUniqueName = await ViewModelService.IsUnique(x => x.Name == boVM.Name);
                    if (isUniqueName)
                    {
                        // 使用常规的方法保存数据，如果需要使用其它方法，需要重写这个方法
                        var saveStatusModel = await ViewModelService.SaveApplicationRoleForCreateOrEdit(boVM);
                        // 如果这个值是 ture ，页面将调转回到列表页
                        boVM.IsSaved = saveStatusModel.SaveSatus;
                    }
                    else
                    {
                        // 处理名称重复校验
                        ModelState.AddModelError("Name", "用户组名称重复，请重新输入一个新的角色组名称。");
                    }
                }
                else
                {
                    var saveStatusModel = await ViewModelService.SaveApplicationRoleForCreateOrEdit(boVM);
                    // 如果这个值是 ture ，页面将调转回到列表页
                    boVM.IsSaved = saveStatusModel.SaveSatus;
                }
            }

            // 设置关联属性
            await ViewModelService.GetApplicationRoleVMRelevanceData(boVM);

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + "：编辑数据";
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }

        [HttpGet]
        public override async Task<IActionResult> Delete(Guid id)
        {
            var status = await ViewModelService.DeleteApplicationRole(id);
            return Json(status);
        }

    }
}
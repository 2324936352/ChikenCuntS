using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.EntitiyModels.OrganzationBusiness;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels.OrganizationBusiness;
using LPFW.ViewModelServices;
using LPFW.WebApplication.BaseTemplateControllers;
using Microsoft.AspNetCore.Mvc;
using LPFW.ViewModelServices.Extensions.OrganizationBusiness;
using LPFW.DataAccess.Tools;

namespace LPFW.WebApplication.Areas.OrganizationBusiness.Controllers
{
    /// <summary>
    /// 教学班级管理
    /// </summary>
    [Area("OrganizationBusiness")]
    public class DepartmentForStudentController : BaseTemplateController<Department, DepartmentVM>
    {
        public DepartmentForStudentController(IViewModelService<Department, DepartmentVM> service) : base(service)
        {
            // 1.设置模块和功能的中文名称
            ModuleName = "组织机构部门管理";
            FunctionName = "部门";

            // 2.设置控制器对应的基本视图路径
            IndexViewPath               = "../../Views/Department/Index";
            ListPartialViewPath         = "../../Views/Department/_List";
            CreateOrEditPartialViewPath = "_CommonCreateOrEditWithModal";
            DetailPartialViewPath       = "_CommonDetailWithModal";

            // 3.设置列表元素
            ListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="名称", Width=0, IsSort = false, SortDesc=""  },
                new TableListItem() { PropertyName = "SortCode", DsipalyName="编码", Width=150, IsSort = false, SortDesc=""},
                new TableListItem() { PropertyName = "EmployeeAmount", DsipalyName = "人数", Width=50, IsSort = false, SortDesc = "" },
                //new TableListItem() { PropertyName = "PositionAmount", DsipalyName = "岗位", Width=50, IsSort = false, SortDesc = "" }
            };

            // 4. 设置编辑和新建数据视图参数
            CreateOrEditItems = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "ParentId", TipsString="如果选择的上级部门为空值，则默认为是根节点部门", DataType = ViewModelDataType.层次下拉单选一 },
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 },
                new CreateOrEditItem() { PropertyName="AddressVM", TipsString="", DataType = ViewModelDataType.联系地址 },

                new CreateOrEditItem() { PropertyName = "OrganizationId", TipsString="", DataType = ViewModelDataType.隐藏  },
            };

            // 5. 设置明细数据视图参数
            DetailItems = new List<DetailItem>()
            {
                new DetailItem() { PropertyName = "OrganizationName", DataType = ViewModelDataType.单行文本  },
                new DetailItem() { PropertyName = "ParentName", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Name", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "SortCode", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Description", DataType = ViewModelDataType.多行文本 }
            };
        }

        public override async Task<IActionResult> Index()
        {
            var boVMCollection = new List<DepartmentVM>();
            ViewData["GroupID"] = "";
            ViewData["GroupName"] = "";
            ViewData["GroupCollection"] = new List<TransactionCenterRegisterVM>();

            // 提取营运组织集合和当前的营运组织
            var transactionCenterRegisterVMCollection = await ViewModelService.GetOtherBoVMCollection<TransactionCenterRegister, TransactionCenterRegisterVM>();
            var transactionCenterRegisterVM = transactionCenterRegisterVMCollection.OrderBy(x => x.SortCode).FirstOrDefault();
            if (transactionCenterRegisterVM != null)
            {
                // 提取缺省的单位集合
                var organizationVMCollection = await ViewModelService.GetOtherBoVMCollection<Organization, OrganizationVM>();
                var organizationVM = organizationVMCollection.OrderBy(x => x.SortCode).FirstOrDefault(x => x.Name == "教学班级组织");

                if (organizationVM != null)
                {
                    // 获取带有层次结构的
                    boVMCollection = await ViewModelService.GetDepartmentVMCollection(NoPaginateListPageParameter, x => x.Organization.Id == organizationVM.Id, y => y.Organization);
                    NoPaginateListPageParameter.TypeID = organizationVM.Id.ToString();
                    NoPaginateListPageParameter.TypeName = organizationVM.Name;
                }
                // 用于类别导航树中选择大类选择器的数据
                ViewData["GroupID"] = transactionCenterRegisterVM.Id;
                ViewData["GroupName"] = transactionCenterRegisterVM.Name;
                ViewData["GroupCollection"] = transactionCenterRegisterVMCollection.OrderBy(x => x.SortCode).ToList();
            }

            ViewData["ListSinglePageParameter"] = NoPaginateListPageParameter;
            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = NoPaginateListPageParameter.TypeName + "的所有部门";

            // 这是所有的 Index 方法都需要调用的方法，用于处理系统侧边栏菜单
            ViewData["ApplicationMenuGroupId"] = "6C20AC55-367B-4648-9486-4CF3139325C4";
            ViewData["ApplicationMenuItemActiveId"] = GetApplicationMenuItemActiveId();

            return View(IndexViewPath, boVMCollection);
        }

        public override async Task<IActionResult> List(string listPageParaJson)
        {
            NoPaginateListPageParameter = Newtonsoft.Json.JsonConvert.DeserializeObject<ListSinglePageParameter>(listPageParaJson);
            var organizationVM = new OrganizationVM();

            #region 根据类型参数处理获取视图模型集合
            var boVMCollection = new List<DepartmentVM>();

            if (String.IsNullOrEmpty(NoPaginateListPageParameter.TypeID))
                boVMCollection = await ViewModelService.GetBoVMCollectionWithHierarchicalStyleAsyn(NoPaginateListPageParameter);
            else
            {
                var typeId = Guid.Parse(NoPaginateListPageParameter.TypeID);
                // 提取关联菜单分区视图模型对象
                organizationVM = await ViewModelService.GetOtherBoVM<Organization, OrganizationVM>(typeId);
                // 这里的类型匹配的表达式  x => x.DemoEntityParent.Id == typeId，一定要在查询中指明关联的实体  y => y.DemoEntityParent
                boVMCollection = await ViewModelService.GetDepartmentVMCollection(NoPaginateListPageParameter, x => x.Organization.Id == typeId, y => y.Organization);
            }
            #endregion

            #region 处理排序参数
            // 1. 将所有的列表元素排序方向清除
            foreach (var item in ListItems)
                item.SortDesc = "";
            // 2. 检查排序对应的是否使用了排序
            if (!String.IsNullOrEmpty(NoPaginateListPageParameter.SortProperty))
            {
                var tableListItem = ListItems.FirstOrDefault(x => x.PropertyName == NoPaginateListPageParameter.SortProperty);
                if (tableListItem != null)
                {
                    // 更新排序参数
                    tableListItem.PropertyName = NoPaginateListPageParameter.SortProperty;
                    tableListItem.SortDesc = NoPaginateListPageParameter.SortDesc;
                }
            }
            #endregion

            NoPaginateListPageParameter.TypeID = organizationVM.Id.ToString();
            NoPaginateListPageParameter.TypeName = organizationVM.Name;

            ViewData["ListSinglePageParameter"] = NoPaginateListPageParameter;
            ViewData["ListItems"] = ListItems;

            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = NoPaginateListPageParameter.TypeName + "的所有部门";
            return PartialView(ListPartialViewPath, boVMCollection);
        }

        public async Task<IActionResult> CreateOrEditWithGroup(Guid id, string typeId)
        {
            var boVM = await ViewModelService.GetBoVMAsyn(id);
            // 设置限制关联属性
            await ViewModelService.GetboVMRelevanceData(boVM, typeId);
            // 设置附件相关的属性
            await ViewModelService.SetAttachmentFileItem(boVM);

            boVM.IsSaved = false;

            var boVMName = "";
            if (!String.IsNullOrEmpty(boVM.Name))
                boVMName = "：" + boVM.Name;

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = "编辑" + boVM.OrganizationName + ":" + FunctionName;
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }

        /// <summary>
        /// 持久化保存视图回传的视图模型对象 
        /// </summary>
        /// <param name="boVM"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrEditWithGroup(DepartmentVM boVM)
        {
            boVM.IsSaved = false;
            if (ModelState.IsValid)
            {
                var saveStatusModel = await ViewModelService.SaveBoWithRelevanceDataAsyn(boVM);

                // 如果这个值是 ture ，页面将调转回到列表页
                boVM.IsSaved = saveStatusModel.SaveSatus;
            }

            // 设置附件相关的属性
            await ViewModelService.SetAttachmentFileItem(boVM);
            // 设置关联属性
            await ViewModelService.GetboVMRelevanceData(boVM, boVM.OrganizationId);

            var boVMName = "";
            if (!String.IsNullOrEmpty(boVM.Name))
                boVMName = "：" + boVM.Name;
            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = "编辑" + boVM.OrganizationName + ":" + FunctionName;
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }

        /// <summary>
        /// 部门详细数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public override async Task<IActionResult> Detail(Guid id)
        {
            var boVM = await ViewModelService.GetBoVMAsyn(id);
            // 设置关联属性
            await ViewModelService.GetboVMRelevanceData(boVM);
            await ViewModelService.SetVMForConfig(boVM);

            ViewData["DetailItems"] = DetailItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = boVM.OrganizationName + "：" + FunctionName;
            return PartialView(DetailPartialViewPath, boVM);
        }

    }
}

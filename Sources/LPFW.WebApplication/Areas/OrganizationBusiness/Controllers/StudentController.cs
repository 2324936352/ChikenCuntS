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
    /// 学生基础数据管理
    /// </summary>
    [Area("OrganizationBusiness")]
    public class StudentController : BasePaginationTemplateController<Employee, EmployeeVM>
    {
        public StudentController(IViewModelService<Employee, EmployeeVM> service) : base(service)
        {
            ModuleName = "组织机构与人员管理";
            FunctionName = "人员";

            // 1. 设置视图路径
            IndexViewPath = "../../Views/Employee/Index";
            ListPartialViewPath = "../../Views/Employee/_List";
            CreateOrEditPartialViewPath = "../../Views/Employee/_CreateOrEdit";
            DetailPartialViewPath = "../../Views/Employee/_Detail";

            // 3. 设置列表视图参数
            ListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "PersonalCode", DsipalyName="学号", Width=90, IsSort = true, SortDesc=""  },
                new TableListItem() { PropertyName = "Name", DsipalyName="姓名", Width=90, IsSort = true, SortDesc=""  },
                new TableListItem() { PropertyName = "Email", DsipalyName = "电子邮件", IsSort = false, SortDesc = "", Width = 200 },
                new TableListItem() { PropertyName = "Description", DsipalyName="简要说明", Width=0, IsSort = false  },
            };

            // 4. 设置编辑和新建数据视图参数
            CreateOrEditItems = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Sex", TipsString="", DataType = ViewModelDataType.性别 },
                new CreateOrEditItem() { PropertyName = "Birthday", TipsString="", DataType = ViewModelDataType.日期 },
                new CreateOrEditItem() { PropertyName = "CredentialsCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "PersonalCode", TipsString="用户名将作为缺省用户名使用，必须是唯一的。", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Mobile", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Email", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 },

                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "DepartmentId", TipsString="", DataType = ViewModelDataType.隐藏  },
                new CreateOrEditItem() { PropertyName = "ApplicationUserId", TipsString="", DataType = ViewModelDataType.隐藏  },
                new CreateOrEditItem() { PropertyName = "CreateDateTime", TipsString="", DataType = ViewModelDataType.隐藏  },
                new CreateOrEditItem() { PropertyName = "UpdateTime", TipsString="", DataType = ViewModelDataType.隐藏  },
                new CreateOrEditItem() { PropertyName = "ExpiredDateTime", TipsString="", DataType = ViewModelDataType.隐藏  },
                new CreateOrEditItem() { PropertyName = "IsStudent", TipsString="", DataType = ViewModelDataType.隐藏  },

                new CreateOrEditItem() { PropertyName = "AddressVM", TipsString="", DataType = ViewModelDataType.联系地址 }
            };

            // 5. 设置明细数据视图参数
            DetailItems = new List<DetailItem>()
            {
                new DetailItem() { PropertyName = "Name", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Description", DataType = ViewModelDataType.多行文本 },
            };
        }

        /// <summary>
        /// 缺省入口
        /// </summary>
        /// <returns></returns>
        public override async Task<IActionResult> Index()
        {
            var organizationId = "";  // 缺省的单位 Id
            var departamentId = "";   // 缺省的部门 Id
            var boVMCollection = new List<EmployeeVM>();

            // 提取平台组织信息
            var organizationSelectorCollection = await ViewModelService.GetOrganizationSelectorItemCollection();
            // 提取缺省单位
            var defaultOrganizationItem = organizationSelectorCollection.FirstOrDefault(x => x.OrganizationName == "教学班级组织");
            if (defaultOrganizationItem != null)
            {
                organizationId = defaultOrganizationItem.OrganizationId;
                var deptVM = await ViewModelService.GetDefaultDepartmentVM(defaultOrganizationItem.OrganizationId);
                if (!String.IsNullOrEmpty(deptVM.Name))
                {
                    departamentId = deptVM.Id.ToString();
                    // 获取类型实体对象的名称
                    PaginateListPageParameter.TypeID = deptVM.Id.ToString();
                    PaginateListPageParameter.TypeName = deptVM.Name;

                    // 获取教师视图模型对象集合
                    boVMCollection = await ViewModelService.GetBoVMCollectionAsyn(PaginateListPageParameter, x => x.Department.Id == deptVM.Id, y => y.Department);
                }
            }

            PaginateListPageParameter.SortProperty = "SortCode";
            PaginateListPageParameter.SortDesc = "Ascend";

            ViewData["PageGroup"] = PaginateListPageParameter.PagenateGroup;
            ViewData["ItemAmount"] = PaginateListPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = PaginateListPageParameter;

            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = PaginateListPageParameter.TypeName + "的所有教师数据";

            // 用于部门导航树中选择部门分区选择器的数据
            ViewData["OrganizationCollection"] = organizationSelectorCollection;
            ViewData["OrganizationId"] = organizationId;
            ViewData["DepartamentId"] = departamentId;

            // 以下两个与系统菜单有关，所有的 Index 方法都需要的
            ViewData["ApplicationMenuGroupId"] = GetApplicationMenuGroupId();
            ViewData["ApplicationMenuItemActiveId"] = GetApplicationMenuItemActiveId();

            return View(IndexViewPath, boVMCollection);
        }

        /// <summary>
        /// 根据指定的部门 Id 处理员工数据列表的入口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> IndexPaginateList(Guid id)
        {
            // 缺省排序属性
            PaginateListPageParameter.SortProperty = "SortCode";
            PaginateListPageParameter.SortDesc = "Ascend";

            // 提取单位集合
            var typeVMCollection = await ViewModelService.GetOtherBoVMCollection<Organization, OrganizationVM>();
            // 提取指定的单位
            var typeVM = await ViewModelService.GetOtherBoVM<Organization, OrganizationVM>(id);

            // 提取缺省部门
            var deptCollection = await ViewModelService.GetOtherBoVMCollection<Department, DepartmentVM>();
            var deptVM = deptCollection.OrderBy(x => x.SortCode).FirstOrDefault();

            // 获取类型实体对象的名称
            PaginateListPageParameter.TypeID = deptVM.Id.ToString();
            PaginateListPageParameter.TypeName = deptVM.Name;

            // 获取员工视图模型对象集合
            var typeId = deptVM.Id;
            var boVMCollection = await ViewModelService.GetBoVMCollectionAsyn(PaginateListPageParameter, x => x.Department.Id == typeId, y => y.Department);

            // 用于部门导航树中选择部门分区选择器的数据
            ViewData["OrganizationID"] = typeVM.Id;
            ViewData["OrganizationName"] = typeVM.Name;
            ViewData["OrganizationCollection"] = typeVMCollection.OrderBy(x => x.SortCode);

            ViewData["PageGroup"] = PaginateListPageParameter.PagenateGroup;
            ViewData["ItemAmount"] = PaginateListPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = PaginateListPageParameter;

            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = PaginateListPageParameter.TypeName + ": 所有学生数据";

            // 以下两个与系统菜单有关，所有的 Index 方法都需要的
            ViewData["ApplicationMenuGroupId"] = GetApplicationMenuGroupId();
            ViewData["ApplicationMenuItemActiveId"] = GetApplicationMenuItemActiveId();

            return View(IndexViewPath, boVMCollection);
        }

        public override async Task<IActionResult> PaginateList(string listPageParaJson)
        {
            // 根据传入的 json 页面参数，转换为 C# 对象
            PaginateListPageParameter = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);

            // 获取视图模型对象集合
            #region 根据类型参数处理获取视图模型集合
            var boVMCollection = new List<EmployeeVM>();
            if (String.IsNullOrEmpty(PaginateListPageParameter.TypeID))
            {
                boVMCollection = await ViewModelService.GetBoVMCollectionAsyn(PaginateListPageParameter);
                // 获取类型实体对象的名称
                PaginateListPageParameter.TypeID = "";
                PaginateListPageParameter.TypeName = "所有单位";
            }
            else
            {
                var typeId = Guid.Parse(PaginateListPageParameter.TypeID);
                // 提取关联菜单分区视图模型对象
                var typeVM = await ViewModelService.GetOtherBoVM<Department, DepartmentVM>(typeId);
                // 这里的类型匹配的表达式  x => x.DemoEntityParent.Id == typeId，一定要在查询中指明关联的实体  y => y.DemoEntityParent
                boVMCollection = await ViewModelService.GetBoVMCollectionAsyn(PaginateListPageParameter, x => x.Department.Id == typeId, y => y.Department);
                // 获取类型实体对象的名称
                PaginateListPageParameter.TypeID = typeVM.Id.ToString();
                PaginateListPageParameter.TypeName = typeVM.Name;

            }
            #endregion

            #region 处理排序参数
            // 1. 将所有的列表元素排序方向清除
            foreach (var item in ListItems)
                item.SortDesc = "";
            // 2. 检查排序对应的是否使用了排序
            if (!String.IsNullOrEmpty(PaginateListPageParameter.SortProperty))
            {
                var tableListItem = ListItems.FirstOrDefault(x => x.PropertyName == PaginateListPageParameter.SortProperty);
                if (tableListItem != null)
                {
                    // 更新排序参数
                    tableListItem.PropertyName = PaginateListPageParameter.SortProperty;
                    tableListItem.SortDesc = PaginateListPageParameter.SortDesc;
                }
            }
            #endregion
            ViewData["PageGroup"] = PaginateListPageParameter.PagenateGroup;
            ViewData["ItemAmount"] = PaginateListPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = PaginateListPageParameter;
            ViewData["Keyword"] = PaginateListPageParameter.Keyword;

            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = PaginateListPageParameter.TypeName + ": 所有员工数据";

            return PartialView(ListPartialViewPath, boVMCollection);
        }

        /// <summary>
        /// 根据员工 id，新建或者调用已存在的员工数据进行编辑，限制在 departmentId 确定的部门范围内
        /// </summary>
        /// <param name="id">员工 Id</param>
        /// <param name="typeId">员工归属的部门 Id，因前端代码的通用性要求，这里类似的统一使用变量名 typeId</param>
        /// <returns></returns>
        public async Task<IActionResult> CreateOrEditWithType(Guid id, string typeId)
        {
            var boVM = await ViewModelService.GetBoVMAsyn(id);
            // 设置限制为指定菜单分区的菜单项的关联属性
            await ViewModelService.GetboVMRelevanceData(boVM, typeId);
            // 设置附件相关的属性
            await ViewModelService.SetAttachmentFileItem(boVM);

            boVM.IsSaved = false;
            boVM.IsStudent = false;

            var boVMName = "";
            if (!String.IsNullOrEmpty(boVM.Name))
                boVMName = "：" + boVM.Name;

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = "编辑" + boVM.DepartmentName + ":" + FunctionName;
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }

        /// <summary>
        /// 保存员工视图模型数据
        /// </summary>
        /// <param name="boVM">员工视图模型对象</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrEditWithType(EmployeeVM boVM)
        {
            boVM.IsSaved = false;
            if (ModelState.IsValid)
            {

                // 如果是新员工，需要处理工号的唯一性
                if (boVM.IsNew)
                {
                    // 检查员工号的唯一性
                    var isUnique = await ViewModelService.IsUnique(x => x.PersonalCode == boVM.PersonalCode);
                    if (isUnique)
                    {
                        var saveStatusModel = await ViewModelService.SaveBoWithRelevanceDataAsyn(boVM);
                        boVM.IsSaved = saveStatusModel.SaveSatus;
                    }
                    else
                    {
                        // 处理员工号重复校验
                        ModelState.AddModelError("PersonalCode", "用户名重复，请重新输入一个新的用户名。");
                    }
                }
                // 否则就直接持久化
                else
                {
                    var saveStatusModel = await ViewModelService.SaveBoWithRelevanceDataAsyn(boVM);
                    boVM.IsSaved = saveStatusModel.SaveSatus;
                }
            }

            // 设置附件相关的属性
            await ViewModelService.SetAttachmentFileItem(boVM);
            // 设置关联属性
            await ViewModelService.GetboVMRelevanceData(boVM, boVM.DepartmentId);

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = "编辑" + boVM.DepartmentName + ":" + FunctionName;
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }

        /// <summary>
        /// 调整员工归属岗位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AdjustEmployeeDepartment(Guid id)
        {
            var boVM = await ViewModelService.GetBoVMAsyn(id);
            // 设置限制为指定菜单分区的菜单项的关联属性
            await ViewModelService.GetboVMRelevanceData(boVM);
            CreateOrEditItems = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "DepartmentId", TipsString="", DataType = ViewModelDataType.层次下拉单选一  },
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.隐藏 },
            };

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            return PartialView("../../Views/Employee/_AdjustEmployeeDepartment", boVM);
        }

        /// <summary>
        /// 保存调动部门处理结果
        /// </summary>
        /// <param name="boVM"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AdjustEmployeeDepartment(EmployeeVM boVM)
        {
            boVM.IsSaved = false;
            if (String.IsNullOrEmpty(boVM.DepartmentId))
                ModelState.AddModelError("DepartmentId", "必须为员工配置归属部门。");
            else
            {
                var saveStatusModel = await ViewModelService.AdjustEmployeeDepartmentAsync(boVM.Id, Guid.Parse(boVM.DepartmentId));
                boVM.IsSaved = saveStatusModel.SaveSatus;
            }

            // 设置关联属性
            await ViewModelService.GetboVMRelevanceData(boVM);

            CreateOrEditItems = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "DepartmentId", TipsString="", DataType = ViewModelDataType.层次下拉单选一  },
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.隐藏 },
            };

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            return PartialView("../../Views/Employee/_AdjustEmployeeDepartment", boVM);
        }

        public async Task<IActionResult> DataByGourpForBootStrapTreeView(Guid id)
        {
            var treeViewNodes = await ViewModelService.GetTreeViewNodeForBootStrapTreeViewCollectionAsyn<Department>(y => y.Organization.Id == id, z => z.Organization);
            return Json(treeViewNodes);
        }

        public async Task<IActionResult> SelectTreeView(Guid id)
        {
            var organizationSelectorCollection = await ViewModelService.GetOrganizationSelectorItemCollection(id.ToString());
            var defaultOrganizationItem = organizationSelectorCollection.FirstOrDefault(x => x.OrganizationName == "教学班级组织");

            ViewData["OrganizationId"] = defaultOrganizationItem.OrganizationId;
            ViewData["DepartamentId"] = defaultOrganizationItem.DefaultDepartmentId;

            return PartialView("../../Views/Employee/_NavigatorWithBootTreeView", organizationSelectorCollection);
        }


    }
}

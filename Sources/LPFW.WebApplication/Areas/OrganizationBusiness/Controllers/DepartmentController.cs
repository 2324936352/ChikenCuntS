using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.ViewModelServices;
using LPFW.WebApplication.BaseTemplateControllers;
using LPFW.ViewModelServices.Extensions.OrganizationBusiness;
using Microsoft.AspNetCore.Mvc;
using LPFW.ViewModels.ControlModels;
using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.OrganzationBusiness;
using LPFW.ViewModels.OrganizationBusiness;

namespace LPFW.WebApplication.Areas.OrganizationBusiness.Controllers
{
    /// <summary>
    /// 教学部门管理
    /// </summary>
    [Area("OrganizationBusiness")]
    public class DepartmentController : BaseTemplateController<Department, DepartmentVM>
    {
        /// <summary>
        /// 构造函数，负责注入服务外，还需要：
        ///   1.设置模块和功能的中文名称；
        ///   2.设置控制器对应的基本视图路径；
        ///   3.设置列表元素；
        ///   4.设置编辑和新建数据视图参数；
        ///   5.设置明细数据视图参数。
        /// </summary>
        /// <param name="service"></param>
        public DepartmentController(IViewModelService<Department, DepartmentVM> service) : base(service)
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

        #region 部门基础数据常规维护
        /// <summary>
        /// 部门管理入口
        /// </summary>
        /// <returns></returns>
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
                var organizationVM = organizationVMCollection.OrderBy(x => x.SortCode).FirstOrDefault(x=>x.Name== "教学机构组织");

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

        /// <summary>
        /// 根据指定单位 Id 获取部门管理入口
        /// </summary>
        /// <param name="id">单位 Id</param>
        /// <returns></returns>
        public async Task<IActionResult> IndexList(Guid id)
        {
            var boVMCollection = new List<DepartmentVM>();

            // 提取单位
            var organizationVM = await ViewModelService.GetOtherBoVM<Organization, OrganizationVM>(id);
            if (organizationVM != null)
            {
                // 获取带有层次结构的
                boVMCollection = await ViewModelService.GetDepartmentVMCollection(NoPaginateListPageParameter, x => x.Organization.Id == organizationVM.Id, y => y.Organization);
                // 获取类型实体对象的名称
                NoPaginateListPageParameter.TypeID = organizationVM.Id.ToString();
                NoPaginateListPageParameter.TypeName = organizationVM.Name;
            }

            ViewData["ListSinglePageParameter"] = NoPaginateListPageParameter;
            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = NoPaginateListPageParameter.TypeName + "的所有部门";

            // 这是所有的 Index 方法都需要调用的方法，用于处理系统侧边栏菜单
            ViewData["ApplicationMenuGroupId"] = GetApplicationMenuGroupId();
            ViewData["ApplicationMenuItemActiveId"] = GetApplicationMenuItemActiveId();

            return View(IndexViewPath, boVMCollection);
        }

        /// <summary>
        /// 在列表页中，执行其它操作之后，返回到列表视图
        /// </summary>
        /// <param name="listPageParaJson"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 根据部门 Id 创建归属于指定单位 Id 的部门视图模型，并返回到 CreateOrEditPartialViewPath
        /// </summary>
        /// <param name="id">待新建或者编辑的部门 Id</param>
        /// <param name="typeId">单位 Id，用于限制其上级部门的选项 Parent 的范围</param>
        /// <returns></returns>
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
        #endregion

        #region 配置部门基础数据
        /// <summary>
        /// 提取部门基础数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DetailInfo(Guid id)
        {
            var boVM = await ViewModelService.GetBoVMAsyn(id);
            await ViewModelService.GetboVMRelevanceData(boVM);
            await ViewModelService.SetVMForConfig(boVM);

            return PartialView("../../Views/TransactionBusiness/Department/_DetailInfo", boVM);
        }

        /// <summary>
        /// 提取部门员工清单数据
        /// </summary>
        /// <param name="id">部门 id </param>
        /// <returns></returns>
        public async Task<IActionResult> EmployeeList(Guid id)
        {
            var typeVM = await ViewModelService.GetBoVMAsyn(id);
            var employeeListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="姓名", Width=150, IsSort = false, SortDesc=""  },
                new TableListItem() { PropertyName = "Email", DsipalyName = "电子邮件", IsSort = false, SortDesc = "", Width = 220 },
                new TableListItem() { PropertyName = "Description", DsipalyName="简要说明", Width=0, IsSort = false  },
            };
            var employeeVMCollection = await ViewModelService.GetOtherBoVMCollection<Employee, EmployeeVM>(x => x.Department.Id == id, y => y.Department);

            ViewData["EmployeeListItems"] = employeeListItems;
            ViewData["FunctionName"] = typeVM.Name + " 全部人员";

            return PartialView("../../Views/TransactionBusiness/Department/_EmployeeList", employeeVMCollection);

        }

        /// <summary>
        /// 提取部门岗位清单数据
        /// </summary>
        /// <param name="id">部门 id </param>
        /// <returns></returns>
        public async Task<IActionResult> PositionList(Guid id)
        {
            var typeVM = await ViewModelService.GetBoVMAsyn(id);
            var positionListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="名称", Width=150, IsSort = false, SortDesc=""  },
                new TableListItem() { PropertyName = "SortCode", DsipalyName="编码", Width=150, IsSort = false, SortDesc=""},
                new TableListItem() { PropertyName = "Description", DsipalyName = "人数", Width=0, IsSort = false, SortDesc = "" }
            };

            var positionVMCollection = await ViewModelService.GetOtherBoVMCollection<Position, PositionVM>(x => x.Department.Id == id, y => y.Department);

            ViewData["PositionListItems"] = positionListItems;
            ViewData["FunctionName"] = typeVM.Name + " 全部岗位";

            return PartialView("../../Views/TransactionBusiness/Department/_PositionList", positionVMCollection);
        }

        /// <summary>
        /// 提取部门绩效指标清单数据
        /// </summary>
        /// <param name="id">部门 id </param>
        /// <returns></returns>
        public async Task<IActionResult> KpiList(Guid id)
        {
            var typeVM = await ViewModelService.GetBoVMAsyn(id);
            return PartialView("../../Views/TransactionBusiness/Department/_KpiList");
        }

        /// <summary>
        /// 部门配置，在前端视图构建配置的基本环境
        /// </summary>
        /// <param name="id">部门 Id </param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Config(Guid id)
        {
            var boVM = await ViewModelService.GetBoVMAsyn(id);
            await ViewModelService.GetboVMRelevanceData(boVM);
            await ViewModelService.SetVMForConfig(boVM);
            // 提取部门人员数据，供视图配置负责人、联系人使用
            var employeeVMCollection = await ViewModelService.GetOtherBoVMCollection<Employee, EmployeeVM>(x => x.Department.Id == id, y => y.Department);
            ViewData["EmployeeVMCollection"] = employeeVMCollection;

            return PartialView("../../Views/TransactionBusiness/Department/_DepartmentConfig", boVM);
        }

        /// <summary>
        /// 配置部门基础信息：配置 CreateOrEditWithGroup 中对于相关的部门负责人，业务联系人等基础属性
        /// </summary>
        /// <param name="id">部门 Id </param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ConfigDetail(Guid id)
        {
            var boVM = await ViewModelService.GetBoVMAsyn(id);
            // 配置关联元素
            await ViewModelService.GetboVMRelevanceData(boVM);
            // 配置负责人、联系人等数据
            await ViewModelService.SetVMForConfig(boVM);
            // 提取部门人员数据，供视图配置负责人、联系人使用
            var employeeVMCollection = await ViewModelService.GetOtherBoVMCollection<Employee, EmployeeVM>(x => x.Department.Id == id, y => y.Department);
            ViewData["EmployeeVMCollection"] = employeeVMCollection;

            return PartialView("../../Views/TransactionBusiness/Department/_DepartmentConfigDetail", boVM);
        }

        /// <summary>
        /// 配置部门负责人
        /// </summary>
        /// <param name="id">选中的人员 Id</param>
        /// <param name="deptId">部门 Id</param>
        /// <returns></returns>
        public async Task<IActionResult> ConfigDetailForDepartmentLeader(Guid id, Guid deptId)
        {
            // 处理负责人数据
            await ViewModelService.SaveDepartmentLeader(id, deptId);

            var boVM = await ViewModelService.GetBoVMAsyn(deptId);
            // 配置关联元素
            await ViewModelService.GetboVMRelevanceData(boVM);
            // 配置负责人、联系人等数据
            await ViewModelService.SetVMForConfig(boVM);
            // 提取部门人员数据，供视图配置负责人、联系人使用
            var employeeVMCollection = await ViewModelService.GetOtherBoVMCollection<Employee, EmployeeVM>(x => x.Department.Id == deptId, y => y.Department);
            ViewData["EmployeeVMCollection"] = employeeVMCollection;

            return PartialView("../../Views/TransactionBusiness/Department/_DepartmentConfigDetail", boVM);
        }

        /// <summary>
        /// 配置部门业务联系人
        /// </summary>
        /// <param name="id">选中的人员 Id</param>
        /// <param name="deptId">部门 Id</param>
        /// <returns></returns>
        public async Task<IActionResult> ConfigDetailForDepartmentContact(Guid id, Guid deptId)
        {
            // 处理负责人数据
            await ViewModelService.SaveDepartmentContact(id, deptId);

            var boVM = await ViewModelService.GetBoVMAsyn(deptId);
            // 配置关联元素
            await ViewModelService.GetboVMRelevanceData(boVM);
            // 配置负责人、联系人等数据
            await ViewModelService.SetVMForConfig(boVM);
            // 提取部门人员数据，供视图配置负责人、联系人使用
            var employeeVMCollection = await ViewModelService.GetOtherBoVMCollection<Employee, EmployeeVM>(x => x.Department.Id == deptId, y => y.Department);
            ViewData["EmployeeVMCollection"] = employeeVMCollection;

            return PartialView("../../Views/TransactionBusiness/Department/_DepartmentConfigDetail", boVM);
        } 
        #endregion

        #region 配置处理下级部门基础数据
        /// <summary>
        /// 配置部门下级部门信息-列表
        /// </summary>
        /// <param name="id">部门 Id </param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ConfigChildren(Guid id)
        {
            var boVM = await ViewModelService.GetBoVMAsyn(id);

            // 提取归属的下级部门
            var boVMCollection = new List<DepartmentVM>();
            boVMCollection = await ViewModelService.GetDepartmentVMCollection(NoPaginateListPageParameter, x => x.Parent.Id == id && x.Parent.Id != x.Id, y => y.Organization);
            ViewData["ChildrenItemCollection"] = boVMCollection;
            ViewData["ChildrenItemListItems"] = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="名称", Width=0, IsSort = false, SortDesc=""  },
                new TableListItem() { PropertyName = "SortCode", DsipalyName="编码", Width=150, IsSort = false, SortDesc=""},
                new TableListItem() { PropertyName = "EmployeeAmount", DsipalyName = "人数", Width=50, IsSort = false, SortDesc = "" },
                new TableListItem() { PropertyName = "PositionAmount", DsipalyName = "岗位", Width=50, IsSort = false, SortDesc = "" }
            };

            return PartialView("../../Views/TransactionBusiness/Department/_DepartmentConfigChildren", boVM);
        }

        /// <summary>
        ///  根据指定的上级部门，编辑或新建一个下级部门视图模型
        /// </summary>
        /// <param name="id">部门 Id </param>
        /// <param name="parentId">上级部门 Id </param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CreateOrEditChildrenItem(Guid id, Guid parentId)
        {
            var boVM = await ViewModelService.GetBoVMAsyn(id, parentId);

            ViewData["CreateOrEditChildrenItems"] = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 },
                new CreateOrEditItem() { PropertyName="AddressVM", TipsString="", DataType = ViewModelDataType.联系地址 },

                new CreateOrEditItem() { PropertyName = "ParentId", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "OrganizationId", TipsString="", DataType = ViewModelDataType.隐藏  },
            };

            return PartialView("../../Views/TransactionBusiness/Department/_CreateOrEditChildrenItem", boVM);
        }

        /// <summary>
        ///  保存部门视图数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrEditChildrenItem(DepartmentVM boVM)
        {
            boVM.IsSaved = false;
            if (ModelState.IsValid)
            {
                var saveStatusModel = await ViewModelService.SaveBoWithRelevanceDataAsyn(boVM);

                // 如果这个值是 ture ，页面将调转回到列表页
                boVM.IsSaved = saveStatusModel.SaveSatus;
            }

            ViewData["CreateOrEditChildrenItems"] = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 },
                new CreateOrEditItem() { PropertyName="AddressVM", TipsString="", DataType = ViewModelDataType.联系地址 },

                new CreateOrEditItem() { PropertyName = "ParentId", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "OrganizationId", TipsString="", DataType = ViewModelDataType.隐藏  },
            };

            return PartialView("../../Views/TransactionBusiness/Department/_CreateOrEditChildrenItem", boVM);
        }

        /// <summary>
        /// 删除指定 id 的下级部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DeleteChildrenItem(Guid id)
        {
            var boVM = await ViewModelService.GetBoVMAsyn(id);
            // 配置关联元素
            await ViewModelService.GetboVMRelevanceData(boVM);

            return PartialView("../../Views/TransactionBusiness/Department/_DeleteChildrenItem", boVM);
        }
        #endregion

        #region 配置部门员工基础信息
        /// <summary>
        /// 配置部门员工基础性息
        /// </summary>
        /// <param name="id">部门 Id </param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ConfigEmployee(Guid id)
        {
            var boVM = await ViewModelService.GetBoVMAsyn(id);

            ViewData["EmployeeItemCollection"] = await ViewModelService.GetOtherBoVMCollection<Employee, EmployeeVM>(x => x.Department.Id == id, y => y.Department);
            ViewData["EmployeeListItems"] = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="姓名", Width=150, IsSort = false, SortDesc=""  },
                new TableListItem() { PropertyName = "Email", DsipalyName = "电子邮件", IsSort = false, SortDesc = "", Width = 220 },
                new TableListItem() { PropertyName = "Description", DsipalyName="简要说明", Width=0, IsSort = false  },
            };
            var employeeVMCollection = await ViewModelService.GetOtherBoVMCollection<Employee, EmployeeVM>(x => x.Department.Id == id, y => y.Department);

            return PartialView("../../Views/TransactionBusiness/Department/_DepartmentConfigEmployee", boVM);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrEidtEmployeeItem(Guid id,Guid deptId) 
        {
            var boVM = await ViewModelService.GetEmployeeVMAsyn(id, deptId);
            ViewData["CreateOrEditEmployeeItems"] = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "PositionId", TipsString="", DataType = ViewModelDataType.普通下拉单选一 },
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Sex", TipsString="", DataType = ViewModelDataType.性别 },
                new CreateOrEditItem() { PropertyName = "Birthday", TipsString="", DataType = ViewModelDataType.日期 },
                new CreateOrEditItem() { PropertyName = "CredentialsCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "PersonalCode", TipsString="员工号将作为缺省用户名使用，必须是唯一的。", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Mobile", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Email", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 },

                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "DepartmentId", TipsString="", DataType = ViewModelDataType.隐藏  },
                new CreateOrEditItem() { PropertyName = "ApplicationUserId", TipsString="", DataType = ViewModelDataType.隐藏  },
                new CreateOrEditItem() { PropertyName = "CreateDateTime", TipsString="", DataType = ViewModelDataType.隐藏  },
                new CreateOrEditItem() { PropertyName = "UpdateTime", TipsString="", DataType = ViewModelDataType.隐藏  },
                new CreateOrEditItem() { PropertyName = "ExpiredDateTime", TipsString="", DataType = ViewModelDataType.隐藏  },

                new CreateOrEditItem() { PropertyName = "EmployeeAddressVM", TipsString="", DataType = ViewModelDataType.联系地址 },
            };

            return PartialView("../../Views/TransactionBusiness/Department/_CreateOrEditEmployeeItem", boVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEidtEmployeeItem(EmployeeVM boVM) 
        {
            boVM.IsSaved = false;
            if (ModelState.IsValid)
            {
                // 如果是新员工，需要处理工号的唯一性
                if (boVM.IsNew)
                {
                    // 检查员工号的唯一性
                    var isUnique = await ViewModelService.IsUniqueEmployee(boVM.PersonalCode);
                    if (isUnique)
                    {
                        var saveStatusModel = await ViewModelService.SaveEmployeeVMAsyn(boVM);
                        boVM.IsSaved = saveStatusModel.SaveSatus;
                    }
                    else
                    {
                        // 处理员工号重复校验
                        ModelState.AddModelError("PersonalCode", "员工号重复，请重新输入一个新的工号。");
                    }
                }
                // 否则就直接持久化
                else
                {
                    var saveStatusModel = await ViewModelService.SaveEmployeeVMAsyn(boVM);
                    boVM.IsSaved = saveStatusModel.SaveSatus;
                }
            }

            ViewData["CreateOrEditEmployeeItems"] = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "PositionId", TipsString="", DataType = ViewModelDataType.普通下拉单选一 },
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Sex", TipsString="", DataType = ViewModelDataType.性别 },
                new CreateOrEditItem() { PropertyName = "Birthday", TipsString="", DataType = ViewModelDataType.日期 },
                new CreateOrEditItem() { PropertyName = "CredentialsCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "PersonalCode", TipsString="员工号将作为缺省用户名使用，必须是唯一的。", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Mobile", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Email", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 },

                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "DepartmentId", TipsString="", DataType = ViewModelDataType.隐藏  },
                new CreateOrEditItem() { PropertyName = "ApplicationUserId", TipsString="", DataType = ViewModelDataType.隐藏  },
                new CreateOrEditItem() { PropertyName = "CreateDateTime", TipsString="", DataType = ViewModelDataType.隐藏  },
                new CreateOrEditItem() { PropertyName = "UpdateTime", TipsString="", DataType = ViewModelDataType.隐藏  },
                new CreateOrEditItem() { PropertyName = "ExpiredDateTime", TipsString="", DataType = ViewModelDataType.隐藏  },

                new CreateOrEditItem() { PropertyName = "EmployeeAddressVM", TipsString="", DataType = ViewModelDataType.联系地址 },
            };

            return PartialView("../../Views/TransactionBusiness/Department/_CreateOrEditEmployeeItem", boVM);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteEmployeeItem(Guid id, Guid deptId)
        {
            var boVM = await ViewModelService.GetEmployeeVMAsyn(id, deptId);
            return PartialView("../../Views/TransactionBusiness/Department/_DeleteEmployeeItem", boVM);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var status = await ViewModelService.DeleteEmployee(id);
            return Json(status);
        }

        #endregion

        #region 配置部门岗位信息
        /// <summary>
        /// 配置部门工作岗位基础信息
        /// </summary>
        /// <param name="id">部门 Id </param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ConfigPosition(Guid id)
        {
            var boVM = await ViewModelService.GetBoVMAsyn(id);

            ViewData["PositionItemCollection"] = await ViewModelService.GetOtherBoVMCollection<Position, PositionVM>(x => x.Department.Id == id, y => y.Department);
            ViewData["PositionListItems"] = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="名称", Width=150, IsSort = false, SortDesc=""  },
                new TableListItem() { PropertyName = "SortCode", DsipalyName="编码", Width=150, IsSort = false, SortDesc=""},
                new TableListItem() { PropertyName = "Description", DsipalyName = "简要说明", Width=0, IsSort = false, SortDesc = "" }
            };

            return PartialView("../../Views/TransactionBusiness/Department/_DepartmentConfigPosition", boVM);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrPositionItem(Guid id, Guid deptId) 
        {
            var boVM = await ViewModelService.GetPositionVMAsyn(id,deptId);
            ViewData["CreateOrEditPositionItems"] = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "DepartmentId", TipsString="", DataType = ViewModelDataType.隐藏  },
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 }
            };
            return PartialView("../../Views/TransactionBusiness/Department/_CreateOrEditPositionItem", boVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrPositionItem(PositionVM boVM)
        {
            boVM.IsSaved = false;
            if (ModelState.IsValid)
            {
                var saveStatusModel = await ViewModelService.SavePositionVMAsyn(boVM);
                boVM.IsSaved = saveStatusModel.SaveSatus;
            }
            ViewData["CreateOrEditPositionItems"] = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "DepartmentId", TipsString="", DataType = ViewModelDataType.隐藏  },
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 }
            };
            return PartialView("../../Views/TransactionBusiness/Department/_CreateOrEditPositionItem", boVM);
        }

        [HttpGet]
        public async Task<IActionResult> DeletePositionItem(Guid id, Guid deptId)
        {
            var boVM = await ViewModelService.GetPositionVMAsyn(id, deptId);
            return PartialView("../../Views/TransactionBusiness/Department/_DeletePositionItem", boVM);
        }

        [HttpGet]
        public async Task<IActionResult> DeletePosition(Guid id)
        {
            var status = await ViewModelService.DeletePosition(id);
            return Json(status);
        }

        #endregion

        #region 配置部门绩效指标数据
        /// <summary>
        /// 配置部门绩效指标基础信息
        /// </summary>
        /// <param name="id">部门 Id </param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ConfigKpi(Guid id)
        {
            ViewData["KpiItemCollection"] = await ViewModelService.GetDepartmentKPIVMCollectionAsyn(id);
            ViewData["KpiListItems"] = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="指标", Width=150, IsSort = false, SortDesc=""  },
                new TableListItem() { PropertyName = "SortCode", DsipalyName="编码", Width=90, IsSort = false, SortDesc=""},
                new TableListItem() { PropertyName = "Benchmark", DsipalyName="基准", Width=60, IsSort = false, SortDesc=""},
                new TableListItem() { PropertyName = "Coefficient", DsipalyName="系数", Width=60, IsSort = false, SortDesc=""},
                new TableListItem() { PropertyName = "Description", DsipalyName = "简要说明", Width=0, IsSort = false, SortDesc = "" }
            };

            var boVM = await ViewModelService.GetBoVMAsyn(id);
            return PartialView("../../Views/TransactionBusiness/Department/_DepartmentConfigKpi", boVM);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrEditKpiItem(Guid id, Guid deptId) 
        {
            var boVM = await ViewModelService.GeDepartmentKPIVMAsyn(id, deptId);

            ViewData["CreateOrEditKpiItems"] = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "OrganizationKPITypeId", TipsString="", DataType = ViewModelDataType.普通下拉单选一 },
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Benchmark", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Coefficient", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 },
                new CreateOrEditItem() { PropertyName = "DepartmentId", TipsString="", DataType = ViewModelDataType.隐藏  },
            };
            return PartialView("../../Views/TransactionBusiness/Department/_CreateOrEditKpiItem", boVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEditKpiItem(DepartmentKPIVM boVM)
        {
            boVM.IsSaved = false;
            if (ModelState.IsValid)
            {
                var saveStatusModel = await ViewModelService.SaveDepartmentKPIAsyn(boVM);
                boVM.IsSaved = saveStatusModel.SaveSatus;
            }

            ViewData["CreateOrEditKpiItems"] = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "OrganizationKPITypeId", TipsString="", DataType = ViewModelDataType.普通下拉单选一 },
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Benchmark", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Coefficient", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 },
                new CreateOrEditItem() { PropertyName = "DepartmentId", TipsString="", DataType = ViewModelDataType.隐藏  },
            };
            return PartialView("../../Views/TransactionBusiness/Department/_CreateOrEditKpiItem", boVM);
        }

        #endregion

        #region 类别导航数据
        /// <summary>
        /// 根据所选的营运组织 Id，获取之下的全部单位，构建成导航树的节点数据
        /// </summary>
        /// <param name="id">大类 Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DataByGourpForBootStrapTreeView(Guid id)
        {
            var treeViewNodes = await ViewModelService.GetTreeViewNodeByOrganization(id);
            return Json(treeViewNodes);
        }

        /// <summary>
        /// 根据所选的营运组织 Id，构建导航树头内容与下拉选项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> SelectTreeView(Guid id)
        {
            // 提取营运组织集合
            var typeVMCollection = await ViewModelService.GetOtherBoVMCollection<TransactionCenterRegister, TransactionCenterRegisterVM>();
            // 当前的营运组织
            var typeVM = typeVMCollection.FirstOrDefault(x => x.Id == id);

            // 提取缺省的单位集合
            var subtypeCollection = await ViewModelService.GetOtherBoVMCollection<Organization, OrganizationVM>();
            var subtypeVM = subtypeCollection.OrderBy(x => x.SortCode).FirstOrDefault();

            // 用于类型导航导航树中大类选择器的数据
            ViewData["GroupID"] = typeVM.Id;
            ViewData["GroupName"] = typeVM.Name;
            ViewData["GroupCollection"] = typeVMCollection.OrderBy(x => x.SortCode);

            ViewData["DefaultTypeID"] = subtypeVM.Id;

            return PartialView("../../Views/TransactionBusiness/Department/_NavigatorWithBootTreeView", typeVMCollection.OrderBy(x => x.SortCode));

        } 
        #endregion

    }
}
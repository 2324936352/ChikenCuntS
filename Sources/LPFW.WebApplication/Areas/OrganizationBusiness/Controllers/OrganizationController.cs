using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.OrganzationBusiness;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels.OrganizationBusiness;
using LPFW.ViewModelServices;
using LPFW.ViewModelServices.Extensions.OrganizationBusiness;
using LPFW.WebApplication.BaseTemplateControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LPFW.WebApplication.Areas.TransactionCenter.Controllers
{
    [Area("OrganizationBusiness")]
    public class OrganizationController : BaseTemplateController<Organization, OrganizationVM>
    {
        public OrganizationController(IViewModelService<Organization, OrganizationVM> service) : base(service)
        {
            // 1. 设置模块和处理对象显示名称
            ModuleName = "营运单位管理";
            FunctionName = "单位管理";

            // 2. 设置视图路径
            IndexViewPath               = "../../Views/Organization/Index";
            ListPartialViewPath         = "../../Views/Organization/_CommonList";
            CreateOrEditPartialViewPath = "../../Views/Organization/_CommonCreateOrEdit";
            DetailPartialViewPath       = "_CommonDetail";

            // 3. 设置列表视图参数
            ListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="单位名称", Width=0, IsSort = true, SortDesc=""  },
                new TableListItem() { PropertyName = "ContactName", DsipalyName="联系人", Width=90, IsSort = false, SortDesc=""  },
                new TableListItem() { PropertyName = "Mobile", DsipalyName="联系电话", Width=120, IsSort = false, SortDesc=""  },
                new TableListItem() { PropertyName = "Email", DsipalyName="电子邮件", Width=250, IsSort = false, SortDesc=""}
                //new TableListItem() { PropertyName = "Description", DsipalyName="简要说明", Width=0, IsSort = false  }
            };

            // 4. 设置编辑和新建数据视图参数
            CreateOrEditItems = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem(){ PropertyName="Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem(){ PropertyName="ContactName", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem(){ PropertyName="Mobile", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem(){ PropertyName="Email", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "AdminUserName", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Password", TipsString = "密码需要至少 8 个字符，并且至少 1 个大写字母，1 个小写字母，1 个数字和 1 个特殊字符。", DataType = ViewModelDataType.密码 },
                new CreateOrEditItem() { PropertyName = "PasswordComfirm", TipsString = "", DataType = ViewModelDataType.密码 },
                new CreateOrEditItem(){ PropertyName="Description", TipsString="", DataType = ViewModelDataType.多行文本 },
                new CreateOrEditItem(){ PropertyName="AddressVM", TipsString="", DataType = ViewModelDataType.联系地址 },

                new CreateOrEditItem(){ PropertyName="SortCode", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem(){ PropertyName="CreateTime", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem(){ PropertyName="ApprovedTime", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem(){ PropertyName="BusinessEntityStatusEnum", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem(){ PropertyName="TransactionCenterRegisterId", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem(){ PropertyName="OrganizationLeaderId", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem(){ PropertyName="OrganzationContactId", TipsString="", DataType = ViewModelDataType.隐藏 },
            };

            // 5. 设置明细数据视图参数
            DetailItems = new List<DetailItem>()
            {
                new DetailItem(){ PropertyName="Name", DataType = ViewModelDataType.单行文本 },
                new DetailItem(){ PropertyName="SortCode", DataType = ViewModelDataType.单行文本 },
                new DetailItem(){ PropertyName="Description", DataType = ViewModelDataType.多行文本 },
            };
        }

        /// <summary>
        /// 缺省单位管理入口
        /// </summary>
        /// <returns></returns>
        public override async Task<IActionResult> Index()
        {

            // 提取归属营运组织的第一个，缺省的时候单位列表集合是第一个营运组织注册的单位
            var typeVMCollection = await ViewModelService.GetOtherBoVMCollection<TransactionCenterRegister, TransactionCenterRegisterVM> ();
            var typeVM = typeVMCollection.OrderBy(x => x.SortCode).FirstOrDefault();

            var boVMCollection = new List<OrganizationVM>();
            if (typeVM != null)
            {
                boVMCollection = await ViewModelService.GetBoVMCollectionAsyn(NoPaginateListPageParameter,x => x.TransactionCenterRegister.Id == typeVM.Id, y => y.TransactionCenterRegister);
            }

            // 获取类型实体对象的名称
            NoPaginateListPageParameter.TypeID = typeVM.Id.ToString();
            NoPaginateListPageParameter.TypeName = typeVM.Name;

            ViewData["ListSinglePageParameter"] = NoPaginateListPageParameter;
            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = NoPaginateListPageParameter.TypeName + "的所有单位";

            // 这是所有的 Index 方法都需要调用的方法，用于处理系统侧边栏菜单
            ViewData["ApplicationMenuGroupId"] = GetApplicationMenuGroupId();
            ViewData["ApplicationMenuItemActiveId"] = GetApplicationMenuItemActiveId();

            return View(IndexViewPath, boVMCollection);
        }

        /// <summary>
        /// 根据指定的 营运组织注册资料 的 Id 获取直辖的单位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> IndexWith(Guid id) 
        {
            var typeVM = await ViewModelService.GetOtherBoVM<TransactionCenterRegister, TransactionCenterRegisterVM>(id);
            var boVMCollection = new List<OrganizationVM>();
            if (typeVM != null)
            {
                boVMCollection = await ViewModelService.GetBoVMCollectionAsyn(NoPaginateListPageParameter, x => x.TransactionCenterRegister.Id == typeVM.Id, y => y.TransactionCenterRegister);
            }
            // 获取类型实体对象的名称
            NoPaginateListPageParameter.TypeID = typeVM.Id.ToString();
            NoPaginateListPageParameter.TypeName = typeVM.Name;

            ViewData["ListSinglePageParameter"] = NoPaginateListPageParameter;
            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = NoPaginateListPageParameter.TypeName + "的所有单位";

            // 这是所有的 Index 方法都需要调用的方法，用于处理系统侧边栏菜单
            ViewData["ApplicationMenuGroupId"] = GetApplicationMenuGroupId();
            ViewData["ApplicationMenuItemActiveId"] = GetApplicationMenuItemActiveId();

            return View(IndexViewPath, boVMCollection);

        }

        /// <summary>
        /// 重写基类的 List 方法，处理单位获取的限制条件
        /// </summary>
        /// <param name="listPageParaJson"></param>
        /// <returns></returns>
        public override async Task<IActionResult> List(string listPageParaJson)
        {
            NoPaginateListPageParameter = Newtonsoft.Json.JsonConvert.DeserializeObject<ListSinglePageParameter>(listPageParaJson);
            //var typeVM = new OrganizationVM();

            #region 根据类型参数处理获取视图模型集合
            var boVMCollection = new List<OrganizationVM>();

            if (String.IsNullOrEmpty(NoPaginateListPageParameter.TypeID)) 
            {
                // 提取归属营运组织的第一个，缺省的时候单位列表集合是第一个营运组织注册的单位
                var typeVMCollection = await ViewModelService.GetOtherBoVMCollection<TransactionCenterRegister, TransactionCenterRegisterVM>();
                var typeVM = typeVMCollection.OrderBy(x => x.SortCode).FirstOrDefault();
                if (typeVM != null)
                {
                    boVMCollection = await ViewModelService.GetBoVMCollectionAsyn(NoPaginateListPageParameter, x => x.TransactionCenterRegister.Id == typeVM.Id, y => y.TransactionCenterRegister);
                }
            }
            else
            {
                var typeId = Guid.Parse(NoPaginateListPageParameter.TypeID);
                boVMCollection = await ViewModelService.GetBoVMCollectionAsyn(NoPaginateListPageParameter, x => x.TransactionCenterRegister.Id == typeId, y => y.TransactionCenterRegister);
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

            //NoPaginateListPageParameter.TypeID = NoPaginateListPageParameter.TypeID;
            //NoPaginateListPageParameter.TypeName = NoPaginateListPageParameter.TypeName;

            ViewData["ListSinglePageParameter"] = NoPaginateListPageParameter;
            ViewData["ListItems"] = ListItems;

            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = NoPaginateListPageParameter.TypeName + "的所有部门";
            return PartialView(ListPartialViewPath, boVMCollection);
        }

        /// <summary>
        /// 根据指定的单位对象和它的上级归属营运组织注册的 Id 新建或编辑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreateOrEditWithTransactionCenterRegister(Guid id, Guid typeId)
        {
            var boVM = await ViewModelService.GetOrganizationVM(id, typeId);
            boVM.IsSaved = false;
            // 如果是编辑数据，则不处理管理员用户相关的信息
            if (!boVM.IsNew)
            {
                boVM.Password = boVM.PasswordComfirm = "1234@Abcd";  // 这个只是简单处理一下前面页面校验数据而已
                CreateOrEditItems = new List<CreateOrEditItem>()
                {
                    new CreateOrEditItem(){ PropertyName="Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem(){ PropertyName="ContactName", TipsString="", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem(){ PropertyName="Mobile", TipsString="", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem(){ PropertyName="Email", TipsString="", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem(){ PropertyName="Description", TipsString="", DataType = ViewModelDataType.多行文本 },
                    new CreateOrEditItem(){ PropertyName="AddressVM", TipsString="", DataType = ViewModelDataType.联系地址 },

                    new CreateOrEditItem() { PropertyName = "AdminUserName", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "Password", TipsString = "", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "PasswordComfirm", TipsString = "", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem(){ PropertyName="SortCode", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem(){ PropertyName="CreateTime", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem(){ PropertyName="ApprovedTime", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem(){ PropertyName="BusinessEntityStatusEnum", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem(){ PropertyName="TransactionCenterRegisterId", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem(){ PropertyName="OrganizationLeaderId", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem(){ PropertyName="OrganzationContactId", TipsString="", DataType = ViewModelDataType.隐藏 },
                };
            }

            var boVMName = "新建";
            if (!String.IsNullOrEmpty(boVM.Name))
                boVMName = "：" + boVM.Name;

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = "编辑" + boVM.TransactionCenterRegisterName + ":" + boVMName;
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }

        /// <summary>
        /// 保存营运组织模型数据
        /// </summary>
        /// <param name="boVM"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrEditWithTransactionCenterRegister(OrganizationVM boVM)
        {
            boVM.IsSaved = false;
            if (ModelState.IsValid)
            {
                // 检查是否存在相关的管理用户
                var hasUser = await ViewModelService.HasUserAsyn(boVM);
                if (!hasUser)
                {
                    ModelState.AddModelError("UserName", "与用户名、电子邮件和移动电话匹配的用户不存在，请检查后重新输入。");
                }

                // 使用常规的方法保存数据，如果需要使用其它方法，需要重写这个方法
                var saveStatusModel = await ViewModelService.SaveBoWithRelevanceDataAsyn(boVM);

                // 如果这个值是 ture ，页面将调转回到列表页
                boVM.IsSaved = saveStatusModel.SaveSatus;
            }

            // 如果是编辑数据，则不处理管理员用户相关的信息
            if (!boVM.IsNew)
            {
                boVM.Password = boVM.PasswordComfirm = "1234@Abcd";  // 这个只是简单处理一下前面页面校验数据而已
                CreateOrEditItems = new List<CreateOrEditItem>()
                {
                    new CreateOrEditItem(){ PropertyName="Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem(){ PropertyName="ContactName", TipsString="", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem(){ PropertyName="Mobile", TipsString="", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem(){ PropertyName="Email", TipsString="", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem(){ PropertyName="Description", TipsString="", DataType = ViewModelDataType.多行文本 },
                    new CreateOrEditItem(){ PropertyName="AddressVM", TipsString="", DataType = ViewModelDataType.联系地址 },

                    new CreateOrEditItem() { PropertyName = "AdminUserName", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "Password", TipsString = "", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "PasswordComfirm", TipsString = "", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem(){ PropertyName="SortCode", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem(){ PropertyName="CreateTime", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem(){ PropertyName="ApprovedTime", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem(){ PropertyName="BusinessEntityStatusEnum", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem(){ PropertyName="TransactionCenterRegisterId", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem(){ PropertyName="OrganizationLeaderId", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem(){ PropertyName="OrganzationContactId", TipsString="", DataType = ViewModelDataType.隐藏 },
                };
            }

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + "：编辑数据";
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }

        /// <summary>
        /// 提取营运申请的清单
        /// </summary>
        /// <returns></returns>
        public override async Task<IActionResult> DataForBootStrapTreeView()
        {
            var treeViewNodes = await ViewModelService.GetTreeViewNodeByParent();
            return Json(treeViewNodes);
        }

    }
}
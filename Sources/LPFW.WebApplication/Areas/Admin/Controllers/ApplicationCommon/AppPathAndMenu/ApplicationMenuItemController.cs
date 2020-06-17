using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.ApplicationCommon.AppPathAndMenu;
using LPFW.ViewModels.ApplicationCommon.AppPathAndMenu;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModelServices;
using LPFW.ViewModelServices.Extensions.ApplicationCommon.AppPathAndMenu;
using LPFW.WebApplication.BaseTemplateControllers;
using Microsoft.AspNetCore.Mvc;

namespace LPFW.WebApplication.Areas.Admin.Controllers.ApplicationCommon.AppPathAndMenu
{
    /// <summary>
    /// 菜单条数据维护控制器
    ///   1.分为菜单分区列表导航和菜单条列表两个显示区，其中菜单条列表采用层次数据列表方式显示；
    ///   2.初始进入（Index）直接限制分区导航相关的第一项范围的菜单条集合列表；
    ///   3.导航选择后限制分区导航相关的所选项项范围的菜单条集合列表；
    ///   4.新建和编辑数据时，分区选择隐含，意味着默认是当前分区， CreateOrEdit(Guid id,string typeId)
    ///     可选的上级菜单条范围，限制在同一个分区的范围内。
    /// </summary>
    [Area("Admin")]
    public class ApplicationMenuItemController : BaseTemplateController<ApplicationMenuItem, ApplicationMenuItemVM>
    {
        public ApplicationMenuItemController(IViewModelService<ApplicationMenuItem, ApplicationMenuItemVM> service) : base(service)
        {
            ModuleName = "应用菜单条目管理";
            FunctionName = "菜单条目";

            // 1.视图路径
            IndexViewPath = "../../Views/ApplicationCommon/ApplicationMenuItem/Index";
            ListPartialViewPath = "../../Views/ApplicationCommon/ApplicationMenuItem/_ListWithModal";
            CreateOrEditPartialViewPath = "_CommonCreateOrEditWithModal";
            DetailPartialViewPath = "_CommonDetailWithModal";

            // 2.列表元素
            ListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "IconString", DsipalyName = "图标", IsSort = false, SortDesc = "", Width = 50, DataType= ViewModelDataType.图标 },
                new TableListItem() { PropertyName = "Name", DsipalyName="名称", Width=250, IsSort = false, SortDesc=""  },
                new TableListItem() { PropertyName = "SortCode", DsipalyName="编码", Width=150, IsSort = false, SortDesc=""},
                new TableListItem() { PropertyName = "UrlString", DsipalyName = "应用路径", IsSort = false, SortDesc = "", Width = 0 }
            };

            // 3. 设置编辑和新建数据视图参数
            CreateOrEditItems = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "ApplicationMenuGroupId", TipsString="", DataType = ViewModelDataType.隐藏  },
                new CreateOrEditItem() { PropertyName = "ParentItemId", TipsString="如果选择的上级菜单为空值，则默认为是根节点菜单", DataType = ViewModelDataType.层次下拉单选一 },
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "UrlString", TipsString = "", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "IconString", TipsString = "", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 }
            };

            // 4. 设置明细数据视图参数
            DetailItems = new List<DetailItem>()
            {
                new DetailItem() { PropertyName = "ApplicationMenuGroupName", DataType = ViewModelDataType.单行文本  },
                new DetailItem() { PropertyName = "ParentItemName", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Name", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "IconString", DataType = ViewModelDataType.图标 },
                new DetailItem() { PropertyName = "UrlString", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "SortCode", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Description", DataType = ViewModelDataType.多行文本 }
            };

        }

        /// <summary>
        /// 入口数据列表，缺省根据导航分区的第一个提取待处理的菜单条数集合
        /// </summary>
        /// <returns></returns>
        public override async Task<IActionResult> Index()
        {
            var boVMCollection = new List<ApplicationMenuItemVM>();
            // 提取关联菜单分区的第一个，缺省的时候列表集合是第一个类型的
            var typeVMCollection = await ViewModelService.GetOtherBoVMCollection<ApplicationMenuGroup,ApplicationMenuGroupVM>();
            var typeVM = typeVMCollection.OrderBy(x => x.SortCode).FirstOrDefault();
            if (typeVM != null)
            {
             // 获取带有层次结构的
               boVMCollection = await ViewModelService.GetBoVMCollectionWithHierarchicalStyleAsyn(NoPaginateListPageParameter, x => x.ApplicationMenuGroup.Id == typeVM.Id, y => y.ApplicationMenuGroup);
            }
            else
                boVMCollection = await ViewModelService.GetBoVMCollectionWithHierarchicalStyleAsyn(NoPaginateListPageParameter);

            // 获取类型实体对象的名称
            NoPaginateListPageParameter.TypeID = typeVM.Id.ToString();
            NoPaginateListPageParameter.TypeName = typeVM.Name;

            ViewData["ListSinglePageParameter"] = NoPaginateListPageParameter;
            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + ": " + NoPaginateListPageParameter.TypeName;

            var appMenuGroup = await ViewModelService.GetOtherBoVMCollection<ApplicationMenuGroup, ApplicationMenuGroupVM>(x => x.Name == "用户");
            ViewData["ApplicationMenuGroupId"] = appMenuGroup.FirstOrDefault().Id.ToString();

            var appMenuItem = await ViewModelService.GetOtherBoVMCollection<ApplicationMenuItem, ApplicationMenuItemVM>(x => x.Name == "分页列表");
            ViewData["ApplicationMenuItemActiveId"] = appMenuItem.FirstOrDefault().Id.ToString();

            return View(IndexViewPath, boVMCollection);
        }

        public override async Task<IActionResult> List(string listPageParaJson)
        {
            NoPaginateListPageParameter = Newtonsoft.Json.JsonConvert.DeserializeObject<ListSinglePageParameter>(listPageParaJson);
            var typeVM = new ApplicationMenuGroupVM();

            #region 根据类型参数处理获取视图模型集合
            var boVMCollection = new List<ApplicationMenuItemVM>();

            if (String.IsNullOrEmpty(NoPaginateListPageParameter.TypeID))
                boVMCollection = await ViewModelService.GetBoVMCollectionWithHierarchicalStyleAsyn(NoPaginateListPageParameter);
            else
            {
                var typeId = Guid.Parse(NoPaginateListPageParameter.TypeID);
                // 提取关联菜单分区视图模型对象
                typeVM =await ViewModelService.GetOtherBoVM<ApplicationMenuGroup,ApplicationMenuGroupVM>(typeId);
                // 这里的类型匹配的表达式  x => x.DemoEntityParent.Id == typeId，一定要在查询中指明关联的实体  y => y.DemoEntityParent
                boVMCollection = await ViewModelService.GetBoVMCollectionWithHierarchicalStyleAsyn(NoPaginateListPageParameter, x => x.ApplicationMenuGroup.Id == typeId, y => y.ApplicationMenuGroup);
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

            NoPaginateListPageParameter.TypeID = typeVM.Id.ToString();
            NoPaginateListPageParameter.TypeName = typeVM.Name;

            ViewData["ListSinglePageParameter"] = NoPaginateListPageParameter;
            ViewData["ListItems"] = ListItems;

            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + ": " + NoPaginateListPageParameter.TypeName;
            return PartialView(ListPartialViewPath, boVMCollection);
        }

        /// <summary>
        /// 新建一个 CreateOrEdit 方法
        /// </summary>
        /// <param name="id">待新建或者编辑的菜单条的 Id</param>
        /// <param name="typeId">用于限制可选下拉选项 ApplicationMenuGroup 和 ParentItem 的范围</param>
        /// <returns></returns>
        public async Task<IActionResult> CreateOrEditWithGroup(Guid id, string typeId)
        {
            var boVM = await ViewModelService.GetBoVMAsyn(id);
            // 设置限制为指定菜单分区的菜单项的关联属性
            await ViewModelService.GetboVMRelevanceData(boVM,typeId);
            // 设置附件相关的属性
            await ViewModelService.SetAttachmentFileItem(boVM);

            boVM.IsSaved = false;

            var boVMName = "";
            if (!String.IsNullOrEmpty(boVM.Name))
                boVMName = "：" + boVM.Name;

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = "编辑" + boVM.ApplicationMenuGroupName + ":" + FunctionName;
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEditWithGroup(ApplicationMenuItemVM boVM)
        {
            boVM.IsSaved = false;
            if (ModelState.IsValid)
            {
                // 使用常规的方法保存数据，如果需要使用其它方法，需要重写这个方法
                var saveStatusModel = await ViewModelService.SaveBoWithRelevanceDataAsyn(boVM);

                // 如果这个值是 ture ，页面将调转回到列表页
                boVM.IsSaved = saveStatusModel.SaveSatus;
            }

            // 设置附件相关的属性
            await ViewModelService.SetAttachmentFileItem(boVM);
            // 设置关联属性
            await ViewModelService.GetboVMRelevanceData(boVM,boVM.ApplicationMenuGroupId);

            var boVMName = "";
            if (!String.IsNullOrEmpty(boVM.Name))
                boVMName = "：" + boVM.Name;
            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = "编辑" + boVM.ApplicationMenuGroupName + ":" + FunctionName;
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
            ViewData["FunctionName"] = "明细数据：" + FunctionName;
            return PartialView(DetailPartialViewPath, boVM);
        }

        public override async Task<IActionResult> DataForBootStrapTreeView()
        {
            var treeViewNodes = await ViewModelService.GetTreeViewNodeByApplicationMenuGroup();
            return Json(treeViewNodes);
        }

    }
}
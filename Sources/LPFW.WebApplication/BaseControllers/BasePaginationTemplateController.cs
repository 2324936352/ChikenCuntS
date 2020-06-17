using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.DataAccess.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using LPFW.ViewModels;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModelServices;
using Microsoft.AspNetCore.Mvc;

namespace LPFW.WebApplication.BaseTemplateControllers
{
    /// <summary>
    /// 分页方式的实体模型数据处理控制器模板
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public class BasePaginationTemplateController<TEntity, TViewModel> : Controller
        where TEntity : class, IEntity, new()
        where TViewModel : class, IEntityViewModel, new()
    {

        // 列表页处理（检索、排序、类别）所需要的参数
        public ListPageParameter PaginateListPageParameter = new ListPageParameter(1, 18);

        // 导航类型的实体模型名称，在使用到导航页模板的时候，派生控制器必须设置这个参数
        public string TypeName;

        // 实体模型和视图数据服务访问处理服务
        private readonly IViewModelService<TEntity, TViewModel> _viewModelService;
        // 公开的实体模型和视图数据服务访问处理服务，便于继承于这个控制器的派生控制器使用
        public IViewModelService<TEntity, TViewModel> ViewModelService { get { return _viewModelService; } }

        public string ModuleName;    // 业务实体所对应的模块的名称
        public string FunctionName;  // 业务实体对应的实际名称，用于在前端显示相关的名称

        
        public string IndexViewPath;                // Index 页面的路径
        public string ListPartialViewPath;          // List 局部页的路径
        public string CreateOrEditPartialViewPath;  // 新建或者编辑局部页的路径
        public string DetailPartialViewPath;        // 新建或者编辑局部页的路径

        public List<TableListItem> ListItems;            // 列表表头的数据
        public List<CreateOrEditItem> CreateOrEditItems; // 编辑数据视图数据清单
        public List<DetailItem> DetailItems;             // 明细数据视图数据清单

        /// <summary>
        /// 构造函数，除了注入的视图模型处理服务外，还需要处理以下配置：
        /// 1. 设置视图路径，在派生控制器中只选择 4 种视图模板的其中一种；
        /// 2. 设置导航类型名称参数；
        /// 3. 设置列表视图参数；
        /// 4. 设置编辑和新建数据视图参数；
        /// 5. 设置明细数据视图参数
        /// </summary>
        /// <param name="service"></param>
        public BasePaginationTemplateController(IViewModelService<TEntity, TViewModel> service)
        {
            _viewModelService = service;

            ModuleName = "常规实体对象数据管理";
            FunctionName = "实体的名称";


            #region 1.设置视图路径，在派生控制器中只选择以下 4 种视图模板的其中一种
            IndexViewPath = "../../Views/BasePaginationTemplate/Index";
            ListPartialViewPath = "_PaginationCommonList";
            CreateOrEditPartialViewPath = "_PaginationCommonCreateOrEdit";
            DetailPartialViewPath = "_PaginationCommonDetail";
            #endregion

            // 2. 设置导航类型名称参数
            TypeName = "";

            // 3. 设置列表视图参数
            ListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="名称", Width=150, IsSort = true, SortDesc=""  },
                new TableListItem() { PropertyName = "SortCode", DsipalyName="编码", Width=150, IsSort = true, SortDesc="Ascend"},
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

        public virtual async Task<IActionResult> Index()
        {
            // 缺省排序属性
            PaginateListPageParameter.SortProperty = "SortCode";
            PaginateListPageParameter.SortDesc = "Ascend"; // 排序方向，未排序：""，升序："Ascend",降序："Descend"

            var boVMCollection = await _viewModelService.GetBoVMCollectionAsyn(PaginateListPageParameter);

            ViewData["PageGroup"] = PaginateListPageParameter.PagenateGroup;
            ViewData["ItemAmount"] = PaginateListPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = PaginateListPageParameter;

            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + ": 所有数据";

            // 以下两个与系统菜单有关，所有的 Index 方法都需要的
            ViewData["ApplicationMenuGroupId"] = GetApplicationMenuGroupId();
            ViewData["ApplicationMenuItemActiveId"] = GetApplicationMenuItemActiveId();

            return View(IndexViewPath, boVMCollection);
        }

        public virtual async Task<IActionResult> PaginateList(string listPageParaJson)
        {
            // 根据传入的 json 页面参数，转换为 C# 对象
            PaginateListPageParameter = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);
            // 获取视图模型对象集合
            var boVMCollection = await _viewModelService.GetBoVMCollectionAsyn(PaginateListPageParameter);

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
            ViewData["ItemAmount"]        = PaginateListPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = PaginateListPageParameter;
            ViewData["Keyword"]           = "";

            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + ": 所有数据";

            return PartialView(ListPartialViewPath, boVMCollection);
        }

        [HttpGet]
        public virtual async Task<IActionResult> CreateOrEdit(Guid id)
        {
            var boVM = await _viewModelService.GetBoVMAsyn(id);
            boVM.IsSaved = false;

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + "：编辑数据";
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }

        [HttpPost]
        public virtual async Task<IActionResult> CreateOrEdit(TViewModel boVM)
        {
            boVM.IsSaved = false;
            if (ModelState.IsValid)
            {
                // 使用常规的方法保存数据，如果需要使用其它方法，需要重写这个方法
                var saveStatusModel = await _viewModelService.SaveBoAsyn(boVM);

                // 如果这个值是 ture ，页面将调转回到列表页
                boVM.IsSaved = saveStatusModel.SaveSatus;
            }

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + "：编辑数据";
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }

        [HttpGet]
        public virtual async Task<IActionResult> Detail(Guid id)
        {
            var boVM = await _viewModelService.GetBoVMAsyn(id);

            ViewData["DetailItems"] = DetailItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + "：明细数据";
            return PartialView(DetailPartialViewPath, boVM);
        }

        [HttpGet]
        public virtual async Task<IActionResult> Delete(Guid id)
        {
            var status = await _viewModelService.DeleteBoAsyn(id);
            return Json(status);
        }

        #region 获取导航节点数据的控制器方法
        /// <summary>
        /// 以 json 格式返回树节点集合，这个方法一般都要进行重写
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<IActionResult> DataForJsTree()
        {
            var boVM = await _viewModelService.GetBoVMAsyn(Guid.NewGuid());
            // 临时数据
            var treeViewNodes = new List<TreeNodeForJsTree>();
            return Json(treeViewNodes);
        }

        /// <summary>
        /// 以 json 格式返回树节点集合，这个方法一般都要进行重写
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<IActionResult> DataForBootStrapTreeView()
        {
            var boVM = await _viewModelService.GetBoVMAsyn(Guid.NewGuid());
            // 临时数据
            var treeViewNodes = new List<TreeNodeForJsTree>();
            return Json(treeViewNodes);
        }
        #endregion

        public string GetApplicationMenuGroupId()
        {
            var controllerAreaName = ControllerContext.RouteData.Values["area"].ToString();
            var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
            if (!String.IsNullOrWhiteSpace(controllerAreaName))
                controllerName = controllerAreaName + "/" + controllerName;

            return _viewModelService.GetApplicationMenuGroupId(controllerName);
        }

        public string GetApplicationMenuItemActiveId()
        {
            var controllerAreaName = ControllerContext.RouteData.Values["area"].ToString();
            var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
            if (!String.IsNullOrWhiteSpace(controllerAreaName))
                controllerName = controllerAreaName + "/" + controllerName;
            return _viewModelService.GetGetApplicationMenuItemActiveId(controllerName);
        }

    }
}
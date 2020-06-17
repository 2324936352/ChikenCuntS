using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.Demo;
using LPFW.Foundation.SpecificationsForEntityModel;
using LPFW.ViewModelServices;
using LPFW.ViewModels;
using LPFW.ViewModels.ControlModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace LPFW.WebApplication.BaseTemplateControllers
{
    /// <summary>
    /// 普通实体模型数据处理控制器模板
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public class BaseTemplateController<TEntity, TViewModel> : Controller
        where TEntity : class, IEntity, new()
        where TViewModel : class, IEntityViewModel, new()
    {
        #region 常规模板的基础属性
        public ListSinglePageParameter NoPaginateListPageParameter
            = new ListSinglePageParameter(); // 列表页处理（检索、排序、类别）所需要的参数
        public string TypeName = "";         // 导航类型的实体模型名称，在使用到导航页模板的时候，派生控制器必须设置这个参数

        public string ModuleName;    // 业务实体所对应的模块的名称
        public string FunctionName;  // 业务实体对应的实际名称，用于在前端显示相关的名称

        public string IndexViewPath;                // Index 页面的路径
        public string ListPartialViewPath;          // List 局部页的路径
        public string CreateOrEditPartialViewPath;  // 新建或者编辑局部页的路径
        public string DetailPartialViewPath;        // 新建或者编辑局部页的路径

        public List<TableListItem> ListItems;            // 列表表头的数据
        public List<CreateOrEditItem> CreateOrEditItems; // 编辑数据视图数据清单
        public List<DetailItem> DetailItems;             // 明细数据视图数据清单

        private readonly IViewModelService<TEntity, TViewModel> _viewModelService;  // 实体模型和视图数据服务访问处理服务
        public IViewModelService<TEntity, TViewModel> ViewModelService
        {
            get { return _viewModelService; }
        }  // 公开的实体模型和视图数据服务访问处理服务，便于继承于这个控制器的派生控制器使用 
        #endregion

        /// <summary>
        /// 构造函数，负责注入的视图模型处理服务，用于为控制器提供适当的视图模型和对视图模型对象进行处理
        /// 的服务，除此之外，还需要处理以下配置：
        ///   1. 设置视图路径，在派生控制器中只选择 4 种视图模板的其中一种；
        ///   2. 设置导航类型名称参数；
        ///   3. 设置列表视图参数；
        ///   4. 设置编辑和新建数据视图参数；
        ///   5. 设置明细数据视图参数
        /// </summary>
        /// <param name="service"> 注入的视图模型服务 </param>
        public BaseTemplateController(IViewModelService<TEntity, TViewModel> service)
        {
            _viewModelService = service;

            ModuleName   = "常规实体对象数据管理";
            FunctionName = "实体的名称";

            // 1.设置视图路径，在派生控制器中只选择以下 4 种视图模板的其中一种
            IndexViewPath               = "../../Views/BaseTemplate/Index";
            ListPartialViewPath         = "_CommonList";
            CreateOrEditPartialViewPath = "_CommonCreateOrEdit";
            DetailPartialViewPath       = "_CommonDetail";

            #region 其它三种模板方式
            //IndexViewPath = "../../Views/BaseTemplate/IndexWithModal";
            //ListPartialViewPath = "_CommonListWithModal";
            //CreateOrEditPartialViewPath = "_CommonCreateOrEditWithModal";
            //DetailPartialViewPath = "_CommonDetailWithModal";

            //IndexViewPath = "../../Views/BaseTemplate/IndexWithNavigator";
            //ListPartialViewPath = "_CommonList";
            //CreateOrEditPartialViewPath = "_CommonCreateOrEdit";
            //DetailPartialViewPath = "_CommonDetail";

            //IndexViewPath = "../../Views/BaseTemplate/IndexWithNavigatorAndModal";
            //ListPartialViewPath = "_CommonListWithModal";
            //CreateOrEditPartialViewPath = "_CommonCreateOrEditWithModal";
            //DetailPartialViewPath = "_CommonDetailWithModal"; 
            #endregion

            // 2. 设置导航类型名称参数
            TypeName = "";
            // 3. 设置列表视图参数
            ListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="名称", Width=250, IsSort = true, SortDesc=""  },
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

        /// <summary>
        /// 常规的控制缺省人口方法，一般负责：
        ///   1. 设置本框架约定常规列表页面参数 NoPaginateListPageParameter 的一些值；
        ///   2. 提取满足一定条件约束的由 TViewModel 泛型定义的视图模型；
        ///   3. 通过常规的 ViewData 字典，向对应的视图提供必要的附加数据
        ///   4. 处理与导航菜单相关额响应数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<IActionResult> Index()
        {
            // 设置常规列表页面参数 NoPaginateListPageParameter
            NoPaginateListPageParameter.TypeName = TypeName;
            NoPaginateListPageParameter.SortProperty = "SortCode";
            NoPaginateListPageParameter.SortDesc = "Ascend";

            // 通过注入的视图服务，提取TViewModel 泛型定义的视图模型
            var boVMCollection = await _viewModelService.GetBoVMCollectionAsyn(NoPaginateListPageParameter);

            // 为前端视图提供必要的附件数据
            ViewData["ListSinglePageParameter"] = NoPaginateListPageParameter;
            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName;

            // 以下两个与系统菜单有关，所有的 Index 方法都需要的
            ViewData["ApplicationMenuGroupId"] = GetApplicationMenuGroupId();
            ViewData["ApplicationMenuItemActiveId"] = GetApplicationMenuItemActiveId();

            // 返回视图，并直接绑定视图模型
            return View(IndexViewPath, boVMCollection);
        }

        /// <summary>
        /// 常规的列表局部视图响应处理，根据前面的局部视图处理情况调用，例如在编辑和新建数据之后，返回到列表视图
        /// </summary>
        /// <param name="listPageParaJson">隐含在视图中 列表页面参数 的 json 数据</param>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<IActionResult> List(string listPageParaJson)
        {
            NoPaginateListPageParameter = Newtonsoft.Json.JsonConvert.
                DeserializeObject<ListSinglePageParameter>(listPageParaJson);
            
            var boVMCollection = await _viewModelService.GetBoVMCollectionAsyn(NoPaginateListPageParameter);

            #region 处理排序参数
            // 1. 将所有的列表元素排序方向清除
            foreach (var item in ListItems)
                item.SortDesc = "";
            // 2. 检查排序对应的是否使用了排序
            if (!String.IsNullOrEmpty(NoPaginateListPageParameter.SortProperty))
            {
                var tableListItem = ListItems.FirstOrDefault(
                    x => x.PropertyName == NoPaginateListPageParameter.SortProperty);
                
                if (tableListItem != null)
                {
                    // 更新排序参数
                    tableListItem.PropertyName = NoPaginateListPageParameter.SortProperty;
                    tableListItem.SortDesc = NoPaginateListPageParameter.SortDesc;
                }
            }
            #endregion

            ViewData["ListSinglePageParameter"] = NoPaginateListPageParameter;
            ViewData["ListItems"] = ListItems;

            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName;
            return PartialView(ListPartialViewPath, boVMCollection);
        }

        /// <summary>
        /// 根据请求的视图模型的 Id ，创建或者提取相应的视图模型
        /// </summary>
        /// <param name="id">数据对象 Id</param>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<IActionResult> CreateOrEdit(Guid id)
        {
            var boVM = await _viewModelService.GetBoVMAsyn(id);
            boVM.IsSaved = false;

            // 使用 ViewData 绑定视图中所需要数据处理条目的规格定义
            ViewData["CreateOrEditItems"] = CreateOrEditItems;

            // 使用 ViewData 绑定视图中所需要显示标题相关的数据
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + "：编辑数据";

            // 返回局部视图，并直接绑定对应的视图模型
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }

        /// <summary>
        /// 根据前端提交的视图模型，通过视图模型服务转交至数据访问处理模块进行持久化处理，
        /// 并向前端请求返回处理的结果状态。
        /// 注意：在本框架中，在视图中提交视图模型数据一般都采用 ajax 方式提交的。
        /// </summary>
        /// <param name="boVM">以表单数据方式提交回来的实体模型数据（强类型数据）</param>
        /// <returns></returns>
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

        /// <summary>
        /// 根据根据请求的视图模型的 Id ，视图模型服务提取相应的视图模型，返回到明细数据
        /// 视图中进行呈现处理
        /// </summary>
        /// <param name="id">数据对象的 Id</param>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<IActionResult> Detail(Guid id)
        {
            var boVM = await _viewModelService.GetBoVMAsyn(id);

            ViewData["DetailItems"] = DetailItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + "：明细数据";
            return PartialView(DetailPartialViewPath, boVM);
        }

        /// <summary>
        /// 根据根据请求的视图模型的 Id，通过视图模型服务删除已经持久化的实体模型对象数据，
        /// 并向前端请求返回处理的结果状态。 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 提取和设置当前导航菜单分组 Id
        /// </summary>
        /// <returns></returns>
        public string GetApplicationMenuGroupId()
        {
            var controllerAreaName = ControllerContext.RouteData.Values["area"].ToString();
            var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
            if (!String.IsNullOrWhiteSpace(controllerAreaName))
                controllerName = controllerAreaName + "/" + controllerName;

            return _viewModelService.GetApplicationMenuGroupId(controllerName);
        }

        /// <summary>
        /// 提取和设置当前的具体的菜单 Id
        /// </summary>
        /// <returns></returns>
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
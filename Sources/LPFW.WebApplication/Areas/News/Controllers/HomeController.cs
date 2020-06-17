using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.OrganzationBusiness;
using LPFW.EntitiyModels.PortalSite;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels.OrganizationBusiness;
using LPFW.ViewModels.PortalSite;
using LPFW.ViewModelServices;
using LPFW.WebApplication.BaseTemplateControllers;
using LPFW.WebApplication.Filters;
using Microsoft.AspNetCore.Mvc;
using LPFW.ViewModelServices.Extensions.PortalSite;

namespace LPFW.WebApplication.Areas.News.Controllers
{
    [Area("News")]
    //[TypeFilter(typeof(TeacherAsyncFilter<Employee, EmployeeVM>), Arguments = new object[] { "21122", "1234@Abcd" })]
    public class HomeController : BasePaginationTemplateController<Article, ArticleVM>
    {
      

        public HomeController(IViewModelService<Article, ArticleVM> service) : base(service)
        {
            ModuleName = "音乐管理";
            FunctionName = "所有音乐";

            // 1. 设置视图路径
            IndexViewPath = "../../Views/Home/IndexWithNavigator";
            ListPartialViewPath = "_PaginationCommonList";
            CreateOrEditPartialViewPath = "_PaginationCommonCreateOrEdit";
            DetailPartialViewPath = "_PaginationCommonDetail";

            // 2. 设置导航类型名称参数
            TypeName = "ArticleTopic";

            // 3. 设置列表视图参数
            ListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="音乐名称", Width=0, IsSort = true, SortDesc=""  },
                new TableListItem() { PropertyName = "ArticleSecondTitle", DsipalyName="歌手名称", Width=0, IsSort = true, SortDesc=""  },
                new TableListItem() { PropertyName = "CreateDate", DsipalyName = "发行日期", IsSort = false, SortDesc = "", Width = 120,DataType= ViewModelDataType.日期 },
                new TableListItem() { PropertyName = "IsPassed", DsipalyName = "发布", IsSort = false, SortDesc = "", Width = 50, DataType= ViewModelDataType.是否 },
            };

            // 4. 设置编辑和新建数据视图参数
            CreateOrEditItems = new List<CreateOrEditItem>()
            {
               
               
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "ArticleSecondTitle", TipsString = "", DataType = ViewModelDataType.单行文本 },
                 new CreateOrEditItem() { PropertyName = "ArticleTopicId", TipsString="", DataType = ViewModelDataType.普通下拉单选一 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString = "", DataType = ViewModelDataType.多行文本 },
                 new CreateOrEditItem() { PropertyName = "IsPassed", TipsString="", DataType = ViewModelDataType.是否 },
                //new CreateOrEditItem() { PropertyName = "ArticleContent", TipsString="", DataType = ViewModelDataType.多行文本 },
                //new CreateOrEditItem() { PropertyName = "ArticleSource", TipsString = "", DataType = ViewModelDataType.单行文本 },

                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "CreateDate", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "UpdateDate", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "PublishDate", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "CloseDate", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "OpenDate", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "CreatorUserId", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "CreatorUserName", TipsString="", DataType = ViewModelDataType.隐藏 },

            };

            // 5. 设置明细数据视图参数
            DetailItems = new List<DetailItem>()
            {
                new DetailItem() { PropertyName = "DemoEntityParentName", DataType = ViewModelDataType.单行文本  },
                new DetailItem() { PropertyName = "DemoCommonName", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "DemoEntityEnum", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Name", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Email", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Mobile", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Description", DataType = ViewModelDataType.多行文本 },
                new DetailItem() { PropertyName = "Sex", DataType = ViewModelDataType.性别 },
                new DetailItem() { PropertyName = "OrderDateTime",DataType = ViewModelDataType.日期 },
                new DetailItem() { PropertyName = "IsFinished", DataType = ViewModelDataType.是否 },
                new DetailItem() { PropertyName = "HtmlContent", DataType = ViewModelDataType.富文本 }
            };
        }
        public Guid OrganzationId { get; set; } = Guid.Empty;   // 单位 Id
        public Guid DepartmentId { get; set; } = Guid.Empty;    // 部门 Id
        public Guid EmplyeeId { get; set; } = Guid.Empty;       // 员工 Id
        public Guid UserId { get; set; } = Guid.Empty;          // 用户 Id
        public override async Task<IActionResult> Index()
        {
            // 缺省排序属性
            PaginateListPageParameter.SortProperty = "SortCode";
            PaginateListPageParameter.SortDesc = "Ascend"; // 排序方向，未排序：""，升序："Ascend",降序："Descend"

            var boVMCollection = await ViewModelService.GetBoVMCollectionAsyn(PaginateListPageParameter);

            ViewData["PageGroup"] = PaginateListPageParameter.PagenateGroup;
            ViewData["ItemAmount"] = PaginateListPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = PaginateListPageParameter;

            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName;

            ViewData["NavigatorTreeViewTitle"] = "音乐栏目";

            // 以下两个与系统菜单有关，所有的 Index 方法都需要的
            ViewData["ApplicationMenuGroupId"] = GetApplicationMenuGroupId();
            ViewData["ApplicationMenuItemActiveId"] = GetApplicationMenuItemActiveId();

            return View(IndexViewPath, boVMCollection);
        }


        public override async Task<IActionResult> PaginateList(string listPageParaJson)
        {
            PaginateListPageParameter = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);
            ViewData["FunctionName"] = FunctionName;

            #region 根据类型参数处理获取视图模型集合
            var boVMCollection = new List<ArticleVM>();
            if (String.IsNullOrEmpty(PaginateListPageParameter.TypeID))
                boVMCollection = await ViewModelService.GetBoVMCollectionAsyn(PaginateListPageParameter);
            else
            {
                var typeId = Guid.Parse(PaginateListPageParameter.TypeID);
                var typeObject = await ViewModelService.GetOtherBoVM<ArticleTopic, ArticleTopicVM>(typeId);
                ViewData["FunctionName"] = typeObject.Name;

                boVMCollection = await ViewModelService.GetArticleVMCollection(PaginateListPageParameter, x => x.ArticleTopic.Id == typeId, i => i.ArticleTopic, i => i.MasterArticle, i => i.MasterArticle.CreatorUser);
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
            ViewData["Keyword"] = "";

            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;

            return PartialView(ListPartialViewPath, boVMCollection);
        }

        [HttpGet]
        public override async Task<IActionResult> CreateOrEdit(Guid id)
        {
            var boVM = await ViewModelService.GetBoVMAsyn(id);
            // 设置关联属性
            await ViewModelService.GetboVMRelevanceData(boVM);
            // 设置附件相关的属性
            await ViewModelService.SetAttachmentFileItem(boVM);
            boVM.CreatorUserId = UserId;

            boVM.IsSaved = false;

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + "：编辑数据";
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }

        [HttpPost]
        public override async Task<IActionResult> CreateOrEdit(ArticleVM boVM)
        {
            boVM.IsSaved = false;
            if (ModelState.IsValid)
            {
                // 使用自定义的扩展方法保存数据
                var saveStatusModel = await ViewModelService.SaveBoWithRelevanceDataAsyn(boVM);

                // 如果这个值是 ture ，页面将调转回到列表页
                boVM.IsSaved = saveStatusModel.SaveSatus;
            }

            // 设置附件相关的属性
            await ViewModelService.SetAttachmentFileItem(boVM);
            // 设置关联属性
            await ViewModelService.GetboVMRelevanceData(boVM);

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + "：编辑数据";
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
            ViewData["FunctionName"] = boVM.Name;
            return PartialView(DetailPartialViewPath, boVM);
        }

        [HttpGet]
        public override async Task<IActionResult> DataForBootStrapTreeView()
        {
            var treeViewNodes = await ViewModelService.GetTreeViewNodeForBootStrapTreeViewWithPlainFacadeItemCollectionAsyn<ArticleTopic>();
            return Json(treeViewNodes);
        }

    }
}

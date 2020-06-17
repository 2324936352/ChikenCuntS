using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.EntitiyModels.OrganzationBusiness;
using LPFW.EntitiyModels.TeachingBusiness;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels.OrganizationBusiness;
using LPFW.ViewModels.TeachingBusiness;
using LPFW.ViewModelServices;
using LPFW.WebApplication.BaseTemplateControllers;
using LPFW.WebApplication.Filters;
using Microsoft.AspNetCore.Mvc;
using LPFW.ViewModelServices.Extensions.TeachingBusiness;
using LPFW.DataAccess.Tools;

namespace LPFW.WebApplication.Areas.Student.Controllers
{
    [Area("Student")]
    //[TypeFilter(typeof(TeacherAsyncFilter<Employee, EmployeeVM>), Arguments = new object[] { "21122", "1234@Abcd" })]
    public class HomeController : BasePaginationTemplateController<Course, CourseVM>
    {
        public Guid OrganzationId { get; set; } = Guid.Empty;   // 单位 Id
        public Guid DepartmentId { get; set; } = Guid.Empty;    // 部门 Id
        public Guid EmplyeeId { get; set; } = Guid.Empty;       // 员工 Id
        public Guid UserId { get; set; } = Guid.Empty;          // 用户 Id

        public HomeController(IViewModelService<Course, CourseVM> service) : base(service)
        {
            PaginateListPageParameter.PageSize = "8";
            ModuleName = "在线课程内容数据管理";
            FunctionName = "课程定义维护";

            // 1. 设置视图路径
            IndexViewPath = "../../Views/Home/Index";
            ListPartialViewPath = "../../Views/Home/_PaginationCommonListWithModal";
            CreateOrEditPartialViewPath = "_PaginationCommonCreateOrEditWithModal";
            DetailPartialViewPath = "_PaginationCommonDetailForModal";

            // 2. 设置导航类型名称参数
            TypeName = "CourseContainer";

            // 3. 设置列表视图参数
            ListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="课程名称", Width=200, IsSort = false, SortDesc=""  },
                new TableListItem() { PropertyName = "SortCode", DsipalyName="编码", Width=100, IsSort = false, SortDesc=""},
                new TableListItem() { PropertyName = "Description", DsipalyName="简要说明", Width=0, IsSort = false  },
                new TableListItem() { PropertyName = "CreatorName", DsipalyName = "编者", IsSort = false, SortDesc = "", Width = 70 },
                //new TableListItem() { PropertyName = "OpenDate", DsipalyName = "开放日期", IsSort = false, SortDesc = "", Width = 100, DataType= ViewModelDataType.日期  },
                //new TableListItem() { PropertyName = "CloseDate", DsipalyName = "关闭日期", IsSort = false, SortDesc = "", Width = 100, DataType= ViewModelDataType.日期  }
            };

            // 4. 设置编辑和新建数据视图参数
            CreateOrEditItems = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "CourseContainerId", TipsString="", DataType = ViewModelDataType.普通下拉单选一 },
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 },
                new CreateOrEditItem() { PropertyName = "OpenDate", TipsString="", DataType = ViewModelDataType.日期 },
                new CreateOrEditItem() { PropertyName = "CloseDate", TipsString="", DataType = ViewModelDataType.日期 },

                new CreateOrEditItem() { PropertyName = "CreatorId", TipsString="", DataType = ViewModelDataType.隐藏 },
            };

            // 5. 设置明细数据视图参数
            DetailItems = new List<DetailItem>()
            {
                new DetailItem() { PropertyName = "CourseContainerName", DataType = ViewModelDataType.单行文本  },
                new DetailItem() { PropertyName = "Name", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "SortCode", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Description", DataType = ViewModelDataType.多行文本 },
                new DetailItem() { PropertyName = "OpenDate", DataType = ViewModelDataType.日期 },
                new DetailItem() { PropertyName = "CloseDate",DataType = ViewModelDataType.日期 },
                new DetailItem() { PropertyName = "CreatorName", DataType = ViewModelDataType.单行文本 },
            };
        }

        public override async Task<IActionResult> Index()
        {
            ViewData["FunctionName"] = FunctionName;
            // 缺省排序属性
            PaginateListPageParameter.SortProperty = "SortCode";
            PaginateListPageParameter.SortDesc = "Ascend";
            var boVMCollection = new List<CourseVM>();
            var defaultTypeCollection = await ViewModelService.GetOtherBoVMCollection<CourseContainer, CourseContainerVM>();

            if (defaultTypeCollection.OrderBy(x => x.SortCode).FirstOrDefault() == null)
                boVMCollection = await ViewModelService.GetBoVMCollectionAsyn(PaginateListPageParameter);
            else
            {
                var defaultTypeId = defaultTypeCollection.OrderBy(x => x.SortCode).FirstOrDefault().Id;
                ViewData["FunctionName"] = defaultTypeCollection.OrderBy(x => x.SortCode).FirstOrDefault().Name;
                boVMCollection = await ViewModelService.GetCourseVMCollection(
                    PaginateListPageParameter,
                    x => x.CourseContainer.Id == defaultTypeId,
                    i => i.CourseContainer, i => i.Creator, i => i.Creator);
            }

            ViewData["PageGroup"] = PaginateListPageParameter.PagenateGroup;
            ViewData["ItemAmount"] = PaginateListPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = PaginateListPageParameter;

            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;


            ViewData["NavigatorTreeViewTitle"] = "课程类型";

            ViewData["CourseContainerCollection"] = defaultTypeCollection;

            // 以下两个与系统菜单有关，所有的 Index 方法都需要的
            ViewData["ApplicationMenuGroupId"] = "6C20AC55-367B-4648-9486-4CF3139325C4";
            ViewData["ApplicationMenuItemActiveId"] = GetApplicationMenuItemActiveId();

            return View(IndexViewPath, boVMCollection);
        }

        public async Task<IActionResult> CourseList(Guid id)
        {
            // 缺省排序属性
            PaginateListPageParameter.SortProperty = "SortCode";
            PaginateListPageParameter.SortDesc = "Ascend";
            var boVMCollection = new List<CourseVM>();
            var defaultTypeCollection = await ViewModelService.GetOtherBoVMCollection<CourseContainer, CourseContainerVM>();

            if (defaultTypeCollection.OrderBy(x => x.SortCode).FirstOrDefault() == null)
                boVMCollection = await ViewModelService.GetBoVMCollectionAsyn(PaginateListPageParameter);
            else
            {
                //var defaultTypeId = defaultTypeCollection.OrderBy(x => x.SortCode).FirstOrDefault().Id;
                boVMCollection = await ViewModelService.GetCourseVMCollection(
                    PaginateListPageParameter,
                    x => x.CourseContainer.Id == id,
                    i => i.CourseContainer, i => i.Creator, i => i.Creator);
            }

            ViewData["PageGroup"] = PaginateListPageParameter.PagenateGroup;
            ViewData["ItemAmount"] = PaginateListPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = PaginateListPageParameter;

            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName;

            ViewData["NavigatorTreeViewTitle"] = "课程类型";

            ViewData["CourseContainerCollection"] = defaultTypeCollection;

            // 以下两个与系统菜单有关，所有的 Index 方法都需要的
            ViewData["ApplicationMenuGroupId"] = "6C20AC55-367B-4648-9486-4CF3139325C4";
            ViewData["ApplicationMenuItemActiveId"] = GetApplicationMenuItemActiveId();

            return View(IndexViewPath, boVMCollection);
        }

        public override async Task<IActionResult> PaginateList(string listPageParaJson)
        {
            PaginateListPageParameter = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);

            var boVMCollection = new List<CourseVM>();
            var typeVM = new CourseContainerVM();

            if (String.IsNullOrEmpty(PaginateListPageParameter.TypeID))
                boVMCollection = await ViewModelService.GetBoVMCollectionAsyn(PaginateListPageParameter);
            else
            {
                var typeId = Guid.Parse(PaginateListPageParameter.TypeID);
                typeVM = await ViewModelService.GetOtherBoVM<CourseContainer, CourseContainerVM>(typeId);
                boVMCollection = await ViewModelService.GetCourseVMCollection(PaginateListPageParameter, x => x.CourseContainer.Id == typeId, i => i.CourseContainer, i => i.Creator, i => i.Creator);
            }

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

            ViewData["PageGroup"] = PaginateListPageParameter.PagenateGroup;
            ViewData["ItemAmount"] = PaginateListPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = PaginateListPageParameter;
            ViewData["Keyword"] = "";

            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = typeVM.Name;

            return PartialView(ListPartialViewPath, boVMCollection);
        }

        [HttpGet]
        public override async Task<IActionResult> CreateOrEdit(Guid id)
        {
            var boVM = await ViewModelService.GetBoVMAsyn(id);
            // 设置关联属性
            await ViewModelService.GetboVMRelevanceData(boVM);

            boVM.IsSaved = false;
            if (boVM.IsNew)
            {
                boVM.CreatorId = UserId;
            }

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + "：编辑数据";
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }

        [HttpPost]
        public override async Task<IActionResult> CreateOrEdit(CourseVM boVM)
        {
            boVM.IsSaved = false;
            if (ModelState.IsValid)
            {
                var saveStatusModel = await ViewModelService.SaveBoWithRelevanceDataAsyn(boVM);

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
            await ViewModelService.GetboVMRelevanceData(boVM);

            ViewData["DetailItems"] = DetailItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = boVM.Name;
            return PartialView(DetailPartialViewPath, boVM);
        }

        /// <summary>
        /// 新建或者编辑课程单元
        /// </summary>
        /// <param name="id">课程单元 Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CreateOrEditCourseItem(Guid id, Guid courseId)
        {
            var courseItemVM = await ViewModelService.GetCourseItemVMAsync(id, courseId, UserId);
            courseItemVM.IsSaved = false;

            ViewData["CreateOrEditItems"] = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "ParentId", TipsString="", DataType = ViewModelDataType.层次下拉单选一},
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 },

                new CreateOrEditItem() { PropertyName = "CreateDate", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "CreatorId", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "CourseId", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "CourseName", TipsString="", DataType = ViewModelDataType.隐藏 },
            };

            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = "编辑" + courseItemVM.CourseName + "课程单元";
            return PartialView("../../Views/Home/_CourseItemCreateOrEdit", courseItemVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEditCourseItem(CourseItemVM courseItemVM)
        {
            courseItemVM.IsSaved = false;
            if (ModelState.IsValid)
            {
                var saveStatusModel = await ViewModelService.SaveCourseItemVMAsync(courseItemVM);
                courseItemVM.IsSaved = saveStatusModel.SaveSatus;
            }

            await ViewModelService.SetCourseItemVMAsync(courseItemVM);

            ViewData["CreateOrEditItems"] = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "ParentId", TipsString="", DataType = ViewModelDataType.层次下拉单选一},
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 },

                new CreateOrEditItem() { PropertyName = "CreateDate", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "CreatorId", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "CourseId", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "CourseName", TipsString="", DataType = ViewModelDataType.隐藏 },
            };

            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = "编辑" + courseItemVM.CourseName + "课程单元";
            return PartialView("../../Views/Home/_CourseItemCreateOrEdit", courseItemVM);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCourseItem(Guid id)
        {
            var status = await ViewModelService.DeleteCourseItemAsync(id);
            return Json(status);
        }

        /// <summary>
        /// 根据课程 Id，获取课程单元内容视图模型，供视图进行处理
        /// </summary>
        /// <param name="id">课程 Id</param>
        /// <returns></returns>
        public async Task<IActionResult> DefaultCourseContentEdit(Guid id)
        {
            var editPartialName = "_CourseContentDetail";
            var defaultTypeCollection = await ViewModelService.GetOtherBoVMCollection<CourseContainer, CourseContainerVM>();
            // 提取缺省的课程单元内容编辑视图模型
            var defaultCourseItemContentVM = await ViewModelService.GetDefaultCourseItemContentVM(id);
            await ViewModelService.SetAttachmentFileItemForOtherBoVM<CourseItemContentVM>(defaultCourseItemContentVM);

            if (String.IsNullOrEmpty(defaultCourseItemContentVM.CourseItemName))
            {
                ViewData["CreateOrEditItems"] = new List<CreateOrEditItem>()
                {
                    new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "UpdateDate", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "EditorId", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "CourseId", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "CourseName", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "CourseItemId", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "CourseItemName", TipsString="", DataType = ViewModelDataType.隐藏 },
                };
                editPartialName = "_CourseContentEdit";
                //return PartialView("../../Views/Course/_CourseContentEdit", defaultCourseItemContentVM);
            }
            else
            {
                //return PartialView("../../Views/Course/_CourseContentDetail", defaultCourseItemContentVM);
            }

            ViewData["EditPartialName"] = editPartialName;
            ViewData["NavigatorTreeViewTitle"] = defaultCourseItemContentVM.CourseName;
            ViewData["CourseContainerCollection"] = defaultTypeCollection;

            ViewData["PageGroup"] = PaginateListPageParameter.PagenateGroup;
            ViewData["ItemAmount"] = PaginateListPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = PaginateListPageParameter;

            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName;

            return View("../../Views/Home/IndexWithNavigatorAndModal", defaultCourseItemContentVM);
        }

        /// <summary>
        /// 根据课程单元 Id，获取课程单元内容视图模型，供视图进行处理
        /// </summary>
        /// <param name="id">课程单元 Id</param>
        /// <returns></returns>
        public async Task<IActionResult> CourseContentEdit(Guid id)
        {
            var defaultCourseItemContentVM = await ViewModelService.GetCourseItemContentVM(id);
            await ViewModelService.SetAttachmentFileItemForOtherBoVM<CourseItemContentVM>(defaultCourseItemContentVM);
            ViewData["CreateOrEditItems"] = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "UpdateDate", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "EditorId", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "CourseId", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "CourseName", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "CourseItemId", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "CourseItemName", TipsString="", DataType = ViewModelDataType.隐藏 },
            };
            return PartialView("../../Views/Home/_CourseContentEdit", defaultCourseItemContentVM);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCourseItemContent(CourseItemContentVM boVM)
        {
            boVM.IsSaved = false;
            if (ModelState.IsValid)
            {
                var saveStatusModel = await ViewModelService.SaveCourseItemContentVM(boVM);
                boVM.IsSaved = saveStatusModel.SaveSatus;
            }
            await ViewModelService.SetAttachmentFileItemForOtherBoVM<CourseItemContentVM>(boVM);
            ViewData["CreateOrEditItems"] = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "UpdateDate", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "EditorId", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "CourseId", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "CourseName", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "CourseItemId", TipsString="", DataType = ViewModelDataType.隐藏 },
                new CreateOrEditItem() { PropertyName = "CourseItemName", TipsString="", DataType = ViewModelDataType.隐藏 },
            };
            return PartialView("../../Views/Home/_CourseContentEdit", boVM);

        }

        [HttpGet]
        public async Task<IActionResult> CourseItemContentDetail(Guid id)
        {
            var defaultCourseItemContentVM = await ViewModelService.GetCourseItemContentVM(id);
            await ViewModelService.SetAttachmentFileItemForOtherBoVM<CourseItemContentVM>(defaultCourseItemContentVM);
            return PartialView("../../Views/Home/_CourseContentDetail", defaultCourseItemContentVM);
        }


        /// <summary>
        /// 根据课程 Id 提取课程单元导航树节点集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> RefreshTreeViewForCourseContentItem(Guid id)
        {
            var treeViewNodeCollection = await ViewModelService.GetTreeViewNodeForCourseContentItem(id);
            return Json(treeViewNodeCollection);
        }

        public override async Task<IActionResult> DataForBootStrapTreeView()
        {
            var treeViewNodeCollection = await ViewModelService.GetTreeViewNodeByCourseContainer();
            return Json(treeViewNodeCollection);
        }

    }
}

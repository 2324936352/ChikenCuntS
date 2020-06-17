using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.Demo;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels.Demo;
using LPFW.ViewModelServices;
using LPFW.ViewModelServices.Extensions.Demo;
using LPFW.WebApplication.BaseTemplateControllers;
using Microsoft.AspNetCore.Mvc;

namespace LPFW.WebApplication.Areas.XDemo.Controllers
{
    [Area("XDemo")]
    public class DemoEntityPaginationController : BasePaginationTemplateController<DemoEntity, DemoEntityVM>
    {
        public DemoEntityPaginationController(IViewModelService<DemoEntity, DemoEntityVM> service) : base(service)
        {
            ModuleName = "常规实体对象 DemoEntity 数据管理";
            FunctionName = "DemoEntity";

            // 1. 设置视图路径
            //IndexViewPath = "../../Views/BasePaginationTemplate/Index";
            //ListPartialViewPath = "_PaginationCommonList";
            //CreateOrEditPartialViewPath = "_PaginationCommonCreateOrEdit";
            //DetailPartialViewPath = "_PaginationCommonDetail";

            IndexViewPath = "../../Views/BasePaginationTemplate/IndexWithNavigator";
            ListPartialViewPath = "_PaginationCommonList";
            CreateOrEditPartialViewPath = "_PaginationCommonCreateOrEdit";
            DetailPartialViewPath = "_PaginationCommonDetail";

            //IndexViewPath = "../../Views/BasePaginationTemplate/IndexWidthModal";
            //ListPartialViewPath = "_PaginationCommonListWithModal";
            //CreateOrEditPartialViewPath = "_PaginationCommonCreateOrEditWithModal";
            //DetailPartialViewPath = "_PaginationCommonDetailForModal";

            //IndexViewPath = "../../Views/BasePaginationTemplate/IndexWithNavigatorAndModal";
            //ListPartialViewPath = "_PaginationCommonListWithModal";
            //CreateOrEditPartialViewPath = "_PaginationCommonCreateOrEditWithModal";
            //DetailPartialViewPath = "_PaginationCommonDetailForModal";

            // 2. 设置导航类型名称参数
            TypeName = "DemoEntityParent";

            // 3. 设置列表视图参数
            ListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=60, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "Name", DsipalyName="姓名", Width=150, IsSort = true, SortDesc=""  },
                //new TableListItem() { PropertyName = "SortCode", DsipalyName="编码", Width=150, IsSort = false, SortDesc=""},   // 如果将 SortCode 设置为自动生成，一般就不在列表中显示了
                new TableListItem() { PropertyName = "Email", DsipalyName = "电子邮件", IsSort = false, SortDesc = "", Width = 220 },
                new TableListItem() { PropertyName = "Mobile", DsipalyName = "电话", IsSort = false, SortDesc = "", Width = 100 },
                new TableListItem() { PropertyName = "Sex", DsipalyName = "性别", IsSort = false, SortDesc = "", Width = 50, DataType=ViewModelDataType.性别 },
                new TableListItem() { PropertyName = "Description", DsipalyName="简要说明", Width=0, IsSort = false  },
                new TableListItem() { PropertyName = "IsFinished", DsipalyName = "状态", IsSort = false, SortDesc = "", Width = 50, DataType= ViewModelDataType.是否 },
                new TableListItem() { PropertyName = "OrderDateTime", DsipalyName = "时间", IsSort = false, SortDesc = "", Width = 220, DataType= ViewModelDataType.日期时间  }
            };

            // 4. 设置编辑和新建数据视图参数
            CreateOrEditItems = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "DemoEntityParentId", TipsString="", DataType = ViewModelDataType.层次下拉单选一  },
                new CreateOrEditItem() { PropertyName = "DemoCommonId", TipsString="", DataType = ViewModelDataType.普通下拉单选一 },
                new CreateOrEditItem() { PropertyName = "DemoEntityEnum", TipsString="", DataType = ViewModelDataType.枚举下拉单选一 },
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Email", TipsString = "", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Mobile", TipsString = "", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Password", TipsString = "密码需要至少 8 个字符，并且至少 1 个大写字母，1 个小写字母，1 个数字和 1 个特殊字符。", DataType = ViewModelDataType.密码 },
                new CreateOrEditItem() { PropertyName = "PasswordComfirm", TipsString = "", DataType = ViewModelDataType.密码 },
                new CreateOrEditItem() { PropertyName = "Sex", TipsString="", DataType = ViewModelDataType.性别 },
                new CreateOrEditItem() { PropertyName = "OrderDateTime", TipsString="", DataType = ViewModelDataType.日期 },
                new CreateOrEditItem() { PropertyName = "Total", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "IsFinished", TipsString="", DataType = ViewModelDataType.是否 },
                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.隐藏 },   // 如果将 SortCode 设置为自动生成，这个隐含的数据是必须的
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 },
                new CreateOrEditItem() { PropertyName = "HtmlContent", TipsString="", DataType = ViewModelDataType.富文本 }
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

        public override async Task<IActionResult> PaginateList(string listPageParaJson)
        {
            PaginateListPageParameter = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);

            #region 根据类型参数处理获取视图模型集合
            var boVMCollection = new List<DemoEntityVM>();
            if (String.IsNullOrEmpty(PaginateListPageParameter.TypeID))
                boVMCollection = await ViewModelService.GetBoVMCollectionAsyn(PaginateListPageParameter);
            else
            {
                var typeId = Guid.Parse(PaginateListPageParameter.TypeID);
                // 这里的类型匹配的表达式  x => x.DemoEntityParent.Id == typeId，一定要在查询中指明关联的实体  y => y.DemoEntityParent
                boVMCollection = await ViewModelService.GetBoVMCollectionAsyn(PaginateListPageParameter, x => x.DemoEntityParent.Id == typeId, y => y.DemoEntityParent);
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

            ViewData["PageGroup"]         = PaginateListPageParameter.PagenateGroup;
            ViewData["ItemAmount"]        = PaginateListPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = PaginateListPageParameter;
            ViewData["Keyword"]           = "";

            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + ": 所有数据";

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

            boVM.IsSaved = false;

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + "：编辑数据";
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }

        [HttpPost]
        public override async Task<IActionResult> CreateOrEdit(DemoEntityVM boVM)
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
            ViewData["FunctionName"] = FunctionName + "：明细数据";
            return PartialView(DetailPartialViewPath, boVM);
        }

        /// <summary>
        /// 重写 BootStrapTreeView 插件节点数据获取方法
        /// </summary>
        /// <returns></returns>
        public override async Task<IActionResult> DataForBootStrapTreeView()
        {
            var treeViewNodes = await ViewModelService.GetTreeViewNodeForBootStrapTreeViewCollectionAsyn<DemoEntityParent>(x => x.Parent);
            return Json(treeViewNodes);
        }
    }
}
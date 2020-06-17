using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.ViewModels.ApplicationCommon.RoleAndUser;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModelServices;
using LPFW.WebApplication.BaseTemplateControllers;
using Microsoft.AspNetCore.Mvc;
using LPFW.ViewModelServices.Extensions.ApplicationCommon.UserAndUser;
using LPFW.DataAccess.Tools;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace LPFW.WebApplication.Areas.Admin.Controllers.ApplicationCommon.RoleAndUser
{
    [Area("Admin")]
    public class ApplicationUserController : BasePaginationTemplateController<ApplicationUser, ApplicationUserVM>
    {
        public ApplicationUserController(IViewModelService<ApplicationUser, ApplicationUserVM> service) : base(service)
        {
            ModuleName = "系统用户管理";
            FunctionName = "用户";

            // 1. 设置视图路径
            IndexViewPath = "../../Views/ApplicationCommon/RoleAndUser/ApplicationUser/Index";
            ListPartialViewPath = "../../Views/ApplicationCommon/RoleAndUser/ApplicationUser/_List";
            CreateOrEditPartialViewPath = "../../Views/ApplicationCommon/RoleAndUser/ApplicationUser/_CreateOrEdit";
            DetailPartialViewPath = "../../Views/ApplicationCommon/RoleAndUser/ApplicationUser/_Detail";

            // 3. 设置列表视图参数
            ListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "OrderNumber", DsipalyName="序号", Width=40, IsSort = false, SortDesc="" },
                new TableListItem() { PropertyName = "UserName", DsipalyName="用户名", Width=100, IsSort = true, SortDesc=""  },
                new TableListItem() { PropertyName = "Name", DsipalyName="昵称", Width=100, IsSort = false, SortDesc=""  },
                new TableListItem() { PropertyName = "Email", DsipalyName = "电子邮件", IsSort = false, SortDesc = "", Width = 150 },
                new TableListItem() { PropertyName = "Telephone", DsipalyName = "电话号码", IsSort = false, SortDesc = "", Width = 150 },
                new TableListItem() { PropertyName = "Sex", DsipalyName = "性别", IsSort = false, SortDesc = "", Width = 100 },
                new TableListItem() { PropertyName = "Birthdays", DsipalyName = "生日日期", IsSort = false, SortDesc = "", Width = 150 },
                //new TableListItem() { PropertyName = "Description", DsipalyName="简要说明", Width=0, IsSort = false  },
            };

            // 4. 设置编辑和新建数据视图参数，在 CreateOrEdit 中单独创建

            // 5. 设置明细数据视图参数
            DetailItems = new List<DetailItem>()
            {
                new DetailItem() { PropertyName = "UserName", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Name", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Email", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Description", DataType = ViewModelDataType.多行文本 },
                 new DetailItem() { PropertyName = "Telephone", DataType = ViewModelDataType.单行文本 },
                new DetailItem() { PropertyName = "Sex", DataType = ViewModelDataType.性别 },
                new DetailItem() { PropertyName = "Birthdays", DataType = ViewModelDataType.日期时间 },

            };
        }

        public override async Task<IActionResult> Index()
        {
            // 缺省排序属性
            PaginateListPageParameter.SortProperty = "UserName";
            PaginateListPageParameter.SortDesc = "Ascend"; // 排序方向，未排序：""，升序："Ascend",降序："Descend"

            var boVMCollection = await ViewModelService.GetApplicationUserVMCollection(PaginateListPageParameter);

            ViewData["PageGroup"] = PaginateListPageParameter.PagenateGroup;
            ViewData["ItemAmount"] = PaginateListPageParameter.ObjectAmount;

            ViewData["ListPageParameter"] = PaginateListPageParameter;

            ViewData["ListItems"] = ListItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = PaginateListPageParameter.TypeName + ": 所有用户数据";

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
            var boVMCollection = await ViewModelService.GetApplicationUserVMCollection(PaginateListPageParameter);

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
            ViewData["FunctionName"] = PaginateListPageParameter.TypeName + ": 所有用户数据";

            return PartialView(ListPartialViewPath, boVMCollection);
        }

        [HttpGet]
        public override async Task<IActionResult> CreateOrEdit(Guid id)
        {
            var boVM = await ViewModelService.GetApplicationUserVMForCreateOrEdit(id);
            // 设置用户关联的数据
            await ViewModelService.GetApplicationUserVMRelevanceData(boVM);
            // 设置附件相关的属性
            await ViewModelService.SetAttachmentFileItem(boVM);

            boVM.IsSaved = false;

            if (boVM.IsNew)
            {
                // 新建用户必须处理密码
                CreateOrEditItems = new List<CreateOrEditItem>()
                {
                    new CreateOrEditItem() { PropertyName = "ApplicationRoleId", TipsString="", DataType = ViewModelDataType.Select层次多选  },
                    new CreateOrEditItem() { PropertyName = "UserName", TipsString="", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem() { PropertyName = "Email", TipsString = "", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem() { PropertyName = "Password", TipsString = "密码需要至少 8 个字符，并且至少 1 个大写字母，1 个小写字母，1 个数字和 1 个特殊字符。", DataType = ViewModelDataType.密码 },
                    new CreateOrEditItem() { PropertyName = "PasswordComfirm", TipsString = "", DataType = ViewModelDataType.密码 },
                    new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 },
                    new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.隐藏 },
                     new CreateOrEditItem() { PropertyName = "Telephone", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem() { PropertyName = "Sex", DataType = ViewModelDataType.性别},
                    new CreateOrEditItem() { PropertyName = "Birthdays", DataType = ViewModelDataType.日期时间 },
                    };
            }
            else
            {
                // 编辑的用户信息是不能处理密码的
                CreateOrEditItems = new List<CreateOrEditItem>()
                {
                    new CreateOrEditItem() { PropertyName = "ApplicationRoleId", TipsString="", DataType = ViewModelDataType.Select层次多选  },
                    new CreateOrEditItem() { PropertyName = "UserName", TipsString="", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem() { PropertyName = "Email", TipsString = "", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem() { PropertyName = "Password", TipsString = "", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "PasswordComfirm", TipsString = "", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 },
                    new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "Telephone", TipsString = "", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem() { PropertyName = "Sex", TipsString="", DataType = ViewModelDataType.性别 },
                    new CreateOrEditItem() { PropertyName = "Birthdays", TipsString="", DataType = ViewModelDataType.日期时间 },

                };
                // 临时存放的密码信息，只是在视图模型校验时候使用，存取数据时会忽略这两个值
                boVM.Password = boVM.PasswordComfirm = "1234@Abcd";
            }

            var boVMName = "";
            if (!String.IsNullOrEmpty(boVM.UserName))
                boVMName = "：" + boVM.UserName;

            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = "编辑" + FunctionName + boVMName;
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }

        [HttpPost]
        public override async Task<IActionResult> CreateOrEdit(ApplicationUserVM boVM)
        {
            boVM.IsSaved = false;
            if (ModelState.IsValid)
            {
                // 新建的用户需要做用户名唯一性校验
                if (boVM.IsNew)
                {
                    var isUnique = await ViewModelService.IsUnique(x => x.UserName == boVM.UserName);
                    if (isUnique)
                    {
                        // 使用自定义的扩展方法保存数据
                        var saveStatusModel = await ViewModelService.SaveApplicationUserVMForCreateOrEdit(boVM);

                        // 如果这个值是 ture ，页面将调转回到列表页
                        boVM.IsSaved = saveStatusModel.SaveSatus;
                        if (saveStatusModel.SaveSatus == false)
                            boVM.ErrorMessage = saveStatusModel.Message;

                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "用户名重复，请重新输入一个新的用户名。");
                    }
                }
                else
                {
                    // 使用自定义的扩展方法保存数据
                    var saveStatusModel = await ViewModelService.SaveApplicationUserVMForCreateOrEdit(boVM);

                    // 如果这个值是 ture ，页面将调转回到列表页
                    boVM.IsSaved = saveStatusModel.SaveSatus;
                    if (saveStatusModel.SaveSatus == false)
                        boVM.ErrorMessage = saveStatusModel.Message;
                }
            }

            if (boVM.IsNew)
            {
                // 新建用户必须处理密码
                CreateOrEditItems = new List<CreateOrEditItem>()
                {
                    new CreateOrEditItem() { PropertyName = "ApplicationRoleId", TipsString="", DataType = ViewModelDataType.Select层次多选  },
                    new CreateOrEditItem() { PropertyName = "UserName", TipsString="", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem() { PropertyName = "Email", TipsString = "", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem() { PropertyName = "Password", TipsString = "密码需要至少 8 个字符，并且至少 1 个大写字母，1 个小写字母，1 个数字和 1 个特殊字符。", DataType = ViewModelDataType.密码 },
                    new CreateOrEditItem() { PropertyName = "PasswordComfirm", TipsString = "", DataType = ViewModelDataType.密码 },
                    new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 },
                    new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "Telephone", TipsString = "", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem() { PropertyName = "Sex", TipsString="", DataType = ViewModelDataType.性别 },
                    new CreateOrEditItem() { PropertyName = "Birthdays", TipsString="", DataType = ViewModelDataType.日期时间 },
                };
            }
            else
            {
                // 编辑的用户信息是不能处理密码的
                CreateOrEditItems = new List<CreateOrEditItem>()
                {
                    new CreateOrEditItem() { PropertyName = "ApplicationRoleId", TipsString="", DataType = ViewModelDataType.Select层次多选  },
                    new CreateOrEditItem() { PropertyName = "UserName", TipsString="", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem() { PropertyName = "Email", TipsString = "", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem() { PropertyName = "Password", TipsString = "", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "PasswordComfirm", TipsString = "", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 },
                    new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.隐藏 },
                    new CreateOrEditItem() { PropertyName = "Telephone", TipsString = "", DataType = ViewModelDataType.单行文本 },
                    new CreateOrEditItem() { PropertyName = "Sex", TipsString="", DataType = ViewModelDataType.性别 },
                    new CreateOrEditItem() { PropertyName = "Birthdays", TipsString="", DataType = ViewModelDataType.日期时间 },
                };
                // 临时存放的密码信息，只是在视图模型校验时候使用，存取数据时会忽略这两个值
                boVM.Password = boVM.PasswordComfirm = "1234@Abcd";
            }

            // 设置用户关联的数据
            await ViewModelService.GetApplicationUserVMRelevanceData(boVM);
            // 设置附件相关的属性
            await ViewModelService.SetAttachmentFileItem(boVM);

            boVM.ErrorMessage = "测试的数据";
            ViewData["CreateOrEditItems"] = CreateOrEditItems;
            ViewData["ModuleName"] = ModuleName;
            ViewData["FunctionName"] = FunctionName + "：编辑数据";
            return PartialView(CreateOrEditPartialViewPath, boVM);
        }

        [HttpGet]
        public override async Task<IActionResult> Delete(Guid id)
        {
            var status = await ViewModelService.DeleteBoAsyn(id);
            return Json(status);
        }

        /// <summary>
        /// 重写 BootStrapTreeView 插件节点数据获取方法
        /// </summary>
        /// <returns></returns>
        public override async Task<IActionResult> DataForBootStrapTreeView()
        {
            var treeViewNodes = await ViewModelService.GetTreeViewNodeForBootStrapTreeViewCollectionAsyn<ApplicationRole>(x => x.ParentRole);
            return Json(treeViewNodes);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(Guid id, string userPasword, string userPaswordConfirm)
        {
            var operationStatus = new OperationStatus() { IsOK = true, OperationValue = "", Message = "" };
            if (String.IsNullOrEmpty(userPasword) || String.IsNullOrEmpty(userPaswordConfirm))
            {
                operationStatus.IsOK = false;
                operationStatus.Message = "密码数据不能为空值。";
                return Json(operationStatus);
            }

            if (userPasword != userPaswordConfirm)
            {
                operationStatus.IsOK = false;
                operationStatus.Message = "密码和重复密码不一致。";
                return Json(operationStatus);
            }

            var isValid = Regex.IsMatch(userPasword, @"((^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,})$)");
            if (isValid != true)
            {
                operationStatus.IsOK = false;
                operationStatus.Message = "密码未能满足安全要求。";
                return Json(operationStatus);
            }

            var user = await ViewModelService.EntityRepository.ApplicationUserManager.FindByIdAsync(id.ToString());
            var token = await ViewModelService.EntityRepository.ApplicationUserManager.GeneratePasswordResetTokenAsync(user);

            var result = await ViewModelService.EntityRepository.ApplicationUserManager.ResetPasswordAsync(user, token, userPasword);
            if (result != IdentityResult.Success)
            {
                operationStatus.IsOK = false;
                operationStatus.Message = "密码更新失败。";
                return Json(operationStatus);
            }



            return Json(operationStatus);
        }

        [HttpGet]
        public async Task<IActionResult> ResetLockoutEnabled(Guid id)
        {
            var operationStatus = new OperationStatus() { IsOK = true, OperationValue = "", Message = "" };
            var user = await ViewModelService.EntityRepository.ApplicationUserManager.FindByIdAsync(id.ToString());
            var status = user.LockoutEnabled;
            user.LockoutEnabled = !status;
            await ViewModelService.EntityRepository.ApplicationUserManager.UpdateAsync(user);

            operationStatus.OperationValue = user.LockoutEnabled.ToString().ToLower();
            return Json(operationStatus);
        }
    }
}
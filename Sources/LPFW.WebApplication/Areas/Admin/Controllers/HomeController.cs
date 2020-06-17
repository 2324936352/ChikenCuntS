using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.EntitiyModels.ApplicationCommon.AppPathAndMenu;
using LPFW.EntitiyModels.OrganzationBusiness;
using LPFW.ViewModels.ApplicationCommon.AppPathAndMenu;
using LPFW.ViewModels.OrganizationBusiness;
using LPFW.ViewModelServices;
using LPFW.ViewModelServices.Extensions.OrganizationBusiness;
using Microsoft.AspNetCore.Mvc;

namespace LPFW.WebApplication.Areas.Admin.Controllers
{
    /// <summary>
    /// 后端管理台入口
    /// </summary>
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IViewModelService<TransactionCenterRegister, TransactionCenterRegisterVM> _service;

        public HomeController(IViewModelService<TransactionCenterRegister, TransactionCenterRegisterVM> service)
        {
            this._service = service;
        }

        /// <summary>
        /// 交易中心业务管理入口
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            // 初始化并获取交易中心视图模型
            var boVM = await _service.InitialDeaultTransactionCenterRegister();

            ViewData["ModuleName"] = boVM.Name;
            ViewData["FunctionName"] = "系统平台后端基础数据管理工作桌面";

            var appMenuGroup = await _service.GetOtherBoVMCollection<ApplicationMenuGroup, ApplicationMenuGroupVM>(x => x.Name == "用户");
            ViewData["ApplicationMenuGroupId"] = appMenuGroup.FirstOrDefault().Id.ToString();

            var appMenuItem = await _service.GetOtherBoVMCollection<ApplicationMenuItem, ApplicationMenuItemVM>(x => x.Name == "分页列表");
            ViewData["ApplicationMenuItemActiveId"] = appMenuItem.FirstOrDefault().Id.ToString();

            return View(boVM);
        }

        #region 初始化导航菜单
        public string GetApplicationMenuGroupId()
        {
            var controllerAreaName = ControllerContext.RouteData.Values["area"].ToString();
            var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
            if (!String.IsNullOrWhiteSpace(controllerAreaName))
                controllerName = controllerAreaName + "/" + controllerName;

            return _service.GetApplicationMenuGroupId(controllerName);
        }
        public string GetApplicationMenuItemActiveId()
        {
            var controllerAreaName = ControllerContext.RouteData.Values["area"].ToString();
            var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
            if (!String.IsNullOrWhiteSpace(controllerAreaName))
                controllerName = controllerAreaName + "/" + controllerName;
            return _service.GetGetApplicationMenuItemActiveId(controllerName);
        }
        #endregion

    }
}
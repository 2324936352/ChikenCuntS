using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.EntitiyModels.OrganzationBusiness;
using LPFW.ViewModels.OrganizationBusiness;
using LPFW.ViewModelServices;
using LPFW.ViewModelServices.Extensions.OrganizationBusiness;
using Microsoft.AspNetCore.Mvc;

namespace LPFW.WebApplication.Areas.TransactionCenter.Controllers
{
    /// <summary>
    /// 交易中心业务工作入口控制器
    /// </summary>
    [Area("OrganizationBusiness")]
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
            ViewData["FunctionName"] = "用户工作桌面";

            ViewData["ApplicationMenuGroupId"] = "6C20AC55-367B-4648-9486-4CF3139325C4";
            ViewData["ApplicationMenuItemActiveId"] = GetApplicationMenuItemActiveId();

            return View(boVM);
        }

        #region 初始化导航菜单
        public string GetApplicationMenuGroupId()
        {
            var controllerAreaName = ControllerContext.RouteData.Values["area"].ToString();
            var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
            if (!String.IsNullOrWhiteSpace(controllerAreaName))
                controllerName = controllerAreaName + "/" + controllerName;
            var a= _service.GetApplicationMenuGroupId(controllerName);
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
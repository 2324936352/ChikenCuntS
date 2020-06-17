using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.EntitiyModels.ApplicationCommon.AppPathAndMenu;
using LPFW.ViewModels.ApplicationCommon.AppPathAndMenu;
using LPFW.ViewModelServices;
using Microsoft.AspNetCore.Mvc;

namespace LPFW.WebApplication.Controllers
{
    /// <summary>
    /// 系统站点跳转服务控制器
    /// </summary>
    public class ApplicationSiteController : Controller
    {
        private readonly IViewModelService<ApplicationMenuItem, ApplicationMenuItemVM> _viewModelService;
        private readonly IViewModelService<ApplicationMenuGroup, ApplicationMenuGroupVM> _applicationMenuGroupViewModelService;

        public ApplicationSiteController(
            IViewModelService<ApplicationMenuItem, ApplicationMenuItemVM> viewModelService,
            IViewModelService<ApplicationMenuGroup, ApplicationMenuGroupVM> applicationMenuGroupViewModelService)
        {
            _viewModelService = viewModelService;
            _applicationMenuGroupViewModelService = applicationMenuGroupViewModelService;

        }

        public async Task<IActionResult> Index()
        {
            var boVM = await _applicationMenuGroupViewModelService.GetBoVMCollectionAsyn();
            return View(boVM);
        }


    }
}
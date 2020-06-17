using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.ApplicationCommon.AppPathAndMenu;
using LPFW.ViewModels.ApplicationCommon.AppPathAndMenu;
using LPFW.ViewModelServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace LPFW.WebApplication.ViewComponets
{
    /// <summary>
    /// 后端系统用户界面侧边栏的导航菜单
    /// </summary>
    public class ApplicationMenuViewComponent : ViewComponent
    {
        private readonly IViewModelService<ApplicationMenuItem, ApplicationMenuItemVM> _viewModelService;
        private readonly IViewModelService<ApplicationMenuGroup, ApplicationMenuGroupVM> _applicationMenuGroupViewModelService;

        public ApplicationMenuViewComponent(
            IViewModelService<ApplicationMenuItem, ApplicationMenuItemVM> service,
            IViewModelService<ApplicationMenuGroup, ApplicationMenuGroupVM> applicationMenuGroupViewModelService
            )
        {
            _viewModelService = service;
            _applicationMenuGroupViewModelService = applicationMenuGroupViewModelService;
        }

        /// <summary>
        /// 组件调用方法
        /// </summary>
        /// <param name="gourpId">菜单组 Id</param>
        /// <param name="activeItemId">当前激活的菜单条目的 Id</param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(Guid gourpId,Guid activeItemId)
        {
            var groupVMCollection = await _applicationMenuGroupViewModelService.GetBoVMCollectionAsyn();
            var groupVM = await _applicationMenuGroupViewModelService.GetBoVMAsyn(gourpId);
            if (groupVM.IsNew == true)  // 表明这是一个新的，将缺省使用分组集合的第一个分区
            {
                groupVM = groupVMCollection.OrderBy(x => x.SortCode).FirstOrDefault();
            }

            // 获取分区内的菜单集合
            var listPageParameter = new ListSinglePageParameter();
            var boVMCollection = new List<ApplicationMenuItemVM>();
            if (!String.IsNullOrEmpty(groupVM.Name))
            {
                boVMCollection = await _viewModelService.GetBoVMCollectionAsyn(listPageParameter, x => x.ApplicationMenuGroup.Id == groupVM.Id, y => y.ApplicationMenuGroup);
            }
            else
                boVMCollection = await _viewModelService.GetBoVMCollectionAsyn(listPageParameter, y => y.ApplicationMenuGroup);

            ViewData["ApplicationMenuGroupCollection"] = groupVMCollection;
            ViewData["ApplicationMenuGroup"] = groupVM;
            ViewData["ApplicationMenuItemActiveId"] = activeItemId;
            return View("Default",boVMCollection);
        }

    }
}

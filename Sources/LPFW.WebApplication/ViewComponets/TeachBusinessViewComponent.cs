using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.ApplicationCommon.AppPathAndMenu;
using LPFW.ViewModels.ApplicationCommon.AppPathAndMenu;
using LPFW.ViewModelServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPFW.WebApplication.ViewComponets
{
    public class TeachBusinessViewComponent : ViewComponent
    {
        private readonly IViewModelService<ApplicationMenuItem, ApplicationMenuItemVM> _service;

        public TeachBusinessViewComponent(IViewModelService<ApplicationMenuItem, ApplicationMenuItemVM> service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid groupId, Guid activeItemId)
        {
            // 获取分区内的菜单集合
            var listPageParameter = new ListSinglePageParameter();
            var boVMCollection = new List<ApplicationMenuItemVM>();
            boVMCollection = await _service.GetBoVMCollectionAsyn(listPageParameter, x => x.ApplicationMenuGroup.Id == groupId, i => i.ApplicationMenuGroup);

            ViewData["ApplicationMenuItemActiveId"] = activeItemId;
            return View("Default", boVMCollection);
        }
    }
}

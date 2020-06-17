using LPFW.DataAccess;
using LPFW.EntitiyModels.ApplicationCommon.Attachments;
using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.ViewModels.ApplicationCommon.RoleAndUser;
using LPFW.ViewModelServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPFW.WebApplication.ViewComponets
{
    /// <summary>
    /// 用户登录信息，用于显示视图中右上角的登录信息
    /// </summary>
    public class LoginUserInfoViewComponent : ViewComponent
    {
        private readonly IViewModelService<ApplicationUser, ApplicationUserVM> _service;

        public LoginUserInfoViewComponent(IViewModelService<ApplicationUser, ApplicationUserVM> service)
        {
            _service = service;
        }

        /// <summary>
        /// 供视图在合适的位置调用的方法
        /// </summary>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userAvatarPath = "/images/avatar/avatar-1.png";
            var userName = "";

            var userIdentity = User.Identity;
            if (!String.IsNullOrEmpty(userIdentity.Name))
            {
                var user = await _service.EntityRepository.ApplicationUserManager.FindByNameAsync(userIdentity.Name);
                var userImage = await  _service.EntityRepository.GetOtherBoAsyn<BusinessImage>(x => x.RelevanceObjectID == user.Id);
                if (userImage != null)
                    userAvatarPath = userImage.UploadPath;
                userName = user.UserName;
            }

            ViewData["LoginUserAvatarPath"] = userAvatarPath;
            ViewData["LoginUserName"] = userName;
            // 返回到指定的视图内容
            return View("Default");
        }

    }
}

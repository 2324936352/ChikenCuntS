using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.ViewModels.ApplicationCommon.RoleAndUser;
using LPFW.ViewModelServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LPFW.WebApplication.Controllers
{
    /// <summary>
    /// 系统登录相关的处理
    /// </summary>
    public class AccountsController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IViewModelService<ApplicationUser, ApplicationUserVM> _service;

        public AccountsController(
            SignInManager<ApplicationUser> signInManager,
            IViewModelService<ApplicationUser, ApplicationUserVM> service
            )
        {
            this._signInManager = signInManager;
            this._service = service;
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="returnUrl">在单击登录操作的 URL，以便登录成功后自动返回</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("/Home/Index");
            return View(new LoginVM() { ReturnUrl = returnUrl });
        }

        /// <summary>
        /// 提交登录令牌
        /// </summary>
        /// <param name="loginVM">登录的视图模型</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                // 提取用户对象
                var user = await _service.EntityRepository.ApplicationUserManager.FindByNameAsync(loginVM.UserName);
                if (user != null)
                {
                    // 检查用户是否被锁定
                    if (user.LockoutEnabled)
                    {
                        // 登录系统
                        var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            if (!String.IsNullOrEmpty(loginVM.ReturnUrl))
                                return Redirect(loginVM.ReturnUrl);
                            else
                                return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewData["LoginStatusString"] = "用户名或者密码错误。";
                            return View(loginVM);
                        }
                    }
                    else
                    {
                        ViewData["LoginStatusString"] = "用户已被锁定，无法登录!";
                        return View(loginVM);
                    }
                }
                else
                {
                    ViewData["LoginStatusString"] = "用户名或者密码错误。";
                    return View(loginVM);
                }

            }
            ViewData["LoginStatusString"] = "用户名或者密码错误";
            return View(loginVM);
        }

        /// <summary>
        /// 直接传入登录视图模型进行登录，方便进行调试，登录成功返回来源视图，失败直接跳转至首页
        /// </summary>
        /// <param name="loginVM"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SimulationLogin(string userName,string password,bool rememberMe, string returnUrl)
        {
            // 提取用户对象
            var user = await _service.EntityRepository.ApplicationUserManager.FindByNameAsync(userName);
            if (user != null)
            {
                // 检查用户是否被锁定
                if (user.LockoutEnabled)
                {
                    // 登录系统
                    var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        if (!String.IsNullOrEmpty(returnUrl))
                            return Redirect(returnUrl);
                        else
                            return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewData["LoginStatusString"] = "用户名或者密码错误。";
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewData["LoginStatusString"] = "用户已被锁定，无法登录!";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewData["LoginStatusString"] = "用户名或者密码错误。";
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// 注销，注销之后统一处理返回至系统首页
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 调试期间的注销用户
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> LogoutAndReturn(string returnUrl)
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
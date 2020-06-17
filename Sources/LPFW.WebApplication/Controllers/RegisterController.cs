using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.ViewModels.ApplicationCommon.RoleAndUser;
using LPFW.ViewModels.BusinessCommon;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModelServices;
using LPFW.ViewModelServices.Extensions.ApplicationCommon.UserAndUser;
using LPFW.WebApplication.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LPFW.WebApplication.Controllers
{
    /// <summary>
    /// 商家注册控制器
    /// </summary>
    public class RegisterController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IViewModelService<ApplicationUser, ApplicationUserVM> _service;

        public RegisterController(
            SignInManager<ApplicationUser> signInManager,
            IViewModelService<ApplicationUser, ApplicationUserVM> service 
            )
        {
            this._signInManager = signInManager;
            this._service = service;
        }

        /// <summary>
        /// 注册入口
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var checkVM = new RegisterCheckVM();
            ViewData["TipString"] = "";
            return View(checkVM);
        }

        /// <summary>
        /// 检查是否勾选服务声明
        /// </summary>
        /// <param name="checkVM"></param>
        /// <returns></returns>
        public IActionResult RegisterCheck(RegisterCheckVM checkVM)
        {
            if (!checkVM.Agree)
            {
                ViewData["TipString"] = "抱歉，你需要勾选同意说明才能进行下一步操作。";
                return View("Index", checkVM);
            }
            else
            {
                return RedirectToAction("RegisterSelectBusinessType");
            }
        }

        /// <summary>
        /// 选择注册类型
        /// </summary>
        /// <returns></returns>
        public IActionResult RegisterSelectBusinessType()
        {
            var typeVM = new RegisterSelectBusinessTypeVM() 
            {
                PurchaserType= RoleCommonClaimEnum.采购商业务, 
                VenderType= RoleCommonClaimEnum.供应商业务 
            };

            ViewData["TipString"] = "";
            return View(typeVM);
        }

        /// <summary>
        /// 检查是否已经选择商家类型
        /// </summary>
        /// <param name="typeVM"></param>
        /// <returns></returns>
        public IActionResult RegisterSelectBusinessTypeCheck(RegisterSelectBusinessTypeVM typeVM)
        {
            if (typeVM.VenderType != typeVM.SelectedType || typeVM.SelectedType != typeVM.PurchaserType)
            {
                if(typeVM.SelectedType== RoleCommonClaimEnum.供应商业务)
                    return RedirectToAction("RegisterVender");

                if(typeVM.SelectedType== RoleCommonClaimEnum.采购商业务)
                    return RedirectToAction("RegisterPurchaser");
            }
            else
            {
                ViewData["TipString"] = "抱歉，你需要选择一个会员类型之后才能进行下一步操作。";
            }

            typeVM = new RegisterSelectBusinessTypeVM()
            {
                PurchaserType = RoleCommonClaimEnum.采购商业务,
                VenderType = RoleCommonClaimEnum.供应商业务
            };
            return View("RegisterSelectBusinessType", typeVM);
        }

        /// <summary>
        /// 导航到体验入口
        /// </summary>
        /// <param name="welcomeString">欢迎说明</param>
        /// <param name="experienceString">体验入口路径</param>
        /// <returns></returns>
        public IActionResult RegisterExperience(string welcomeString, string experienceString)
        {
            ViewData["WelcomeString"] = welcomeString;
            ViewData["ExperienceString"] = experienceString;
            return View();
        }
    }
}
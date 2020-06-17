using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.EntitiyModels.OrganzationBusiness;
using LPFW.Foundation.SpecificationsForEntityModel;
using LPFW.ViewModels;
using LPFW.ViewModelServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPFW.WebApplication.Filters
{
    public class TeacherAsyncFilter<TEntity, TViewModel> : IAsyncActionFilter
        where TEntity : class, IEntity, new()
        where TViewModel : class, IEntityViewModel, new()
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IViewModelService<TEntity, TViewModel> _service;

        private string _userName;
        private string _password;

        private Guid _organizationId  = Guid.Empty;
        private Guid _departmentId    = Guid.Empty;
        private Guid _emplyeeId       = Guid.Empty;
        private Guid _userId          = Guid.Empty;

        /// <summary>
        /// 构造函数，注入过滤器所需的服务
        /// </summary>
        /// <param name="signInManager"></param>
        /// <param name="service"></param>
        public TeacherAsyncFilter(SignInManager<ApplicationUser> signInManager, IViewModelService<TEntity, TViewModel> service)
        {
            _service = service;
            _signInManager = signInManager;
        }

        /// <summary>
        /// 构造函数，注入过滤器所需的服务，和模拟登录需要额用户名和密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="signInManager"></param>
        /// <param name="service"></param>
        public TeacherAsyncFilter(string userName, string password, SignInManager<ApplicationUser> signInManager, IViewModelService<TEntity, TViewModel> service)
        {
            _service = service;
            _userName = userName;
            _password = password;
            _signInManager = signInManager;
        }

        /// <summary>
        /// 启动 Action 时执行的方法：从登录用户信息中确定用户归属的采购商的 Id，如果没有的话，或者不是采购商用户的话，则跳转至 _Error 局部视图
        /// </summary>
        /// <param name="context"></param>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 如果提供密码和用户名，则直接模拟登录
            if (!String.IsNullOrEmpty(_password) && !String.IsNullOrEmpty(_userName))
            {
                var user = await _service.EntityRepository.ApplicationUserManager.FindByNameAsync(_userName);
                var login = await _signInManager.PasswordSignInAsync(user, _password, true, lockoutOnFailure: false);
                if (!login.Succeeded)
                {
                    var result = new PartialViewResult { ViewName = "_Error" };
                    context.Result = result;
                }
                else
                {
                    _organizationId = await _service.CommonGetOrganizationId(_userName);
                    if (_organizationId != Guid.Empty)
                    {
                        await _SetPDEU(user);

                        var purchaseIdProperty = context.Controller.GetType().GetProperty("OrganzationId");
                        if (purchaseIdProperty != null)
                            purchaseIdProperty.SetValue(context.Controller, _organizationId);

                        var departmentIdProperty = context.Controller.GetType().GetProperty("DepartmentId");
                        if (departmentIdProperty != null)
                            departmentIdProperty.SetValue(context.Controller, _departmentId);

                        var emplyeeIdProperty = context.Controller.GetType().GetProperty("EmplyeeId");
                        if (emplyeeIdProperty != null)
                            emplyeeIdProperty.SetValue(context.Controller, _emplyeeId);

                        var userIdProperty = context.Controller.GetType().GetProperty("UserId");
                        if (userIdProperty != null)
                            userIdProperty.SetValue(context.Controller, _userId);

                        await next();
                    }
                    else
                    {
                        var result = new PartialViewResult { ViewName = "_Error" };
                        context.Result = result;
                    }
                }
            }
            else
            {
                // 如果只提供用户名--调试时使用
                if (!String.IsNullOrEmpty(_userName))
                {
                    var user = await _service.EntityRepository.ApplicationUserManager.FindByNameAsync(_userName);
                    if (user != null)
                    {
                        _organizationId = await _service.CommonGetPurchaserId(_userName);
                        if (_organizationId != Guid.Empty)
                        {
                            await _SetPDEU(user);

                            var purchaseIdProperty = context.Controller.GetType().GetProperty("OrganzationId");
                            if (purchaseIdProperty != null)
                                purchaseIdProperty.SetValue(context.Controller, _organizationId);

                            var departmentIdProperty = context.Controller.GetType().GetProperty("DepartmentId");
                            if (departmentIdProperty != null)
                                departmentIdProperty.SetValue(context.Controller, _departmentId);

                            var emplyeeIdProperty = context.Controller.GetType().GetProperty("EmplyeeId");
                            if (emplyeeIdProperty != null)
                                emplyeeIdProperty.SetValue(context.Controller, _emplyeeId);

                            var userIdProperty = context.Controller.GetType().GetProperty("UserId");
                            if (userIdProperty != null)
                                userIdProperty.SetValue(context.Controller, _userId);

                            await next();
                        }
                        else
                        {
                            var result = new PartialViewResult { ViewName = "_Error" };
                            context.Result = result;
                        }
                    }
                    else
                    {
                        var result = new PartialViewResult { ViewName = "_Error" };
                        context.Result = result;
                    }

                }
                // 否则按照正常的登录用户处理
                else
                {
                    var controller = context.Controller as Controller;
                    var userIdentity = controller.User.Identity;
                    if (!String.IsNullOrEmpty(userIdentity.Name))
                    {
                        // 如果返回的的是空值，则表明该用户不是采购商用户
                        _organizationId = await _service.CommonGetOrganizationId(userIdentity.Name);
                        if (_organizationId != Guid.Empty)
                        {
                            var user = await _service.EntityRepository.ApplicationUserManager.FindByNameAsync(userIdentity.Name);
                            await _SetPDEU(user);

                            var purchaseIdProperty = context.Controller.GetType().GetProperty("OrganzationId");
                            if (purchaseIdProperty != null)
                                purchaseIdProperty.SetValue(context.Controller, _organizationId);

                            var departmentIdProperty = context.Controller.GetType().GetProperty("DepartmentId");
                            if (departmentIdProperty != null)
                                departmentIdProperty.SetValue(context.Controller, _departmentId);

                            var emplyeeIdProperty = context.Controller.GetType().GetProperty("EmplyeeId");
                            if (emplyeeIdProperty != null)
                                emplyeeIdProperty.SetValue(context.Controller, _emplyeeId);

                            var userIdProperty = context.Controller.GetType().GetProperty("UserId");
                            if (userIdProperty != null)
                                userIdProperty.SetValue(context.Controller, _userId);

                            await next();
                        }
                        else
                        {
                            var result = new PartialViewResult { ViewName = "_Error"  };
                            context.Result = result;
                        }
                    }
                    else
                    {
                        // 获取当前请求路径
                        var areaPath = context.RouteData.Values["area"].ToString();
                        var controllerPath = context.RouteData.Values["controller"].ToString();
                        var actionPath = context.RouteData.Values["action"].ToString();

                        // 跳转至登录页面
                        var result = new RedirectToActionResult("Login", "Account", new { area = "/", returnUrl = "/" + areaPath + "/" + controllerPath + "/" + actionPath });
                        context.Result = result;
                    }
                }
            }
        }

        private async Task _SetPDEU(ApplicationUser user)
        {
            _userId = user.Id;

            var userClaimCollection = await _service.EntityRepository.ApplicationUserManager.GetClaimsAsync(user);

            var organizationClaim = userClaimCollection.FirstOrDefault(x => x.Type == UserCommonClaimEnum.单位.ToString());
            if (organizationClaim != null)
                _organizationId = Guid.Parse(organizationClaim.Value);

            var departmentClaim = userClaimCollection.FirstOrDefault(x => x.Type == UserCommonClaimEnum.部门.ToString());
            if (departmentClaim != null)
                _departmentId = Guid.Parse(departmentClaim.Value);

            var employee = await _service.EntityRepository.GetOtherBoAsyn<Employee>(x => x.User.Id == user.Id && x.IsStudent == false, y => y.User);
            if (employee != null)
                _emplyeeId = employee.Id;
        }

    }
}

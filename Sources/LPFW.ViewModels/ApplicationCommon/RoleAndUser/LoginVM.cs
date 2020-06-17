using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.ApplicationCommon.RoleAndUser
{
    public class LoginVM
    {
        [Required(ErrorMessage = "用户名是必须的。")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "密码是必须的。")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }    // 登录成功后返回的路径，缺省返回到入口首页
        public bool RememberMe { get; set; }     // 记住我

        public LoginVM() { }
    }
}

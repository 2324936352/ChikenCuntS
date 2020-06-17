using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.MusicEntity
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserInfo : ApplicationUser
    {
        
        public string Login { get; set; } //账号
        public string UserPwd { get; set; }//密码

        public virtual Power Power1 { get; set; } //权限
        public UserInfo()
        {
            this.Id = Guid.NewGuid();
        }
    }
}

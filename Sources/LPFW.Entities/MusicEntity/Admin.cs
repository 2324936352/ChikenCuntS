using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.MusicEntity
{
    /// <summary>
    /// 管理员
    /// 继承自用户信息
    /// </summary>
    public class Admin:UserInfo
    {
      
        public string AdminNumber { get; set; } //编号       

        public Admin()
        {
            this.Id = Guid.NewGuid();
        }
    }
}

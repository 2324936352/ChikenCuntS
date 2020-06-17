using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.EntitiyModels.ApplicationCommon.AppPathAndMenu
{
    /// <summary>
    /// 应用菜单分组，在一般的应用中，应用菜单分组可用于对应到子系统
    /// </summary>
    public class ApplicationMenuGroup : Entity
    {
        [StringLength(500)]
        public string PortalUrl { get; set; } // 分区应用入口路径

        public ApplicationMenuGroup()
        {
            this.Id = Guid.NewGuid();
        }
    }
}

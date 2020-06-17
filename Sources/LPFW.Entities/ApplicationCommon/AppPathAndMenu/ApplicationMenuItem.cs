using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.EntitiyModels.ApplicationCommon.AppPathAndMenu
{
    /// <summary>
    /// 应用菜单条
    /// </summary>
    public class ApplicationMenuItem : Entity
    {
        [StringLength(300)]
        public string UrlString { get; set; }         // 菜单条对应的应用路径
        [StringLength(50)]
        public string IconString { get; set; }        // 图标
        [StringLength(50)]
        public string ItemStartTipString { get; set; }// 标识，可用于在菜单中合适的起始位置呈现菜单对应数据的状态
        public string ItemEndString { get; set; }     // 标识，可用于在菜单中合适的终止位置呈现菜单对应数据的状态

        public ApplicationMenuItem ParentItem { get; set; }             // 上级菜单条目
        public ApplicationMenuGroup ApplicationMenuGroup { get; set; }  // 归属菜单分组

        public ApplicationMenuItem()
        {
            this.Id = Guid.NewGuid();
        }
    }
}

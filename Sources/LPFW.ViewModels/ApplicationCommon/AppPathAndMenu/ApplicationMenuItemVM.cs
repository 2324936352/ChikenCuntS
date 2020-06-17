using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.ApplicationCommon.AppPathAndMenu
{
    public class ApplicationMenuItemVM:EntityViewModel
    {
        [Display(Name = "应用路径")]
        [StringLength(500, ErrorMessage = "你输入的数据超出限制500个字符的长度。")]
        public string UrlString { get; set; }

        [Display(Name = "菜单图标")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public string IconString { get; set; }

        // 对于应用时赋值的属性，不需要在前端处理，仅保留其属性规约即可
        public string ItemStartTipString { get; set; }
        public string ItemEndString { get; set; }

        [Display(Name = "上级菜单")]
        public string ParentItemId { get; set; }
        [Display(Name = "上级菜单")]
        public string ParentItemName { get; set; }
        public List<SelfReferentialItem> ParentItemItemCollection { get; set; }

        [Display(Name = "归属分组")]
        [Required(ErrorMessage = "归属分组是必须选择的。")]
        public string ApplicationMenuGroupId { get; set; }
        [Display(Name = "归属分组")]
        public string ApplicationMenuGroupName { get; set; }
        public List<PlainFacadeItem> ApplicationMenuGroupItemCollection { get; set; }
    }
}

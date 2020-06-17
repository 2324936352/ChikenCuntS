using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.ApplicationCommon.AppPathAndMenu
{
    public class ApplicationMenuGroupVM:EntityViewModel
    {
        [Required(ErrorMessage = "名称不能为空值。")]
        [Display(Name = "分组名称")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public override string Name { get; set; }

        [Required(ErrorMessage = "分组编码不能为空值。")]
        [Display(Name = "分组编码")]
        [StringLength(150, ErrorMessage = "你输入的数据超出限制150个字符的长度。")]
        public override string SortCode { get; set; }

        [Required(ErrorMessage = "分组入口不能为空值。")]
        [Display(Name = "分组路径")]
        [StringLength(500, ErrorMessage = "你输入的数据超出限制500个字符的长度。")]
        public string PortalUrl { get; set; }

    }
}

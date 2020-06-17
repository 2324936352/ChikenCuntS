using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.OrganizationBusiness
{
    /// <summary>
    /// 组织指标类型视图模型
    /// </summary>
    public class OrganizationKPITypeVM : EntityViewModel
    {
        [Required(ErrorMessage = "名称不能为空值。")]
        [Display(Name = "绩效类型名称")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public override string Name { get; set; }

        [Required(ErrorMessage = "分组编码不能为空值。")]
        [Display(Name = "类型编码")]
        [StringLength(150, ErrorMessage = "你输入的数据超出限制150个字符的长度。")]
        public override string SortCode { get; set; }
    }
}

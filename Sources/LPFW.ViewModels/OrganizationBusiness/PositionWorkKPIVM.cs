using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.OrganizationBusiness
{
    /// <summary>
    /// 岗位作业绩效指标视图模型
    /// </summary>
    public class PositionWorkKPIVM:EntityViewModel
    {
        [Required(ErrorMessage = "指标名称不能为空值。")]
        [Display(Name = "指标名称")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public override string Name { get; set; }

        [Required(ErrorMessage = "编码不能为空值。")]
        [Display(Name = "编码")]
        [StringLength(150, ErrorMessage = "你输入的数据超出限制150个字符的长度。")]
        public override string SortCode { get; set; }

        [Required(ErrorMessage = "指标基准不能为空值。")]
        [Display(Name = "指标基准")]
        public int Benchmark { get; set; }

        [Required(ErrorMessage = "绩效计算系数不能为空值。")]
        [Display(Name = "绩效系数")]
        public float Coefficient { get; set; }

        [Display(Name = "指标类型")]
        [Required(ErrorMessage = "指标必须选择。")]
        public string OrganizationKPITypeId { get; set; }
        [Display(Name = "指标类型")]
        public string OrganizationKPITypeName { get; set; }
        public List<PlainFacadeItem> OrganizationKPITypeItemCollection { get; set; }

        [Display(Name = "归属岗位")]
        [Required(ErrorMessage = "归属岗位必须选择。")]
        public string PositionId { get; set; }
        [Display(Name = "归属岗位")]
        public string PositionName { get; set; }
        public List<PlainFacadeItem> PositionItemCollection { get; set; }

        public Guid DepartemntId { get; set; }
    }
}

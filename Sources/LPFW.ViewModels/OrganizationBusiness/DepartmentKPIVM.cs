using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.OrganizationBusiness
{
    public class DepartmentKPIVM : EntityViewModel
    {
        [Required(ErrorMessage = "名称不能为空值。")]
        [Display(Name = "绩效类型")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public override string Name { get; set; }

        [Required(ErrorMessage = "编码不能为空值。")]
        [Display(Name = "类型编码")]
        [StringLength(150, ErrorMessage = "你输入的数据超出限制150个字符的长度。")]
        public override string SortCode { get; set; }

        [Required(ErrorMessage = "指标基准不能为空值。")]
        [Display(Name = "指标基准")]
        public int Benchmark { get; set; }      // 指标基准值，单位人工完成单项作业的指标基数

        [Required(ErrorMessage = "绩效计算系数不能为空值。")]
        [Display(Name = "绩效系数")]
        public float Coefficient { get; set; }  // 绩效计算系数，这是根据指标的重要性来定义的

        [Display(Name = "指标类型")]
        public string OrganizationKPITypeId { get; set; }
        [Display(Name = "指标类型")]
        public string OrganizationKPITypeName { get; set; }
        public List<PlainFacadeItem> OrganizationKPITypeItemCollection { get; set; }

        public string DepartmentId { get; set; }
    }
}

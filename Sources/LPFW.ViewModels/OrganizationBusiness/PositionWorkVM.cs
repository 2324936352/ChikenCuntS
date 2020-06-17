using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.OrganizationBusiness
{
    /// <summary>
    /// 岗位业务作业视图模型
    /// </summary>
    public class PositionWorkVM:EntityViewModel
    {
        [Required(ErrorMessage = "作业名称不能为空值。")]
        [Display(Name = "作业名称")]
        [StringLength(50, ErrorMessage = "你输入的数据超出限制50个字符的长度。")]
        public override string Name { get; set; }

        [Required(ErrorMessage = "作业路径不能为空值。")]
        [Display(Name = "作业路径")]
        [StringLength(300, ErrorMessage = "你输入的数据超出限制300个字符的长度。")]
        public string WorkActionUrl { get; set; }

        [Display(Name = "归属岗位")]
        [Required(ErrorMessage = "归属岗位必须选择。")]
        public string PositionId { get; set; }
        [Display(Name = "归属岗位")]
        public string PositionName { get; set; }
        public List<PlainFacadeItem> PositionItemCollection { get; set; }

        public Guid DepartemntId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using LPFW.ViewModels.ControlModels;

namespace LPFW.ViewModels.OrganizationBusiness
{
    /// <summary>
    /// 组织业务过程视图模型
    /// </summary>
    public class OrganizationBusinessProcessVM : EntityViewModel
    {
        [Required(ErrorMessage = "过程名称不能为空值。")]
        [Display(Name = "过程名称")]
        [StringLength(50, ErrorMessage = "你输入的数据超出限制50个字符的长度。")]
        public override string Name { get; set; }

        [Display(Name = "单位")]
        public string OrganizationId { get; set; }
        [Display(Name = "单位")]
        public string OrganizationName { get; set; }
        public List<PlainFacadeItem> OrganizationItemCollection { get; set; }

        public List<OrganizationBusinessProcessWithWorkVM> OrganizationBusinessProcessWithWorkVMCollection { get; set; }
    }
}

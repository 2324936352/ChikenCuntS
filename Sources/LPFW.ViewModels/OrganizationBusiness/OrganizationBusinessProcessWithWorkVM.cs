using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.OrganizationBusiness
{
    /// <summary>
    /// 业务过程活动清单条目视图模型，数据通过选择配给岗位作业关联
    /// </summary>
    public class OrganizationBusinessProcessWithWorkVM: EntityViewModel
    {
        [Required(ErrorMessage = "作业顺序不能为空值。")]
        [Display(Name = "作业顺序")]
        public string StepNumber { get; set; }

        public Guid PositionWorkId { get; set; }                 // 岗位工作 Id

        public Guid OrganizationBusinessProcessId { get; set; }  // 关联过程 Id

    }
}

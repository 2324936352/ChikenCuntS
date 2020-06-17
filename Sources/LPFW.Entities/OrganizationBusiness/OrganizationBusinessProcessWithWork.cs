using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.EntitiyModels.OrganzationBusiness
{
    /// <summary>
    /// 组织业务过程中的岗位业务作业清单
    /// </summary>
    public class OrganizationBusinessProcessWithWork : EntityBase
    {
        [StringLength(20)]
        public string StepNumber { get; set; }                                                // 作业序号
        public virtual OrganizationBusinessProcess OrganizationBusinessProcess { get; set; }  // 归属组织过程
        public virtual PositionWork PositionWork { get; set; }                                // 岗位工作

        public OrganizationBusinessProcessWithWork() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}

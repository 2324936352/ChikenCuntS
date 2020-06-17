using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.OrganzationBusiness
{
    /// <summary>
    /// 员工负责的岗位业务作业（授权配置）
    /// </summary>
    public class EmployeeWithPositionWork : EntityBase
    {
        public virtual Employee Employee { get; set; }
        public virtual PositionWork PositionWork { get; set; }

        public EmployeeWithPositionWork() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}

using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.OrganzationBusiness
{
    /// <summary>
    /// 部门负责人
    /// </summary>
    public class DepartmentLeader:EntityBase
    {
        public virtual Employee Leader { get; set; }
        public virtual Department Department { get; set; }

        public DepartmentLeader() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}

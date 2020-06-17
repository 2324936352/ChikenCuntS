using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.OrganzationBusiness
{
    /// <summary>
    /// 部门工作联系人
    /// </summary>
    public class DepartmentContact:EntityBase
    {
        public virtual Employee Contact { get; set; }
        public virtual Department Department { get; set; }

        public DepartmentContact() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}

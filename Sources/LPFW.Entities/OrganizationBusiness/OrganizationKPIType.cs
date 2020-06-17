using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.OrganzationBusiness
{
    /// <summary>
    /// 组织指标类型：约束指标的分类，例如：敬业，技能，完成度...
    /// </summary>
    public class OrganizationKPIType:Entity
    {
        public OrganizationKPIType() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}

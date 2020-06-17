using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.OrganzationBusiness
{
    /// <summary>
    /// 机构岗位，有时候也可以看成是工种定义
    ///   SortCode 用作岗位编码，具有实际的业务意义。
    /// </summary>
    public class Position:Entity
    {
        public virtual Department Department { get; set; }  // 岗位归属的部门

        public Position()
        {
            this.Id = Guid.NewGuid();
        }
    }
}

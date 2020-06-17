using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.EntitiyModels.OrganzationBusiness
{
    /// <summary>
    /// 岗位业务作业
    ///   Name：作业名称
    ///   Description：简要描述
    ///   SortCode：作业编码
    /// </summary>
    public class PositionWork : Entity
    {
        [StringLength(300)]
        public string WorkActionUrl { get; set; } // 作业操作对应的应用系统访问路径

        public virtual Position Position { get; set; }
        public virtual ICollection<PositionWorkKPI> PositionWorkKPIs { get; set; }  // 岗位作业绩效指标清单

        public PositionWork() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}

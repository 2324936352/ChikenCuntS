using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.EntitiyModels.PortalSite;
using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.CollaborationTeam
{
    /// <summary>
    /// 项目任务，为了完成某个工作所需要配置的项目任务
    ///   Name: 任务名称
    ///   Description：任务简要描述
    /// </summary>
    public class TeamProjectTask : Entity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int WorkValue { get; set; }       // 工作价值，价值的算法采用相对估算的方式进行
        public int EffortValue { get; set; }     // 工作量值，工作量值的算法采用相对估算的方式进行
        public int DefficultValue { get; set; }  // 难度值，采用相对估算的方式进行

        public virtual ApplicationUser User { get; set; }
        public virtual TeamProjectWork Work { get; set; }

        public TeamProjectTask() 
        {
            this.Id = Guid.NewGuid();
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<Article>();
        }
    }
}

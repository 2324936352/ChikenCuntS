using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.CollaborationTeam
{
    /// <summary>
    /// 项目里程碑（可以理解成阶段、冲刺、迭代等）
    ///   Name：里程碑名称
    ///   Description：里程碑简要说明
    ///   SortCode: 编码
    /// </summary>
    public class TeamProjectMileStone : Entity
    {
        public DateTime StartDate { get; set; }  // 开始日期
        public DateTime EndDate { get; set; }    // 结束日期

        public virtual TeamProjectMileStone Parent { get; set; }     // 上级里程碑
        public virtual TeamProject Project { get; set; }             // 项目

        public TeamProjectMileStone() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}

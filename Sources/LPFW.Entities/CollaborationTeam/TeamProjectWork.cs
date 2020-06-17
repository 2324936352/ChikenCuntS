using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.CollaborationTeam
{
    /// <summary>
    /// 项目工作，在每个里程碑期间，为了完成相应的阶段产品或者总成产品的某个部件所需要进行工作，一般来说包括：
    /// 策划、计划、定义、检查、验收等。
    ///   Name：工作名称
    ///   Description：工作职责说明
    ///   SortCode：工作编码
    /// </summary>
    public class TeamProjectWork : Entity
    {
        public int WorkValue { get; set; }       // 工作价值，价值的算法采用相对估算的方式进行
        public int EffortValue { get; set; }     // 工作量值，工作量值的算法采用相对估算的方式进行
        public int DefficultValue { get; set; }  // 难度值，采用相对估算的方式进行

        // 工作成果约束

        public virtual TeamProjectMileStone MileStone { get; set; }  // 归属的里程碑

        public TeamProjectWork() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}

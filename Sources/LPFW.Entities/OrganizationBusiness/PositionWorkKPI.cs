using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.OrganzationBusiness
{
    /// <summary>
    /// 岗位业务作业绩效指标定义
    ///   Name：指标名称，例如复杂度、工作量、价值量（销售数量、生产数量...）
    ///   Description：指标简要描述
    ///   SortCode：作业编码
    /// </summary>
    public class PositionWorkKPI : Entity
    {
        public int Benchmark { get; set; }      // 指标基准值，单位人工完成单项作业的指标基数
        public float Coefficient { get; set; }  // 绩效计算系数，这是根据指标的重要性来定义的

        public virtual Position Position { get; set; }  // 归属的岗位
        public virtual OrganizationKPIType KPIType { get; set; }  // 指标类型

        public PositionWorkKPI() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}

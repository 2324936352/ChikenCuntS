using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.OrganzationBusiness
{
    /// <summary>
    /// 员工绩效记录，根据分配给员工的岗位及其对应的岗位作业，定义每个考核指标的执行考核的值
    /// </summary>
    public class EmployeeKPIMetricItem:EntityBase
    {
        public int DefaultValue { get; set; }        // 缺省的完成值（来自系统的业务作业日志数据）
        public int AdjustmentValue { get; set; }     // 调整之后的完成
        public float PerformanceValue { get; set; }  // 绩效值，根据指标的基准值、加权系数和 DefaultValue 或者 AdjustmentValue

        public virtual PositionWorkKPI PositionWorkKPI { get; set; }

        public EmployeeKPIMetricItem() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}

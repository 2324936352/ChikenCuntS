using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.OrganzationBusiness
{
    /// <summary>
    /// 部门绩效度量报告
    ///   Name：绩效报告名称（部门名称起止日期）
    ///   Description：简要描述
    ///   SortCode：自动处理
    /// </summary>
    public class DepartmentKPIMetric : Entity
    {
        public DateTime StartDateTime { get; set; }      // 测评开始时间
        public DateTime EndDateTime { get; set; }        // 测评结束时间
        public float TotalPerformenceValue { get; set; } // 绩效合计分值，根据 EmployeeKPIMetricItemCollection 中 PerformanceValue 合计而成

        public KPIMetricLifeCycleEnum MetricLifeCycle { get; set; } // 测评周期类型，如果配置了，自动处理 StartDateTime 和 StartDateTime
        public virtual Department Department { get; set; }          // 考核部门
        public virtual ICollection<DepartmentKPIMetricItem> DepartmentKPIMetricItemCollection { get; set; }  // 绩效记录表

        public DepartmentKPIMetric()
        {
            this.Id = Guid.NewGuid();
            this.StartDateTime = EndDateTime = DateTime.Now;
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<DepartmentKPIMetric>();
        }
    }
}

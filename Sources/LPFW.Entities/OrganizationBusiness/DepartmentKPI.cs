using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.OrganzationBusiness
{
    /// <summary>
    /// 组织部门业务绩效指标
    ///   Name：指标名称
    ///   Decription：指标描述
    ///   SortCode：指标编码，
    /// </summary>
    public class DepartmentKPI : Entity
    {
        public int Benchmark { get; set; }      // 指标基准值，单位人工完成单项作业的指标基数
        public float Coefficient { get; set; }  // 绩效计算系数，这是根据指标的重要性来定义的

        public virtual OrganizationKPIType KPIType { get; set; }

        public DepartmentKPI() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}

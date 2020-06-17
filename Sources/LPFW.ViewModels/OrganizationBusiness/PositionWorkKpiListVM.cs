using System;
using System.Collections.Generic;

namespace LPFW.ViewModels.OrganizationBusiness
{
    /// <summary>
    /// 处理工作岗位作业绩效指标列表清单视图模型
    /// </summary>
    public class PositionWorkKpiListVM
    {
        public Guid Id { get; set; }            // 岗位 Id
        public string OrderNumber { get; set; } // 序号
        public string Name { get; set; }        // 岗位名称
        public string SortCode { get; set; }    // 岗位编码

        public List<PositionWorkKPIVM> PositionWorkKPIVMCollection { get; set; }  // 岗位作业绩效指标视图模型集合
    }
}

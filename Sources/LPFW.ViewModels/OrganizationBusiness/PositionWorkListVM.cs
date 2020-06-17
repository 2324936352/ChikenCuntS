using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.ViewModels.OrganizationBusiness
{
    /// <summary>
    /// 处理工作岗位作业列表清单视图模型
    /// </summary>
    public class PositionWorkListVM
    {
        public Guid Id { get; set; }            // 岗位 Id
        public string OrderNumber { get; set; } // 序号
        public string Name { get; set; }        // 岗位名称
        public string SortCode { get; set; }    // 岗位编码

        public List<PositionWorkVM> PositionWorkVMCollection { get; set; }  // 岗位作业视图模型
    }
}

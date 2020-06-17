using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.ViewModels.OrganizationBusiness
{
    /// <summary>
    /// 通用的单位选择器，用于构建 组织-单位 选择的下拉选项，以便在平台营运管理子系统中
    /// 对于各类数据管理所需要的对应单位之下的部门管理的导航处理，构建下拉选项集合的方式：
    ///   1. 遍历 TransactionCenterRegister 转成 OrganizationSelectorVM 对象；
    ///   2. 遍历 Organization 转成 OrganizationSelectorVM 对象，作为子对象，对应
    ///      加入前面的节点之内。
    /// </summary>
    public class OrganizationSelectorVM
    {
        public string TransactionCenterRegisterId { get; set; }
        public string OrganizationId { get; set; }                 // 遍历 TransactionCenterRegister 时，为空字符串
        public string TransactionCenterRegisterName { get; set; }
        public string OrganizationName { get; set; }
        public string DorpdownItemName { get; set; }               // 下拉项的显示名称，如果是 Organization 项，缩进两个中文空格
        public string SortCode { get; set; }
        public bool IsCurrent { get; set; }                        // 是否当前选中
        public string DefaultDepartmentId { get; set; }            // 选中时缺省的归属部门 Id
    }
}

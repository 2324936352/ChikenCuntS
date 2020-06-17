using LPFW.ViewModels.BusinessCommon;
using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.OrganizationBusiness
{
    /// <summary>
    /// 部门视图模型
    /// </summary>
    public class DepartmentVM : EntityViewModel
    {
        [Display(Name = "创建日期")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "更新日期")]
        public DateTime UpdateDate { get; set; }
        [Display(Name = "人数")]
        public int EmployeeAmount { get; set; }
        [Display(Name = "岗位数量")]
        public int PositionAmount { get; set; }

        [Display(Name = "单位")]
        public string OrganizationId { get; set; }
        [Display(Name = "单位")]
        public string OrganizationName { get; set; }
        public List<PlainFacadeItem> OrganizationItemCollection { get; set; }

        [Display(Name = "上级部门")]
        public string ParentId { get; set; }
        [Display(Name = "上级部门")]
        public string ParentName { get; set; }
        public List<SelfReferentialItem> ParentItemCollection { get; set; }

        [Display(Name = "角色组")]
        public string ApplicationRoleId { get; set; }
        [Display(Name = "角色组")]
        public string ApplicationRoleName { get; set; }

        [Display(Name = "联系地址")]
        public CommonAddressVM AddressVM { get; set; }

        public SimplifiedPersonVM LeaderVM { get; set; }   // 负责人
        public SimplifiedPersonVM ContactVM { get; set; }  // 业务联系人
        public List<SelfReferentialItem> DepartmentLinkCollection {   get; set; }  // 部门层次链接元素集合
    }
}

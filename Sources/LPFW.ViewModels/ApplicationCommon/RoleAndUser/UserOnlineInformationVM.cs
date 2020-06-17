using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.ViewModels.ApplicationCommon.RoleAndUser
{
    /// <summary>
    /// 登录用户的在线信息，并且在 IViewModelService 提供相应的访问方法，用于在需要使用用户登录及关联的其它实体数据的元素.
    ///   1. OrganizationId：在获取的时候，对于: 
    ///      Employee  获取的是 LPFW.EntitiyModels.OrganzationBusiness.OperationOrganization.Organization 的 Id；
    ///      PurchaserEmployee 获取的是 CommodityPurchaser 的 Id；
    ///      VenderEmployee 获取的是 CommodityVender 的 Id；
    ///   2. DepartmentId，在获取的时候，对于
    ///      Employee  获取的是 LPFW.EntitiyModels.OrganzationBusiness.OperationOrganization.Department 的 Id；
    ///      PurchaserEmployee 获取的是 PurchaserDepartment 的 Id；
    ///      VenderEmployee 获取的是 VenderDepartment 的 Id；
    /// 在具体的方法中再根据方法处理的对象，获取相应的值，例如在采购订单中，
    /// 肯定会关联下单的用户，在获取这个模型对象数据后，需要自己再处理：
    ///   1. 根据 OrganizationId 获取采购商 CommodityVender 对象；
    ///   2. 根据 DepartmentId 获取采购商的内部部门 PurchaserDepartment 对象；
    ///   3. 根据 Id 获取采购商下单员工 PurchaserEmployee 对象。
    /// </summary>
    public class UserOnlineInformationVM
    {
        public Guid Id { get; set; }               // 登录用户 Id
        public string UserName { get; set; }       // 登录的用户名
        public string Name { get; set; }           // 登录的用户显示名称
        public string MasterRoleId { get; set; }   // 宿主角色组 Id

        public string RegisterId { get; set; }         // 用户归属注册资料 Id

        public string OrganizationId { get; set; }     // 用户归属单位 Id
        public string OrganizationName { get; set; }

        public string DepartmentId { get; set; }       // 用户归属的单位部门 Id
        public string DepartmentName { get; set; } 

        public string PositionId { get; set; }         // 用户归属的单位部门岗位 Id
        public string PositionName { get; set; }       // 

        public string PositionWorkId { get; set; }     // 用户归属的单位部门岗位的主要工作 Id
        public string PositionWorkName { get; set; } 

        public List<RoleItem> RoleItemCollection { get; set; }     // 加载归属角色组基本信息
        public List<ClaimItem> ClaimItemCollection { get; set; }   // 加载归属角色组基本信息

        public RoleCommonClaimEnum RoleCommonClaimEnum { get; set; }

        /// <summary>
        /// 简单的内嵌类，用于承载用户归属的角色组
        /// </summary>
        public class RoleItem
        {
            public Guid RoleID { get; set; }
            public string RoleName { get; set; }
        }

        /// <summary>
        /// 简单的内嵌类，用于获取用户的身份申明
        /// </summary>
        public class ClaimItem 
        {
            public string ClaimName { get; set; }
            public string ClaimValue { get; set; }
        }
    }

    
}

using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.EntitiyModels.ApplicationCommon.RoleAndUser
{
    /// <summary>
    /// 系统角色
    ///   1. 平台系统管理员可以创建和维护任何角色组，具体的管理组方式是以单位导航，然后进入相关的管理；
    ///   2. 任何单位注册的时候，创建一个根角色作为该注册单位的系统管理角色，归属于这个角色的用户即为
    ///      该单位的系统管理员，角色组的 Id 与注册的 Id 一致；
    ///   3. 注册单位创建内部单位的时候，同时创建对应的角色组，角色名称 Name 采用单位名+上级部门+当前
    ///      部门名称，显示名 DisplayName 为当前部门名称。
    /// </summary>
    public class ApplicationRole :IdentityRole<Guid>,IEntity
    {
        [StringLength(500)]
        public string Description { get; set; }     // 部门描述
        [StringLength(200)]
        public string SortCode { get; set; }        // 角色编码
        [StringLength(200)]
        public string DisplayName { get; set; }     // 显示名

        public bool IsPseudoDelete { get; set; }

        public virtual ApplicationRole ParentRole { get; set; }  // 上级用户组

        public ApplicationRole() : base()
        {
            this.Id = Guid.NewGuid();
        }
        public ApplicationRole(string roleName) : base(roleName)
        {
            this.Id = Guid.NewGuid();
            this.Name = roleName;
        }

    }
}

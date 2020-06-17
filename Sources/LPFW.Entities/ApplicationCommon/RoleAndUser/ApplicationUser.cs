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
    /// 系统用户
    ///   1. 可以在独立的管理控制器中处理数据，路径：/Admin/ApplicationUser；
    ///   2. 在各个部门内创建人员时候，缺省创建用户，创建的用户名，目前是人员的工号，未来会根据业务规则再约束自动创建的用户名；
    ///   3. 在创建人员时候，需要将人员加入其所在部门的关联的角色组，并且设置为其核心角色组；
    ///   4. Name（昵称）直接作为用户的显示名，一般就是用户的姓名；
    ///   5. Description 简要描述用户在系统中的工作。
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>, IEntity
    {
        [StringLength(500)]
        public string Name { get; set; }  // 昵称
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(200)]
        public string SortCode { get; set; }
        public bool IsPseudoDelete { get; set; }
        public string Telephone { get; set; }//电话
        public bool Sex { get; set; }//性别
        public DateTime Birthdays { get; set; }//生日

        public string City { get; set; }//城市
        public ApplicationUser() : base()
        {
            this.Id = Guid.NewGuid();
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<ApplicationUser>();
        }
        public ApplicationUser(string userName) : base(userName)
        {
            this.Id = Guid.NewGuid();
            this.UserName = userName;
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<ApplicationUser>();
        }

    }
}

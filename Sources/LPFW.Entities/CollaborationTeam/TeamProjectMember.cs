using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.CollaborationTeam
{
    /// <summary>
    /// 项目团队成员，成员的范围来自创建项目的时候，创建人本人所在的角色关联的部门范围限制
    /// </summary>
    public class TeamProjectMember : EntityBase
    {
        public virtual TeamProject Project { get; set; }   // 项目
        public virtual ApplicationUser User { get; set; }  // 成员

        public TeamProjectMember() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}

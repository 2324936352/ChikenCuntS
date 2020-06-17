using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.TeachingBusiness
{
    public class Course : Entity
    {
        public DateTime OpenDate { get; set; }  // 公开日期
        public DateTime CloseDate { get; set; } // 关闭日期

        public virtual CourseContainer CourseContainer { get; set; }     // 归属的课程大类（容器）
        public virtual ApplicationUser Creator { get; set; }             // 课程创建人
        public virtual ApplicationUser CourseAdministrator { get; set; } // 课程管理员

        public Course()
        {
            this.Id = Guid.NewGuid();
            Name = Description = "";
            OpenDate = CloseDate = DateTime.Now;
        }

    }
}

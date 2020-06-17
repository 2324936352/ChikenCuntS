using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.TeachingBusiness
{
    /// <summary>
    /// 课程结构单元，用于构建教学单元、教学单元的内部结构等
    /// </summary>
    public class CourseItem : Entity
    {
        public DateTime CreateDate { get; set; }  // 创建日期

        public virtual CourseItem Parent { get; set; }                    // 上级结构单元
        public virtual Course Course { get; set; }                        // 归属课程
        public virtual CourseItemContent CourseItemContent { get; set; }  // 单元内容
        public virtual ApplicationUser Creator { get; set; }              // 创建人
        
        public CourseItem()
        {
            this.Id = Guid.NewGuid();
            Name = Description = SortCode = "";
            CreateDate = DateTime.Now;
        }
    }
}

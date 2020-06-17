using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.EntitiyModels.TeachingBusiness
{
    /// <summary>
    /// 课程结构单元的配置内容：
    ///   Name 作为主标题
    ///   Description 作为简要介绍
    /// </summary>
    public class CourseItemContent : Entity
    {
        [StringLength(200)]
        public string SecondTitle { get; set; }         // 副标题
        [StringLength(500)]
        public string HeadContent { get; set; }         // 页眉内容
        [StringLength(500)]
        public string FootContent { get; set; }         // 页脚内容
        public DateTime UpdateDate { get; set; }        // 更新日期
        
        public string BodyContent { get; set; }         // 正文内容

        public virtual ApplicationUser Editor { get; set; }  // 创建人

        public CourseItemContent()
        {
            this.Id = Guid.NewGuid();
            Name = Description = SecondTitle = HeadContent = FootContent = "";
            UpdateDate = DateTime.Now;
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<CourseItemContent>();
        }
    }
}

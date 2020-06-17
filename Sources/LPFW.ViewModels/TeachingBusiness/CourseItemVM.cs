using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.TeachingBusiness
{
    public class CourseItemVM : EntityViewModel
    {
        [Display(Name = "创建日期")]
        public DateTime CreateDate { get; set; }  // 创建日期

        [Display(Name = "上级单元")]
        public string ParentId { get; set; }
        [Display(Name = "上级单元")]
        public string ParentName { get; set; }
        public List<SelfReferentialItem> ParentItemCollection { get; set; }

        public Guid CourseId { get; set; }
        public string CourseName { get; set; }

        public Guid CreatorId { get; set; }
        public string CreatorName { get; set; }

        public CourseItemContentVM CourseItemContentVM { get; set; }  // 单元内容

    }
}

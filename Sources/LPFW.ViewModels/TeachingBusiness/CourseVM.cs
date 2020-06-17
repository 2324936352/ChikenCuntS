using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.TeachingBusiness
{
    public class CourseVM : EntityViewModel
    {
        [Display(Name = "开放日期")]
        public DateTime OpenDate { get; set; } 
        [Display(Name = "关闭日期")]
        public DateTime CloseDate { get; set; } 

        [Display(Name = "课程类型")]
        public string CourseContainerId { get; set; }
        [Display(Name = "课程类型")]
        public string CourseContainerName { get; set; }
        public List<PlainFacadeItem> CourseContainerItemCollection { get; set; }

        public Guid CreatorId { get; set; }
        [Display(Name = "编者")]
        public string CreatorName { get; set; }

        public Guid CourseAdministratorId { get; set; }
        public string CourseAdministratorName { get; set; }

    }
}

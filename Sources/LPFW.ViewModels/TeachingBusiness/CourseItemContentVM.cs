using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.TeachingBusiness
{
    public class CourseItemContentVM : EntityViewModel
    {
        [Required(ErrorMessage = "单元标题不能为空值。")]
        [Display(Name = "单元标题")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制25个字符的长度。")]
        public override string Name { get; set; }

        [Display(Name = "副标题")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制25个字符的长度。")]
        public string SecondTitle { get; set; }

        [Display(Name = "页眉")]
        [StringLength(200, ErrorMessage = "你输入的数据超出限制25个字符的长度。")]
        public string HeadContent { get; set; }

        [Display(Name = "页脚")]
        [StringLength(200, ErrorMessage = "你输入的数据超出限制25个字符的长度。")]
        public string FootContent { get; set; }

        [Display(Name = "更新日期")]
        public DateTime UpdateDate { get; set; }

        [Display(Name = "内容正文")]
        public string BodyContent { get; set; }

        public Guid EditorId { get; set; }    // 创建人
        public string EditorName { get; set; }  // 创建人

        public Guid CourseId { get; set; }
        public string CourseName { get; set; }

        public Guid CourseItemId { get; set; }
        public string CourseItemName { get; set; }

        public Guid CourseContainerId { get; set; }
        public string CourseContainerName { get; set; }
    }
}

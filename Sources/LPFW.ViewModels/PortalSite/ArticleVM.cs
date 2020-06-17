using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.PortalSite
{
    public class ArticleVM : EntityViewModel
    {
        [Required(ErrorMessage = "歌曲名称不能为空值。")]
        [Display(Name = "音乐名称")]
        [StringLength(50, ErrorMessage = "你输入的数据超出限制25个字符的长度。")]
        public override string Name { get; set; }

        [Display(Name = "歌手")]
        [StringLength(50, ErrorMessage = "你输入的数据超出限制25个字符的长度。")]
        public string ArticleSecondTitle { get; set; }

        [Required(ErrorMessage = "内容提要不能为空值。")]
        [Display(Name = "简介")]
        [StringLength(150, ErrorMessage = "你输入的数据超出限制150个字符的长度。")]
        public override string Description { get; set; }

        [Display(Name = "正文")]
        public string ArticleContent { get; set; }

        [Display(Name = "来源说明")]
        [StringLength(150, ErrorMessage = "你输入的数据超出限制150个字符的长度。")]
        public string ArticleSource { get; set; }                 // 文章来源

        public DateTime CreateDate { get; set; }                  // 创建日期
        public DateTime UpdateDate { get; set; }                  // 更新日期
        public DateTime PublishDate { get; set; }                 // 发表日期
        public DateTime CloseDate { get; set; }                   // 关闭日期
        public DateTime OpenDate { get; set; }                    // 公开日期

        [Display(Name = "是否发布")]
        public bool IsPassed { get; set; }                        // 是否发布

        [Required(ErrorMessage = "必须选择音乐类型。")]
        [Display(Name = "类型")]
        public string ArticleTopicId { get; set; }
        [Display(Name = "类型")]
        public string ArticleTopicName { get; set; }
        [Display(Name = "类型")]
        public List<PlainFacadeItem> ArticleTopicItemCollection { get; set; }

        public Guid CreatorUserId { get; set; }
        public string CreatorUserName { get; set; }

    }
}

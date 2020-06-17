using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LPFW.EntitiyModels.PortalSite
{
    /// <summary>
    /// 文章反馈意见
    ///   Name: 评论人别名
    ///   Description: 评论内容摘要
    ///   SortCode:
    /// </summary>
    public class ArticleComment: Entity
    {
        [StringLength(200)]
        public string Title { get; set; }                              // 标题
        [StringLength(1000)]
        public string Comment { get; set; }                           // 评论内容，限制 1000 字符
        public DateTime CommentDate { get; set; }                     // 评论发表时间

        public virtual ArticleComment Parent { get; set; }             // 评论的上一层，如果是与自身一样，则认为是开头话题评论
        public virtual Article MasterArticle { get; set; }             // 关联的文章
        public virtual ApplicationUser CommentWritor { get; set; }     // 评论人

        public ArticleComment()
        {
            this.Id = Guid.NewGuid();
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<ArticleComment>();
        }
    }
}

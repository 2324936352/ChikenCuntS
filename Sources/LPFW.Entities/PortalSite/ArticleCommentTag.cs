using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.EntitiyModels.PortalSite
{
    /// <summary>
    /// 文章反馈意见内容标签定义，在本系统中，标签不允许包含空格等标点符号
    ///   Name: 标签名称
    ///   Description: 标签说明
    /// </summary>
    public class ArticleCommentTag : Entity
    {
        public Int32 RefrenceCount { get; set; }   // 标签引用次数

        public ArticleCommentTag()
        {
            this.Id = Guid.NewGuid();
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<ArticleCommentTag>();
        }

    }
}

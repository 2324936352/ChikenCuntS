using LPFW.EntitiyModels.ApplicationCommon.Attachments;
using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.EntitiyModels.PortalSite
{
    /// <summary>
    /// 文章关联的附件（除了图片、视频等已经规约过类型以外的其他文件）
    ///   Name: 文件标题
    ///   Description: 文件描述
    /// </summary>
    public class ArticleWithFile:Entity
    {
        public virtual Article MasterArticle { get; set; }
        public virtual BusinessFile File { get; set; }

        public ArticleWithFile()
        {
            Id = Guid.NewGuid();
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<ArticleWithFile>();
        }
    }
}

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
    /// 文章关联的视频文件
    ///   Name：视频标题
    ///   Descrption：视频描述
    /// </summary>
    public class ArticleWithVideo : Entity
    {
        public virtual Article MasterArticle { get; set; }
        public virtual BusinessVideo Video { get; set; }

        public ArticleWithVideo()
        {
            Id = Guid.NewGuid();
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<ArticleWithVideo>();
        }
    }
}

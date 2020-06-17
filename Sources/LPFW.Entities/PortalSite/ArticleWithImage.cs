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
    /// 文章管理的图片
    ///   Name: 图片标题
    ///   Description: 图片描述
    ///   
    /// 关于图片置顶处理：如果是置顶图片，则需要在存储时做缩小处理，置顶图片一般在文字的第一自然段下方，
    ///                  其它图片，按照编码次序，在文章末尾排布。 
    /// </summary>
    public class ArticleWithImage:Entity
    {
        public bool IsTop { get; set; }          // 是否是置顶图片

        public virtual Article MasterArticle { get; set; }
        public virtual BusinessImage Image { get; set; }

        public ArticleWithImage()
        {
            Id = Guid.NewGuid();
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<ArticleWithImage>();
        }
    }
}

using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.EntitiyModels.PortalSite
{
    /// <summary>
    /// 关联文章定义
    /// </summary>
    public class ArticleRelevance:EntityBase
    {
        public virtual Article MasterArticle { get; set; }
        public virtual Article RelevanceArticle { get; set; }

        public ArticleRelevance()
        {
            Id = Guid.NewGuid();
        }
    }
}

using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.EntitiyModels.PortalSite
{
    public class ArticleInTopic : EntityBase
    {
        [StringLength(10)]
        public string Name { get; set; }
        [StringLength(50)]
        public string SortCode { get; set; }

        public virtual Article MasterArticle { get; set; }
        public virtual ArticleTopic ArticleTopic { get; set; }

        public ArticleInTopic()
        {
            Id = Guid.NewGuid();
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<ArticleInTopic>();
        }
    }
}

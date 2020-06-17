using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.EntitiyModels.PortalSite
{
    public class ArticleInType:EntityBase
    {
        public virtual Article MasterArticle { get; set; }
        public virtual ArticleType ArticleType { get; set; }

        public ArticleInType()
        {
            Id = Guid.NewGuid();
        }
    }
}

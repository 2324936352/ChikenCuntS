using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.PortalSite
{
    /// <summary>
    /// 文章与标签关系
    /// </summary>
    public class ArticleWithTag : EntityBase
    {
        public Article Article { get; set; }  // 文章
        public ArticleTag Tag { get; set; }   // 标签

        public ArticleWithTag() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}

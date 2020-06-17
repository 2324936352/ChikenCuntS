using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.PortalSite
{
    /// <summary>
    /// 反馈意见与标签关系
    /// </summary>
    public class ArticleCommentWithTag : EntityBase
    {
        public ArticleComment Comment { get; set; }
        public ArticleCommentTag Tag { get; set; }

        public ArticleCommentWithTag() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}

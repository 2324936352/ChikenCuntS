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
    /// 文章关联主题
    /// </summary>
    public class ArticleTopic:Entity
    {
        public virtual BusinessImage TopicImage { get; set; }

        public ArticleTopic()
        {
            this.Id = Guid.NewGuid();
            this.Name = this.Description = "";
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<ArticleTopic>();
        }
    }
}

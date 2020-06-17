using LPFW.EntitiyModels.ApplicationCommon.Attachments;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LPFW.EntitiyModels.PortalSite
{
    /// <summary>
    /// 文章类型
    /// </summary>
    public class ArticleType : Entity
    {
        public virtual ArticleType ParentType { get; set; }          // 上级类型
        public virtual BusinessImage ArticleTypeImage { get; set; }  // 类型图标图片

        public ArticleType()
        {
            Id = Guid.NewGuid();
            Name = Description = "";
        }
    }
}

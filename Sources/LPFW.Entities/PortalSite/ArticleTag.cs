using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.PortalSite
{
    /// <summary>
    /// 文章内容标签定义，在本系统中，标签不允许包含空格等标点符号
    ///   Name: 标签名称
    ///   Description: 标签说明
    /// </summary>
    public class ArticleTag : Entity
    {
        public Int32 RefrenceCount { get; set; }   // 标签引用次数

        public ArticleTag()
        {
            this.Id = Guid.NewGuid();
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<ArticleTag>();
        }
    }
}

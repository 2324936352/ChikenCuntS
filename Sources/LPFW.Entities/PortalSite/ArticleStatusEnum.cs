using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.PortalSite
{
    /// <summary>
    /// 文章状态
    /// </summary>
    public enum ArticleStatusEnum
    {
        创建编辑中,
        已提交审核,
        审核处理中,
        已提交审批,
        审批发布中,
        已发布,
        过期归档,
        已作废
    }
}

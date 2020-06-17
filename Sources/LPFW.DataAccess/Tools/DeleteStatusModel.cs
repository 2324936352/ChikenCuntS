using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LPFW.DataAccess.Tools
{
    /// <summary>
    /// 删除操作结果状态模型
    /// </summary>
    public class DeleteStatusModel
    {
        public bool DeleteSatus { get; set; }  // 是否成功，true 成功
        public string Message { get; set; }    // 操作结果信息，特别要处理如果失败要获取失败信息
    }
}

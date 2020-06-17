using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.DataAccess.Tools
{
    /// <summary>
    /// 业务实体数据操作结果
    /// </summary>
    public class EntityProcessResult
    {
        public bool Succeeded { get; set; }         // 是否成功
        public List<string> Messages { get; set; }  // 操作相关的消息集合
        public object BusinessObject { get; set; }  // 返回的处理的对象
    }
}

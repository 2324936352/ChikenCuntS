using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.DataAccess.Tools
{
    /// <summary>
    /// 保存操作结果模型
    /// </summary>
    public class SaveStatusModel
    {
        public bool SaveSatus { get; set; }  // 是否成功，true 成功
        public string Message { get; set; }  // 操作结果信息，特别要处理如果失败要获取失败信息
    }
}

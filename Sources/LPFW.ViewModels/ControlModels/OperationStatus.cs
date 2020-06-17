using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.ViewModels.ControlModels
{
    /// <summary>
    /// 简单地对前端提交的操作请求处理结果，一般用在前端提交一些类似状态更新的请求，在系统处理之后，
    /// 将处理结果通过json回传，供前端进行判断。
    /// </summary>
    public class OperationStatus
    {
        [JsonProperty("isOK")]
        public bool IsOK { get; set; }             // 操作是否成功
        [JsonProperty("operationValue")]
        public string OperationValue { get; set; } // 操作的结果数据
        [JsonProperty("message")]
        public string Message { get; set; }        // 操作执行的消息
    }
}

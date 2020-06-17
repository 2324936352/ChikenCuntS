using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace LPFW.EntitiyModels.Tools
{
    /// <summary>
    /// 为实体模型的常规处理（初始化）提供的一些静态方法
    /// </summary>
    public static class EntityHelper
    {
        /// <summary>
        /// 提取根据系统时间生成 SortCode 所需要的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string SortCodeByDefaultDateTime<T>()
        {
            var nowTime = DateTime.Now;
            string timeStampString = nowTime.ToString("yyyy-MM-dd-hh-mm-ss-ffff", DateTimeFormatInfo.InvariantInfo);

            var entityName = typeof(T).Name;
            string result = entityName + "_" + timeStampString;
            return result;
        }
    }
}

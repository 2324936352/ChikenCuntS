using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.DataAccess.Tools
{
    /// <summary>
    /// 分页器中当前的第一页及后一页
    /// </summary>
    public class PagenateGroupItem
    {
        public int FirstIndex { get; set; }
        public int LastIndex { get; set; }
    }
}

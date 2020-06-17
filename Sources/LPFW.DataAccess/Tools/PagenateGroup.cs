using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.DataAccess.Tools
{
    /// <summary>
    /// 分页器分组信息
    /// </summary>
    public class PagenateGroup
    {
        public PagenateGroupItem PagenateGroupItem { get; set; }
        public int PageAmount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public bool HasPreviousItem { get; set; }
        public bool HasNextItem { get; set; }
    }
}

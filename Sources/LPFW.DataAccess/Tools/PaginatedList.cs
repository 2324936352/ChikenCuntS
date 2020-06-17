using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPFW.DataAccess.Tools
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPageCount { get; private set; }

        public PaginatedList() { }

        public PaginatedList(int pageIndex, int pageSize, int totalCount, IQueryable<T> source)
        {
            AddRange(source);

            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPageCount = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        public bool HasPreviousPage  // 是否有前一页
        {
            get { return (PageIndex > 1); }
        }

        public bool HasNextPage // 是否有后一页
        {
            get { return (PageIndex < TotalPageCount); }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPFW.DataAccess.Tools
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// 针对 IQueryable<T>的扩展方法，用于提取分页数据对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            var totalCount = query.Count();
            if (pageSize == 0)
            {
                pageSize = query.Count();
            }
            var collection = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return new PaginatedList<T>(pageIndex, pageSize, totalCount, collection);
        }

        /// <summary>
        /// 与上面相同的处理，只是采用异步方式实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            var totalCount = await query.CountAsync();
            if (pageSize == 0)
            {
                pageSize = await query.CountAsync();
            }
            var collection = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return new PaginatedList<T>(pageIndex, pageSize, totalCount, collection);
        }
    }
}

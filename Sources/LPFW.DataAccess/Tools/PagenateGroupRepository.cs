using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.DataAccess.Tools
{
    public class PagenateGroupRepository
    {
        /// <summary>
        /// 根据传入的分割后的泛型数据集合，构建一个供前端分页组件使用的模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">数据源</param>
        /// <param name="counter">在分页器上显示页面导航按钮的个数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        public static PagenateGroup GetItem<T>(PaginatedList<T> source, int counter, int pageIndex)
        {
            var pgCollection = _GetCollection<T>(source, counter);
            var selectGroup = new PagenateGroup()
            {
                PageIndex = pageIndex,
                PageAmount = source.TotalPageCount,
                HasNextItem = source.HasNextPage,
                HasPreviousItem = source.HasPreviousPage,
                PageSize = source.PageSize
            };
            foreach (var item in pgCollection)
            {
                if (pageIndex >= item.FirstIndex && pageIndex <= item.LastIndex)
                {
                    selectGroup.PagenateGroupItem = item;
                }
            }
            return selectGroup;
        }

        /// <summary>
        /// 根据分页器所需要每一组显示的导航元素个数，将页码划分为相应的组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="counter"></param>
        /// <returns></returns>
        private static List<PagenateGroupItem> _GetCollection<T>(PaginatedList<T> source, int counter)
        {
            var pgCollection = new List<PagenateGroupItem>();

            int groupAmount = 0;               // 分组个数
            if (source.TotalPageCount % counter == 0)
            {
                groupAmount = source.TotalPageCount / counter;
            }
            else
            {
                groupAmount = source.TotalPageCount / counter + 1;
            }

            for (int i = 0; i < groupAmount; i++)
            {
                var firstIndex = (i * counter) + 1;
                var lastIndex = ((i + 1) * counter);
                if (lastIndex > source.TotalPageCount)
                    lastIndex = source.TotalPageCount;

                pgCollection.Add(new PagenateGroupItem() { FirstIndex = firstIndex, LastIndex = lastIndex });

            }
            return pgCollection;
        }
    }
}

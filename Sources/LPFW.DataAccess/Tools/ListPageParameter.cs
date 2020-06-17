using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LPFW.DataAccess.Tools
{
    /// <summary>
    /// 分页器，用于为前端列表页进行数据分页列表时的常规属性处理与转换
    /// </summary>
    public class ListPageParameter
    {
        public string TypeID { get; set; }           // 对应的类型ID
        public string TypeName { get; set; }         // 对应的类型名称
        public string PageIndex { get; set; }        // 当前页码
        public string PageSize { get; set; }         // 每页数据条数 为"0"时显示所有
        public string PageAmount { get; set; }       // 相关对象列表分页处理分页数量
        public string ObjectAmount { get; set; }     // 相关的对象的总数
        public string Keyword { get; set; }          // 当前的关键词
        public string SortProperty { get; set; }     // 排序属性
        public string SortDesc { get; set; }         // 排序方向，缺省值正向 Default，前端用开关方式转为逆向：Descend
        public string SelectedObjectID { get; set; } // 当前页面处理中处理的焦点对象 ID
        public bool IsSearch { get; set; }           // 当前是否为检索

        public string OtherProperty01 { get; set; }  // 附加属性01，供前端在视图和控制器之间摆渡需要的自定义数据
        public string OtherProperty02 { get; set; }  // 附加属性02，供前端在视图和控制器之间摆渡需要的自定义数据
        public string OtherProperty03 { get; set; }  // 附加属性03，供前端在视图和控制器之间摆渡需要的自定义数据

        public PagenateGroup PagenateGroup { get; set; }  // 分页器

        public bool HasPreviousPage // 是否有前一页
        {
            get
            {
                int pageIndex;
                int.TryParse(PageIndex, out pageIndex);

                return (pageIndex > 1);
            }
        }

        public bool HasNextPage     // 是否有后一页
        {
            get
            {
                int pageIndex;
                int.TryParse(PageIndex, out pageIndex);
                int pageAmount;
                int.TryParse(PageAmount, out pageAmount);

                return (pageIndex < pageAmount);
            }
        }

        public ListPageParameter()
        {
            TypeID           = "";
            TypeName         = "";
            PageIndex        = "1";
            PageSize         = "18";
            Keyword          = "";
            SortProperty     = "SortCode";
            SortDesc         = "default";
            SelectedObjectID = "";
            IsSearch         = false;
            PageAmount       = "0";
            ObjectAmount     = "0";
            OtherProperty01  = "";
            OtherProperty02  = "";
            OtherProperty03  = "";
        }

        public ListPageParameter(int pageIndex, int pageSize)
        {
            TypeID           = "";
            PageIndex        = pageIndex.ToString();
            PageSize         = pageSize.ToString();
            Keyword          = "";
            SortProperty     = "SortCode";
            SortDesc         = "default";
            SelectedObjectID = "";
            IsSearch         = false;
            PageAmount       = "0";
            ObjectAmount     = "0";
            OtherProperty01  = "";
            OtherProperty02  = "";
            OtherProperty03  = "";
        }

    }
}

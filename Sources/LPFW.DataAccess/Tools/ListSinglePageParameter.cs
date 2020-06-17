using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.DataAccess.Tools
{
    /// <summary>
    /// 用于在单页数据显示的列表条件约束
    /// </summary>
    public class ListSinglePageParameter
    {
        public string TypeID { get; set; }           // 对应的类型ID
        public string TypeName { get; set; }         // 对应的类型名称
        public string Keyword { get; set; }          // 当前的关键词
        public string SortProperty { get; set; }     // 排序属性
        public string SortDesc { get; set; }         // 排序方向，未排序：""，升序："Ascend",降序："Descend"
        public string SelectedObjectID { get; set; } // 当前页面处理中处理的焦点对象 ID
        public bool IsSearch { get; set; }           // 当前是否为检索
        public string OtherProperty01 { get; set; }  // 附加属性01，供前端在视图和控制器之间摆渡需要的自定义数据
        public string OtherProperty02 { get; set; }  // 附加属性02，供前端在视图和控制器之间摆渡需要的自定义数据
        public string OtherProperty03 { get; set; }  // 附加属性03，供前端在视图和控制器之间摆渡需要的自定义数据

        public ListSinglePageParameter()
        {
            TypeID           = "";
            TypeName         = "";
            Keyword          = "";
            SortProperty     = "SortCode";
            SortDesc         = "";
            SelectedObjectID = "";
            IsSearch         = false;
            OtherProperty01  = "";
            OtherProperty02  = "";
            OtherProperty03  = "";
        }
    }
}

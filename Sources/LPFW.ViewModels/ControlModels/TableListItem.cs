using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.ViewModels.ControlModels
{
    /// <summary>
    /// 列表表头元素定义
    /// </summary>
    public class TableListItem
    {
        public string PropertyName { get; set; }  // 属性名称
        public string DsipalyName { get; set; }   // 属性名称
        public int Width { get; set; }            // 列表宽度
        public bool IsSort { get; set; }          // 是否用于排序
        public string SortDesc { get; set; }      // 排序方向，未排序：""，升序："Ascend",降序："Descend"
        public ViewModelDataType DataType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.ViewModels.ControlModels
{
    /// <summary>
    /// 明细数据视图数据清单条目定义
    /// </summary>
    public class DetailItem
    {
        public string PropertyName { get; set; }
        public ViewModelDataType DataType { get; set; }
    }
}

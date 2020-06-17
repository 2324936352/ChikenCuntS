using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.ViewModels.ControlModels
{
    /// <summary>
    /// 编辑数据视图数据清单条目定义
    /// </summary>
    public class CreateOrEditItem
    {
        public string PropertyName { get; set; }          // 属性名称
        public string TipsString { get; set; }            // 输入提示说明    
        public ViewModelDataType DataType { get; set; }   // 数据类型
        public bool IsPlainText { get; set; }             // 是否以平文字显示
        public bool IsReadonly { get; set; }              // 是否不允许编辑
        public bool IsReadonlyPlainText { get; set; }     // 是否不允许编辑且以平文字显示
    }
}

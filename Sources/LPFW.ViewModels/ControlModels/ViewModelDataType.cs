using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.ViewModels.ControlModels
{
    /// <summary>
    /// 定义视图模型属性的常规种类
    /// </summary>
    public enum ViewModelDataType
    {
        单行文本,
        多行文本,
        密码,
        日期,
        日期时间,
        性别,
        是否,
        普通下拉单选一,
        层次下拉单选一,
        枚举下拉单选一,
        Select多选,
        Select层次多选,
        CheckBox多选,
        富文本,
        图标,
        联系地址,
        货币,
        隐藏
    }
}

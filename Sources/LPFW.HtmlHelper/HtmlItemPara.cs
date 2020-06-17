using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.HtmlHelper
{
    /// <summary>
    /// 简单的用于对视图模型属性元素数据过渡处理的变量封装的类，主要用 CreateOrEdit 元素，
    /// 一般都是通过对视图模型的属性进行处理之后获取具体的数据，用于在 htmlhelper 扩展方
    /// 法中生成相应的 html 代码。
    /// </summary>
    public class HtmlItemPara
    {
        public string ItemDisplay { get; set; }        // 属性显示名
        public string Errorclass { get; set; }         // 属性校验错误套用的 CSS 类名称
        public string ItemName { get; set; }           // 属性名称，用于 html 元素的 id 和 name 的赋值
        public string PlaceholderString { get; set; }  // 属性数据处理提示水印文字
        public object ItemValue { get; set; }          // 属性的值
        public string ErrorTip { get; set; }           // 属性数据校验出错的提示文字
        public string DescValue { get; set; }          // 属性的描述性提示文字，用于在编辑数据的控件下一行显示
        public bool IsPlainText { get; set; }          // 是否以平文字显示
        public bool IsReadonly { get; set; }           // 是否关闭编辑
        public bool IsReadonlyPlainText { get; set; }  // 是否关闭编辑且已平文字显示

    }
}

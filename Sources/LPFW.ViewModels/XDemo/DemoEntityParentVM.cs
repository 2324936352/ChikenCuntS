using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.Demo
{
    public class DemoEntityParentVM:EntityViewModel
    {
        /*
         * 在类似下面的对于实体模型映射关联属性的命名，需要创建三个遵守以下规则：
         *   1. 关联对象的键值 Id 映射到字符串，命名规则：属性名+Id，其作用是在和实体模型互相转换时，通过这个值关联对应的关联；
         *   2. 关联对象的 Name 映射到字符串，命名规则：属性名+Name，其作用是在需要显示关联对象的信息时候使用，例如通过链接进行相关操作；
         *   3. 关联对象的集合映射到 List<SelfReferentialItem>(层次结构) / List<PlainFacadeItem>（平面结构）集合中，
         *      命名规则：属性名：ItemItemCollection，其作用是是在编辑视图模型数据时，提供相关选项清单。
         * 以上规则需要严格遵守。
         */
        [Display(Name = "上级节点")]
        public string ParentId { get; set; }
        [Display(Name = "上级节点")]
        public string ParentName { get; set; }
        public List<SelfReferentialItem> ParentItemCollection { get; set; }
    }
}

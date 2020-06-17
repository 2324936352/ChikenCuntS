using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.Demo
{
    /// <summary>
    /// 附加的修饰类，用于修饰或者限定核心实体 DemoEntity 的特性、行为范围等，例如：
    ///   1. 描述 DemoEntity 的地址；
    ///   2. 描述 DemoEntity 的拥有者。
    /// </summary>
    public class DemoCommon : Entity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DemoCommon()
        {
            this.Id = Guid.NewGuid();
        }
    }
}

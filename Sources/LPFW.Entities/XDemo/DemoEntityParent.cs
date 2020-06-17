using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.Demo
{
    /// <summary>
    /// 核心实体的容器，为核心实体提供存在的环境，也可以理解成核心实体的归属对象或者拥有者，例如：
    ///   1. 如果将核心实体看成是人员，这个实体就可以看成是单位或部门，即单位和部门是人员的存在环境；
    ///   2. 如果将核心实体看成是订单，这个实体可以看成是订单商家。
    /// </summary>
    public class DemoEntityParent:Entity
    {
        public virtual DemoEntityParent Parent { get; set; }  // 上级对象

        public DemoEntityParent()
        {
            this.Id = Guid.NewGuid();
        }
    }
}

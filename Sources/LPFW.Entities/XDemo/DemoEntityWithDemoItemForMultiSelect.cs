using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.Demo
{
    /// <summary>
    /// 演示的关联约束类，负责建立和约束 DemoEntity 和 DemoItemForMultiSelect 之间的多对多的关联关系。
    /// </summary>
    public class DemoEntityWithDemoItemForMultiSelect:EntityBase
    {
        public virtual DemoEntity DemoEntity { get; set; }
        public virtual DemoItemForMultiSelect DemoItemForMultiSelect { get; set; }

        public DemoEntityWithDemoItemForMultiSelect()
        {
            this.Id = Guid.NewGuid();
        }
    }
}


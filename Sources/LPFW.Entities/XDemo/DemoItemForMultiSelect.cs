using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.Demo
{
    /// <summary>
    /// 用于演示与核心实体 DemoEntity 具有多对多关联关系的简单的模拟实体
    /// </summary>
    public class DemoItemForMultiSelect : Entity
    {
        public DemoItemForMultiSelect() 
        {
            this.Id = Guid.NewGuid();
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<DemoItemForMultiSelect>();

        }
    }
}

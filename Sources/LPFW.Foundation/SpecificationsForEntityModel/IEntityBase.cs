using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.Foundation.SpecificationsForEntityModel
{
    /// <summary>
    /// 实体模型基类规格接口，框架中直接采用 guid 作为键值属性
    /// </summary>
    public interface IEntityBase
    {
        Guid Id { get; set; }
    }
}

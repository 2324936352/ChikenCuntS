using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.Foundation.SpecificationsForEntityModel
{
    /// <summary>
    /// 实体模型基本规格接口定义
    /// </summary>
    public interface IEntity : IEntityBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 简要说明
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// 业务编码，如果用到业务编码，最好在前端视图模型中约束输入校验格式，否则使用 
        ///    LPFW.EntitiyModels.Tools.UtilitiesForEntity.SortCodeByDefaultDateTime
        ///    方法初始化。
        /// 这个属性是数据排序处理的缺省属性 
        /// </summary>
        string SortCode { get; set; }

        /// <summary>
        /// 删除操作时是否应用伪删除，一般情况下，使用伪删除，意味着应用中有一个垃圾箱的功能，可以应用恢复和清空操作。
        /// </summary>
        bool IsPseudoDelete { get; set; }
    }
}

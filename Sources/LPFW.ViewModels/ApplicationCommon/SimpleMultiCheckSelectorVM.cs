using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.ViewModels.ApplicationCommon
{
    /// <summary>
    /// 用于加载和传输多选方式回传的数据
    /// </summary>
    public class SimpleMultiCheckSelectorVM
    {
        public Guid MasterId { get; set; }                                  // 关联的 Entity 对象的 Id
        public string[] TobeSelectPlainFacadeItemIdCollection { get; set; } // 选中项的 Id 值的集合

        public Guid TypeId { get; set; }
        public bool IsSaved { get; set; }
        public string TypeName { get; set; }
    }
}

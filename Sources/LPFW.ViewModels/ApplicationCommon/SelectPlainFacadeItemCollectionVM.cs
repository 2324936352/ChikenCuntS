using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.ViewModels.ApplicationCommon
{
    /// <summary>
    /// 用于呈现两端多选多的前端视图模型，两边都采用 input type="checkbox" 处理勾选
    /// </summary>
    public class SelectPlainFacadeItemCollectionVM
    {
        public Guid MasterId { get; set; }  // 关联的 Entity 对象的 Id
        public string[] TobeSelectPlainFacadeItemIdCollection { get; set; }
        public List<PlainFacadeItem> TobeSelectPlainFacadeItemCollection { get; set; }

        public string[] SelectedPlainFacadeItemIdCollection { get; set; }
        public List<PlainFacadeItem> SelectedPlainFacadeItemCollection { get; set; }
    }
}

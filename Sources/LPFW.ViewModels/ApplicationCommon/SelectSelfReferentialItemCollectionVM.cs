using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.ViewModels.ApplicationCommon
{
    public class SelectSelfReferentialItemCollectionVM
    {
        public Guid MasterId { get; set; }  // 关联的 Entity 对象的 Id

        public string[] SelectedSelfReferentialItemIdCollection { get; set; }                   // 之前已经关联的元素的 Id 数组
        public List<SelfReferentialItem> SelectedSelfReferentialItemCollection { get; set; }    // 之前已经关联的元素映射成 SelfReferentialItem 集合

        public string[] ToBeSelectSelfReferentialItemIdCollection { get; set; }                   // 待选关联的元素的 Id 数组
        public List<SelfReferentialItem> ToBeSelectSelfReferentialItemCollection { get; set; }  // 待选的，处理时还是全部选出，只是如果存在已选的，列表条目 disable

    }
}

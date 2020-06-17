
using LPFW.ViewModels.ControlModels;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.MusicViewModel
{
    public class MusicTypeViewModel:EntityViewModel
    {

        public string MusicTypeName { get; set; }//音乐类型名称
        public string MusicTypeId { get; set; }//音乐类型ID

        public List<SelfReferentialItem> MusicTypeItemCollection { get; set; }

        //[Display(Name = "样式种类")]
        //[Required(ErrorMessage = "样式种类是必须选择的。")]
        //public MusicTypeEnum MusicTypeEnum { get; set; }
        //public string MusicTypeEnumName { get; set; }
        //public List<PlainFacadeItem> MusicTypeEnumItemCollection { get; set; }

        //[Display(Name = "音乐ID（父类）")]
        //[Required(ErrorMessage = "上级类型必须选择的。")]
        //public string MusicId { get; set; }
        //[Display(Name = "音乐名字（父类）")]
        //public string MusicName { get; set; }
        //[Display(Name = "音乐集合（父类）")]
        //public List<SelfReferentialItem> MusicItemCollection { get; set; }



        //[Display(Name = "规格配置")]
        //public string[] MusicTypeItemForMultiSelectId { get; set; }
        //[Display(Name = "规格配置")]
        //public string[] MusicTypeItemForMultiSelectName { get; set; }
        //public List<PlainFacadeItem> MusicTypeItemForMultiSelectItemCollection { get; set; }

        //public List<MusicItemVM> MusicItemVMCollection { get; set; }  // 用于模拟订单数据处理的数据
    }
}

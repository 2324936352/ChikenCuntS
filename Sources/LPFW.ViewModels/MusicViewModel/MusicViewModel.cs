using LPFW.EntitiyModels.MusicEntity;
using LPFW.ViewModels.ControlModels;
using System;
using LPFW.ViewModels.MusicViewModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.MusicViewModel
{
    public class MusicViewModel : EntityViewModel
    {


        [Required(ErrorMessage = "歌手名称不能为空！")]
        [Display(Name = "歌手名称")]
        [StringLength(20, ErrorMessage = "歌手名称过长")]
        public string SingerName { get; set; }

        //[Required(ErrorMessage = "专辑不能为空！")]
        //[Display(Name = "专辑")]
        //[StringLength(20, ErrorMessage = "专辑")]
        //public Album Album { get; set; }

        //[Required(ErrorMessage = "音乐类型不能为空！")]
        //[Display(Name = "音乐类型")]
        //[StringLength(20, ErrorMessage = "音乐类型")]
        //public MusicTypeEntity MusicType { get; set; }

        public string MusicTypeId { get; set; }

        public string MusicTypeName { get; set; }
        public List<SelfReferentialItem> MusicTypeItemCollection { get; set; }

    }
}

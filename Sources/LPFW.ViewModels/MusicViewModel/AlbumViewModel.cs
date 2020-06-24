using LPFW.EntitiyModels.MusicEntity;
using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.MusicViewModel
{
    public class AlbumViewModel:EntityViewModel
    {
        [Required(ErrorMessage = "音乐专辑名称不能为空！")]
        [Display(Name = "音乐专辑名称")]
        [StringLength(20, ErrorMessage = "音乐专辑名称过长")]
        public string Album { get; set; }

        [Required(ErrorMessage = "音乐不能为空！")]
        [Display(Name = "音乐")]
        [StringLength(20, ErrorMessage = "音乐")]
        public string Music { get; set; }
        [Display(Name="歌手名称")]
        public List<string> SingerName { get; set; }
        [Display(Name = "发行时间")]
        [Required]
        public DateTime IssueTime { get; set; }
        [Display(Name = "图片路径")]
        public string PhotoUrl { get; set; }
        [Display(Name = "价格")]
        [Required(ErrorMessage = "请输入数字")]
        [RegularExpression(@"^\d{1,10}(\.\d{1,2})?$",ErrorMessage = "请输入数字")]
        public decimal Price { get; set; }

        //public virtual MusicEntity MusicId { get; set; }//音乐上级数据
        public string MusicTypeId { get; set; }
        public string MusicTypeName { get; set; }
      
        public string MusicId { get; set; }
        public string MusicName { get; set; }
        public List<SelfReferentialItem> AlbumItemCollection { get; set; }
    }
}

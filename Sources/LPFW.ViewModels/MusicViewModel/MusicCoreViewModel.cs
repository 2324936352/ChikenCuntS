using LPFW.EntitiyModels.MusicUIEntity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LPFW.ViewModels.MusicViewModel
{
    public class MusicCoreViewModel
    {

        
        [Required(ErrorMessage = "请输入歌名"), MaxLength(50, ErrorMessage = "长度不能超过50个字符")]
        [Display(Name = "歌曲名称")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "音乐类型")]
        public MusicTypeEnum? TypeName { get; set; }
        [Display(Name = "歌手名称")]
        public string SingerName { get; set; }

        [Display(Name = "歌曲文件")]
        public List<IFormFile> MusicPath { get; set; }

        [Display(Name = "歌词文件")]
        public List<IFormFile> lyricPath { get; set; }

        [Display(Name="歌曲图片")]
        public List<IFormFile> Photos { get; set; }

    }
}

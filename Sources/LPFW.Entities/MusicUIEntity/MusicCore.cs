using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LPFW.EntitiyModels.MusicUIEntity
{
    public class MusicCore
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "请输入歌名"), MaxLength(50, ErrorMessage = "长度不能超过50个字符")]
        [Display(Name = "歌曲名称")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "音乐类型")]
        public MusicTypeEnum? TypeName { get; set; }

        [Display(Name = "歌手名称")]
        public string SingerName { get; set; }

        [Display(Name = "歌曲图片")]
        public string PhotoPath { get; set; }

        [Display(Name = "歌曲文件")]
        public string MusicPath { get; set; }

        [Display(Name = "歌词文件")]
        public string lyricName { get; set; }









    }
}

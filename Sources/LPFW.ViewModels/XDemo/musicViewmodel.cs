using LPFW.EntitiyModels.Demo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.XDemo
{
    public class MusicViewmodel:EntityViewModel
    {
        [Required(ErrorMessage = "任务承担人名称不能为空值。")]
        [Display(Name = "任务承担人名称")]
        [StringLength(10, ErrorMessage = "你输入的数据超出限制10个字符的长度。")]
        public string UndertakerName { get; set; }

        [Display(Name = "结束时间")]
        [Required(ErrorMessage = "结束时间是必须选择的。")]
        [DataType(DataType.DateTime, ErrorMessage = "日期时间数据格式错误。")]
        public DateTime EndTime { get; set; }

        [Display(Name = "是否完成")]
        public bool IsFinished { get; set; }
    }
}

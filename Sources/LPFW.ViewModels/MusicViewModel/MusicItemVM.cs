using LPFW.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.ViewModels.MusicViewModel
{
    /// <summary>
    /// 音乐订单条目
    /// </summary>
    public class MusicItemVM:EntityViewModel
    {
        public string MusicId { get; set; }//订单音乐ID

        public string MusicName { get; set; }//订单音乐名称
        
    }
}

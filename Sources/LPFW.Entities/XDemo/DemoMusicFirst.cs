using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.XDemo
{
   public  class DemoMusicFirst
    {
        public int MusicId { get; set; }//音乐Id
        public string  MusicName { get; set; }//音乐名称
        public string  AlbumName { get; set; }//专辑名称
        public DateTime GroundingTime { get; set; }//上架时间
        public DateTime UnderTime { get; set; }//下架时间
        public string MusicPath { get; set; }//歌曲路径
    }
}

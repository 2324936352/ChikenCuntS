using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.MusicEntity
{
    /// <summary>
    /// 音乐
    /// 
    /// 
    /// </summary>
   public  class MusicEntity:Entity
    {
        
        public string SingerName { get; set; }//歌手名称

        //public string MusicName { get; set; }//音乐名称

        //public virtual Album Album { get; set; }//专辑

        public virtual MusicTypeEntity MusicType { get; set; }//音乐类型

        public MusicEntity()
        {
            this.Id = Guid.NewGuid();
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<MusicEntity>();
        }
       
    }
}

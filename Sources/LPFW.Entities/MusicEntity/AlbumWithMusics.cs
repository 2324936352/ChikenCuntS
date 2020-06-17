using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.MusicEntity
{
    /// <summary>
    /// album和Musi多对多的关联
    /// 一张专辑可以有多首歌，一首歌可以出现在多张专辑
    /// </summary>
   public  class AlbumWithMusics:EntityBase
    {
        public virtual Album Album { get; set; }

        public virtual MusicTypeEntity Music { get; set; }

        public AlbumWithMusics()
        {
            this.Id = Guid.NewGuid();
        }
    }
}

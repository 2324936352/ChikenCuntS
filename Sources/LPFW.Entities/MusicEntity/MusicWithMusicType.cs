using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.MusicEntity
{
   public  class MusicWithMusicType:EntityBase
    {
        public virtual MusicTypeEntity MusiType { get; set; }
        public virtual MusicEntity Music { get; set; } //音乐

        public MusicWithMusicType()
        {
            this.Id = Guid.NewGuid();
        }
    }
}

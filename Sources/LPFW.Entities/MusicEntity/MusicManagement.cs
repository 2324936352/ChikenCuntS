using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.MusicEntity
{
    /// <summary>
    /// 音乐管理
    /// </summary>
    public class MusicManagement:Entity
    {
        public string GroundingTime { get; set; }

        public virtual Album Album { get; set; }
        public virtual MusicEntity Music { get; set; }
       
        
        public MusicManagement()
        {
            this.Id = Guid.NewGuid();
        }
    }
}

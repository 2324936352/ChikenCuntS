using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.MusicEntity
{
    public  class AdminWhthMusicManagement:EntityBase
    {
        public virtual Admin Admin { get; set; }
        public virtual MusicManagement MusicManagements { get; set; }

        public AdminWhthMusicManagement()
        {
            this.Id = Guid.NewGuid();
        }
    }
}

using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.MusicEntity
{
    /// <summary>
    /// 专辑
    /// </summary>
   public  class Album:Entity
    {
        public string SingerName { get; set; }//歌手名称

        public DateTime IssueTime { get; set; }//发行时间

        public decimal Price { get; set; }//价格

        public string PhotoUrl { get; set; }//专辑图片路径

       

        public virtual Album MusicEntity { get; set; }
        public Album()
        {
            this.Id = Guid.NewGuid();
            IssueTime = DateTime.Now;
        }
                      
    }
}

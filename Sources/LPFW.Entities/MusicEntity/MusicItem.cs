using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LPFW.EntitiyModels.MusicEntity
{
    /// <summary>
    /// 属于 Music 的条目，类似如果把 Music 看成是订单，这个就可以看成是订单条目
    /// </summary>
    public class MusicItem:Entity
    {
        public int Amount { get; set; }
        [Column(TypeName = "decimal(18, 6)")]
        public decimal UnitPrice { get; set; }
        [Column(TypeName = "decimal(18, 6)")]
        public decimal SubTotal { get; set; }

        public virtual MusicTypeEntity Music { get; set; }



        public MusicItem()
        {
            this.Id = Guid.NewGuid();
            SortCode = EntityHelper.SortCodeByDefaultDateTime<MusicItem>();
        }
    }
}

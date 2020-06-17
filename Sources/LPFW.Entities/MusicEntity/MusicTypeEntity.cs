
using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.MusicEntity
{
    /// <summary>
    /// 音乐类型实体
    /// </summary>
    public class MusicTypeEntity : Entity
    {
        //public virtual Music01 Music { get; set; } //音乐
        public virtual MusicTypeEntity MusicType{ get; set; }//音乐类型上级数据

        //public virtual Music01 Music01 { get; set; }//父类音乐专辑
        //public virtual MusicManagement MusicManagement { get; set; }//音乐上下架管理

        //public virtual MusicTypeEnum MusicTypeEnum { get; set; }//音乐类型管理枚举
        //public virtual MusicCommon MusicCommon { get; set; }   // 其它的附加修饰属性
        public MusicTypeEntity()
        {
            this.Id = Guid.NewGuid();
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<MusicTypeEntity>();
        }
    }
    
}

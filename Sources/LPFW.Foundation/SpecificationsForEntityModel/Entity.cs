using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.Foundation.SpecificationsForEntityModel
{
    /// <summary>
    /// 接口 IEntity 的实现
    /// </summary>
    public abstract class Entity : IEntity
    {
        [Key]
        public Guid Id { get; set; }              // 实体模型对象 Id（唯一的）
        [StringLength(100)]
        public string Name { get; set; }          // 实体模型对象对象名称 
        [StringLength(500)]
        public string Description { get; set; }   // 实体模型对象简要描述
        [StringLength(100)]
        public string SortCode { get; set; }      // 实体模型对象业务编码
        public bool IsPseudoDelete { get; set; }  // 删除处理模式
    }
}

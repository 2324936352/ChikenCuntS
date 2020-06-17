using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LPFW.EntitiyModels.Demo
{
    /// <summary>
    /// 核心实体，演示用的，在理解这个实体的时候，可以把它看成是一份订单模型。
    ///   除了演示实体关系以外，附加的目的是示例各种属性在前端的实现。
    /// </summary>
    public class DemoEntity : Entity
    {
        [StringLength(100)]
        public string Email { get; set; }                               // 电子邮件
        [StringLength(20)]
        public string Mobile { get; set; }                              // 移动电话
        [StringLength(20)]
        public string Password { get; set; }                            // 密码
        public bool Sex { get; set; }                                   // 性别
        public DateTime OrderDateTime { get; set; }                     // 下单时间
        public bool IsFinished { get; set; }                            // 完成状态
        public int Amount { get; set; }                                 // 数量
        [Column(TypeName = "decimal(18, 6)")]                           // 针对计算精度特别约束对应数据库表的字段类型定义
        public decimal Total { get; set; }                              // 金额
        public string HtmlContent { get; set; }                         // 富文本内容，用于演示富文本的编辑处理，在有需要使用富文本编辑内容时，需要使用与此相同的属性名

        public virtual DemoEntityParent DemoEntityParent { get; set; }  // 归属的上级类型
        public virtual DemoEntityEnum DemoEntityEnum { get; set; }      // 枚举，有限数量的修饰属性
        public virtual DemoCommon DemoCommon { get; set; }              // 其它的附加修饰属性

        public DemoEntity()
        {
            this.Id = Guid.NewGuid();
            this.OrderDateTime = DateTime.Now;
            
            // 如果 SortCode 本身需要赋予业务作用的话，则不采用下面的赋值方式，而应该在实际处理的时候进行赋值
            // 例如如果把这个看成是订单号，则需要根据业务特性进行赋值
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<DemoEntity>();
        }
    }
}

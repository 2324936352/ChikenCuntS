using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.Demo
{
    /// <summary>
    /// 模拟的订单条目，这里是以 DemoEntity 作为订单核心数据
    /// </summary>
    public class DemoEntityItemVM:EntityViewModel
    {
        [Display(Name = "商品名")]
        [Required(ErrorMessage = "商品名称不能为空。")]
        public override string Name { get; set; }

        [Display(Name = "商品数量")]
        [Required(ErrorMessage = "请确定商品数量")]
        public int Amount { get; set; }

        [Display(Name = "参考单价")]
        [Required(ErrorMessage = "参考单价是必须的")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "商品小计")]
        [DataType(DataType.Currency)]
        public decimal SubTotal { get; set; }

        public string DemoEntityId { get; set; }    // 归属订单 Id
        public string DemoEntityName { get; set; }  // 归属订单名称

    }
}

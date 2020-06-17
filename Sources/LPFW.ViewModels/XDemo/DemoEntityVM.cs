using LPFW.EntitiyModels.Demo;
using LPFW.ViewModels.BusinessCommon;
using LPFW.ViewModels.Common;
using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text;

namespace LPFW.ViewModels.Demo
{
    public class DemoEntityVM : EntityViewModel
    {
        // 简单说明一下重写继承自 EntityViewModel 的写法
        [Required(ErrorMessage = "姓名不能为空值。")]
        [Display(Name = "姓名")]
        [StringLength(10, ErrorMessage = "你输入的数据超出限制10个字符的长度。")]
        public override string Name { get; set; }

        [Required(AllowEmptyStrings=true)]
        public override string SortCode { get; set; }

        [Display(Name = "电子邮件")]
        [Required(ErrorMessage = "电子邮件数据是必须的。")]
        [EmailAddress(ErrorMessage = "请输入合法的电子邮件地址。")]
        public string Email { get; set; }

        [Display(Name = "移动电话")]
        [RegularExpression(@"((^13[0-9]{1}[0-9]{8}|^15[0-9]{1}[0-9]{8}|^14[0-9]{1}[0-9]{8}|^16[0-9]{1}[0-9]{8}|^17[0-9]{1}[0-9]{8}|^18[0-9]{1}[0-9]{8}|^19[0-9]{1}[0-9]{8})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", ErrorMessage = "电话号码数据不合规！"),
            Required(ErrorMessage = "移动电话号码数据是必须的。"),
            MaxLength(11, ErrorMessage = "电话号码超过11位数！"),
            MinLength(11, ErrorMessage = "电话号码长度不足11位数！")]
        public string Mobile { get; set; }

        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "密码是必须的。")]
        [RegularExpression(@"((^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,})$)", ErrorMessage = "密码至少8个字符，至少1个字母，1个数字和1个特殊字符！")]
        public string Password { get; set; }

        [Display(Name = "重复密码")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "密码必须一致")]
        public string PasswordComfirm { get; set; }

        [Display(Name = "性别")]
        public bool Sex { get; set; }

        [Display(Name = "下单日期")]
        [Required(ErrorMessage = "下单日期是必须选择的。")]
        [DataType(DataType.DateTime, ErrorMessage = "日期时间数据格式错误。")]
        public DateTime OrderDateTime { get; set; }

        [Display(Name = "是否完成")]
        public bool IsFinished { get; set; }

        [Display(Name = "数量")]
        [Required(ErrorMessage = "数量数据是必须选择的。")]
        public int Amount { get; set; }

        [Display(Name = "金额")]
        [Required(ErrorMessage = "金额数据是必须选择的。")]
        [DataType(DataType.Currency, ErrorMessage = "金额数据格式错误。")]
        public decimal Total { get; set; }

        [Display(Name = "Html编辑")]
        public string HtmlContent { get; set; }

        [Display(Name = "样式种类")]
        [Required(ErrorMessage = "样式种类是必须选择的。")]
        public DemoEntityEnum DemoEntityEnum { get; set; }
        public string DemoEntityEnumName { get; set; }
        public List<PlainFacadeItem> DemoEntityEnumItemCollection { get; set; }

        [Display(Name = "归属类型")]
        [Required(ErrorMessage = "上级类型必须选择的。")]
        public string DemoEntityParentId { get; set; }
        [Display(Name = "归属类型")]
        public string DemoEntityParentName { get; set; }
        public List<SelfReferentialItem> DemoEntityParentItemCollection { get; set; }

        [Display(Name = "附加类型")]
        public string DemoCommonId { get; set; }
        [Display(Name = "附加类型")]
        public string DemoCommonName { get; set; }
        public List<PlainFacadeItem> DemoCommonItemCollection { get; set; }

        [Display(Name = "规格配置")]
        public string[] DemoItemForMultiSelectId { get; set; }
        [Display(Name = "规格配置")]
        public string[] DemoItemForMultiSelectName { get; set; }
        public List<PlainFacadeItem> DemoItemForMultiSelectItemCollection { get; set; }

        public List<DemoEntityItemVM> DemoEntityItemVMCollection { get; set; }  // 用于模拟订单数据处理的数据

        [Display(Name = "联系地址")]
        public CommonAddressVM AddressVM { get; set; }


    }
}

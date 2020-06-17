using LPFW.EntitiyModels.BusinessCommon.Status;
using LPFW.ViewModels.BusinessCommon;
using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.OrganizationBusiness
{
    /// <summary>
    /// 交易平台营运组织注册资料视图模型
    /// </summary>
    public class TransactionCenterRegisterVM : EntityViewModel
    {
        [Required(ErrorMessage = "组织名称不能为空值。")]
        [Display(Name = "组织名称")]
        [StringLength(50, ErrorMessage = "你输入的数据超出限制50个字符的长度。")]
        public override string Name { get; set; }

        [Required(ErrorMessage = "联系人不能为空值。")]
        [Display(Name = "联系人")]
        [StringLength(20, ErrorMessage = "你输入的数据超出限制20个字符的长度。")]
        public string ContactName { get; set; }

        [Display(Name = "移动电话")]
        [RegularExpression(@"((^13[0-9]{1}[0-9]{8}|^15[0-9]{1}[0-9]{8}|^14[0-9]{1}[0-9]{8}|^16[0-9]{1}[0-9]{8}|^17[0-9]{1}[0-9]{8}|^18[0-9]{1}[0-9]{8}|^19[0-9]{1}[0-9]{8})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", ErrorMessage = "电话号码数据不合规！"),
            Required(ErrorMessage = "移动电话号码数据是必须的。"),
            MaxLength(11, ErrorMessage = "电话号码超过11位数！"),
            MinLength(11, ErrorMessage = "电话号码长度不足11位数！")]
        public string Mobile { get; set; }

        [Display(Name = "电子邮件")]
        [Required(ErrorMessage = "电子邮件数据是必须的。")]
        [EmailAddress(ErrorMessage = "请输入合法的电子邮件地址。")]
        public string Email { get; set; }

        [Required(ErrorMessage = "管理员用户名不能为空值。")]
        [Display(Name = "管理员用户名")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public string AdminUserName { get; set; }

        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "密码是必须的。")]
        [RegularExpression(@"((^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,})$)", ErrorMessage = "密码至少8个字符，至少1个小写字母，一个大写字母，1个数字和1个特殊字符！")]
        public string Password { get; set; }

        [Display(Name = "重复密码")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "重复密码与密码必须一致")]
        public string PasswordComfirm { get; set; }

        [Display(Name = "创建日期")]
        public DateTime CreateTime { get; set; }

        [Display(Name = "审核日期")]
        public DateTime ApprovedTime { get; set; }

        [Display(Name = "审核状态")]
        public BusinessEntityStatusEnum BusinessEntityStatusEnum { get; set; }
        public string BusinessEntityStatusEnumName { get; set; }
        public List<PlainFacadeItem> BusinessEntityStatusEnumItemCollection { get; set; }

        [Display(Name = "联系地址")]
        public CommonAddressVM AddressVM { get; set; }
    }
}

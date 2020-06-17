using LPFW.EntitiyModels.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.BusinessCommon
{
    /// <summary>
    /// 在站点注册的资料
    /// </summary>
    public class SiteRegisterVM:EntityViewModel
    {
        public string SiteRegisterType { get; set; }  // 注册类型
        public int SiteRegisterStep { get; set; }  // 注册步骤

        [Required(ErrorMessage = "单位名称不能为空值。")]
        [Display(Name = "单位名称")]
        [StringLength(50, ErrorMessage = "你输入的数据超出限制50个字符的长度。")]
        public override string Name { get; set; }

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

        [Display(Name = "联系地址")]
        public CommonAddressVM AddressVM { get; set; }

        public SiteRegisterVM()
        {
            this.Id = Guid.NewGuid();
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<SiteRegisterVM>();
            this.AddressVM = new CommonAddressVM();
        }
    }
}

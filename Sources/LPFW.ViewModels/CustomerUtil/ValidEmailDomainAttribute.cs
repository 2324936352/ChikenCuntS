using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.CustomerUtil
{
    /// <summary>
    /// 重写邮箱验证
    /// </summary>
    public class ValidEmailDomainAttribute: ValidationAttribute
    {
        private readonly string allowedDomain;


        public ValidEmailDomainAttribute(string allowedDomain)
        {
            this.allowedDomain = allowedDomain;
        }

        public override bool IsValid(object value)
        {
            string[] strings = value.ToString().Split('@');

            return strings[1].ToUpper() == allowedDomain.ToUpper();
        }
    }
}

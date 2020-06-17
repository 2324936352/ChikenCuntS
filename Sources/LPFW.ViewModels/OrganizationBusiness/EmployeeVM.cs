using LPFW.ViewModels.BusinessCommon;
using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.OrganizationBusiness
{
    /// <summary>
    /// 中心员工基础数据视图模型
    /// </summary>
    public class EmployeeVM : EntityViewModel
    {
        [Required(ErrorMessage = "姓名不能为空值。")]
        [Display(Name = "姓名")]
        [StringLength(50, ErrorMessage = "你输入的数据超出限制25个字符的长度。")]
        public override string Name { get; set; }

        [Display(Name = "姓氏")]
        //[StringLength(25, ErrorMessage = "你输入的数据超出限制25个字符的长度。")]
        public string FirstName { get; set; }

        [Display(Name = "名字")]
        //[StringLength(25, ErrorMessage = "你输入的数据超出限制25个字符的长度。")]
        public string LastName { get; set; }

        [Display(Name = "性别")]
        public bool Sex { get; set; }

        [Display(Name = "出生日期")]
        [Required(ErrorMessage = "出生日期是必须选择的。")]
        [DataType(DataType.DateTime, ErrorMessage = "日期时间数据格式错误。")]
        public DateTime Birthday { get; set; }

        // todo 身份证号的正则表达式
        [Display(Name = "身份证号")]
        public string CredentialsCode { get; set; }

        [Display(Name = "用户名")]
        [Required(ErrorMessage = "用户名是必须选择的。")]
        public string PersonalCode { get; set; }  // 员工工号将作为缺省的用户名创建与员工关联的系统用户

        [Display(Name = "个人头像")]
        public string AvatarPath { get; set; }

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

        [Display(Name = "创建日期")]
        public DateTime CreateDateTime { get; set; }

        [Display(Name = "更新日期")]
        public DateTime UpdateTime { get; set; }

        [Display(Name = "停用日期")]
        public DateTime ExpiredDateTime { get; set; }

        [Display(Name = "是否在职")]
        public bool IsActive { get; set; }

        [Display(Name = "关联用户")]
        public string ApplicationUserId { get; set; }
        [Display(Name = "关联用户")]
        public string ApplicationUserName { get; set; }


        [Required(ErrorMessage = "归属部门是必须选择的。")]
        [Display(Name = "归属部门")]
        public string DepartmentId { get; set; }
        [Display(Name = "归属部门")]
        public string DepartmentName { get; set; }
        public List<SelfReferentialItem> DepartmentItemCollection { get; set; }

        [Display(Name = "工作岗位")]
        public string PositionId { get; set; }
        [Display(Name = "工作岗位")]
        public string PositionName { get; set; }
        public List<PlainFacadeItem> PositionItemCollection { get; set; }

        [Display(Name = "联系地址")]
        public CommonAddressVM AddressVM { get; set; }

        public bool IsStudent { get; set; }

    }
}

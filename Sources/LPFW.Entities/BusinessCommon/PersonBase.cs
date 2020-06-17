using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.EntitiyModels.BusinessCommon
{
    /// <summary>
    /// 人员的抽象类，可作为系统中所有的人员的定义的基类，基类里的 Name 属性用于姓名
    /// </summary>
    public abstract class PersonBase:Entity
    {

        [StringLength(26)]
        public string FirstName { get; set; }           // 姓氏
        [StringLength(50)]
        public string LastName { get; set; }             // 名字
        public bool Sex { get; set; }                    // 性别
        public DateTime Birthday { get; set; }           // 出生日期
        [StringLength(26)]                               
        public string CredentialsCode { get; set; }      // 身份证编号
        [StringLength(50)]                               
        public string PersonalCode { get; set; }         // 类似工号，可以看成是人员在组织内的任意的工作号码
        [StringLength(350)]                              
        public string AvatarPath { get; set; }           // 人员头像路径
                                                         
        [StringLength(20)]                               
        public string Mobile { get; set; }               // 手机号码
        [StringLength(100)]                              
        public string Email { get; set; }                // 电子邮箱
                                                         
        public DateTime CreateDateTime { get; set; }     // 入职日期，这两个属性可以看成是人员在每个组织里的生命周期时间
        public DateTime ExpiredDateTime { get; set; }    // 离职日期
        public DateTime UpdateTime { get; set; }         // 信息更新时间
                                                         
        public PersonBase()
        {
            this.Id = Guid.NewGuid();
            this.Birthday = this.CreateDateTime = this.ExpiredDateTime = UpdateTime = DateTime.Now;
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<PersonBase>();
        }
    }
}

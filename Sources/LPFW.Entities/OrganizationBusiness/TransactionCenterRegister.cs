using LPFW.EntitiyModels.BusinessCommon;
using LPFW.EntitiyModels.BusinessCommon.Status;
using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.EntitiyModels.OrganzationBusiness
{
    /// <summary>
    /// 营运组织注册资料，再首期开发实现中，约定只有一个中心，并且只有一个初始系统管理员未来可以作为云服务的租户账号处理。
    ///   Name：注册组织名称
    ///   Description：注册组织简要描述
    /// 
    /// 注册成功后，也可以是平台的最高级别的管理员直接手工创建，AdminUserName 代表的用户缺省为这个营运组织的核心管理员
    /// </summary>
    public class TransactionCenterRegister:Entity
    {
        [StringLength(20)]
        public string ContactName { get; set; }     // 联系人姓名
        [StringLength(20)]
        public string Mobile { get; set; }          // 手机号码
        [StringLength(100)]
        public string Email { get; set; }           // 电子邮箱
        [StringLength(100)]
        public string AdminUserName { get; set; }   // 管理员账号
        [StringLength(100)]
        public string Password { get; set; }        // 管理员账号登录密码   
        public DateTime CreateTime { get; set; }    // 申请时间
        public DateTime ApprovedTime { get; set; }  // 审核通过时间

        public virtual CommonAddress Address { get; set; }                     // 地址
        public BusinessEntityStatusEnum BusinessEntityStatusEnum { get; set; } // 申请处理状态

        public TransactionCenterRegister()
        {
            this.Id = Guid.NewGuid();
            this.CreateTime = DateTime.Now;
            this.ApprovedTime = DateTime.Now;
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<TransactionCenterRegister>();
            this.Address = new CommonAddress() { Id = this.Id, SortCode = "", Name = "中国", ProvinceName = "", CityName = "", CountyName = "", DetailName = "" };
        }
    }
}

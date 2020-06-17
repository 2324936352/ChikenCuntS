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
    /// 营运组织单位，也可以看成是营运组织单位划分，仅适合根节点的部门
    ///   在实际的应用中，部门分组是考虑到根据部门某些特性，例如内部单位、外部单位之类的结构。
    ///   在数据管理上，支持类似内部管理员可以管理内部组织结构，外部单位可以管理外部单位之类
    ///   的要求。
    ///   负责人、联系人的配给，都是在后续的人员基础数据给定之后才能进行配置
    /// 由最高级的系统管理员处理，联系人相关数据缺省值就是处理数据的用户
    /// </summary>
    public class Organization : Entity
    {
        [StringLength(20)]
        public string ContactName { get; set; }     // 联系人姓名
        [StringLength(20)]
        public string Mobile { get; set; }          // 联系人手机号码
        [StringLength(100)]
        public string Email { get; set; }           // 联系人电子邮箱
        [StringLength(100)]
        public string AdminUserName { get; set; }   // 管理员账号
        [StringLength(100)]
        public string Password { get; set; }        // 管理员账号登录密码   
        public DateTime CreateTime { get; set; }    // 申请时间
        public DateTime ApprovedTime { get; set; }  // 审核通过时间

        public BusinessEntityStatusEnum BusinessEntityStatusEnum { get; set; }             // 创建处理状态
        public virtual CommonAddress Address { get; set; }                                 // 单位地址
        public virtual TransactionCenterRegister TransactionCenterRegister { get; set; }   // 归属的营运组织（交易中心）
        public virtual Employee OrganizationLeader { get; set; }                           // 单位负责人
        public virtual Employee OrganzationContact { get; set; }                           // 单位联系人

        public Organization()
        {
            this.Id = Guid.NewGuid();
            this.CreateTime = DateTime.Now;
            this.ApprovedTime = DateTime.Now;
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<Organization>();
            this.Address = new CommonAddress() { Id = this.Id, SortCode = "", Name = "中国", ProvinceName = "", CityName = "", CountyName = "", DetailName = "" };
        }
    }
}

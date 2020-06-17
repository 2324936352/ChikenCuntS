using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.EntitiyModels.BusinessCommon.Status;
using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.BusinessCommon
{
    /// <summary>
    /// 业务对象状态更改审核处理记录
    ///  Name:审核业务名称 + 业务对象名称(例如 采购商注册资料审核-小洞天餐饮集团)，也可以自行根据实际的审核业务需要定义
    ///  Description: 如果审核不通过，则需要给出说明意见
    /// </summary>
    public class BusinessEntityAudit : Entity
    {
        public Guid EntityId { get; set; }           // 待审业务对象的 Id
        public bool IsPassed { get; set; }           // 是否通过
        public DateTime AudiltDateTime { get; set; } // 审核时间
        
        public BusinessEntityStatusEnum EntityStatusEnum { get; set; } // 待审业务对象的处理状态
        public virtual ApplicationUser Auditor { get; set; }           // 审核人

        public BusinessEntityAudit() 
        {
            this.Id = Guid.NewGuid();
            this.AudiltDateTime = DateTime.Now;
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<BusinessEntityAudit>();
        }
    }
}

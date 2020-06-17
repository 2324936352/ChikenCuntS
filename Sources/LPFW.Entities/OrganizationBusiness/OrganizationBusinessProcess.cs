using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.OrganzationBusiness
{
    /// <summary>
    /// 组织业务过程定义,是指企业组织里为了完成一个业务目标，例如下单过程定义处理的一系列的具有次序的
    /// 岗位作业的集合：编辑终端采购订单-审核订单-统单-审核采购统单-发布采购订单。
    ///   Name：业务过程名称
    ///   Description：业务过程简要描述
    ///   SortCode：编码
    /// </summary>
    public class OrganizationBusinessProcess : Entity
    {
        public virtual Organization Organization { get; set; } // 归属组织

        public OrganizationBusinessProcess() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}

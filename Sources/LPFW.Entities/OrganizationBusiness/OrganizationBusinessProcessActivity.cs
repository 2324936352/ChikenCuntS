using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.OrganzationBusiness
{
    /// <summary>
    /// 业务过程节点节点定义，采用简单的单链表方式构建，在创建 OrganizationBusinessProcess 时，缺省创建一个头节点
    ///   Name：节点名称
    ///   Description：节点描述
    /// </summary>
    public class OrganizationBusinessProcessActivity : Entity
    {
        public Guid OrganizationBusinessProcessId { get; set; }  // 关联的单位业务过程的 Id，只是便于查询的一个简单属性
        public Guid PreviousNodeId { get; set; }                 // 前置节点 Id
        public Guid NextNodeId { get; set; }                     // 后置节点 Id，后置节点为 null 时，是尾节点

        public virtual PositionWork PositionWork { get; set; }  // 节点值，节点值为 null 时，为头节点，归属同一个过程定义中，不允许重复

        public OrganizationBusinessProcessActivity() 
        {
            this.Id = Guid.NewGuid();
            this.SortCode= EntityHelper.SortCodeByDefaultDateTime<OrganizationBusinessProcessActivity>();
        }
    }
}

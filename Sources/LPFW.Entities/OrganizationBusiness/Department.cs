using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.EntitiyModels.BusinessCommon;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;

namespace LPFW.EntitiyModels.OrganzationBusiness
{
    /// <summary>
    /// 组织部门机构
    ///   创建部门的时候，自动生成一个部门的角色组，以便以后部门的负责人，可以在这个用户组之下创建自己部门的用户子组
    ///   Name：部门名称
    ///   Decrition：部门简单描述，一般用于描述部门的工作职责
    ///   SortCode：部门编码
    /// </summary>
    public class Department:Entity
    {
        public DateTime CreateDate { get; set; }  // 创建日期
        public DateTime UpdateDate { get; set; }  // 更新日期
        public int EmployeeAmount { get; set; }   // 部门人数，获取部门对象时候自动计算产生

        public virtual Organization Organization { get; set; }        // 归属单位
        public virtual Department Parent { get; set; }                // 上级部门
        public virtual CommonAddress Address { get; set; }            // 创建或者编辑根节点时应该创建地址，其下属的所有节点的地址缺省使用上级节点地址，但可以自己再编辑
        public virtual ApplicationRole ApplicationRole { get; set; }  // 这个属性在新建部门的时候自动创建，一旦删除和编辑这个部门，关联角色组随之变更

        public virtual ICollection<DepartmentKPI> DepartmentKPICollection { get; set; }  // 部门绩效指标清单

        public Department()
        {
            this.Id = Guid.NewGuid();
            CreateDate = UpdateDate = DateTime.Now;

            // 缺省地址示例，实际使用时需要另外处理地址的编辑操作
            this.Address = new CommonAddress() { Id = this.Id, SortCode = "", Name = "中国", ProvinceName = "", CityName = "", CountyName = "", DetailName = "" };
        }

    }
}

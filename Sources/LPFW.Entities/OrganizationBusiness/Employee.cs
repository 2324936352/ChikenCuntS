using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.EntitiyModels.BusinessCommon;
using LPFW.EntitiyModels.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.OrganzationBusiness
{
    /// <summary>
    /// 员工
    /// </summary>
    public class Employee : PersonBase
    {
        public bool IsActive { get; set; }                    // 是否在职

        public bool IsStudent { get; set; }

        public virtual ApplicationUser User { get; set; }     // 关联用户
        public virtual Department Department { get; set; }    // 归属部门
        public virtual Position Position { get; set; }        // 归属岗位，在前端输入数据时，要处理这个数据来源与归属部门的级联输入
        public virtual CommonAddress Address { get; set; }    // 员工联系地址

        public Employee()
        {
            this.Id = Guid.NewGuid();
            this.IsActive = true;
            this.Birthday = this.CreateDateTime = this.ExpiredDateTime = UpdateTime = DateTime.Now;
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<Employee>();
            this.Address = new CommonAddress() { Id = this.Id, SortCode = "", Name = "中国", ProvinceName = "", CityName = "", CountyName = "", DetailName = "" };
        }
    }
}

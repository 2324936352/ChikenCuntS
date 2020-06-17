using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.ApplicationCommon.RoleAndUser
{
    /// <summary>
    /// 平台通用的用户令牌资格声明
    /// </summary>
    public enum UserCommonClaimEnum
    {
        用户类型,   // 对应使用 RoleCommonClaimEnum 的值
        注册资料,   // 根据用户类型，分别对应相应的实体模型对象的 Id 
        单位,       // 根据用户类型，分别对应相应的实体模型对象的 Id 
        部门,       // 同上
        岗位,       // 同上
        岗位工作,   // 同上
        宿主角色
    }
}

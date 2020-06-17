using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.ApplicationCommon.RoleAndUser
{
    /// <summary>
    /// 角色组令牌声明类型，也用于限制 UserCommonClaimEnum 中的 用户类型 的值
    /// </summary>
    public enum RoleCommonClaimEnum
    {
        系统管理,
        综合管理,
        平台业务,
        采购商业务,
        供应商业务,
        电商业务,
        金融服务业务,
        物流服务业务,
        个人业务,
        其它,
        学生
    }
}

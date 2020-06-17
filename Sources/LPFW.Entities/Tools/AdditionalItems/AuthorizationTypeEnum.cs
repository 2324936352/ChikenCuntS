using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.Tools.AdditionalItems
{
    /// <summary>
    /// 对于指定的资源（业务实体）访问权限设置定义
    /// </summary>
    public enum AuthorizationTypeEnum
    {
        完全权限,     // 包含可以分配的最高权限
        管理权限,     // 包含对资源 CRUD 全部操作
        编辑权限,     // 包含对于自己创建的资源对象的全部 CRUD，别人分配的资源的 RU 权限
        完全阅读权限,  // 包含对授权资源对象的完整的 R 权限
        局部阅读权限,  // 根据业务逻辑指定阅读内容范围的 R 权限
        无权限
    }
}

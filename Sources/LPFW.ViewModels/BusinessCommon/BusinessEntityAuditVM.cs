using LPFW.EntitiyModels.BusinessCommon.Status;
using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.BusinessCommon
{
    /// <summary>
    /// 业务对象状态更改审核处理记录视图模型
    /// </summary>
    public class BusinessEntityAuditVM : EntityViewModel
    {
        [Display(Name = "是否通过")]
        public bool IsPassed { get; set; }
        [Display(Name = "审核说明")]
        [StringLength(500, ErrorMessage = "你输入的数据超出限制500个字符的长度。")]
        public override string Description { get; set; }

        public Guid EntityId { get; set; }           // 待审业务对象的 Id
        public DateTime AudiltDateTime { get; set; } // 审核时间

        public BusinessEntityStatusEnum BusinessEntityStatusEnum { get; set; }
        public string BusinessEntityStatusEnumName { get; set; }
        public List<PlainFacadeItem> BusinessEntityStatusEnumItemCollection { get; set; }

        public Guid AuditorId { get; set; }
        public string AuditorName { get; set; }
    }
}

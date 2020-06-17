using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.Demo;
using LPFW.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels
{
    /// <summary>
    /// 视图模型基类，是接口类 IEntityViewModel 的具体实现
    /// </summary>
    public class EntityViewModel : IEntityViewModel
    {
        public Guid Id { get; set; }                 // 映射实体模型对象的 Id
        [Display(Name = "序号")]
        public string OrderNumber { get; set; }      // 序号，用于在列表呈现数据的序号
        public bool IsNew { get; set; }              // 对应的实体对象是否是新创建的实体对象
        public bool IsSaved { get; set; }            // 对应的实体对象是不是已经保存
        public bool IsCurrent { get; set; }          // 对应的实体对象是否是当前处理的
        public string ErrorMessage { get; set; }     // 对应的实体对象在处理时发生的错误消息

        [Required(ErrorMessage = "名称不能为空值。")]
        [Display(Name = "名称")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public virtual string Name { get; set; }

        [Display(Name = "简要说明")]
        [StringLength(1000, ErrorMessage = "你输入的数据超出限制1000个字符的长度。")]
        public virtual string Description { get; set; }

        [Required(ErrorMessage = "业务编码不能为空值。")]
        [Display(Name = "业务编码")]
        [StringLength(150, ErrorMessage = "你输入的数据超出限制150个字符的长度。")]
        public virtual string SortCode { get; set; }



        #region 用于处理附件数据的通用的视图模型属性
        public virtual List<BusinessFileVM> BusinessFileVMCollection { get; set; }

        public virtual BusinessImageVM Avatar { get; set; }
        public virtual List<BusinessImageVM> BusinessImageVMCollection { get; set; }

        public virtual BusinessImageVM BusinessVideoCoverPage { get; set; }
        public virtual BusinessVideoVM BusinessVideoVM { get; set; } 
        #endregion
    }
}

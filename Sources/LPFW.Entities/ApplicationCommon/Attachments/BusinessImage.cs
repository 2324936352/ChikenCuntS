using LPFW.EntitiyModels.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPFW.EntitiyModels.ApplicationCommon.Attachments
{
    /// <summary>
    /// 上传图像文件信息
    /// </summary>
    public class BusinessImage : Entity
    {
        [StringLength(100)]
        public string DisplayName { get; set; }      // 图片显示名称
        [StringLength(256)]
        public string OriginalFileName { get; set; } // 图片原始文件
        public DateTime UploadedTime { get; set; }   // 图片上传时间
        [StringLength(256)]
        public string UploadPath { get; set; }       // 图片上传保存路径
        [StringLength(256)]
        public string UploadFileSuffix { get; set; } // 上传文件的后缀名
        public long FileSize { get; set; }           // 文件字节数
        [StringLength(120)]
        public string IconString { get; set; }       // 文件物理格式图标

        public bool IsForTitle { get; set; }         // 是否作为标题文件使用，如果作为标题文件使用，列表时候可以作为引导图片，明细可以作为第一张缺省图片
        public bool IsUnique { get; set; }           // 是否是唯一的，这是相对 RelevanceObjectID 来说的，如果是，在持久化的时候需要单独处理
        public bool IsAvatar { get; set; }           // 是否作为头像使用

        public Guid RelevanceObjectID { get; set; }  // 使用该图片的业务对象的 id
        public Guid UploaderID { get; set; }         // 关联上传人ID

        public BusinessImage()
        {
            this.Id = Guid.NewGuid();
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<BusinessImage>();
            this.UploadedTime = DateTime.Now;
            this.IsForTitle = false;
        }
    }
}

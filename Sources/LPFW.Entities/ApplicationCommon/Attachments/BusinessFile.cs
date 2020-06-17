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
    /// 上传普通文件信息
    /// </summary>
    public class BusinessFile : Entity
    {
        public DateTime AttachmentTimeUploaded { get; set; }  // 附件上传时间
        [StringLength(500)]
        public string OriginalFileName { get; set; }          // 附件原始文件名称
        [StringLength(500)]
        public string UploadPath { get; set; }                // 附件上传保存路径
        public bool IsInDB { get; set; }                      // 附件存放格式，如果使用二进制方式存在数据库中，则使用下一个属性进行处理
        [StringLength(10)]
        public string UploadFileSuffix { get; set; }          // 上传文件的后缀名
        public byte[] BinaryContent { get; set; }             // 附件存放的二进制内容
        public long FileSize { get; set; }                    // 文件大小
        [StringLength(120)]
        public string IconString { get; set; }                // 文件物理格式图标
        public bool IsUnique { get; set; }                    // 是否是唯一的，这是相对 RelevanceObjectID 来说的，如果是，在持久化的时候需要单独处理

        public Guid RelevanceObjectID { get; set; }           // 关联对象ID
        public Guid UploaderID { get; set; }                  // 关联上传人ID

        public BusinessFile() 
        {
            this.Id = Guid.NewGuid();
            this.SortCode = EntityHelper.SortCodeByDefaultDateTime<BusinessFile>();
            this.AttachmentTimeUploaded = DateTime.Now;
        }
    }
}

using LPFW.EntitiyModels.ApplicationCommon.Attachments;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.ViewModels.Common
{
    public class BusinessFileVM
    {
        public Guid Id { get; set; }
        public string ObjectId { get; set; }
        public string FileDisplayName { get; set; }
        public string FilePath { get; set; }

        public BusinessFileVM()
        {
            this.Id = Guid.NewGuid();
        }

        public BusinessFileVM(BusinessFile bo)
        {
            this.Id = bo.Id;
            this.ObjectId = bo.RelevanceObjectID.ToString();
            this.FileDisplayName = bo.Name;
            this.FilePath = bo.UploadPath;
        }
    }
}

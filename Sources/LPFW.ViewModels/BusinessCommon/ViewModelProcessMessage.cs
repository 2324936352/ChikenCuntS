using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.ViewModels.BusinessCommon
{
    public class ViewModelProcessMessage
    {
        public string MasterObjectId { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
        public string TipMessage { get; set; }
    }
}

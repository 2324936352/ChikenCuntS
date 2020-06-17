using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.BusinessCommon
{
    public class CommonAddress:AddressBase
    {
        public CommonAddress()
        {
            Id = Guid.NewGuid();
            SortCode = "510000";
            Name = "中国";
            ProvinceName = "广西壮族自治区";
            CityName = "柳州";
            CountyName = "柳北区";
            DetailName = "锦绣路9号 江东水岸6栋1单元501";
        }
    }
}

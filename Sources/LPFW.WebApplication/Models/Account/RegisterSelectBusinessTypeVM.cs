using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPFW.WebApplication.Models.Account
{
    public class RegisterSelectBusinessTypeVM
    {
        public RoleCommonClaimEnum PurchaserType { get; set; }
        public RoleCommonClaimEnum VenderType { get; set; }

        public RoleCommonClaimEnum SelectedType { get; set; }

    }
}

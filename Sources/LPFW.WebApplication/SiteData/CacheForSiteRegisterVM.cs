using LPFW.ViewModels.BusinessCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPFW.WebApplication.SiteData
{
    /// <summary>
    /// 简单的静态类，用于存放用户注册资料
    /// </summary>
    public static class CacheForSiteRegisterVM
    {
        private static List<SiteRegisterVM> _siteRegisterVMCollection = new List<SiteRegisterVM>();

        public static bool Add(SiteRegisterVM registerVM)
        {
            var has = _siteRegisterVMCollection.Any(x => x.Id == registerVM.Id);
            if (has)
                return false;
            else
            {
                _siteRegisterVMCollection.Add(registerVM);
                return true;
            }
        }

        public static SiteRegisterVM Get(Guid id)
        {
            return _siteRegisterVMCollection.FirstOrDefault(x => x.Id == id);
        }

        public static void Remove(Guid id)
        {
            var registerVM= _siteRegisterVMCollection.FirstOrDefault(x => x.Id == id);
            if (registerVM != null)
                _siteRegisterVMCollection.Remove(registerVM);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.EntitiyModels.Demo;
using LPFW.ViewModels.Demo;
using LPFW.ViewModelServices;
using LPFW.WebApplication.BaseTemplateControllers;
using Microsoft.AspNetCore.Mvc;

namespace LPFW.WebApplication.Controllers
{
    [Area("XDemo")]
    public class ModeController : BaseTemplateController<DemoCommon,DemoCommonVM>
    {
        public ModeController(IViewModelService<DemoCommon, DemoCommonVM> service) : base(service)
        {
            ModuleName = "普通实体 DemoCommon 数据管理";
            FunctionName = "DemoCommon";
        }
    }
}
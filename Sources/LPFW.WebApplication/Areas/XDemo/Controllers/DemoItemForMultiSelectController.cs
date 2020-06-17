using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.EntitiyModels.Demo;
using LPFW.ViewModelServices;
using LPFW.ViewModels.Demo;
using LPFW.WebApplication.BaseTemplateControllers;
using Microsoft.AspNetCore.Mvc;
using LPFW.EntitiyModels.XDemo;
using LPFW.ViewModels.XDemo;

namespace LPFW.WebApplication.Areas.XDemo.Controllers
{
    [Area("XDemo")]
    public class DemoItemForMultiSelectController : BaseTemplateController<MusicDemo, MusicViewmodel>
    {
        public DemoItemForMultiSelectController(IViewModelService<MusicDemo, MusicViewmodel> service) : base(service)
        {
            ModuleName = "常规实体对象 MusicViewmodel 数据管理";
            FunctionName = "MusicViewmodel";

            //// 如果沿用公共的路径，下面的赋值可以省掉
            //IndexViewPath = "../../Views/BaseTemplate/Index";
            //ListPartialViewPath = "_CommonList";
            //CreateOrEditPartialViewPath = "_CommonCreateOrEdit";
            //DetailPartialViewPath = "_CommonDetail";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.EntitiyModels.Demo;
using LPFW.ViewModelServices;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels.Demo;
using LPFW.WebApplication.BaseTemplateControllers;
using Microsoft.AspNetCore.Mvc;

namespace LPFW.WebApplication.Areas.XDemo.Controllers
{
    /// <summary>
    /// 这是最简单的实体模型类数据处理的样例
    /// </summary>
    [Area("XDemo")]
    public class DemoCommonController : BaseTemplateController<DemoCommon, DemoCommonVM>
    {
        /// <summary>
        /// 构造函数，负责注入数据访问服务，和初始化一些视图参数
        /// </summary>
        /// <param name="service"> 注入的视图模型服务，用于为控制器提供适当的视图模型和对视图模型对象进行处理的服务 </param>
        public DemoCommonController(IViewModelService<DemoCommon,DemoCommonVM> service) : base(service)
        {
            ModuleName = "普通实体 DemoCommon 数据管理";
            FunctionName = "DemoCommon";
        }
    }
}
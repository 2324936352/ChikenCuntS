using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.EntitiyModels.OrganzationBusiness;
using LPFW.EntitiyModels.PortalSite;
using LPFW.ViewModels.OrganizationBusiness;
using LPFW.ViewModels.PortalSite;
using LPFW.ViewModelServices;
using LPFW.WebApplication.BaseTemplateControllers;
using LPFW.WebApplication.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LPFW.WebApplication.Areas.News.Controllers
{
    [Area("News")]
    //[TypeFilter(typeof(TeacherAsyncFilter<Employee, EmployeeVM>), Arguments = new object[] { "21122", "1234@Abcd" })]
    public class ArticleTopicController : BaseTemplateController<ArticleTopic, ArticleTopicVM>
    {
        public ArticleTopicController(IViewModelService<ArticleTopic, ArticleTopicVM> service) : base(service)
        {
            ModuleName = "音乐管理";
            FunctionName = "音乐栏目";
        }
    }
}

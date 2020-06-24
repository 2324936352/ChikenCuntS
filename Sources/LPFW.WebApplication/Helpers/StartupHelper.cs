using LPFW.DataAccess;
using LPFW.EntitiyModels.ApplicationCommon.AppPathAndMenu;
using LPFW.EntitiyModels.ApplicationCommon.Attachments;
using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.EntitiyModels.Demo;
using LPFW.EntitiyModels.MusicEntity;
using LPFW.EntitiyModels.OrganzationBusiness;
using LPFW.EntitiyModels.PortalSite;
using LPFW.EntitiyModels.TeachingBusiness;
using LPFW.EntitiyModels.XDemo;
using LPFW.ViewModels.ApplicationCommon.AppPathAndMenu;
using LPFW.ViewModels.ApplicationCommon.RoleAndUser;
using LPFW.ViewModels.Demo;
using LPFW.ViewModels.MusicViewModel;
using LPFW.ViewModels.OrganizationBusiness;
using LPFW.ViewModels.PortalSite;
using LPFW.ViewModels.TeachingBusiness;
using LPFW.ViewModels.XDemo;
using LPFW.ViewModelServices;
using LPFW.WebApplication.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPFW.WebApplication.Helpers
{
    /// <summary>
    /// 为 Startup 提供一些配置处理的助理程序
    /// </summary>
    public static class StartupHelper
    {
        /// <summary>
        /// 扩展 IServiceCollection， 用于配置系统业务实体相关服务的依赖注入元素
        /// </summary>
        /// <param name="services"></param>
        public static void BusinessEntityDependencyInjector(this IServiceCollection services) 
        {
            #region ApplicationCommon 部分
            services.AddScoped<IEntityRepository<BusinessImage>, EntityRepository<BusinessImage>>();
            services.AddScoped<IEntityRepository<BusinessFile>, EntityRepository<BusinessFile>>();
            services.AddScoped<IEntityRepository<BusinessVideo>, EntityRepository<BusinessVideo>>();

            services.AddScoped<IEntityRepository<ApplicationMenuGroup>, EntityRepository<ApplicationMenuGroup>>();
            services.AddScoped<IViewModelService<ApplicationMenuGroup, ApplicationMenuGroupVM>, ViewModelService<ApplicationMenuGroup, ApplicationMenuGroupVM>>();

            services.AddScoped<IEntityRepository<ApplicationMenuItem>, EntityRepository<ApplicationMenuItem>>();
            services.AddScoped<IViewModelService<ApplicationMenuItem, ApplicationMenuItemVM>, ViewModelService<ApplicationMenuItem, ApplicationMenuItemVM>>();

            services.AddScoped<IEntityRepository<ApplicationRole>, EntityRepository<ApplicationRole>>();
            services.AddScoped<IViewModelService<ApplicationRole, ApplicationRoleVM>, ViewModelService<ApplicationRole, ApplicationRoleVM>>();

            services.AddScoped<IEntityRepository<ApplicationUser>, EntityRepository<ApplicationUser>>();
            services.AddScoped<IViewModelService<ApplicationUser, ApplicationUserVM>, ViewModelService<ApplicationUser, ApplicationUserVM>>();

            #endregion

            #region 平台基础业务管理部分
            services.AddScoped<IEntityRepository<Organization>, EntityRepository<Organization>>();
            services.AddScoped<IViewModelService<Organization, OrganizationVM>, ViewModelService<Organization, OrganizationVM>>();

            services.AddScoped<IEntityRepository<OrganizationBusinessProcess>, EntityRepository<OrganizationBusinessProcess>>();
            services.AddScoped<IViewModelService<OrganizationBusinessProcess, OrganizationBusinessProcessVM>, ViewModelService<OrganizationBusinessProcess, OrganizationBusinessProcessVM>>();

            services.AddScoped<IEntityRepository<OrganizationKPIType>, EntityRepository<OrganizationKPIType>>();
            services.AddScoped<IViewModelService<OrganizationKPIType, OrganizationKPITypeVM>, ViewModelService<OrganizationKPIType, OrganizationKPITypeVM>>();

            services.AddScoped<IEntityRepository<Department>, EntityRepository<Department>>();
            services.AddScoped<IViewModelService<Department, DepartmentVM>, ViewModelService<Department, DepartmentVM>>();

            services.AddScoped<IEntityRepository<DepartmentKPI>, EntityRepository<DepartmentKPI>>();
            services.AddScoped<IViewModelService<DepartmentKPI, DepartmentKPIVM>, ViewModelService<DepartmentKPI, DepartmentKPIVM>>();

            services.AddScoped<IEntityRepository<Employee>, EntityRepository<Employee>>();
            services.AddScoped<IViewModelService<Employee, EmployeeVM>, ViewModelService<Employee, EmployeeVM>>();


            services.AddScoped<IEntityRepository<EmployeeKPIMetricItem>, EntityRepository<EmployeeKPIMetricItem>>();

            services.AddScoped<IEntityRepository<EmployeeWithPositionWork>, EntityRepository<EmployeeWithPositionWork>>();

            services.AddScoped<IEntityRepository<Position>, EntityRepository<Position>>();
            services.AddScoped<IViewModelService<Position, PositionVM>, ViewModelService<Position, PositionVM>>();

            services.AddScoped<IEntityRepository<PositionWork>, EntityRepository<PositionWork>>();
            services.AddScoped<IViewModelService<PositionWork, PositionWorkVM>, ViewModelService<PositionWork, PositionWorkVM>>();

            services.AddScoped<IEntityRepository<PositionWorkKPI>, EntityRepository<PositionWorkKPI>>();
            services.AddScoped<IViewModelService<PositionWorkKPI, PositionWorkKPIVM>, ViewModelService<PositionWorkKPI, PositionWorkKPIVM>>();

            services.AddScoped<IEntityRepository<TransactionCenterRegister>, EntityRepository<TransactionCenterRegister>>();
            services.AddScoped<IViewModelService<TransactionCenterRegister, TransactionCenterRegisterVM>, ViewModelService<TransactionCenterRegister, TransactionCenterRegisterVM>>();
            #endregion

            #region 课程资源管理部分
            services.AddScoped<IEntityRepository<CourseContainer>, EntityRepository<CourseContainer>>();
            services.AddScoped<IViewModelService<CourseContainer, CourseContainerVM>, ViewModelService<CourseContainer, CourseContainerVM>>();

            services.AddScoped<IEntityRepository<Course>, EntityRepository<Course>>();
            services.AddScoped<IViewModelService<Course, CourseVM>, ViewModelService<Course, CourseVM>>();
            #endregion

            #region 新闻信息
            services.AddScoped<IEntityRepository<ArticleTopic>, EntityRepository<ArticleTopic>>();
            services.AddScoped<IViewModelService<ArticleTopic, ArticleTopicVM>, ViewModelService<ArticleTopic, ArticleTopicVM>>();

            services.AddScoped<IEntityRepository<Article>, EntityRepository<Article>>();
            services.AddScoped<IViewModelService<Article, ArticleVM>, ViewModelService<Article, ArticleVM>>();

            services.AddScoped<IEntityRepository<DemoItemForMultiSelect>, EntityRepository<DemoItemForMultiSelect>>();
            services.AddScoped<IViewModelService<DemoItemForMultiSelect,DemoItemForMultiSelectVM>, ViewModelService<DemoItemForMultiSelect, DemoItemForMultiSelectVM>>();

            services.AddScoped<IEntityRepository<ArticleInTopic>, EntityRepository<ArticleInTopic>>();
            #endregion

            #region Demo 部分
            services.AddScoped<IEntityRepository<DemoEntityParent>, EntityRepository<DemoEntityParent>>();
            services.AddScoped<IViewModelService<DemoEntityParent, DemoEntityParentVM>, ViewModelService<DemoEntityParent, DemoEntityParentVM>>();

            services.AddScoped<IEntityRepository<DemoCommon>, EntityRepository<DemoCommon>>();
            services.AddScoped<IViewModelService<DemoCommon, DemoCommonVM>, ViewModelService<DemoCommon, DemoCommonVM>>();
            
            services.AddScoped<IEntityRepository<DemoEntity>, EntityRepository<DemoEntity>>();
            services.AddScoped<IViewModelService<DemoEntity, DemoEntityVM>, ViewModelService<DemoEntity, DemoEntityVM>>();

            services.AddScoped<IEntityRepository<MusicDemo>, EntityRepository<MusicDemo>>();
            services.AddScoped<IViewModelService<MusicDemo, MusicViewmodel>, ViewModelService<MusicDemo, MusicViewmodel>>();

            //音乐依赖注入
            services.AddScoped<IEntityRepository<MusicEntity>, EntityRepository<MusicEntity>>();
            services.AddScoped<IViewModelService<MusicEntity, MusicViewModel>, ViewModelService<MusicEntity, MusicViewModel>>();
            //音乐类型依赖注入
            services.AddScoped<IEntityRepository<MusicTypeEntity>, EntityRepository<MusicTypeEntity>>();
            services.AddScoped<IViewModelService<MusicTypeEntity, MusicTypeViewModel>, ViewModelService<MusicTypeEntity, MusicTypeViewModel>>();
            //专辑
            services.AddScoped<IEntityRepository<Album>, EntityRepository<Album>>();
            services.AddScoped<IViewModelService<Album, AlbumViewModel>, ViewModelService<Album , AlbumViewModel>>();

            #endregion
        }

        /// <summary>
        /// 扩展 IServiceCollection， 用于配置控制器的过滤器
        /// </summary>
        /// <param name="services"></param>
        public static void BusinessFilterConfig(this IServiceCollection services) 
        {
            services.AddScoped<TeacherAsyncFilter<Employee, EmployeeVM>>();
        }
    }
}

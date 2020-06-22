using LPFW.EntitiyModels.ApplicationCommon.AppPathAndMenu;
using LPFW.EntitiyModels.ApplicationCommon.Attachments;
using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.EntitiyModels.Demo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using LPFW.EntitiyModels.BusinessCommon;
using LPFW.EntitiyModels.OrganzationBusiness;
using LPFW.EntitiyModels.PortalSite;
using LPFW.EntitiyModels.TeachingBusiness;
using LPFW.EntitiyModels.XDemo;
using LPFW.EntitiyModels.MusicEntity;
using LPFW.EntitiyModels.MusicUIEntity;

namespace LPFW.ORM
{
    /// <summary>
    /// 实体模型对象与关系数据库映射上下文关系
    /// </summary>
    public class LpDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid> {
        public LpDbContext(DbContextOptions<LpDbContext> options) : base(options) { }

        #region 应用系统中公共的部分

        public DbSet<ApplicationMenuGroup> ApplicationMenuGroups { get; set; }
        public DbSet<ApplicationMenuItem> ApplicationMenuItems { get; set; }

        public DbSet<BusinessFile> BusinessFiles { get; set; }
        public DbSet<BusinessImage> BusinessImages { get; set; }
        public DbSet<BusinessVideo> BusinessVideos { get; set; }

        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<CommonAddress> CommonAddresses { get; set; }

        #endregion

        #region 平台营运管理服务组织机构和人力资源相关部分

        public DbSet<TransactionCenterRegister> TransactionCenterRegisters { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationBusinessProcess> OrganizationBusinessProcesses { get; set; }
        public DbSet<OrganizationBusinessProcessWithWork> OrganizationBusinessProcessWithWorks { get; set; }
        public DbSet<OrganizationKPIType> OrganizationKPITypes { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentKPI> DepartmentKPIs { get; set; }
        public DbSet<DepartmentKPIMetric> DepartmentKPIMetrics { get; set; }
        public DbSet<DepartmentKPIMetricItem> DepartmentKPIMetricItems { get; set; }
        public DbSet<DepartmentLeader> DepartmentLeaders { get; set; }
        public DbSet<DepartmentContact> DepartmentContacts { get; set; }

        public DbSet<Position> Positions { get; set; }
        public DbSet<PositionWork> PositionWorks { get; set; }
        public DbSet<PositionWorkKPI> PositionWorkKPIs { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeKPIMetric> EmployeeKPIMetrics { get; set; }
        public DbSet<EmployeeKPIMetricItem> EmployeeKPIMetricItems { get; set; }
        public DbSet<EmployeeWithPositionWork> EmployeeWithPositionWorks { get; set; }
        public DbSet<OrganizationBusinessProcessActivity> OrganizationBusinessProcessActivities { get; set; }

        #endregion

        #region 平台门户与综合信息服务子系统
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleComment> ArticleComments { get; set; }
        public DbSet<ArticleCommentTag> ArticleCommentTags { get; set; }
        public DbSet<ArticleCommentWithTag> ArticleCommentWithTags { get; set; }
        public DbSet<ArticleInTopic> ArticleInTopics { get; set; }
        public DbSet<ArticleInType> ArticleInTypes { get; set; }
        public DbSet<ArticleRelevance> ArticleRelevances { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<ArticleTopic> ArticleTopics { get; set; }
        public DbSet<ArticleType> ArticleTypes { get; set; }
        public DbSet<ArticleWithTag> ArticleWithTags { get; set; }
        #endregion

        #region 课程资源管理部分
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseItem> CourseItems { get; set; }
        public DbSet<CourseItemContent> CourseItemContents { get; set; }
        public DbSet<CourseWithRoles> CourseWithRoles { get; set; }
        public DbSet<CourseWithUsers> CourseWithUsers { get; set; }
        #endregion

        #region Demo 示例
        public DbSet<DemoEntityParent> DemoEntityParents { get; set; }
        public DbSet<DemoEntity> DemoEntities { get; set; }
        public DbSet<DemoEntityItem> DemoEntityItems { get; set; }
        public DbSet<DemoCommon> DemoCommons { get; set; }
        public DbSet<DemoItemForMultiSelect> DemoItemForMultiSelects { get; set; }
        public DbSet<DemoEntityWithDemoItemForMultiSelect> DemoEntityWithDemoItemForMultiSelects { get; set; }
        #endregion


        public DbSet<MusicDemo> MDemo { get; set; }
        public DbSet<MusicCore> MusicCores { get; set; }
        //public DbSet<MusicUser> MusicUsers { get; set; }
        #region 音乐堡垒实体
        //public DbSet<Album> Albums { get; set; }//
        //public DbSet<AlbumWithMusics> AlbumWithMusics { get; set; }
        public DbSet<MusicEntity> Music { get; set; }
        public DbSet<MusicTypeEntity> MusicType { get; set; }

        //public DbSet<MusicManagement> MusicManagements { get; set; }

        //public DbSet<Power> Powers { get; set; }
        //public DbSet<UserInfo> UserInfos { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
        }
    }
}

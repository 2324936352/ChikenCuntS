using LPFW.EntitiyModels.ApplicationCommon.AppPathAndMenu;
using LPFW.EntitiyModels.Demo;
using LPFW.ORM;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPFW.DataAccess.Seeds
{
    public class ApplicationDataSeed
    {
        static LpDbContext _entitiesContext;

        public static void InitialEntity(LpDbContext dbContext)
        {
            _entitiesContext = dbContext;
            _ForDemoEntities();
            _ForApplicationMenuGroupAndMenuItem();
        }

        private static void _ForDemoEntities()
        {
            if (!_entitiesContext.DemoEntityParents.Any())
            {
                var demoEntityParent01 = new DemoEntityParent() { Name = "种类001", Description = "简要说明", SortCode = "demoEntityParent01" };
                var demoEntityParent02 = new DemoEntityParent() { Name = "种类002", Description = "简要说明", SortCode = "demoEntityParent02" };
                var demoEntityParent03 = new DemoEntityParent() { Name = "种类003", Description = "简要说明", SortCode = "demoEntityParent03" };
                var demoEntityParent04 = new DemoEntityParent() { Name = "种类004", Description = "简要说明", SortCode = "demoEntityParent04" };
                var demoEntityParent05 = new DemoEntityParent() { Name = "种类005", Description = "简要说明", SortCode = "demoEntityParent05" };
                var demoEntityParent0501 = new DemoEntityParent() { Name = "种类005001", Description = "简要说明", SortCode = "demoEntityParent0501" };
                var demoEntityParent050101 = new DemoEntityParent() { Name = "种类005001001", Description = "简要说明", SortCode = "demoEntityParent0050101" };

                demoEntityParent01.Parent = demoEntityParent01;
                demoEntityParent02.Parent = demoEntityParent02;
                demoEntityParent03.Parent = demoEntityParent03;
                demoEntityParent04.Parent = demoEntityParent04;
                demoEntityParent05.Parent = demoEntityParent05;
                demoEntityParent0501.Parent = demoEntityParent05;
                demoEntityParent050101.Parent = demoEntityParent0501;

                _entitiesContext.DemoEntityParents.Add(demoEntityParent01);
                _entitiesContext.DemoEntityParents.Add(demoEntityParent02);
                _entitiesContext.DemoEntityParents.Add(demoEntityParent03);
                _entitiesContext.DemoEntityParents.Add(demoEntityParent04);
                _entitiesContext.DemoEntityParents.Add(demoEntityParent05);
                _entitiesContext.DemoEntityParents.Add(demoEntityParent0501);
                _entitiesContext.DemoEntityParents.Add(demoEntityParent050101);

                _entitiesContext.SaveChanges();
            }

            if (!_entitiesContext.DemoEntities.Any())
            { 
                var dep01 = _entitiesContext.DemoEntityParents.FirstOrDefault(x=>x.Name=="种类001");
                var dep02 = _entitiesContext.DemoEntityParents.FirstOrDefault(x => x.Name == "种类003");
                var dep03 = _entitiesContext.DemoEntityParents.FirstOrDefault(x => x.Name == "种类005");
                var dep04 = _entitiesContext.DemoEntityParents.FirstOrDefault(x => x.Name == "种类005001");
                var dep05 = _entitiesContext.DemoEntityParents.FirstOrDefault(x => x.Name == "种类005001001");

                for (int i = 1; i < 201; i++)
                {
                    var suffix = i.ToString();
                    if (suffix.Length ==1 )
                        suffix = "00" + suffix;
                    if (suffix.Length == 2)
                        suffix = "0" + suffix;


                    var demoEntity = new DemoEntity()
                    {
                        Name = "样例数据" + suffix,
                        Description = dep01.Name + "-" + suffix,
                        Email = "demo" + suffix + "@outlook.com",
                        Mobile = "13617808" + suffix,
                        DemoEntityParent=dep01,
                    };

                    _entitiesContext.DemoEntities.Add(demoEntity);
                }
                for (int i = 201; i < 401; i++)
                {
                    var suffix = i.ToString();
                    if (suffix.Length == 1)
                        suffix = "00" + suffix;
                    if (suffix.Length == 2)
                        suffix = "0" + suffix;


                    var demoEntity = new DemoEntity()
                    {
                        Name = "样例数据" + suffix,
                        Description = dep02.Name + "-" + suffix,
                        Email = "demo" + suffix + "@outlook.com",
                        Mobile = "13617808" + suffix,
                        DemoEntityParent = dep02,
                    };
                    _entitiesContext.DemoEntities.Add(demoEntity);
                }
                for (int i = 401; i < 601; i++)
                {
                    var suffix = i.ToString();
                    if (suffix.Length == 1)
                        suffix = "00" + suffix;
                    if (suffix.Length == 2)
                        suffix = "0" + suffix;


                    var demoEntity = new DemoEntity()
                    {
                        Name = "样例数据" + suffix,
                        Description = dep03.Name + "-" + suffix,
                        Email = "demo" + suffix + "@outlook.com",
                        Mobile = "13617808" + suffix,
                        DemoEntityParent = dep03,
                    };
                    _entitiesContext.DemoEntities.Add(demoEntity);
                }
                for (int i = 601; i < 801; i++)
                {
                    var suffix = i.ToString();
                    if (suffix.Length == 1)
                        suffix = "00" + suffix;
                    if (suffix.Length == 2)
                        suffix = "0" + suffix;


                    var demoEntity = new DemoEntity()
                    {
                        Name = "样例数据" + suffix,
                        Description = dep04.Name + "-" + suffix,
                        Email = "demo" + suffix + "@outlook.com",
                        Mobile = "13617808" + suffix,
                        DemoEntityParent = dep04,
                    };
                    _entitiesContext.DemoEntities.Add(demoEntity);
                }
                for (int i = 801; i < 1001; i++)
                {
                    var suffix = i.ToString();
                    if (suffix.Length == 1)
                        suffix = "00" + suffix;
                    if (suffix.Length == 2)
                        suffix = "0" + suffix;


                    var demoEntity = new DemoEntity()
                    {
                        Name = "样例数据" + suffix,
                        Description = dep05.Name + "-" + suffix,
                        Email = "demo" + suffix + "@outlook.com",
                        Mobile = "13617808" + suffix,
                        DemoEntityParent = dep05,
                    };
                    _entitiesContext.DemoEntities.Add(demoEntity);
                }

                _entitiesContext.SaveChanges();
            }

        }

        private static void _ForApplicationMenuGroupAndMenuItem()
        {
            // 子系统清单
            if (!_entitiesContext.ApplicationMenuGroups.Any())
            {
                var groupCollection = new List<ApplicationMenuGroup>()
                {
                    
                    new ApplicationMenuGroup(){ Name="用户", SortCode="MG002", Description="",PortalUrl ="/Admin"  },
                    new ApplicationMenuGroup(){ Name="管理员", SortCode="MG001", Description="", PortalUrl ="/News" },
                    new ApplicationMenuGroup(){ Name="市场经理", SortCode="MG003", Description="",PortalUrl ="/Teacher"  },
                    //new ApplicationMenuGroup(){ Name="学生课程学习中心", SortCode="MG004", Description="",PortalUrl ="/Student"  },
                    //new ApplicationMenuGroup(){ Name="站点文章数据管理", SortCode="MG005", Description="",PortalUrl ="/News"  },
                    //new ApplicationMenuGroup(){ Name="框架应用基础演示", SortCode="MG006", Description="",PortalUrl ="/XDemo/DemoCommon"  },
                };
                _entitiesContext.ApplicationMenuGroups.AddRange(groupCollection);
                _entitiesContext.SaveChanges();
                //
            }

            if (!_entitiesContext.ApplicationMenuItems.Any())
            {
                #region 基础数据数据管理
                var group001 = _entitiesContext.ApplicationMenuGroups.FirstOrDefault(x => x.Name == "用户");
                // 菜单分类
                var menuItemCollection001 = new List<ApplicationMenuItem>()
                {
                    new ApplicationMenuItem(){ Name="音乐后台菜单应用", IconString="fa fa-bars", SortCode="MG001-001", UrlString="", ApplicationMenuGroup=group001 },
                    new ApplicationMenuItem(){ Name="系统角色与用户管理", IconString="fa fa-users", SortCode="MG001-002", UrlString="", ApplicationMenuGroup=group001 },
                    //new ApplicationMenuItem(){ Name="常规实体管理演示", IconString="fa fa-cog", SortCode="MG001-003", UrlString="", ApplicationMenuGroup=group001 },
                };
                foreach (var item in menuItemCollection001)
                    item.ParentItem = item;

                var parentMenuItem001 = menuItemCollection001.FirstOrDefault(x => x.Name == "音乐后台菜单应用");
                // 具体的菜单
                var menuItemCollection001001 = new List<ApplicationMenuItem>()
                {
                    new ApplicationMenuItem(){ Name="音乐菜单分区管理", IconString="", SortCode="MG001-001-001", UrlString="/Admin/ApplicationMenuGroup", ApplicationMenuGroup=group001, ParentItem = parentMenuItem001 },
                    new ApplicationMenuItem(){ Name="音乐菜单条目管理", IconString="", SortCode="MG001-001-002", UrlString="/Admin/ApplicationMenuItem", ApplicationMenuGroup=group001, ParentItem = parentMenuItem001 }
                };

                var parentMenuItem002 = menuItemCollection001.FirstOrDefault(x => x.Name == "系统角色与用户管理");
                var menuItemCollection001002 = new List<ApplicationMenuItem>()
                {
                    new ApplicationMenuItem(){ Name="音乐用户组管理", IconString="", SortCode="MG001-002-001", UrlString="/Admin/ApplicationRole", ApplicationMenuGroup=group001, ParentItem = parentMenuItem002 },
                    new ApplicationMenuItem(){ Name="音乐用户管理", IconString="", SortCode="MG001-002-002", UrlString="/Admin/ApplicationUser", ApplicationMenuGroup=group001, ParentItem = parentMenuItem002 }
                };
                _entitiesContext.ApplicationMenuItems.AddRange(menuItemCollection001);
                _entitiesContext.ApplicationMenuItems.AddRange(menuItemCollection001001);
                _entitiesContext.ApplicationMenuItems.AddRange(menuItemCollection001002);
                #endregion

                #region 组织和教学资源管理
                var group002 = _entitiesContext.ApplicationMenuGroups.FirstOrDefault(x => x.Name == "管理员");
                var parentMenuItem004 = new ApplicationMenuItem() { Name = "音乐管理", IconString = "fa fa-users", SortCode = "DP001", UrlString = "#", ApplicationMenuGroup = group002 };
                parentMenuItem004.ParentItem = parentMenuItem004;

                var menuItemCollection004001 = new List<ApplicationMenuItem>()
                {
                    new ApplicationMenuItem(){ Name="音乐类型", IconString="", SortCode="DP001-001", UrlString="/Music/MusicType", ApplicationMenuGroup=group002, ParentItem = parentMenuItem004 },
                    new ApplicationMenuItem(){ Name="音乐数据", IconString="", SortCode="DP001-002", UrlString="/Music/Music", ApplicationMenuGroup=group002, ParentItem = parentMenuItem004 },
                    new ApplicationMenuItem(){ Name="专辑管理", IconString="", SortCode="DP001-004", UrlString="/Music/Album", ApplicationMenuGroup=group002, ParentItem = parentMenuItem004 },
                    new ApplicationMenuItem(){ Name="用户数据管理", IconString="", SortCode="DP001-005", UrlString="/Music/UserInfo", ApplicationMenuGroup=group002, ParentItem = parentMenuItem004 }
                };
                _entitiesContext.ApplicationMenuItems.Add(parentMenuItem004);
                _entitiesContext.ApplicationMenuItems.AddRange(menuItemCollection004001);

                var parentMenuItem005 = new ApplicationMenuItem() { Name = "音乐堡垒数据维护", IconString = "fa fa-desktop", SortCode = "DP002", UrlString = "#", ApplicationMenuGroup = group002 };
                parentMenuItem005.ParentItem = parentMenuItem005;

                var menuItemCollection004002 = new List<ApplicationMenuItem>()
                {
                    new ApplicationMenuItem(){ Name="音乐分类管理", IconString="", SortCode="DP002-001", UrlString="/TeachingBusiness/CourseContainer", ApplicationMenuGroup=group002, ParentItem = parentMenuItem005 },
                    new ApplicationMenuItem(){ Name="音乐堡垒管理", IconString="", SortCode="DP002-002", UrlString="/TeachingBusiness/Course", ApplicationMenuGroup=group002, ParentItem = parentMenuItem005 },
                };
                _entitiesContext.ApplicationMenuItems.Add(parentMenuItem005);
                _entitiesContext.ApplicationMenuItems.AddRange(menuItemCollection004002);
                #endregion

                #region 教师课程数据管理
                var group003 = _entitiesContext.ApplicationMenuGroups.FirstOrDefault(x => x.Name == "市场经理");

                #endregion

                #region 学生课程学习中心
                var group004 = _entitiesContext.ApplicationMenuGroups.FirstOrDefault(x => x.Name == "音乐中心");

                #endregion

                #region 站点新闻数据管理.
                var group005 = _entitiesContext.ApplicationMenuGroups.FirstOrDefault(x => x.Name == "音乐文章数据管理");

                #endregion

                #region 框架演示
                var group006 = _entitiesContext.ApplicationMenuGroups.FirstOrDefault(x => x.Name == "框架应用基础演示");
                var parentMenuItem003 = new ApplicationMenuItem() { Name = "常规实体管理演示", IconString = "fa fa-cog", SortCode = "MG001-003", UrlString = "", ApplicationMenuGroup = group006 };
                parentMenuItem003.ParentItem = parentMenuItem003;

                var menuItemCollection001003 = new List<ApplicationMenuItem>()
                {
                    new ApplicationMenuItem(){ Name="音乐实体", IconString="", SortCode="MG006-003-001", UrlString="/XDemo/DemoCommon", ApplicationMenuGroup=group006, ParentItem = parentMenuItem003 },
                    new ApplicationMenuItem(){ Name="无分页列表", IconString="", SortCode="MG006-003-002", UrlString="/Xdemo/DemoEntity", ApplicationMenuGroup=group006, ParentItem = parentMenuItem003 },
                    new ApplicationMenuItem(){ Name="分页列表", IconString="", SortCode="MG006-003-003", UrlString="/XDemo/DemoEntityPagination", ApplicationMenuGroup=group006, ParentItem = parentMenuItem003 },
                    new ApplicationMenuItem(){ Name="层次结构对象", IconString="", SortCode="MG006-003-004", UrlString="/Xdemo/DemoEntityParent", ApplicationMenuGroup=group006, ParentItem = parentMenuItem003 }
                };
                _entitiesContext.ApplicationMenuItems.Add(parentMenuItem003);
                _entitiesContext.ApplicationMenuItems.AddRange(menuItemCollection001003); 
                #endregion

                _entitiesContext.SaveChanges();
            }
        }
    }
}

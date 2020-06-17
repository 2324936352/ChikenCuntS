using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.ApplicationCommon.Attachments;
using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.EntitiyModels.BusinessCommon;
using LPFW.EntitiyModels.OrganzationBusiness;
using LPFW.ViewModels.BusinessCommon;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels.OrganizationBusiness;
using LPFW.ViewModelServices.Tools;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LPFW.ViewModelServices.Extensions.OrganizationBusiness
{
    /// <summary>
    /// 针对  <see cref="IViewModelService{Department, DepartmentVM}" /> 的视图模型服务的扩展方法
    /// </summary>
    public static class DepartmentModelServiceExtension
    {
        /// <summary>
        /// 获取部门视图模型对象集合
        /// </summary>
        /// <param name="service"></param>
        /// <param name="listPageParameter">单页数据显示定义模型对象</param>
        /// <param name="navigatorPredicate">过滤条件</param>
        /// <param name="includeProperty">包含的关联关系</param>
        /// <returns>部门</returns>
        public static async Task<List<DepartmentVM>> GetDepartmentVMCollection(
            this IViewModelService<Department, DepartmentVM> service, 
            ListSinglePageParameter listPageParameter, 
            Expression<Func<Department, bool>> navigatorPredicate, 
            Expression<Func<Department, object>> includeProperty
            )
        {
            var boCollection = await service.EntityRepository.GetBoCollectionAsyn(listPageParameter, navigatorPredicate, includeProperty);
            var selfReferentialItemCollection = SelfReferentialItemFactory<Department>.GetCollection(boCollection.ToList(), true);
            var boVMCollection = new List<DepartmentVM>();
            int count = 0;
            foreach (var item in boCollection.OrderBy(x => x.SortCode))
            {
                var boVM = new DepartmentVM();
                item.MapToViewModel<Department, DepartmentVM>(boVM);
                boVM.OrderNumber = (++count).ToString();

                // 处理名称缩进，看起来有层次感
                var sItem = selfReferentialItemCollection.FirstOrDefault(x => x.ID == item.Id.ToString());
                if (sItem != null)
                    boVM.Name = sItem.DisplayName;

                var typeId = item.Id;
                // 提取员工数量
                boVM.EmployeeAmount = await service.EntityRepository.GetOtherBoAmountAsyn<Employee>(x=>x.Department.Id==typeId, y=>y.Department);
                // 提取岗位数量
                boVM.PositionAmount = await service.EntityRepository.GetOtherBoAmountAsyn<Position>(x => x.Department.Id == typeId, y => y.Department);

                boVMCollection.Add(boVM);
            }

            return boVMCollection;

        }

        /// <summary>
        /// 设置部门对象关联数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task GetboVMRelevanceData(this IViewModelService<Department, DepartmentVM> service, DepartmentVM boVM)
        {
            var boId = boVM.Id;

            var bo = await service.EntityRepository.GetBoAsyn(boId, x => x.Parent, y => y.Organization, z => z.ApplicationRole,i=>i.Address);
            if (bo == null)
                bo = new Department();

            // 设置当前对象的上级部门的视图模型数据
            if (bo.Parent != null)
            {
                boVM.ParentId = bo.Parent.Id.ToString();
                boVM.ParentName = bo.Parent.Name;
            }
            // 设置供给前端视图模型选择上级元素时的下拉选项集合
            var sourceItems01 = await service.EntityRepository.GetOtherBoCollectionAsyn<Department>();
            boVM.ParentItemCollection = SelfReferentialItemFactory<Department>.GetCollection(sourceItems01.OrderBy(x => x.SortCode).ToList(), true);

            // 设置归属单位
            if (bo.Organization != null)
            {
                boVM.OrganizationId = bo.Organization.Id.ToString();
                boVM.OrganizationName = bo.Organization.Name;
            }
            var sourceItems02 = await service.EntityRepository.GetOtherBoCollectionAsyn<Organization>();
            boVM.OrganizationItemCollection = PlainFacadeItemFactory<Organization>.Get(sourceItems02.OrderBy(x => x.SortCode).ToList());

            // 设置关联的角色组
            if (bo.ApplicationRole != null)
            {
                boVM.ApplicationRoleId = bo.ApplicationRole.Id.ToString();
                boVM.ApplicationRoleName = bo.ApplicationRole.Name;
            }

            // 设置联系地址
            if (bo.Address != null)
                boVM.AddressVM = new CommonAddressVM(bo.Address);
            else
                boVM.AddressVM = new CommonAddressVM();
        }

        #region 配置部门基础信息
        /// <summary>
        /// 根据部门归属的单位处理部门的关联数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <param name="groupId">单位对象 Id</param>
        /// <returns></returns>
        public static async Task GetboVMRelevanceData(this IViewModelService<Department, DepartmentVM> service, DepartmentVM boVM, string groupId)
        {
            var boId = boVM.Id;

            var bo = await service.EntityRepository.GetBoAsyn(boId, x => x.Parent, y => y.Organization, z => z.ApplicationRole, w => w.Address);
            if (bo == null)
                bo = new Department();

            // 设置上级关系的视图模型数据
            if (bo.Parent != null)
            {
                boVM.ParentId = bo.Parent.Id.ToString();
                boVM.ParentName = bo.Parent.Name;
            }
            // 设置供给前端视图模型选择上级元素时的下拉选项集合，这时候限制其可选的下拉数据，都是归属指定 groupId 的菜单条
            var sourceItems01 = await service.EntityRepository.GetOtherBoCollectionAsyn<Department>(x => x.Organization.Id == Guid.Parse(groupId), y => y.Organization);
            boVM.ParentItemCollection = SelfReferentialItemFactory<Department>.GetCollection(sourceItems01.OrderBy(x => x.SortCode).ToList(), true);

            // 设置选择关联的部门属性
            var menuGroup = await service.EntityRepository.GetOtherBoAsyn<Organization>(Guid.Parse(groupId));
            bo.Organization = menuGroup;
            if (bo.Organization != null)
            {
                boVM.OrganizationId = bo.Organization.Id.ToString();
                boVM.OrganizationName = bo.Organization.Name;
            }
            var sourceItems02 = await service.EntityRepository.GetOtherBoCollectionAsyn<Organization>();
            boVM.OrganizationItemCollection = PlainFacadeItemFactory<Organization>.Get(sourceItems02.OrderBy(x => x.SortCode).ToList());

            // 设置关联的角色组
            if (bo.ApplicationRole != null)
            {
                boVM.ApplicationRoleId = bo.ApplicationRole.Id.ToString();
                boVM.ApplicationRoleName = bo.ApplicationRole.Name;
            }

            // 设置联系地址
            if (bo.Address != null)
                boVM.AddressVM = new CommonAddressVM(bo.Address);
            else
                boVM.AddressVM = new CommonAddressVM();

        }

        /// <summary>
        /// 初始化配置部门相关基础数据：负责人、联系人、部门层级链
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task SetVMForConfig(this IViewModelService<Department, DepartmentVM> service, DepartmentVM boVM)
        {
            var boId = boVM.Id;
            var bo = await service.EntityRepository.GetBoAsyn(boId, x => x.Parent, y => y.Organization, z => z.ApplicationRole);

            boVM.DepartmentLinkCollection = await SelfReferentialItemFactory<Department>.GetCollectionToRootAsyn(service.EntityRepository, bo);

            #region 单位负责人
            var leaderCollection = await service.EntityRepository.GetOtherBoCollectionAsyn<DepartmentLeader>(x => x.Department.Id == bo.Id, y => y.Department, z => z.Leader);
            var leader = leaderCollection.FirstOrDefault();
            if (leader != null)
            {
                var employee = await service.EntityRepository.GetOtherBoAsyn<Employee>(leader.Leader.Id, x => x.User, z => z.Address);
                var addr = "未配置联系地址。";
                if (employee.Address != null)
                    if (!String.IsNullOrEmpty(employee.Address.Description))
                        addr = employee.Address.Description;

                boVM.LeaderVM = new SimplifiedPersonVM()
                {
                    Id = employee.Id.ToString(),
                    Name = employee.Name,
                    Mobile = employee.Mobile,
                    Email = employee.Email,
                    Summary = employee.Description,
                    RelevanceObjectId = bo.Id,
                    JobTitle = "部门负责人",
                    Address = addr
                };
                // 提取头像
                if (employee.User != null)
                {
                    var userImage = await service.EntityRepository.GetOtherBoAsyn<BusinessImage>(x => x.RelevanceObjectID == employee.User.Id);
                    if (userImage != null)
                        boVM.LeaderVM.AvatarPath = userImage.UploadPath;
                    else
                        boVM.LeaderVM.AvatarPath = "/images/avatar/avatar-1.png";
                }
            }
            else
            {
                boVM.LeaderVM = new SimplifiedPersonVM()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "请选择部门负责人",
                    Mobile = "",
                    Email = "",
                    Summary = "",
                    JobTitle = "选择后匹配",
                    RelevanceObjectId = bo.Id,
                    AvatarPath = "/images/avatar/avatar-5.png"
                };
            }
            #endregion

            #region 业务联系人
            var contactCollection = await service.EntityRepository.GetOtherBoCollectionAsyn<DepartmentContact>(x => x.Department.Id == bo.Id, y => y.Department, z => z.Contact);
            var contact = contactCollection.FirstOrDefault();
            if (contact != null)
            {
                var employee = await service.EntityRepository.GetOtherBoAsyn<Employee>(contact.Contact.Id, x => x.User, z => z.Address);
                var addr = "未配置联系地址。";
                if (employee.Address != null)
                    if (!String.IsNullOrEmpty(employee.Address.Description))
                        addr = employee.Address.Description;

                boVM.ContactVM = new SimplifiedPersonVM()
                {
                    Id = employee.Id.ToString(),
                    Name = employee.Name,
                    Mobile = employee.Mobile,
                    Email = employee.Email,
                    Summary = employee.Description,
                    RelevanceObjectId = bo.Id,
                    Address = addr,
                    JobTitle = "业务联系人"
                };
                // 提取头像
                if (employee.User != null)
                {
                    var userImage = await service.EntityRepository.GetOtherBoAsyn<BusinessImage>(x => x.RelevanceObjectID == employee.User.Id);
                    if (userImage != null)
                        boVM.ContactVM.AvatarPath = userImage.UploadPath;
                    else
                        boVM.ContactVM.AvatarPath = "/images/avatar/avatar-1.png";
                }
            }
            else
            {
                boVM.ContactVM = new SimplifiedPersonVM()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "请选择业务联系人",
                    Mobile = "",
                    Email = "",
                    Summary = "",
                    JobTitle = "选择后匹配",
                    RelevanceObjectId = bo.Id,
                    AvatarPath = "/images/avatar/avatar-5.png"
                };
            }
            #endregion
        }

        /// <summary>
        /// 保存视图模型到持久层，包括关联的归属单位、归属上级部门、联系地址、系统角色。
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task<SaveStatusModel> SaveBoWithRelevanceDataAsyn(this IViewModelService<Department, DepartmentVM> service, DepartmentVM boVM)
        {
            var saveStatus = new SaveStatusModel() { SaveSatus = true, Message = "" };
            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.GetBoAsyn(boVM.Id, x => x.Address);
            if (bo == null)
                bo = new Department();

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel(bo);

            // 处理关联的归属单位
            if (!String.IsNullOrEmpty(boVM.OrganizationId))
            {
                var id = Guid.Parse(boVM.OrganizationId);
                var item = await service.EntityRepository.GetOtherBoAsyn<Organization>(id);
                bo.Organization = item;
            }

            // 处理关联的上级单位
            if (!String.IsNullOrEmpty(boVM.ParentId))
            {
                var parentId = Guid.Parse(boVM.ParentId);
                var parentItem = await service.EntityRepository.GetOtherBoAsyn<Department>(parentId);
                bo.Parent = parentItem;
            }
            else
                bo.Parent = bo;

            // 处理联系地址
            if (boVM.AddressVM != null)
                boVM.AddressVM.UpdateCommonAddress(bo.Address);

            // 持久化处理
            saveStatus = await service.EntityRepository.SaveBoAsyn(bo);
            if (saveStatus.SaveSatus == false)
                return saveStatus;

            // 处理关联的角色组
            var masterRole = await service.EntityRepository.ApplicationRoleManager.FindByIdAsync(bo.Id.ToString());
            if (masterRole == null)
            {
                masterRole = new ApplicationRole()
                {
                    Id = bo.Id,
                    Name = bo.Name + "|" + bo.Id.ToString(), // 在同名时通过附件的 Id 区别
                    Description = bo.Name + "|" + bo.CreateDate,
                    DisplayName = bo.Name,
                    SortCode = bo.SortCode
                };

                // 设置上级角色组
                var parentRole = await service.EntityRepository.ApplicationRoleManager.FindByIdAsync(bo.Parent.Id.ToString());
                if (parentRole != null)
                    masterRole.ParentRole = parentRole;
                else
                    masterRole.ParentRole = masterRole;

                // 持久化
                var result = await service.EntityRepository.ApplicationRoleManager.CreateAsync(masterRole);
                if (!result.Succeeded)
                {
                    saveStatus.SaveSatus = false;
                    saveStatus.Message = "无法创建缺省的角色组。";
                    return saveStatus;
                }

                // 为角色添加声明标识
                await service.EntityRepository.ApplicationRoleManager.AddClaimAsync(masterRole, new System.Security.Claims.Claim("类型", "部门"));
                await service.EntityRepository.ApplicationRoleManager.AddClaimAsync(masterRole, new System.Security.Claims.Claim("级别", "新建单位时创建"));
                await service.EntityRepository.ApplicationRoleManager.AddClaimAsync(masterRole, new System.Security.Claims.Claim("宿主单位", boVM.ParentId));
            }

            // 执行持久化处理
            saveStatus = await service.EntityRepository.SaveBoAsyn(bo);
            return saveStatus;
        }

        /// <summary>
        /// 处理配置部门负责人持久化
        /// </summary>
        /// <param name="service"></param>
        /// <param name="employeeId"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public static async Task SaveDepartmentLeader(this IViewModelService<Department, DepartmentVM> service, Guid employeeId, Guid deptId)
        {
            var department = await service.EntityRepository.GetBoAsyn(deptId);
            var employee = await service.EntityRepository.GetOtherBoAsyn<Employee>(employeeId);
            var departmentLeader = await service.EntityRepository.GetOtherBoAsyn<DepartmentLeader>(x => x.Department.Id == deptId && x.Leader.Id == employeeId, y => y.Department, z => z.Leader);
            if (departmentLeader == null)
            {
                departmentLeader = new DepartmentLeader() { Department = department, Leader = employee };
            }
            else
            {
                departmentLeader.Leader = employee;
                departmentLeader.Department = department;
            }

            await service.EntityRepository.SaveOtherBoAsyn<DepartmentLeader>(departmentLeader);
        }

        /// <summary>
        /// 处理配置部门业务联系人持久化
        /// </summary>
        /// <param name="service"></param>
        /// <param name="employeeId"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public static async Task SaveDepartmentContact(this IViewModelService<Department, DepartmentVM> service, Guid employeeId, Guid deptId)
        {
            var department = await service.EntityRepository.GetBoAsyn(deptId);
            var employee = await service.EntityRepository.GetOtherBoAsyn<Employee>(employeeId);
            var departmentContact = await service.EntityRepository.GetOtherBoAsyn<DepartmentContact>(x => x.Department.Id == deptId && x.Contact.Id == employeeId, y => y.Department, z => z.Contact);
            if (departmentContact == null)
            {
                departmentContact = new DepartmentContact() { Department = department, Contact = employee };
            }
            else
            {
                departmentContact.Contact = employee;
                departmentContact.Department = department;
            }

            await service.EntityRepository.SaveOtherBoAsyn<DepartmentContact>(departmentContact);
        } 
        #endregion

        #region 配置下级部门相关的扩展
        /// <summary>
        /// 根据指定的上级部门 Id，和当前部门 Id，构建一个下级部门的视图模型对象
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static async Task<DepartmentVM> GetBoVMAsyn(this IViewModelService<Department, DepartmentVM> service, Guid id, Guid parentId) 
        {
            var boVM = await service.GetBoVMAsyn(id);
            var bo = await service.EntityRepository.GetBoAsyn(id, x => x.Parent, y => y.Organization, z => z.ApplicationRole);
            if (bo == null)
                bo = new Department();

            var parent = await service.EntityRepository.GetBoAsyn(parentId, x => x.Parent, y => y.Organization, z => z.ApplicationRole);
            if (bo.Parent != null)
            {
                boVM.ParentId = bo.Parent.Id.ToString();
                boVM.ParentName = bo.Parent.Name;
            }
            else
            {
                boVM.ParentId   = parent.Id.ToString();
                boVM.ParentName = parent.Name;
            }

            if (bo.Organization != null)
            {
                boVM.OrganizationId = bo.Organization.Id.ToString();
                boVM.OrganizationName = bo.Organization.Name;
            }
            else 
            {
                boVM.OrganizationId   = parent.Organization.Id.ToString();
                boVM.OrganizationName = parent.Organization.Name;
            }

            // 设置关联的角色组
            if (bo.ApplicationRole != null)
            {
                boVM.ApplicationRoleId = bo.ApplicationRole.Id.ToString();
                boVM.ApplicationRoleName = bo.ApplicationRole.Name;
            }

            // 设置联系地址
            if (bo.Address != null)
                boVM.AddressVM = new CommonAddressVM(bo.Address);
            else
                boVM.AddressVM = new CommonAddressVM();

            return boVM;
        }
        #endregion

        #region 配置部门员工基础数据相关的扩展

        public static async Task<EmployeeVM> GetEmployeeVMAsyn(this IViewModelService<Department, DepartmentVM> service, Guid id, Guid deptId) 
        {
            var boVM = await service.GetOtherBoVM<Employee, EmployeeVM>(id);
            var bo = await service.EntityRepository.GetOtherBoAsyn<Employee>(id, w => w.Department, x => x.Position, y => y.User, z => z.Address);
            if (bo == null)
                bo = new Employee();

            // 归属部门相关
            if (bo.Department != null)
            {
                boVM.DepartmentId = bo.Department.Id.ToString();
                boVM.DepartmentName = bo.Department.Name;
            }
            else 
            {
                var dept = await service.EntityRepository.GetBoAsyn(deptId);
                boVM.DepartmentId   = dept.Id.ToString();
                boVM.DepartmentName = dept.Name;
            }

            var sourceItems01 = await service.EntityRepository.GetOtherBoCollectionAsyn<Department>();
            boVM.DepartmentItemCollection = SelfReferentialItemFactory<Department>.GetCollection(sourceItems01.OrderBy(x => x.SortCode).ToList(), true);

            // 归属岗位相关
            if (bo.Position != null)
            {
                boVM.PositionId = bo.Position.Id.ToString();
                boVM.PositionName = bo.Position.Name;
            }
            var sourceItems02 = await service.EntityRepository.GetOtherBoCollectionAsyn<Position>();
            boVM.PositionItemCollection = PlainFacadeItemFactory<Position>.Get(sourceItems02.OrderBy(x => x.SortCode).ToList());

            // 设置关联用户相关
            if (bo.User != null)
            {
                boVM.ApplicationUserId = bo.User.Id.ToString();
                boVM.ApplicationUserName = bo.User.UserName;
            }

            // 设置联系地址
            if (bo.Address != null)
            {
                var addressVM = new CommonAddressVM(bo.Address);
                boVM.AddressVM = addressVM;
            }
            else
                boVM.AddressVM = new CommonAddressVM();

            return boVM;
        }

        public static async Task<SaveStatusModel> SaveEmployeeVMAsyn(this IViewModelService<Department, DepartmentVM> service, EmployeeVM boVM) 
        {
            var organizationId = "";   // 员工归属的组织机构的 Id
            var prevDepartmentId = ""; // 员工数据更新之前的部门 Id
            var saveStatus = new SaveStatusModel() { SaveSatus = true, Message = "" };
            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.GetOtherBoAsyn<Employee>(boVM.Id);
            if (bo == null)
                bo = new Employee();

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel(bo);

            // 处理关联的部门
            if (!String.IsNullOrEmpty(boVM.DepartmentId))
            {
                var id = Guid.Parse(boVM.DepartmentId);
                var item = await service.EntityRepository.GetOtherBoAsyn<Department>(id, x => x.Organization);
                bo.Department = item;
                // 提取关联的组织机构 id
                organizationId = item.Organization.Id.ToString();

            }

            // 处理关联的岗位
            if (!String.IsNullOrEmpty(boVM.PositionId))
            {
                var id = Guid.Parse(boVM.PositionId);
                var item = await service.EntityRepository.GetOtherBoAsyn<Position>(id);
                bo.Position = item;
            }

            // 处理关联的用户
            if (String.IsNullOrEmpty(boVM.ApplicationUserId))   // 尚未关联用户
            {
                bo.User = new EntitiyModels.ApplicationCommon.RoleAndUser.ApplicationUser();
                bo.User.Id = bo.Id;
                bo.User.UserName = bo.PersonalCode;  // 使用员工工号作为缺省的用户名  
                bo.User.Name = bo.Department.Name + "_" + bo.Name;
                bo.User.Email = bo.Email;
                bo.User.PhoneNumber = bo.Mobile;
                bo.User.Description = bo.Description;

                //// 使用缺省密码创建用户
                //var result = await service.EntityRepository.ApplicationUserManager.CreateAsync(bo.User, "1234@Abcd");
                //if (result != IdentityResult.Success)
                //{
                //    // 返回错误信息
                //    saveStatus.SaveSatus = false;
                //    saveStatus.Message = "初始化员工用户失败。";
                //}

                //// 使用缺省密码 1234@Abcd  创建用户
                //var userRole = await service.EntityRepository.ApplicationRoleManager.FindByIdAsync(bo.Department.Id.ToString());

                //if (userRole != null)
                //{
                //    await service.EntityRepository.ApplicationUserManager.AddToRoleAsync(bo.User, userRole.Name);
                //}

                //// 维护员工用户的标识内容
                //var userClaims = new List<System.Security.Claims.Claim>()
                //{
                //    new System.Security.Claims.Claim("宿主角色组", userRole.Id.ToString()),
                //    new System.Security.Claims.Claim("宿主单位", organizationId),
                //    new System.Security.Claims.Claim("宿主部门", boVM.DepartmentId)
                //};
                //await service.EntityRepository.ApplicationUserManager.AddClaimsAsync(bo.User, userClaims);

            }
            else  // 已经关联角色组
            {
                var user = await service.EntityRepository.ApplicationUserManager.FindByIdAsync(boVM.ApplicationUserId);
                user.UserName = bo.Id.ToString();
                user.Name = bo.Department.Name + "_" + bo.Name;
                user.Email = bo.Email;
                user.PhoneNumber = bo.Mobile;
                user.Description = bo.Description;
                // 更新用户信息
                await service.EntityRepository.ApplicationUserManager.UpdateAsync(user);
                bo.User = user;

                // 处理部门变更的角色组变更
                if (prevDepartmentId != boVM.DepartmentId)
                {
                    var prevRole = await service.EntityRepository.ApplicationRoleManager.FindByIdAsync(prevDepartmentId);
                    if (prevRole != null)
                    {
                        // 移除原来归属的用户组
                        await service.EntityRepository.ApplicationUserManager.RemoveFromRoleAsync(bo.User, prevRole.Name);
                        // 添加到当前归属部门的用户组
                        var newRole = await service.EntityRepository.ApplicationRoleManager.FindByIdAsync(boVM.DepartmentId);
                        if (newRole != null)
                        {
                            await service.EntityRepository.ApplicationUserManager.AddToRoleAsync(bo.User, newRole.Name);

                            #region 处理用户Claim
                            var userClaimCollection = await service.EntityRepository.ApplicationUserManager.GetClaimsAsync(bo.User);

                            var roleClaim = userClaimCollection.FirstOrDefault(x => x.Type == "宿主角色组");
                            await service.EntityRepository.ApplicationUserManager.RemoveClaimAsync(bo.User, roleClaim);

                            var deptClaim = userClaimCollection.FirstOrDefault(x => x.Type == "宿主部门");
                            await service.EntityRepository.ApplicationUserManager.RemoveClaimAsync(bo.User, deptClaim);

                            var userClaims = new List<System.Security.Claims.Claim>()
                            {
                                new System.Security.Claims.Claim("宿主角色组", boVM.DepartmentId),
                                new System.Security.Claims.Claim("宿主部门", boVM.DepartmentId)
                            };
                            await service.EntityRepository.ApplicationUserManager.AddClaimsAsync(bo.User, userClaims);
                            #endregion
                        }
                    }
                }

            }

            // 处理关联的员工地址
            //var tempAddr = await service.EntityRepository.GetOtherBoAsyn<CommonAddress>(boVM.EmployeeAddressVM.Id);
            //if (tempAddr != null)
            //    boVM.EmployeeAddressVM.MapToBo(tempAddr);  // 将视图模型的地址数据映射到 tempAddr
            //else
            //{
            //    tempAddr = new CommonAddress();
            //    boVM.EmployeeAddressVM.MapToBo(tempAddr);  // 将视图模型的地址数据映射到 tempAddr
            //}
            //// 持久化 tempAddr 并返回到 bo.Address
            //bo.Address = await service.EntityRepository.SaveOtherBoAsyn<CommonAddress>(tempAddr);

            // 执行持久化处理
            saveStatus = await service.EntityRepository.SaveOtherBoWithStatusAsyn<Employee>(bo);

            return saveStatus;
        }

        public static async Task<bool> IsUniqueEmployee(this IViewModelService<Department, DepartmentVM> service, string personalCode) 
        {
            var target = await service.EntityRepository.GetOtherBoAmountAsyn<Employee>(x => x.PersonalCode == personalCode);
            if (target > 0)
                return false;
            else
                return true;
        }

        public static async Task<DeleteStatusModel> DeleteEmployee(this IViewModelService<Department, DepartmentVM> service, Guid id)
        {
            return await service.EntityRepository.DeleteOtherBoAsyn<Employee>(id);
        }

        #endregion

        #region 配置部门岗位基础数据相关的扩展方法
        public static async Task<PositionVM> GetPositionVMAsyn(this IViewModelService<Department, DepartmentVM> service, Guid id, Guid deptId)
        {
            var boVM = await service.GetOtherBoVM<Position, PositionVM>(id);
            var bo = await service.EntityRepository.GetOtherBoAsyn<Position>(id, w => w.Department);
            if (bo == null)
                bo = new Position();

            // 归属部门相关
            if (bo.Department != null)
            {
                boVM.DepartmentId = bo.Department.Id.ToString();
                boVM.DepartmentName = bo.Department.Name;
            }
            else
            {
                var dept = await service.EntityRepository.GetBoAsyn(deptId);
                boVM.DepartmentId = dept.Id.ToString();
                boVM.DepartmentName = dept.Name;
            }
            var sourceItems01 = await service.EntityRepository.GetOtherBoCollectionAsyn<Department>();
            boVM.DepartmentItemCollection = SelfReferentialItemFactory<Department>.GetCollection(sourceItems01.OrderBy(x => x.SortCode).ToList(), true);

            return boVM;
        }

        public static async Task<SaveStatusModel> SavePositionVMAsyn(this IViewModelService<Department, DepartmentVM> service, PositionVM boVM) 
        {
            var saveStatus = new SaveStatusModel() { SaveSatus = true, Message = "" };
            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.GetOtherBoAsyn<Position>(boVM.Id);
            if (bo == null)
                bo = new Position();

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel(bo);

            // 处理关联的上级单位
            if (!String.IsNullOrEmpty(boVM.DepartmentId))
            {
                var parentId = Guid.Parse(boVM.DepartmentId);
                var parentItem = await service.EntityRepository.GetBoAsyn(parentId);
                bo.Department = parentItem;
            }

            // 执行持久化处理
            saveStatus = await service.EntityRepository.SaveOtherBoWithStatusAsyn<Position>(bo);
            return saveStatus;
        }

        public static async Task<DeleteStatusModel> DeletePosition(this IViewModelService<Department, DepartmentVM> service, Guid id)
        {
            return await service.EntityRepository.DeleteOtherBoAsyn<Position>(id);
        }
        #endregion

        #region 配置部门绩效指标
        /// <summary>
        /// 根据指定的部门 id ，返回部门的全部的 KPI 视图模型对象集合
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<List<DepartmentKPIVM>> GetDepartmentKPIVMCollectionAsyn(this IViewModelService<Department, DepartmentVM> service, Guid id)
        {
            var result = new List<DepartmentKPIVM>();
            var dept = await service.EntityRepository.GetBoAsyn(id, x => x.DepartmentKPICollection);

            if (dept.DepartmentKPICollection != null)
            {
                int count = 0;
                foreach (var item in dept.DepartmentKPICollection.OrderBy(x => x.SortCode))
                {
                    var boVM = new DepartmentKPIVM();
                    item.MapToViewModel<DepartmentKPI, DepartmentKPIVM>(boVM);
                    boVM.OrderNumber = (++count).ToString();
                    boVM.DepartmentId = id.ToString();
                    result.Add(boVM);
                }
            }
            return result;
        }

        /// <summary>
        /// 提取部门 KPI 指标视图模型对象，如果没有，则新建一个 KPI 对象，并且限定其归属部门由部门 deptId 确定
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id">KPI 对象 id</param>
        /// <param name="deptId">归属部门对象 id</param>
        /// <returns></returns>
        public static async Task<DepartmentKPIVM> GeDepartmentKPIVMAsyn(this IViewModelService<Department, DepartmentVM> service, Guid id, Guid deptId)
        {
            var boVM = await service.GetOtherBoVM<DepartmentKPI, DepartmentKPIVM>(id);
            var bo = await service.EntityRepository.GetOtherBoAsyn<DepartmentKPI>(id, w => w.KPIType);
            if (bo == null)
                bo = new DepartmentKPI();

            // 归属部门相关
            if (bo.KPIType != null)
            {
                boVM.OrganizationKPITypeId = bo.KPIType.Id.ToString();
                boVM.OrganizationKPITypeName = bo.KPIType.Name;
            }
            var sourceItems02 = await service.EntityRepository.GetOtherBoCollectionAsyn<OrganizationKPIType>();
            boVM.OrganizationKPITypeItemCollection = PlainFacadeItemFactory<OrganizationKPIType>.Get(sourceItems02.OrderBy(x => x.SortCode).ToList());
            boVM.DepartmentId = deptId.ToString();
            return boVM;
        }

        /// <summary>
        /// 保存部门 KPI 指标数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task<SaveStatusModel> SaveDepartmentKPIAsyn(this IViewModelService<Department, DepartmentVM> service, DepartmentKPIVM boVM)
        {
            var isNew = false;
            var saveStatus = new SaveStatusModel() { SaveSatus = true, Message = "" };
            var dept = await service.EntityRepository.GetOtherBoAsyn<Department>(Guid.Parse(boVM.DepartmentId), w => w.DepartmentKPICollection);

            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.GetOtherBoAsyn<DepartmentKPI>(boVM.Id);
            if (bo == null)
            {
                isNew = true;
                bo = new DepartmentKPI(); 
            }

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel(bo);

            // 处理关联的上级单位
            if (!String.IsNullOrEmpty(boVM.OrganizationKPITypeId))
            {
                var typeId = Guid.Parse(boVM.OrganizationKPITypeId);
                bo.KPIType = await service.EntityRepository.GetOtherBoAsyn<OrganizationKPIType>(typeId); 
            }

            // 执行持久化处理
            if (isNew)
            {
                dept.DepartmentKPICollection.Add(bo);
                saveStatus = await service.EntityRepository.SaveOtherBoWithStatusAsyn<DepartmentKPI>(bo);
            }
            else
            {
                saveStatus = await service.EntityRepository.SaveOtherBoWithStatusAsyn<DepartmentKPI>(bo);
            }
            return saveStatus;
        }

        #endregion

        /// <summary>
        /// 提取 Organization 的所有数据作为前端导航树的数据节点
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static async Task<List<TreeNodeForBootStrapTreeView>> GetTreeViewNodeByOrganization(this IViewModelService<Department, DepartmentVM> service,Guid id)
        {
            var itemCollection = await service.EntityRepository.GetOtherBoCollectionAsyn<Organization>(x => x.TransactionCenterRegister.Id == id, t => t.TransactionCenterRegister);
            var selfReferentialItemCollection = new List<SelfReferentialItem>();
            foreach (var item in itemCollection.OrderBy(x => x.SortCode))
            {
                var sItem = new SelfReferentialItem()
                {
                    ID = item.Id.ToString(),
                    ParentID = item.Id.ToString(),
                    DisplayName = item.Name,
                    SortCode = item.SortCode
                };
                selfReferentialItemCollection.Add(sItem);
            }
            // 构建树节点集合
            var result = TreeViewFactoryForBootSrapTreeView.GetTreeNodes(selfReferentialItemCollection);

            return result;
        }
    }
}

using LPFW.DataAccess.Tools;
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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LPFW.ViewModelServices.Extensions.OrganizationBusiness
{
    /// <summary>
    /// 针对  <see cref="IViewModelService{Employee, EmployeeVM}" /> 的视图模型服务的扩展方法
    /// </summary>
    public static class EmployeeModelServiceExtension
    {
        /// <summary>
        /// 为简单创建的员工视图模型对象配置关联属性，以及在编辑员工信息时提供相关的选项数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task GetboVMRelevanceData(this IViewModelService<Employee, EmployeeVM> service, EmployeeVM boVM)
        {
            var boId = boVM.Id;

            var bo = await service.EntityRepository.GetBoAsyn(boId, w => w.Department, x => x.Position, y => y.User, z => z.Address); 
            if (bo == null)
                bo = new Employee();

            // 归属部门相关
            if (bo.Department != null)
            {
                boVM.DepartmentId = bo.Department.Id.ToString();
                boVM.DepartmentName = bo.Department.Name;
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

        }

        /// <summary>
        /// 根据指定归属部门 Id 处理关联数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <param name="groupId">部门分区对象 Id</param>
        /// <returns></returns>
        public static async Task GetboVMRelevanceData(this IViewModelService<Employee, EmployeeVM> service, EmployeeVM boVM, string typeId)
        {
            var boId = boVM.Id;

            var bo = await service.EntityRepository.GetBoAsyn(boId, w => w.Department, x => x.Position, y => y.User, z => z.Address);
            if (bo == null)
                bo = new Employee();

            // 设置上级关系的视图模型数据
            if (bo.Department != null)
            {
                boVM.DepartmentId = bo.Department.Id.ToString();
                boVM.DepartmentName = bo.Department.Name;
            }
            else
            {
                // 为新建的视图模型对象直接指定归属实体对象
                var deaultType = await service.EntityRepository.GetOtherBoAsyn<Department>(Guid.Parse(typeId));
                boVM.DepartmentId = deaultType.Id.ToString();
                boVM.DepartmentName = deaultType.Name;
            }
            var sourceItems01 = await service.EntityRepository.GetOtherBoCollectionAsyn<Department>();
            boVM.DepartmentItemCollection = SelfReferentialItemFactory<Department>.GetCollection(sourceItems01.OrderBy(x => x.SortCode).ToList(), true);

            // 归属岗位相关
            if (bo.Position != null)
            {
                boVM.PositionId = bo.Position.Id.ToString();
                boVM.PositionName = bo.Position.Name;
            }

            // 待选岗位限制在已经配置给这个部门的岗位里
            var deptID = Guid.Parse(typeId);
            var sourceItems02 = await service.EntityRepository.GetOtherBoCollectionAsyn<Position>(x => x.Department.Id == deptID, y => y.Department);
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
        }

        /// <summary>
        /// 保存员工数据并创建员工系统登录账号
        ///   1. 员工 Id 与对应的用户 Id 一致；
        ///   2. 缺省使用员工号和密码 1234@Abcd 创建用户登录账号；
        ///   3. 为员工登录账号配置宿主角色组、宿主机构、宿主部门的 Claim 标识。
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task<SaveStatusModel> SaveBoWithRelevanceDataAsyn(this IViewModelService<Employee, EmployeeVM> service, EmployeeVM boVM)
        {
            var registerId = "";       // 员工归属的单位注册
            var organizationId = "";   // 员工归属的单位的 Id
            var prevDepartmentId = ""; // 员工数据更新之前的部门 Id
            
            var saveStatus = new SaveStatusModel() { SaveSatus = true, Message = "" };
            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.GetBoAsyn(boVM.Id,x=>x.Department, x => x.Address);
            if (bo == null)
                bo = new Employee();
            else
            {
                prevDepartmentId = bo.Department.Id.ToString();
            }

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel(bo);

            // 处理关联的部门
            if (!String.IsNullOrEmpty(boVM.DepartmentId))
            {
                var id = Guid.Parse(boVM.DepartmentId);
                var item = await service.EntityRepository.GetOtherBoAsyn<Department>(id,x=>x.Organization);
                bo.Department = item;
                
                // 提取关联的单位 id
                organizationId = item.Organization.Id.ToString();
                // 提取关联的注册
                var roganization = await service.EntityRepository.GetOtherBoAsyn<Organization>(item.Organization.Id, x=>x.TransactionCenterRegister);
                registerId = roganization.TransactionCenterRegister.Id.ToString();
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
                bo.User = new ApplicationUser()
                {
                    UserName = boVM.PersonalCode,  // 使用员工工号作为缺省的用户名  
                    Name = boVM.Name,
                    Email = boVM.Email,
                    PhoneNumber = boVM.Mobile,
                    Description = boVM.Description
                };

                // 使用缺省密码 1234@Abcd  创建用户
                var result = await service.EntityRepository.ApplicationUserManager.CreateAsync(bo.User, "1234@Abcd");
                if (result != IdentityResult.Success)
                {
                    // 返回错误信息
                    saveStatus.SaveSatus = false;
                    saveStatus.Message = "初始化员工用户失败。";
                }

                // 提取用户缺省关联的角色组
                var userRole = await service.EntityRepository.ApplicationRoleManager.FindByIdAsync(bo.Department.Id.ToString());
                if (userRole != null)
                {
                    await service.EntityRepository.ApplicationUserManager.AddToRoleAsync(bo.User, userRole.Name);
                }

                if (String.IsNullOrEmpty(boVM.PositionId))
                    boVM.PositionId = "";

                // 维护员工用户的标识内容
                await service.CommonSetUserClarm(bo.User.UserName, UserCommonClaimEnum.用户类型, RoleCommonClaimEnum.平台业务.ToString());
                await service.CommonSetUserClarm(bo.User.UserName, UserCommonClaimEnum.注册资料, registerId);
                await service.CommonSetUserClarm(bo.User.UserName, UserCommonClaimEnum.单位, organizationId);
                await service.CommonSetUserClarm(bo.User.UserName, UserCommonClaimEnum.部门, boVM.DepartmentId);
                await service.CommonSetUserClarm(bo.User.UserName, UserCommonClaimEnum.岗位, boVM.PositionId);
                await service.CommonSetUserClarm(bo.User.UserName, UserCommonClaimEnum.岗位工作, boVM.PositionId);
                await service.CommonSetUserClarm(bo.User.UserName, UserCommonClaimEnum.宿主角色, userRole.Id.ToString());

            }
            else  // 已经关联角色组
            {
                var user = await service.EntityRepository.ApplicationUserManager.FindByIdAsync(boVM.ApplicationUserId);
                user.UserName = bo.PersonalCode;
                user.Name = bo.Name;
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
                            await service.CommonSetUserClarm(bo.User.Name, UserCommonClaimEnum.宿主角色, newRole.Id.ToString());
                        }
                    }
                }

                if (String.IsNullOrEmpty(boVM.PositionId))
                    boVM.PositionId = "";
                // 维护员工用户的标识内容
                await service.CommonSetUserClarm(bo.User.UserName, UserCommonClaimEnum.用户类型, RoleCommonClaimEnum.平台业务.ToString());
                await service.CommonSetUserClarm(bo.User.UserName, UserCommonClaimEnum.注册资料, registerId);
                await service.CommonSetUserClarm(bo.User.UserName, UserCommonClaimEnum.单位, organizationId);
                await service.CommonSetUserClarm(bo.User.UserName, UserCommonClaimEnum.部门, boVM.DepartmentId);
                await service.CommonSetUserClarm(bo.User.UserName, UserCommonClaimEnum.岗位, boVM.PositionId);
                await service.CommonSetUserClarm(bo.User.UserName, UserCommonClaimEnum.岗位工作, "");
            }

            // 处理关联的员工地址
            if (boVM.AddressVM != null)
                boVM.AddressVM.UpdateCommonAddress(bo.Address);

            // 执行持久化处理
            saveStatus = await service.EntityRepository.SaveBoAsyn(bo);
            return saveStatus;
        }

        public static async Task<SaveStatusModel> AdjustEmployeeDepartmentAsync(this IViewModelService<Employee, EmployeeVM> service, Guid id,Guid deptId) 
        {
            var saveStatus = new SaveStatusModel() { SaveSatus = true, Message = "" };

            var bo = await service.EntityRepository.GetBoAsyn(id, w => w.Department,y=>y.User);
            var prevDepartmentId = bo.Department.Id;

            var dept = await service.EntityRepository.GetOtherBoAsyn<Department>(deptId);
            bo.Department = dept;

            // 处理部门变更的角色组变更
            if (prevDepartmentId != deptId)
            {
                var prevRole = await service.EntityRepository.ApplicationRoleManager.FindByIdAsync(prevDepartmentId.ToString());
                if (prevRole != null)
                {
                    // 移除原来归属的用户组
                    await service.EntityRepository.ApplicationUserManager.RemoveFromRoleAsync(bo.User, prevRole.Name);
                    // 添加到当前归属部门的用户组
                    var newRole = await service.EntityRepository.ApplicationRoleManager.FindByIdAsync(deptId.ToString());
                    if (newRole != null)
                    {
                        await service.EntityRepository.ApplicationUserManager.AddToRoleAsync(bo.User, newRole.Name);

                        var userClaimCollection = await service.EntityRepository.ApplicationUserManager.GetClaimsAsync(bo.User);

                        var roleClaim = userClaimCollection.FirstOrDefault(x => x.Type == "宿主角色组");
                        await service.EntityRepository.ApplicationUserManager.RemoveClaimAsync(bo.User, roleClaim);

                        var deptClaim = userClaimCollection.FirstOrDefault(x => x.Type == "宿主部门");
                        await service.EntityRepository.ApplicationUserManager.RemoveClaimAsync(bo.User, deptClaim);

                        var userClaims = new List<System.Security.Claims.Claim>()
                            {
                                new System.Security.Claims.Claim("宿主角色组", deptId.ToString()),
                                new System.Security.Claims.Claim("宿主部门", deptId.ToString())
                            };
                        await service.EntityRepository.ApplicationUserManager.AddClaimsAsync(bo.User, userClaims);
                    }
                }
            }

            // 执行持久化处理
            saveStatus = await service.EntityRepository.SaveBoAsyn(bo);
            return saveStatus;

        }

        public static async Task<DepartmentVM> GetDefaultDepartmentVM(this IViewModelService<Employee, EmployeeVM> service, string organizationId) 
        {
            var deptVM = new DepartmentVM();
            // 提取缺省部门 Id
            var oId = Guid.Parse(organizationId);
            var deptCollection = await service.EntityRepository.GetOtherBoCollectionAsyn<Department>(x => x.Organization.Id == oId, y => y.Organization);
            var dept = deptCollection.OrderBy(x => x.SortCode).FirstOrDefault();
            if (dept != null)
            {
                deptVM = await service.GetOtherBoVM<Department, DepartmentVM>(dept.Id);
            }

            return deptVM;
        }
    }
}

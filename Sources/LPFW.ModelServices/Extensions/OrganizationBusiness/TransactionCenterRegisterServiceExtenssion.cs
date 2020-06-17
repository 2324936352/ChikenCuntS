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
using System.Text;
using System.Threading.Tasks;

namespace LPFW.ViewModelServices.Extensions.OrganizationBusiness
{
    /// <summary>
    /// 针对  <see cref="IViewModelService{TransactionCenterRegister,TransactionCenterRegisterVM}" /> 的视图模型服务的扩展方法
    /// </summary>
    public static class TransactionCenterRegisterServiceExtenssion
    {
        /// <summary>
        /// 检查是不是已经存在交易中心缺省实例，如果没有则创建一个，并且以后只能有着一个
        /// 目前分别在 TransactionCenter/Home 和 Admin/Home 中调用，以后需要处理掉
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static async Task<TransactionCenterRegisterVM> InitialDeaultTransactionCenterRegister(this IViewModelService<TransactionCenterRegister, TransactionCenterRegisterVM> service)
        {
            var bo =  await service.EntityRepository.GetBoAsyn();
            if (bo == null)
            {
                // 创建交易中心实例并持久化
                bo = new TransactionCenterRegister()
                {
                    Name = "自定义在线教学平台管理中心",
                    Description = "在线教学平台服务管理中心。",
                    Mobile ="13900000000",
                    Email ="LightPoint@outlook.com",
                    BusinessEntityStatusEnum = EntitiyModels.BusinessCommon.Status.BusinessEntityStatusEnum.完成,
                    Address= new EntitiyModels.BusinessCommon.CommonAddress()
                    {
                        Id = Guid.NewGuid(),
                        SortCode = "",
                        Name = "中国",
                        ProvinceName = "广西",
                        CityName = "柳州",
                        CountyName = "鱼峰区",
                        DetailName = "社湾路28号柳州职业技术学院竞择楼 888"
                    }
                };
                await service.EntityRepository.SaveBoAsyn(bo);

                // 创建缺省角色组
                var masterRole = await service.EntityRepository.ApplicationRoleManager.FindByIdAsync(bo.Id.ToString());
                if (masterRole == null)
                {
                    masterRole = new ApplicationRole()
                    {
                        Id          = bo.Id,
                        Name        = bo.Name,
                        Description = bo.Name + bo.CreateTime,
                        DisplayName = "平台系统管理员",
                        SortCode    = bo.SortCode
                    };
                    // 作为根角色组
                    masterRole.ParentRole = masterRole;
                    // 持久化
                    await service.EntityRepository.ApplicationRoleManager.CreateAsync(masterRole);
                    // 为角色添加声明标识
                    await service.EntityRepository.ApplicationRoleManager.AddClaimAsync(masterRole, new System.Security.Claims.Claim("类型", "交易中心"));
                    await service.EntityRepository.ApplicationRoleManager.AddClaimAsync(masterRole, new System.Security.Claims.Claim("级别", "应用时创建"));
                    await service.EntityRepository.ApplicationRoleManager.AddClaimAsync(masterRole, new System.Security.Claims.Claim("宿主单位", "自己"));
                }

                // 创建缺省的平台管理员用户
                var user = new ApplicationUser()
                {
                    UserName ="administrator",
                    Name ="Light Point",
                    PhoneNumber =bo.Mobile,
                    Email =bo.Email
                };
                // 持久化
                await service.EntityRepository.ApplicationUserManager.CreateAsync(user, "1234@Abcd");
                // 用户应该归属的宿主角色组
                await service.EntityRepository.ApplicationUserManager.AddToRoleAsync(user, masterRole.Name);
                // 设置用户的标识
                var userClaims = new List<System.Security.Claims.Claim>()
                {
                    new System.Security.Claims.Claim("宿主角色组", masterRole.Id.ToString()),
                    new System.Security.Claims.Claim("宿主单位", masterRole.Id.ToString()),
                    new System.Security.Claims.Claim("宿主部门", masterRole.Id.ToString())
                };
                await service.EntityRepository.ApplicationUserManager.AddClaimsAsync(user, userClaims);
            }

            var boVM = await service.GetBoVMAsyn(bo.Id, x => x.Address);

            return boVM;
        }

        /// <summary>
        /// 为营运组织注册资料视图模型配置关联数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task GetboVMRelevanceData(this IViewModelService<TransactionCenterRegister, TransactionCenterRegisterVM> service, TransactionCenterRegisterVM boVM)
        {
            var boId = boVM.Id;

            var bo = await service.EntityRepository.GetBoAsyn(boId, x => x.Address);
            if (bo == null)
                bo = new TransactionCenterRegister();

            // 设置联系地址
            if (bo.Address != null)
                boVM.AddressVM = new CommonAddressVM(bo.Address);
            else
                boVM.AddressVM = new CommonAddressVM();

            // 设置审核状态，获取与枚举值相关的处理
            if (!boVM.IsNew)
            {
                boVM.BusinessEntityStatusEnum = bo.BusinessEntityStatusEnum;
                boVM.BusinessEntityStatusEnumName = bo.BusinessEntityStatusEnum.ToString();
            }
            boVM.BusinessEntityStatusEnumItemCollection = PlainFacadeItemFactory<TransactionCenterRegister>.GetByEnum(bo.BusinessEntityStatusEnum);

        }

        /// <summary>
        /// 持久化营运组织注册资料
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task<SaveStatusModel> SaveBoWithRelevanceDataAsyn(this IViewModelService<TransactionCenterRegister, TransactionCenterRegisterVM> service, TransactionCenterRegisterVM boVM)
        {
            var saveStatus = new SaveStatusModel() { SaveSatus = true, Message = "" };
            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.GetBoAsyn(boVM.Id, x => x.Address);
            if (bo == null)
                bo = new TransactionCenterRegister();

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel(bo);

            // 处理联系地址
            if (boVM.AddressVM != null)
                boVM.AddressVM.UpdateCommonAddress(bo.Address);

            // 处理审核状态
            bo.BusinessEntityStatusEnum = boVM.BusinessEntityStatusEnum;

            // 执行持久化处理
            saveStatus = await service.EntityRepository.SaveBoAsyn(bo);
            if (saveStatus.SaveSatus == false)
                return saveStatus;

            // 对于新的注册资料，创建维护的用户和对应的用户组
            if (boVM.IsNew)
            {
                // 创建缺省的角色组
                var masterRole = await service.EntityRepository.ApplicationRoleManager.FindByIdAsync(bo.Id.ToString());
                if (masterRole == null)
                {
                    masterRole = new ApplicationRole()
                    {
                        Id = bo.Id,
                        Name = bo.Name,
                        Description = bo.Name + bo.CreateTime,
                        DisplayName = bo.Name,
                        SortCode = bo.SortCode
                    };
                    // 作为根角色组
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
                    await service.EntityRepository.ApplicationRoleManager.AddClaimAsync(masterRole, new System.Security.Claims.Claim("类型", "营运组织"));
                    await service.EntityRepository.ApplicationRoleManager.AddClaimAsync(masterRole, new System.Security.Claims.Claim("级别", "注册时创建"));
                    await service.EntityRepository.ApplicationRoleManager.AddClaimAsync(masterRole, new System.Security.Claims.Claim("宿主单位", "自己"));
                }

                // 创建缺省的管理员用户
                var user = new ApplicationUser
                {
                    UserName = bo.AdminUserName,
                    Email = bo.Email,
                    PhoneNumber = bo.Mobile
                };
                var userResult = await service.EntityRepository.ApplicationUserManager.CreateAsync(user, bo.Password);
                if (userResult == IdentityResult.Success)
                {
                    // 获取用户应该归属的宿主角色组
                    await service.EntityRepository.ApplicationUserManager.AddToRoleAsync(user, masterRole.Name);
                    // 设置用户的标识
                    var userClaims = new List<System.Security.Claims.Claim>()
                {
                    new System.Security.Claims.Claim("宿主角色组", masterRole.Id.ToString()),
                    new System.Security.Claims.Claim("宿主单位", masterRole.Id.ToString()),
                    new System.Security.Claims.Claim("宿主部门", masterRole.Id.ToString())
                };
                    await service.EntityRepository.ApplicationUserManager.AddClaimsAsync(user, userClaims);
                }
                else
                {
                    saveStatus.SaveSatus = false;
                    saveStatus.Message = "创建用户失败。";
                }
            }

            return saveStatus;
        }

    }
}

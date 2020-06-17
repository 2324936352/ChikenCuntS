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
using System.Text;
using System.Threading.Tasks;

namespace LPFW.ViewModelServices.Extensions.OrganizationBusiness
{
    /// <summary>
    /// 针对  <see cref="IViewModelService{Organization,OrganizationVM}" /> 的视图模型服务的扩展方法
    /// </summary>
    public static class OrganizationModelServiceExtension
    {
        /// <summary>
        /// 设置组织对象的常规关联关系
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task GetboVMRelevanceData(this IViewModelService<Organization, OrganizationVM> service, OrganizationVM boVM)
        {
            var boId = boVM.Id;

            var bo = await service.EntityRepository.GetBoAsyn(boId, x => x.Address);
            if (bo == null)
                bo = new Organization();

            // 设置联系地址
            if (bo.Address != null)
                boVM.AddressVM = new CommonAddressVM(bo.Address);
            else
                boVM.AddressVM = new CommonAddressVM();
        }

        /// <summary>
        /// 根据单位 boId 和 营运组织 Id，返回单位视图模型对象
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boId">单位对象 Id</param>
        /// <param name="typeID">营运组织对象 Id</param>
        /// <returns></returns>
        public static async Task<OrganizationVM> GetOrganizationVM(this IViewModelService<Organization, OrganizationVM> service, Guid boId, Guid typeID)
        {
            var bo = await service.EntityRepository.GetBoAsyn(boId, a => a.Address, t => t.TransactionCenterRegister, l => l.OrganizationLeader, c => c.OrganzationContact);
            var boVM = ConversionForViewModelHelper.GetFromEntity<Organization, OrganizationVM>(bo);

            if (bo == null)
                bo = new Organization();

            // 设置联系地址
            if (bo.Address != null)
                boVM.AddressVM = new CommonAddressVM(bo.Address);
            else
                boVM.AddressVM = new CommonAddressVM();

            // 设置归属的营运组织（交易中心）
            if (bo.TransactionCenterRegister == null)
            {
                var transactionCenterRegister      = await service.EntityRepository.GetOtherBoAsyn<TransactionCenterRegister>(typeID);
                boVM.TransactionCenterRegisterId   = transactionCenterRegister.Id.ToString();
                boVM.TransactionCenterRegisterName = transactionCenterRegister.Name;
            }
            else 
            {
                boVM.TransactionCenterRegisterId   = bo.TransactionCenterRegister.Id.ToString();
                boVM.TransactionCenterRegisterName = bo.TransactionCenterRegister.Name;
            }
            var sourceItems = await service.EntityRepository.GetOtherBoCollectionAsyn<TransactionCenterRegister>();
            boVM.TransactionCenterRegisterItemCollection = PlainFacadeItemFactory<TransactionCenterRegister>.Get(sourceItems.OrderBy(x => x.SortCode).ToList());

            // 处理单位负责人
            if (bo.OrganizationLeader != null) 
            {
                boVM.TransactionCenterRegisterId = bo.OrganizationLeader.Id.ToString();
                boVM.OrganizationLeaderName      = bo.OrganizationLeader.Name;
                boVM.OrganizationLeaderMobile    = bo.OrganizationLeader.Mobile;
                boVM.OrganizationLeaderEmail     = bo.OrganizationLeader.Email;
                boVM.OrganizationLeaderSummary   = "联系电话：" + boVM.OrganizationLeaderMobile + "；电子邮件：<a href=‘mailto:" + boVM.OrganizationLeaderEmail + "’>" + boVM.OrganizationLeaderEmail + "</a>";
            }
            // 处理单位联系人
            if (bo.OrganzationContact != null)
            {
                boVM.OrganzationContactId      = bo.OrganzationContact.Id.ToString();
                boVM.OrganzationContactName    = bo.OrganzationContact.Name;
                boVM.OrganzationContactMobile  = bo.OrganzationContact.Mobile;
                boVM.OrganzationContactEmail   = bo.OrganzationContact.Email;
                boVM.OrganzationContactSummary = "联系电话：" + boVM.OrganzationContactMobile + "；电子邮件：<a href=‘mailto:" + boVM.OrganzationContactEmail + "’>" + boVM.OrganzationContactEmail + "</a>";
            }

            return boVM;
        }

        /// <summary>
        /// 持久化营运单位信息， 持久化处理之后，还需要：
        ///   1. 创建或编辑一个角色组，
        ///   2. 将对应的用户加入这个角色组
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task<SaveStatusModel> SaveBoWithRelevanceDataAsyn(this IViewModelService<Organization, OrganizationVM> service, OrganizationVM boVM)
        {
            var saveStatus = new SaveStatusModel() { SaveSatus = true, Message = "" };
            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.GetBoAsyn(boVM.Id, x => x.Address);
            if (bo == null)
                bo = new Organization();

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel(bo);

            // 处理归属营运组织
            var tId = Guid.Parse(boVM.TransactionCenterRegisterId);
            var transactionCenterRegister = await service.EntityRepository.GetOtherBoAsyn<TransactionCenterRegister>(tId);
            bo.TransactionCenterRegister = transactionCenterRegister;

            // 处理联系地址
            if (boVM.AddressVM != null)
                boVM.AddressVM.UpdateCommonAddress(bo.Address);

            // 执行持久化处理
            saveStatus = await service.EntityRepository.SaveBoAsyn(bo);
            if (saveStatus.SaveSatus == false)
                return saveStatus;

            #region 1.创建缺省的角色组
            var masterRole = await service.EntityRepository.ApplicationRoleManager.FindByIdAsync(bo.Id.ToString());
            if (masterRole == null)
            {
                masterRole = new ApplicationRole()
                {
                    Id          = bo.Id,
                    Name        = bo.Name,
                    Description = bo.Name + "_" + bo.CreateTime,
                    DisplayName = bo.Name,
                    SortCode    = bo.SortCode
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
                await service.EntityRepository.ApplicationRoleManager.AddClaimAsync(masterRole, new System.Security.Claims.Claim("类型", "营运单位"));
                await service.EntityRepository.ApplicationRoleManager.AddClaimAsync(masterRole, new System.Security.Claims.Claim("级别", "新建单位时创建"));
                await service.EntityRepository.ApplicationRoleManager.AddClaimAsync(masterRole, new System.Security.Claims.Claim("宿主单位", boVM.TransactionCenterRegisterId));
            }
            #endregion

            // 创建缺省的管理员用户
            var user = await service.EntityRepository.ApplicationUserManager.FindByNameAsync(bo.AdminUserName);
            if (user == null) 
            {
                user = new ApplicationUser
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

        /// <summary>
        /// 根据用户名、电子邮件和移动电话检查是否存在相应的用户
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task<bool> HasUserAsyn(this IViewModelService<Organization, OrganizationVM> service, OrganizationVM boVM)
        {
            return await service.EntityRepository.HasOtherBoAsyn<ApplicationUser>(x => x.UserName == boVM.AdminUserName && x.Email == boVM.Email && x.PhoneNumber == boVM.Mobile);
        }

        /// <summary>
        /// 提取 TransactionCenterRegister 的所有数据作为前端导航树的数据节点
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static async Task<List<TreeNodeForBootStrapTreeView>> GetTreeViewNodeByParent(this IViewModelService<Organization, OrganizationVM> service)
        {
            var menuGroupCollection = await service.EntityRepository.GetOtherBoCollectionAsyn<TransactionCenterRegister>();
            var selfReferentialItemCollection = new List<SelfReferentialItem>();
            foreach (var item in menuGroupCollection.OrderBy(x => x.SortCode))
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

using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.ViewModels.ApplicationCommon.RoleAndUser;
using Microsoft.EntityFrameworkCore;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModelServices.Tools;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LPFW.Foundation.Tools;

namespace LPFW.ViewModelServices.Extensions.ApplicationCommon.UserAndUser
{
    public static class ApplicationUserModelServiceExtension
    {
        /// <summary>
        /// 专门为 ApplicationUser 定制的视图模型对象获取方法，这方法用于 CreateOrEdit
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<ApplicationUserVM> GetApplicationUserVMForCreateOrEdit(this IViewModelService<ApplicationUser, ApplicationUserVM> service, Guid id)
        {
            var isNew = false;
            var bo = await service.EntityRepository.ApplicationUserManager.FindByIdAsync(id.ToString());

            if (bo == null)
            {
                bo = new ApplicationUser();
                isNew = true;
            }
            var boVM = new ApplicationUserVM();
            boVM.IsNew = isNew;

            // 映射基本属性值
            bo.MapToViewModel<ApplicationUser, ApplicationUserVM>(boVM);

            // 获取用户关联的角色组名称集合
            var roleNameCollection = await service.EntityRepository.ApplicationUserManager.GetRolesAsync(bo);
            // 获取关联用户的角色组集合
            var roleCollection = new List<ApplicationRole>();
            foreach (var item in roleNameCollection)
            {
                roleCollection.Add(await service.EntityRepository.ApplicationRoleManager.FindByNameAsync(item));
            }

            boVM.ApplicationRoleId = new string[roleCollection.Count]; 
            boVM.ApplicationRoleName = new string[roleCollection.Count];
            roleCollection.OrderBy(x => x.SortCode);

            for(int i=0;i<roleCollection.Count;i++)
            {
                boVM.ApplicationRoleId[i] = roleCollection[i].Id.ToString();
                boVM.ApplicationRoleName[i] = roleCollection[i].Name;
            }

            // 设置供给前端视图模型选择上级元素时的下拉选项集合
            //var sourceItems01 = await service.EntityRepository.GetOtherBoCollectionAsyn<ApplicationRole>();
            //boVM.ApplicationRoleItemCollection = SelfReferentialItemFactory<ApplicationRole>.GetCollection(sourceItems01.OrderBy(x => x.SortCode).ToList(), true);

            return boVM;

        }

        public static async Task GetApplicationUserVMRelevanceData(this IViewModelService<ApplicationUser, ApplicationUserVM> service, ApplicationUserVM boVM)
        {
            var bo = await service.EntityRepository.ApplicationUserManager.FindByIdAsync(boVM.Id.ToString());
            if (bo == null)
                bo = new ApplicationUser();

            // 获取用户关联的角色组名称集合
            var roleNameCollection = await service.EntityRepository.ApplicationUserManager.GetRolesAsync(bo);
            // 获取关联用户的角色组集合
            var roleCollection = new List<ApplicationRole>();
            foreach (var item in roleNameCollection)
            {
                roleCollection.Add(await service.EntityRepository.ApplicationRoleManager.FindByNameAsync(item));
            }

            boVM.ApplicationRoleId = new string[roleCollection.Count];
            boVM.ApplicationRoleName = new string[roleCollection.Count];
            roleCollection.OrderBy(x => x.SortCode);

            for (int i = 0; i < roleCollection.Count; i++)
            {
                boVM.ApplicationRoleId[i] = roleCollection[i].Id.ToString();
                boVM.ApplicationRoleName[i] = roleCollection[i].Name;
            }

            // 设置供给前端视图模型选择上级元素时的下拉选项集合
            var sourceItems01 = await service.EntityRepository.GetOtherBoCollectionAsyn<ApplicationRole>();
            boVM.ApplicationRoleItemCollection = SelfReferentialItemFactory<ApplicationRole>.GetCollection(sourceItems01.OrderBy(x => x.SortCode).ToList(), true);
        }

        public static async Task<SaveStatusModel> SaveApplicationUserVMForCreateOrEdit(this IViewModelService<ApplicationUser, ApplicationUserVM> service, ApplicationUserVM boVM)
        {
            var saveStatus = new SaveStatusModel() { SaveSatus=true, Message="" };
            var isNew = false;
            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.ApplicationUserManager.FindByIdAsync(boVM.Id.ToString());
            if (bo == null)
            {
                bo = new ApplicationUser();
                isNew = true;
            }

            // 将视图模型的数据映射到实体模型并作持久化
            boVM.MapToEntityModel<ApplicationUserVM, ApplicationUser>(bo);
            if (isNew)
            {
                // 使用用户对象和密码创建新用户
                var result = await service.EntityRepository.ApplicationUserManager.CreateAsync(bo,boVM.Password);
                if (result != IdentityResult.Success)
                {
                    saveStatus.SaveSatus = false;
                    saveStatus.Message = "创建用户数据失败。";
                }
            }
            else
            {
                // 更新用户对象数据
                var result = await service.EntityRepository.ApplicationUserManager.UpdateAsync(bo);
                if (result != IdentityResult.Success)
                {
                    saveStatus.SaveSatus = false;
                    saveStatus.Message = "编辑用户数据失败。";
                }
            }

            // 更新用户关联的角色
            if(saveStatus.SaveSatus==true)
            {
                // 移除原有的角色组
                var roleNameCollection = await service.EntityRepository.ApplicationUserManager.GetRolesAsync(bo);
                await service.EntityRepository.ApplicationUserManager.RemoveFromRolesAsync(bo, roleNameCollection);

                // 重新添加角色组
                if (boVM.ApplicationRoleId != null)
                {
                    for (int i = 0; i < boVM.ApplicationRoleId.Length; i++)
                    {
                        var role = await service.EntityRepository.ApplicationRoleManager.FindByIdAsync(boVM.ApplicationRoleId[i]);
                        if (role != null)
                            await service.EntityRepository.ApplicationUserManager.AddToRoleAsync(bo, role.Name);
                    }
                }

            }

            // 更新用户组关联的声明标识，用于关联相关的处理因素
            var userClaimCollection = await service.EntityRepository.ApplicationUserManager.GetClaimsAsync(bo);
            var userClaim = userClaimCollection.FirstOrDefault(x => x.Type == "宿主角色组");
            if (userClaim == null)
            {
                userClaim = new System.Security.Claims.Claim("宿主角色组", "");
                await service.EntityRepository.ApplicationUserManager.AddClaimAsync(bo, userClaim);
            }
            else
            {
                // 删除原有的声明标识
                await service.EntityRepository.ApplicationUserManager.RemoveClaimAsync(bo, userClaim);
                userClaim = new System.Security.Claims.Claim("宿主角色组", "");
                await service.EntityRepository.ApplicationUserManager.AddClaimAsync(bo, userClaim);
            }
            //await service.EntityRepository.ApplicationUserManager.UpdateAsync(bo);

            return saveStatus;
        }

        public static async Task<DeleteStatusModel> DeleteApplicationUser(this IViewModelService<ApplicationUser, ApplicationUserVM> service, Guid id)
        {
            var deleteStatusModel = new DeleteStatusModel() { DeleteSatus = true, Message = "" };
            var user = await service.EntityRepository.ApplicationUserManager.FindByIdAsync(id.ToString());
            var result = await service.EntityRepository.ApplicationUserManager.DeleteAsync(user);
            if (result != IdentityResult.Success)
            {
                deleteStatusModel.DeleteSatus = false;
                deleteStatusModel.Message = "删除用户错误：其它依赖角色组相关的业务数据存在，不能删除。";
            }
            return deleteStatusModel;

        }

        public static async Task<List<ApplicationUserVM>> GetApplicationUserVMCollection(this IViewModelService<ApplicationUser, ApplicationUserVM> service, ListPageParameter paginateListPageParameter)
        {
            var boVMCollection = new List<ApplicationUserVM>();
            var defaultRole = service.EntityRepository.ApplicationRoleManager.Roles.OrderBy(x => x.SortCode).FirstOrDefault();
            if (defaultRole != null)
            {
                if (!String.IsNullOrEmpty(paginateListPageParameter.TypeID))
                {
                    defaultRole = await  service.EntityRepository.ApplicationRoleManager.FindByIdAsync(paginateListPageParameter.TypeID);
                }
                paginateListPageParameter.TypeName = defaultRole.Name;
                var pageIndex = Int16.Parse(paginateListPageParameter.PageIndex);
                var pageSize = Int16.Parse(paginateListPageParameter.PageSize);
                var keyword = "";

                if (!String.IsNullOrEmpty(paginateListPageParameter.Keyword))
                    keyword = paginateListPageParameter.Keyword;

                // 查询条件表达式
                //var predicateExpession = service.EntityRepository.GetConditionExpression(keyword);
                Expression<Func<ApplicationUser, bool>> predicateExpession = x =>
                       x.UserName.Contains(keyword) ||
                       x.Email.Contains(keyword) ||
                       x.Name.Contains(keyword) ||
                       x.Description.Contains(keyword) ||
                       x.PhoneNumber.Contains(keyword);

                var tempCollection = await service.EntityRepository.ApplicationUserManager.GetUsersInRoleAsync(defaultRole.Name);
                if (tempCollection.Count > 0)
                {
                    var tempUser = tempCollection.AsQueryable().Where(predicateExpession);

                    PaginatedList<ApplicationUser> boCollection = new PaginatedList<ApplicationUser>();
                    // 排序表达式
                    var sortExpession = LambdaHelper.GetPropertyExpression<ApplicationUser,object>(paginateListPageParameter.SortProperty);
                    // 排序
                    if (paginateListPageParameter.SortDesc.ToLower() == "descend")
                        boCollection = tempUser.OrderByDescending(sortExpession).ToPaginatedList(pageIndex, pageSize); 
                    else
                        boCollection = tempUser.OrderBy(sortExpession).ToPaginatedList(pageIndex, pageSize);

                    paginateListPageParameter.PageAmount = boCollection.TotalPageCount.ToString();
                    paginateListPageParameter.ObjectAmount = boCollection.TotalCount.ToString();
                    paginateListPageParameter.PagenateGroup = PagenateGroupRepository.GetItem<ApplicationUser>(boCollection, 10, pageIndex);
                    paginateListPageParameter.TypeName = defaultRole.Name;

                    // 初始化视图模型起始序号
                    int count = (int.Parse(paginateListPageParameter.PageIndex) - 1) * int.Parse(paginateListPageParameter.PageSize);
                    foreach (var user in boCollection)
                    {
                        var boVM = new ApplicationUserVM();
                        // 映射基本属性值
                        user.MapToViewModel<ApplicationUser, ApplicationUserVM>(boVM);

                        // 获取用户关联的角色组名称集合
                        var roleNameCollection = await service.EntityRepository.ApplicationUserManager.GetRolesAsync(user);
                        // 获取关联用户的角色组集合
                        var roleCollection = new List<ApplicationRole>();
                        foreach (var item in roleNameCollection)
                        {
                            roleCollection.Add(await service.EntityRepository.ApplicationRoleManager.FindByNameAsync(item));
                        }

                        boVM.ApplicationRoleId = new string[roleCollection.Count];
                        boVM.ApplicationRoleName = new string[roleCollection.Count];
                        roleCollection.OrderBy(x => x.SortCode);

                        for (int i = 0; i < roleCollection.Count; i++)
                        {
                            boVM.ApplicationRoleId[i] = roleCollection[i].Id.ToString();
                            boVM.ApplicationRoleName[i] = roleCollection[i].Name;
                        }

                        boVM.OrderNumber = (++count).ToString();
                        boVMCollection.Add(boVM);
                    }
                }

            }
            return boVMCollection;
        }

    }
}

using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.ViewModels.ApplicationCommon.RoleAndUser;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModelServices.Tools;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPFW.ViewModelServices.Extensions.ApplicationCommon.RoleAndUser
{
    public static class ApplicationRoleModelServiceExtension
    {
        public static async Task<ApplicationRoleVM> GetApplicationRoleVMForCreateOrEdit(this IViewModelService<ApplicationRole, ApplicationRoleVM> service, Guid id)
        {
            var isNew = false;
            var bo = await service.EntityRepository.ApplicationRoleManager.FindByIdAsync(id.ToString());
            if (bo == null)
            {
                bo = new ApplicationRole();
                isNew = true;
            }
            var boVM = new ApplicationRoleVM();
            boVM.IsNew = isNew;

            // 映射基本属性值
            bo.MapToViewModel<ApplicationRole, ApplicationRoleVM>(boVM);

            // 设置上级关系的视图模型数据
            if (bo.ParentRole != null)
            {
                boVM.ParentRoleId = bo.ParentRole.Id.ToString();
                boVM.ParentRoleName = bo.ParentRole.Name;
            }
            // 设置供给前端视图模型选择上级元素时的下拉选项集合
            var sourceItems01 = service.EntityRepository.ApplicationRoleManager.Roles;
            boVM.ParentRoleItemCollection = SelfReferentialItemFactory<ApplicationRole>.GetCollection(sourceItems01.OrderBy(x => x.SortCode).ToList(), true);

            return boVM;
        }

        public static async Task GetApplicationRoleVMRelevanceData(this IViewModelService<ApplicationRole, ApplicationRoleVM> service, ApplicationRoleVM boVM)
        {
            var boId = boVM.Id;

            var bo = await service.EntityRepository.ApplicationRoleManager.FindByIdAsync(boVM.Id.ToString());
            if (bo == null)
                bo = new ApplicationRole();

            // 设置上级关系的视图模型数据
            if (bo.ParentRole != null)
            {
                boVM.ParentRoleId = bo.ParentRole.Id.ToString();
                boVM.ParentRoleName = bo.ParentRole.Name;
            }
            // 设置供给前端视图模型选择上级元素时的下拉选项集合
            var sourceItems01 = service.EntityRepository.ApplicationRoleManager.Roles;
            boVM.ParentRoleItemCollection = SelfReferentialItemFactory<ApplicationRole>.GetCollection(sourceItems01.OrderBy(x => x.SortCode).ToList(), true);
        }

        public static async Task<SaveStatusModel> SaveApplicationRoleForCreateOrEdit(this IViewModelService<ApplicationRole, ApplicationRoleVM> service, ApplicationRoleVM boVM)
        {
            var saveStatus = new SaveStatusModel() { SaveSatus = true, Message = "" };

            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.ApplicationRoleManager.FindByIdAsync(boVM.Id.ToString());
            var isNew = false;
            if (bo == null)
            {
                bo = new ApplicationRole();
                isNew = true;
            }

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel<ApplicationRoleVM, ApplicationRole>(bo);
            // 处理关联的 ParentItem
            if (!String.IsNullOrEmpty(boVM.ParentRoleId))
            {
                var parentId = Guid.Parse(boVM.ParentRoleId);
                var parentItem = await service.EntityRepository.ApplicationRoleManager.FindByIdAsync(parentId.ToString());
                bo.ParentRole = parentItem;
            }
            else
                bo.ParentRole = bo;

            if (isNew)
            {
                var result = await service.EntityRepository.ApplicationRoleManager.CreateAsync(bo);
                if (result != IdentityResult.Success)
                {
                    saveStatus.SaveSatus = false;
                    saveStatus.Message = "创建角色组数据失败。";
                }
            }
            else
            {
                var result = await service.EntityRepository.ApplicationRoleManager.UpdateAsync(bo);
                if (result != IdentityResult.Success)
                {
                    saveStatus.SaveSatus = false;
                    saveStatus.Message = "创建角色组数据失败。";
                }
            }
            return saveStatus;
        }

        public static async Task<DeleteStatusModel> DeleteApplicationRole(this IViewModelService<ApplicationRole, ApplicationRoleVM> service, Guid id)
        {
            var deleteStatusModel = new DeleteStatusModel() { DeleteSatus = true, Message = "" };
            var role = await  service.EntityRepository.ApplicationRoleManager.FindByIdAsync(id.ToString());
            // 检查是否存在下级元素
            var subItemCollection = service.EntityRepository.ApplicationRoleManager.Roles.Where(x => x.ParentRole.Id == role.Id);
            if (subItemCollection.Count() > 1)
            {
                deleteStatusModel.DeleteSatus = false;
                deleteStatusModel.Message = "删除角色组错误：\n" +
                    "1. 其它依赖角色组相关的业务数据存在，不能删除。\n";
                return deleteStatusModel;
            }
            // 执行删除
            var result = await service.EntityRepository.ApplicationRoleManager.DeleteAsync(role);
            if (result != IdentityResult.Success)
            {
                deleteStatusModel.DeleteSatus = false;
                deleteStatusModel.Message = "删除角色组错误：\n" +
                    "1. 角色组中还拥有用户，不能删除；\n" +
                    "2. 角色组还拥有下级关联的角色组；\n" +
                    "3. 其它依赖角色组相关的业务数据存在，不能删除。\n";
            }
            return deleteStatusModel;

        }
    }
}

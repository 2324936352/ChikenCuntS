using LPFW.DataAccess;
using LPFW.DataAccess.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using LPFW.ViewModelServices.Tools;
using LPFW.ViewModels;
using LPFW.ViewModels.Common;
using LPFW.ViewModels.ControlModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using LPFW.ViewModels.ApplicationCommon.RoleAndUser;
using Microsoft.AspNetCore.Identity;
using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.ViewModels.OrganizationBusiness;
using LPFW.EntitiyModels.OrganzationBusiness;

namespace LPFW.ViewModelServices
{
    /// <summary>
    /// 常规的视图模型数据处理服务接口 <see cref="IViewModelService{TEntity,TViewModel}" /> 的具体实现。
    /// </summary>
    /// <typeparam name="TEntity">实体模型类型</typeparam>
    /// <typeparam name="TViewModel">视图模型类型</typeparam>
    public class ViewModelService<TEntity, TViewModel> : IViewModelService<TEntity, TViewModel>
        where TEntity : class, IEntity, new()
        where TViewModel : class, IEntityViewModel, new()
    {
        private readonly IEntityRepository<TEntity> _entityRepository;

        public IEntityRepository<TEntity> EntityRepository
        {
            get { return _entityRepository; }
        }

        public ViewModelService(IEntityRepository<TEntity> entityRepository)
        {
            this._entityRepository = entityRepository;
        }

        public virtual async Task<TViewModel> GetBoVMAsyn(Guid id)
        {
            // 提取泛型指定的业务实体模型对象
            var bo = await _entityRepository.GetBoAsyn(id);
            
            // 转换为视图模型并返回
            return ConversionForViewModelHelper.GetFromEntity<TEntity, TViewModel>(bo);
        }

        public virtual async Task<TViewModel> GetBoVMAsyn(Guid id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var bo = await _entityRepository.GetBoAsyn(id,includeProperties);
            return ConversionForViewModelHelper.GetFromEntity<TEntity, TViewModel>(bo);
        }

        public virtual async Task<List<TViewModel>> GetBoVMCollectionAsyn()
        {
            var boCollection = await _entityRepository.GetBoCollectionAsyn();
            return ConversionForViewModelHelper.GetFromEntityCollection<TEntity, TViewModel>(boCollection);
        }

        public virtual async Task<List<TViewModel>> GetBoVMCollectionAsyn(string keyword)
        {
            var boCollection = await _entityRepository.GetBoCollectionAsyn(keyword);
            return ConversionForViewModelHelper.GetFromEntityCollection<TEntity, TViewModel>(boCollection);
        }

        public virtual async Task<List<TViewModel>> GetBoVMCollectionAsyn(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var boCollection = await _entityRepository.GetBoCollectionAsyn(includeProperties);
            return ConversionForViewModelHelper.GetFromEntityCollection<TEntity, TViewModel>(boCollection);
        }

        public virtual async Task<List<TViewModel>> GetBoVMCollectionAsyn(ListPageParameter listPageParameter)
        {
            var boCollection = await _entityRepository.GetBoPaginateAsyn(listPageParameter);
            return ConversionForViewModelHelper.GetFromEntityCollection<TEntity, TViewModel>(boCollection, listPageParameter);
        }

        public virtual async Task<List<TViewModel>> GetBoVMCollectionAsyn(ListPageParameter listPageParameter, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var boCollection = await _entityRepository.GetBoPaginateAsyn(listPageParameter,includeProperties);
            return ConversionForViewModelHelper.GetFromEntityCollection<TEntity, TViewModel>(boCollection, listPageParameter);
        }

        public async Task<List<TViewModel>> GetBoVMCollectionAsyn(ListPageParameter listPageParameter, Expression<Func<TEntity, bool>> navigatorPredicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var boCollection = await _entityRepository.GetBoPaginateAsyn(listPageParameter,navigatorPredicate, includeProperties);
            return ConversionForViewModelHelper.GetFromEntityCollection<TEntity, TViewModel>(boCollection, listPageParameter);
        }


        public async Task<List<TViewModel>> GetBoVMCollectionAsyn(ListSinglePageParameter listPageParameter)
        {
            var boCollection = await _entityRepository.GetBoCollectionAsyn(listPageParameter);
            return ConversionForViewModelHelper.GetFromEntityCollection<TEntity, TViewModel>(boCollection);
        }

        public async Task<List<TViewModel>> GetBoVMCollectionAsyn(ListSinglePageParameter listPageParameter, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var boCollection = await _entityRepository.GetBoCollectionAsyn(listPageParameter, includeProperties);
            return ConversionForViewModelHelper.GetFromEntityCollection<TEntity, TViewModel>(boCollection);
        }

        public async Task<List<TViewModel>> GetBoVMCollectionAsyn(ListSinglePageParameter listPageParameter, Expression<Func<TEntity, bool>> navigatorPredicate, Expression<Func<TEntity, object>> includeProperty)
        {
            var boCollection = await _entityRepository.GetBoCollectionAsyn(listPageParameter, navigatorPredicate, includeProperty);
            return ConversionForViewModelHelper.GetFromEntityCollection<TEntity, TViewModel>(boCollection);
        }

        public async Task<List<TViewModel>> GetBoVMCollectionAsyn(ListSinglePageParameter listPageParameter, Expression<Func<TEntity, bool>> navigatorPredicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var boCollection = await _entityRepository.GetBoCollectionAsyn(listPageParameter, navigatorPredicate, includeProperties);
            var boVMCollection = new List<TViewModel>();
            int count = 0;
            foreach (var item in boCollection)
            {
                var boVM = new TViewModel();
                item.MapToViewModel<TEntity, TViewModel>(boVM);
                boVM.OrderNumber = (++count).ToString();
                boVMCollection.Add(boVM);
            }

            return boVMCollection;
        }


        public async Task<List<TViewModel>> GetBoVMCollectionWithHierarchicalStyleAsyn()
        {
            var boCollection = await _entityRepository.GetBoCollectionAsyn();
            return ConversionForViewModelHelper.GetFromEntityCollectionWithHierarchicalStyle<TEntity, TViewModel>(boCollection);
        }

        public async Task<List<TViewModel>> GetBoVMCollectionWithHierarchicalStyleAsyn(ListSinglePageParameter listPageParameter)
        {
            var boCollection = await _entityRepository.GetBoCollectionAsyn(listPageParameter);
            return ConversionForViewModelHelper.GetFromEntityCollectionWithHierarchicalStyle<TEntity, TViewModel>(boCollection);
        }

        public async Task<List<TViewModel>> GetBoVMCollectionWithHierarchicalStyleAsyn(ListSinglePageParameter listPageParameter, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var boCollection = await _entityRepository.GetBoCollectionAsyn(listPageParameter, includeProperties);
            return ConversionForViewModelHelper.GetFromEntityCollectionWithHierarchicalStyle<TEntity, TViewModel>(boCollection);
        }

        public async Task<List<TViewModel>> GetBoVMCollectionWithHierarchicalStyleAsyn(ListSinglePageParameter listPageParameter, Expression<Func<TEntity, bool>> navigatorPredicate, Expression<Func<TEntity, object>> includeProperty)
        {
            var boCollection = await _entityRepository.GetBoCollectionAsyn(listPageParameter, navigatorPredicate, includeProperty);
            return ConversionForViewModelHelper.GetFromEntityCollectionWithHierarchicalStyle<TEntity, TViewModel>(boCollection);
        }

        public async Task<TOtherViewModel> GetOtherBoVM<TOrther, TOtherViewModel>(Guid id)
            where TOrther : class, IEntity, new()
            where TOtherViewModel : class, IEntityViewModel, new()
        {
            var bo = await _entityRepository.GetOtherBoAsyn<TOrther>(id);
            return ConversionForViewModelHelper.GetFromEntity<TOrther, TOtherViewModel>(bo);
        }

        /// <summary>
        /// 根据实体模型类型 TOrther，获取对应类型 TOtherViewModel 的视图模型对象集合
        /// </summary>
        /// <typeparam name="TOrther"></typeparam>
        /// <typeparam name="TOtherViewModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<TOtherViewModel>> GetOtherBoVMCollection<TOrther, TOtherViewModel>()
            where TOrther : class, IEntity, new()
            where TOtherViewModel : class, IEntityViewModel, new()
        {
            var boCollection = await _entityRepository.GetOtherBoCollectionAsyn<TOrther>();
            return ConversionForViewModelHelper.GetFromEntityCollection<TOrther, TOtherViewModel>(boCollection);
        }

        public async Task<List<TOtherViewModel>> GetOtherBoVMCollection<TOrther, TOtherViewModel>(Expression<Func<TOrther, bool>> predicate)
            where TOrther : class, IEntity, new()
            where TOtherViewModel : class, IEntityViewModel, new()
        {
            var boCollection = await _entityRepository.GetOtherBoCollectionAsyn<TOrther>(predicate);
            return ConversionForViewModelHelper.GetFromEntityCollection<TOrther, TOtherViewModel>(boCollection);
        }

        public async Task<List<TOtherViewModel>> GetOtherBoVMCollection<TOrther, TOtherViewModel>(Expression<Func<TOrther, bool>> predicate, params Expression<Func<TOrther, object>>[] includeProperties)
            where TOrther : class, IEntity, new()
            where TOtherViewModel : class, IEntityViewModel, new()
        {
            var boCollection = await _entityRepository.GetOtherBoCollectionAsyn<TOrther>(predicate,includeProperties);
            return ConversionForViewModelHelper.GetFromEntityCollection<TOrther, TOtherViewModel>(boCollection);
        }


        public virtual async Task<SaveStatusModel> SaveBoAsyn(TViewModel boVM)
        {
            var bo = await _entityRepository.GetBoAsyn(boVM.Id);
            if (bo == null)
                bo = new TEntity();

            boVM.MapToEntityModel<TViewModel, TEntity>(bo);

            var saveStatus = await _entityRepository.SaveBoAsyn(bo);

            return saveStatus;
        }

        public virtual async Task<DeleteStatusModel> DeleteBoAsyn(Guid id)
        {
            var deleteStatus = await _entityRepository.DeleteBoAsyn(id);
            return deleteStatus;
        }

        public virtual async Task<bool> IsUnique(Expression<Func<TEntity, bool>> uniquePredicate)
        {
            return !await _entityRepository.HasBoAsyn(uniquePredicate); 
        }

        public virtual async Task<List<TreeNodeForJsTree>> GetTreeViewNodeForJsTreeCollectionAsyn<SelfReferentialEntity>(Expression<Func<SelfReferentialEntity, object>> includeProperty) where SelfReferentialEntity : class, IEntity, new()
        {
            // 提取 <SelfReferentialEntity> 数据集合
            var selfReferentialEntityCollection = await _entityRepository.EntitiesContext.Set<SelfReferentialEntity>().Include(includeProperty).OrderBy(x=>x.SortCode).ToListAsync();
            // 转换为 SelfReferentialItem 集合
            var selfReferentialItemCollection = SelfReferentialItemFactory<SelfReferentialEntity>.GetCollection(selfReferentialEntityCollection, false);

            // 构建树节点集合
            var result = TreeViewFactoryForJsTree.GetTreeNodes(selfReferentialItemCollection);

            return result;
        }

        public virtual async Task<List<TreeNodeForBootStrapTreeView>> GetTreeViewNodeForBootStrapTreeViewCollectionAsyn<SelfReferentialEntity>(Expression<Func<SelfReferentialEntity, object>> includeProperty) where SelfReferentialEntity : class, IEntity, new()
        {
            // 提取 <SelfReferentialEntity> 数据集合
            var selfReferentialEntityCollection = await _entityRepository.EntitiesContext.Set<SelfReferentialEntity>().Include(includeProperty).OrderBy(x => x.SortCode).ToListAsync();
            // 转换为 SelfReferentialItem 集合
            var selfReferentialItemCollection = SelfReferentialItemFactory<SelfReferentialEntity>.GetCollection(selfReferentialEntityCollection, false);

            // 构建树节点集合
            var result = TreeViewFactoryForBootSrapTreeView.GetTreeNodes(selfReferentialItemCollection);

            return result;

        }

        public virtual async Task<List<TreeNodeForBootStrapTreeView>> GetTreeViewNodeForBootStrapTreeViewCollectionAsyn<SelfReferentialEntity>(
            Expression<Func<SelfReferentialEntity, bool>> predicate,
            Expression<Func<SelfReferentialEntity, object>> includeProperty) 
            where SelfReferentialEntity : class, IEntity, new()
        {
            // 提取 <SelfReferentialEntity> 数据集合
            var selfReferentialEntityCollection = await _entityRepository.EntitiesContext.Set<SelfReferentialEntity>().Include(includeProperty).Where(predicate).OrderBy(x => x.SortCode).ToListAsync();
            // 转换为 SelfReferentialItem 集合
            var selfReferentialItemCollection = SelfReferentialItemFactory<SelfReferentialEntity>.GetCollection(selfReferentialEntityCollection, false);

            // 构建树节点集合
            var result = TreeViewFactoryForBootSrapTreeView.GetTreeNodes(selfReferentialItemCollection);

            return result;

        }

        public virtual async Task<List<TreeNodeForBootStrapTreeView>> GetTreeViewNodeForBootStrapTreeViewWithPlainFacadeItemCollectionAsyn<PlainFacadeItemEntity>() where PlainFacadeItemEntity : class, IEntity, new() 
        {
            var sourceItems= await _entityRepository.EntitiesContext.Set<PlainFacadeItemEntity>().OrderBy(x => x.SortCode).ToListAsync();
            // 转换为 SelfReferentialItem 集合
            var plainFacadeItemCollection =  PlainFacadeItemFactory<PlainFacadeItemEntity>.Get(sourceItems.OrderBy(x => x.SortCode).ToList());
            var result = TreeViewFactoryForBootSrapTreeView.GetTreeNodes(plainFacadeItemCollection);
            return result;
        }

        /// <summary>
        /// 处理视图模型相关的附件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public virtual async Task SetAttachmentFileItem(TViewModel boVM)
        {
            var id = boVM.Id;

            // 初始化普遍文件集合
            boVM.BusinessFileVMCollection = new List<BusinessFileVM>();
            var fileCollection = await _entityRepository.GetBusinessFileCollectionAsyn(id);
            foreach (var item in fileCollection.OrderBy(x => x.SortCode))
            {
                boVM.BusinessFileVMCollection.Add(new BusinessFileVM(item));
            }

            // 初始化头像
            var avatar = await _entityRepository.GetAvatar(id);
            if (avatar != null)
                boVM.Avatar = new BusinessImageVM(avatar);
            else
                boVM.Avatar = new BusinessImageVM() { ObjectId = id.ToString() };

            // 初始化图片集合
            boVM.BusinessImageVMCollection = new List<BusinessImageVM>();
            var imageCollection = await _entityRepository.GetBusinessImageCollectionAsyn(id);
            foreach (var item in imageCollection.OrderBy(x => x.SortCode))
            {
                boVM.BusinessImageVMCollection.Add(new BusinessImageVM(item));
            }

            // 初始化单一视频问价
            var video = await _entityRepository.GetBusinessVideoAsyn(id);
            if (video != null)
                boVM.BusinessVideoVM = new BusinessVideoVM(video);
            else
                boVM.BusinessVideoVM = new BusinessVideoVM() { ObjectId = id.ToString()};

            // 封面图片（获取单一图片）
            var image = await _entityRepository.GetBusinessImageAsyn(id);
            if (image != null)
                boVM.BusinessVideoCoverPage = new BusinessImageVM(image);
            else
                boVM.BusinessVideoCoverPage = new BusinessImageVM() {  ObjectId = id.ToString()};
        }

        public virtual async Task SetAttachmentFileItemForOtherBoVM<TOtherViewModel>(TOtherViewModel boVM) where TOtherViewModel : class, IEntityViewModel, new() 
        {
            var id = boVM.Id;

            // 初始化普遍文件集合
            boVM.BusinessFileVMCollection = new List<BusinessFileVM>();
            var fileCollection = await _entityRepository.GetBusinessFileCollectionAsyn(id);
            foreach (var item in fileCollection.OrderBy(x => x.SortCode))
            {
                boVM.BusinessFileVMCollection.Add(new BusinessFileVM(item));
            }

            // 初始化头像
            var avatar = await _entityRepository.GetAvatar(id);
            if (avatar != null)
                boVM.Avatar = new BusinessImageVM(avatar);
            else
                boVM.Avatar = new BusinessImageVM() { ObjectId = id.ToString() };

            // 初始化图片集合
            boVM.BusinessImageVMCollection = new List<BusinessImageVM>();
            var imageCollection = await _entityRepository.GetBusinessImageCollectionAsyn(id);
            foreach (var item in imageCollection.OrderBy(x => x.SortCode))
            {
                boVM.BusinessImageVMCollection.Add(new BusinessImageVM(item));
            }

            // 初始化单一视频问价
            var video = await _entityRepository.GetBusinessVideoAsyn(id);
            if (video != null)
                boVM.BusinessVideoVM = new BusinessVideoVM(video);
            else
                boVM.BusinessVideoVM = new BusinessVideoVM() { ObjectId = id.ToString() };

            // 封面图片（获取单一图片）
            var image = await _entityRepository.GetBusinessImageAsyn(id);
            if (image != null)
                boVM.BusinessVideoCoverPage = new BusinessImageVM(image);
            else
                boVM.BusinessVideoCoverPage = new BusinessImageVM() { ObjectId = id.ToString() };

        }

        public virtual string GetApplicationMenuGroupId(string urlString)
        {
            var id = Guid.NewGuid().ToString();
            var group = _entityRepository.EntitiesContext.ApplicationMenuGroups.FirstOrDefault(x=>x.PortalUrl.Contains(urlString));
            if (group != null)
                id = group.Id.ToString();
            else
            {
                var menuItem = _entityRepository.EntitiesContext.ApplicationMenuItems.Include(x => x.ApplicationMenuGroup).FirstOrDefault(x => x.UrlString.Contains(urlString));
                if (menuItem != null)
                    id = menuItem.ApplicationMenuGroup.Id.ToString();
            }

            return id;
        }

        public virtual string GetGetApplicationMenuItemActiveId(string urlString)
        {
            var id = Guid.NewGuid().ToString();
            var group = _entityRepository.EntitiesContext.ApplicationMenuItems.Include(x => x.ParentItem).FirstOrDefault(x => x.UrlString.Contains(urlString));
            if (group != null)
                id = group.ParentItem.Id.ToString();

            return id;

        }

        public virtual async Task<UserOnlineInformationVM> GetUserOnlineInformationVM(string userName)
        {
            // 1. 获取用户并初始化 userOnlineInformationVM
            var user = await _entityRepository.ApplicationUserManager.FindByNameAsync(userName);
            var userOnlineInformationVM = new UserOnlineInformationVM
            {
                Id = user.Id,
                UserName = user.UserName,
                Name = user.Name,
                MasterRoleId = "",
                RegisterId="",
                OrganizationId = "",
                OrganizationName="",
                DepartmentId = "",
                DepartmentName="",
                PositionId="",
                PositionName="",
                PositionWorkId="",
                PositionWorkName="",
                RoleItemCollection = new List<UserOnlineInformationVM.RoleItem>(),
                ClaimItemCollection= new List<UserOnlineInformationVM.ClaimItem>()
            };

            // 2. 获取用户 Claims
            var userClaimCollection = await _entityRepository.ApplicationUserManager.GetClaimsAsync(user);
            foreach (var item in userClaimCollection) 
                userOnlineInformationVM.ClaimItemCollection.Add(new UserOnlineInformationVM.ClaimItem() { ClaimName = item.Type, ClaimValue = item.Value });
            
            // 3. 获取用户关联的全部角色
            var roleNameCollection = await _entityRepository.ApplicationUserManager.GetRolesAsync(user);
            foreach (var roleName in roleNameCollection)
            {
                var role = await _entityRepository.ApplicationRoleManager.FindByNameAsync(roleName);
                var roleItem = new UserOnlineInformationVM.RoleItem() { RoleID = role.Id, RoleName = role.Name };
                userOnlineInformationVM.RoleItemCollection.Add(roleItem);
            }

            /* 4. 处理用户关联的令牌标识声明数据，参考在所有处理员工数据的方法中（例如 EmployeeModelServiceExtension.SaveBoWithRelevanceDataAsyn ）维护员工用户的标识内容：
                await service.CommonSetUserClarm(bo.User.UserName, UserCommonClaimEnum.用户类型, RoleCommonClaimEnum.平台业务.ToString());
                await service.CommonSetUserClarm(bo.User.UserName, UserCommonClaimEnum.注册资料, registerId);
                await service.CommonSetUserClarm(bo.User.UserName, UserCommonClaimEnum.单位, organizationId);
                await service.CommonSetUserClarm(user.UserName, UserCommonClaimEnum.部门, boVM.DepartmentId);
                await service.CommonSetUserClarm(user.UserName, UserCommonClaimEnum.岗位, boVM.PositionId);
                await service.CommonSetUserClarm(user.UserName, UserCommonClaimEnum.岗位工作, boVM.PositionWorkId);
                await service.CommonSetUserClarm(bo.User.UserName, UserCommonClaimEnum.宿主角色, userRole.Id.ToString());
             */
            var userTypeCliam= userClaimCollection.FirstOrDefault(x => x.Type == UserCommonClaimEnum.用户类型.ToString());
            if (userTypeCliam != null)
                userOnlineInformationVM.RoleCommonClaimEnum = (RoleCommonClaimEnum)Enum.Parse(typeof(RoleCommonClaimEnum),userTypeCliam.Value);

            var roleClaim = userClaimCollection.FirstOrDefault(x => x.Type == UserCommonClaimEnum.宿主角色.ToString());
            if (roleClaim != null)
                userOnlineInformationVM.MasterRoleId = (roleClaim.Value);

            var registerClaim = userClaimCollection.FirstOrDefault(x => x.Type == UserCommonClaimEnum.注册资料.ToString());
            if (registerClaim != null)
                userOnlineInformationVM.RegisterId = registerClaim.Value;

            var organizationClaim = userClaimCollection.FirstOrDefault(x => x.Type == UserCommonClaimEnum.单位.ToString());
            if (organizationClaim != null)
                userOnlineInformationVM.OrganizationId = organizationClaim.Value;

            var departmentClaim = userClaimCollection.FirstOrDefault(x => x.Type == UserCommonClaimEnum.部门.ToString());
            if (departmentClaim != null)
                userOnlineInformationVM.DepartmentId = departmentClaim.Value;

            var positionClaim = userClaimCollection.FirstOrDefault(x => x.Type == UserCommonClaimEnum.岗位.ToString());
            if (positionClaim != null)
                userOnlineInformationVM.PositionId = positionClaim.Value;

            var positionWorkClaim = userClaimCollection.FirstOrDefault(x => x.Type == UserCommonClaimEnum.岗位.ToString());
            if (positionWorkClaim != null)
                userOnlineInformationVM.PositionWorkId = positionWorkClaim.Value;

            return userOnlineInformationVM;
        }

        /// <summary>
        /// 获取平台营运管理子系统通用的单位选择器元素
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<OrganizationSelectorVM>> GetOrganizationSelectorItemCollection(string organzationId = null) 
        {
            var result = new List<OrganizationSelectorVM>();
            var transactionCenterRegisterCollection = await EntityRepository.GetOtherBoCollectionAsyn<TransactionCenterRegister>();
            foreach (var item in transactionCenterRegisterCollection.OrderBy(x => x.SortCode))
            {
                var itemVM = new OrganizationSelectorVM()
                {
                    TransactionCenterRegisterId = item.Id.ToString(),
                    OrganizationId = "",
                    TransactionCenterRegisterName = item.Name,
                    OrganizationName = "",
                    DorpdownItemName = item.Name,
                    SortCode = item.SortCode
                };
                result.Add(itemVM);

                var organzationCollection = await EntityRepository.GetOtherBoCollectionAsyn<Organization>(x => x.TransactionCenterRegister.Id == item.Id, y => y.TransactionCenterRegister);
                foreach (var oItem in organzationCollection) 
                {
                    var oItemVM = new OrganizationSelectorVM()
                    {
                        TransactionCenterRegisterId = item.Id.ToString(),
                        OrganizationId = oItem.Id.ToString(),
                        TransactionCenterRegisterName = item.Name,
                        OrganizationName = oItem.Name,
                        DorpdownItemName = "　　" + oItem.Name,  // 这里添加的两个中文空格字符，用于显示缩进的效果
                        SortCode = oItem.SortCode
                    };
                    result.Add(oItemVM);
                }
            }

            // 处理缺省单位和缺省的单位部门
            if (String.IsNullOrEmpty(organzationId))
            {
                result.ForEach(x => x.IsCurrent = false);
                // 设置缺省当前单位
                var t = result.FirstOrDefault();
                if (t != null)
                {
                    var o = result.FirstOrDefault(x => x.TransactionCenterRegisterId == t.TransactionCenterRegisterId && x.OrganizationId != "");
                    if (o != null)
                    {
                        o.IsCurrent = true;
                        // 提取缺省部门 Id
                        var oId = Guid.Parse(o.OrganizationId);
                        var deptCollection = await EntityRepository.GetOtherBoCollectionAsyn<Department>(x=>x.Organization.Id==oId,y=>y.Organization);
                        var dept = deptCollection.OrderBy(x=>x.SortCode).FirstOrDefault();
                        if (dept != null)
                        {
                            o.DefaultDepartmentId = dept.Id.ToString();
                        }
                    }
                }
            }
            else 
            {
                result.ForEach(x => x.IsCurrent = false);
                var item = result.FirstOrDefault(x => x.OrganizationId == organzationId);
                item.IsCurrent = true;
                // 提取缺省部门 Id
                var oId = Guid.Parse(item.OrganizationId);
                var deptCollection = await EntityRepository.GetOtherBoCollectionAsyn<Department>(x => x.Organization.Id == oId, y => y.Organization);
                var dept = deptCollection.OrderBy(x => x.SortCode).FirstOrDefault();
                if (dept != null)
                {
                    item.DefaultDepartmentId = dept.Id.ToString();
                }
            }

            return result;
        }

        /// <summary>
        /// 根据用户名获取用户归属单位的 Id
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public async Task<Guid> CommonGetOrganizationId(string userName) 
        {
            // 1. 获取用户并初始化 userOnlineInformationVM
            var user = await _entityRepository.ApplicationUserManager.FindByNameAsync(userName);
            // 2. 获取用户 Claims
            var userClaimCollection = await _entityRepository.ApplicationUserManager.GetClaimsAsync(user);

            // 2.1. 提取用户业务类型
            var claimType = userClaimCollection.FirstOrDefault(x => x.Type == UserCommonClaimEnum.用户类型.ToString());
            if (claimType == null)
                return Guid.Empty;
            else
            {
                if (claimType.Value == RoleCommonClaimEnum.平台业务.ToString())
                {
                    // 2.1.1. 提取归属单位
                    var organizationClaim = userClaimCollection.FirstOrDefault(x => x.Type == UserCommonClaimEnum.单位.ToString());
                    if (organizationClaim == null)
                        return Guid.Empty;
                    else
                        return Guid.Parse(organizationClaim.Value);
                }
                else
                    return Guid.Empty;
            }
        }

        /// <summary>
        /// 通过用户名获取用户归属的供应商单位 Id
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public async Task<Guid> CommonGetPurchaserId(string userName) 
        {
            // 1. 获取用户并初始化 userOnlineInformationVM
            var user = await _entityRepository.ApplicationUserManager.FindByNameAsync(userName);
            // 2. 获取用户 Claims
            var userClaimCollection = await _entityRepository.ApplicationUserManager.GetClaimsAsync(user);
            
            // 2.1. 提取用户业务类型
            var claimType= userClaimCollection.FirstOrDefault(x => x.Type == UserCommonClaimEnum.用户类型.ToString());
            if (claimType == null)
                return Guid.Empty;
            else 
            {
                if (claimType.Value == RoleCommonClaimEnum.采购商业务.ToString())
                {
                    // 2.1.1. 提取归属单位
                    var organizationClaim = userClaimCollection.FirstOrDefault(x => x.Type == UserCommonClaimEnum.单位.ToString());
                    if (organizationClaim == null)
                        return Guid.Empty;
                    else
                        return Guid.Parse(organizationClaim.Value);
                }
                else
                    return Guid.Empty;
            }
        }

        /// <summary>
        /// 通过用户名获取用户归属的供应商单位 Id
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public async Task<Guid> CommonGetVenderId(string userName)
        {
            // 1. 获取用户并初始化 userOnlineInformationVM
            var user = await _entityRepository.ApplicationUserManager.FindByNameAsync(userName);
            // 2. 获取用户 Claims
            var userClaimCollection = await _entityRepository.ApplicationUserManager.GetClaimsAsync(user);

            // 2.1. 提取用户业务类型
            var claimType = userClaimCollection.FirstOrDefault(x => x.Type == UserCommonClaimEnum.用户类型.ToString());
            if (claimType == null)
                return Guid.Empty;
            else
            {
                if (claimType.Value == RoleCommonClaimEnum.供应商业务.ToString())
                {
                    // 2.1.1. 提取归属单位
                    var organizationClaim = userClaimCollection.FirstOrDefault(x => x.Type == UserCommonClaimEnum.单位.ToString());
                    if (organizationClaim == null)
                        return Guid.Empty;
                    else
                        return Guid.Parse(organizationClaim.Value);
                }
                else
                    return Guid.Empty;
            }
        }

        /// <summary>
        /// 设置用户的登录令牌的声明
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="claimEnum">令牌声明名称</param>
        /// <param name="claimValue">声明对应的值</param>
        /// <returns></returns>
        public async Task CommonSetUserClarm(string userName, UserCommonClaimEnum claimName, string claimValue) 
        {
            var user = await _entityRepository.ApplicationUserManager.FindByNameAsync(userName);
            var userClaimCollection = await _entityRepository.ApplicationUserManager.GetClaimsAsync(user);

            var claim = userClaimCollection.FirstOrDefault(x => x.Type == claimName.ToString());
            if (claim == null)
            {
                claim = new System.Security.Claims.Claim(claimName.ToString(), claimValue);
                await _entityRepository.ApplicationUserManager.AddClaimAsync(user, claim);
            }
            else 
            {
                var newclaim = new System.Security.Claims.Claim(claimName.ToString(), claimValue);
                // 更新令牌声明
                await _entityRepository.ApplicationUserManager.ReplaceClaimAsync(user, claim, newclaim);
            }
        }

    }
}

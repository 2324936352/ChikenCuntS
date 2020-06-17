using LPFW.DataAccess;
using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.Foundation.SpecificationsForEntityModel;
using LPFW.ViewModels;
using LPFW.ViewModels.ApplicationCommon.RoleAndUser;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels.OrganizationBusiness;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LPFW.ViewModelServices
{
    /// <summary>
    /// 实体模型与视图模型转换服务，这个服务接口一般在 MVC 控制器中注入，其基本职责：
    ///   1. 负责将实体模型对象转换为视图模型对象，提供给前端 UI 或者其它服务使用；
    ///   2. 负责将前端回传的参数、视图模型对象数据，按照要求提交给 
    ///      接口 <see cref="IEntityRepository{TEntityModel}"/> 进行持久化处理
    /// </summary>
    /// <typeparam name="TEntityModel">实体模型类型</typeparam>
    /// <typeparam name="TViewModel">视图模型类型</typeparam>
    public interface IViewModelService<TEntityModel, TViewModel>
        where TEntityModel : class, IEntity, new()
        where TViewModel : class, IEntityViewModel, new()
    {
        #region 1.接口属性，用于暴露一些继承自父接口的变量在这里使用
        /// <summary>
        /// 数据访问仓储服务
        /// </summary>
        IEntityRepository<TEntityModel> EntityRepository { get; }
        #endregion

        #region 2.获取单一视图模型
        /// <summary>
        /// 根据传入的实体模型的 id 的值，创建和返回一个视图模型对象，如果在实体对象不存在相应 id 的对象，则返回一个带有空数据视图模型对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TViewModel> GetBoVMAsyn(Guid id);

        /// <summary>
        /// 根据实体模型对象 id 以及相关的关联对象包含引用的 Lambda 表达式获取对应的视图模型对象，
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeProperties">lambda 表达式数组，参数形式：x=>x.User, y=>y.Dempartment ... </param>
        /// <returns></returns>
        Task<TViewModel> GetBoVMAsyn(Guid id, params Expression<Func<TEntityModel, object>>[] includeProperties);
        #endregion

        #region 3.获取无分页限制的视图对象集合
        /// <summary>
        /// 获取模型对象集合，缺省按照 SortCode 排序
        /// </summary>
        /// <returns></returns>
        Task<List<TViewModel>> GetBoVMCollectionAsyn();

        /// <summary>
        /// 根据关键词匹配的结果，返回视图模型对象集合，缺省按照 SortCode 排序
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        Task<List<TViewModel>> GetBoVMCollectionAsyn(string keyword);

        /// <summary>
        /// 关联对象包含引用的 Lambda 表达式获取对应的 API 模型对象集合
        /// </summary>
        /// <param name="includeProperties">lambda 表达式数组，参数形式：x=>x.User, y=>y.Dempartment ... </param>
        /// <returns></returns>
        Task<List<TViewModel>> GetBoVMCollectionAsyn(params Expression<Func<TEntityModel, object>>[] includeProperties);
        #endregion

        #region 4.获取根据多页分页模型 ListPageParameter 限制的分页后的视图模型对象集合
        /// <summary>
        /// 根据分页模型 ListPageParameter 对象数据，返回视图模型对应的对象集合，亦即分页处理后的数据集合，页面参数 listPageParameter 也会更新，便于相关的前端使用
        /// </summary>
        /// <param name="listPageParameter">分页处理模型对象</param>
        /// <returns>分页后的数据集合</returns>
        Task<List<TViewModel>> GetBoVMCollectionAsyn(ListPageParameter listPageParameter);

        /// <summary>
        /// 根据分页模型 ListPageParameter 以及包含引用的 Lambda 表达式，返回视图模型对应的对象集合，亦即分页处理后的数据集合，页面参数 listPageParameter 也会更新，便于相关的前端使用
        /// </summary>
        /// <param name="listPageParameter">分页处理模型对象</param>
        /// <param name="includeProperties">关联实体模型属性定义的lambda 表达式数组，参数形式：x=>x.User, y=>y.Dempartment ...</param>
        /// <returns>分页后的数据集合</returns>
        Task<List<TViewModel>> GetBoVMCollectionAsyn(ListPageParameter listPageParameter, params Expression<Func<TEntityModel, object>>[] includeProperties); 

        /// <summary>
        /// 根据分页模型 ListPageParameter 、关联模型属性过滤条件表达式以及包含引用的 Lambda 表达式，返回视图模型对应的对象集合，亦即分页处理后的数据集合，
        /// 页面参数 listPageParameter 也会更新，便于相关的前端使用。
        /// </summary>
        /// <param name="listPageParameter">分页处理模型对象</param>
        /// <param name="navigatorPredicate">关联模型属性过滤条件表达式，参数形式：x=>x.Department.Id == "5dd4b62d-1175-43d1-917d-66f427b5c537" </param>
        /// <param name="includeProperties">关联实体模型属性定义的lambda 表达式数组，参数形式：x=>x.User, y=>y.Dempartment ...</param>
        /// <returns></returns>
        Task<List<TViewModel>> GetBoVMCollectionAsyn(ListPageParameter listPageParameter, Expression<Func<TEntityModel, bool>> navigatorPredicate, params Expression<Func<TEntityModel, object>>[] includeProperties);
        #endregion

        #region 5.获取根据单页分页模型 ListSinglePageParameter 限制的分页后的视图模型对象集合
        /// <summary>
        /// 根据单页数据模型（不分页）ListSinglePageParameter 的对象，返回处理结果
        /// </summary>
        /// <param name="listPageParameter">单页数据列表对象集合约束模型对象</param>
        /// <returns></returns>
        Task<List<TViewModel>> GetBoVMCollectionAsyn(ListSinglePageParameter listPageParameter);

        /// <summary>
        /// 根据单页列表数据约束模型对象以及包含引用的 Lambda 表达式，返回视图模型对应的对象集合
        /// </summary>
        /// <param name="listPageParameter">单页数据列表对象集合约束模型对象</param>
        /// <param name="includeProperties">关联实体模型属性定义的lambda 表达式数组，参数形式：x=>x.User, y=>y.Dempartment ...</param>
        /// <returns></returns>
        Task<List<TViewModel>> GetBoVMCollectionAsyn(ListSinglePageParameter listPageParameter, params Expression<Func<TEntityModel, object>>[] includeProperties);

        /// <summary>
        /// 根据单页列表数据约束模型对象 、关联模型属性过滤条件表达式以及包含引用的 Lambda 表达式，返回视图模型对应的对象集合
        /// </summary>
        /// <param name="listPageParameter">单页数据列表对象集合约束模型对象</param>
        /// <param name="navigatorPredicate">关联模型属性过滤条件表达式，参数形式：x=>x.Department.Id == "5dd4b62d-1175-43d1-917d-66f427b5c537" </param>
        /// <param name="includeProperty">关联实体模型属性定义的lambda 表达式数组，参数形式：x=>x.User, y=>y.Dempartment ...</param>
        /// <returns></returns>
        Task<List<TViewModel>> GetBoVMCollectionAsyn(ListSinglePageParameter listPageParameter, Expression<Func<TEntityModel, bool>> navigatorPredicate, Expression<Func<TEntityModel, object>> includeProperty);

        /// <summary>
        /// 根据单页列表数据约束模型对象 、关联模型属性过滤条件表达式以及包含引用的 Lambda 表达式，返回视图模型对应的对象集合
        /// </summary>
        /// <param name="listPageParameter">单页数据列表对象集合约束模型对象</param>
        /// <param name="navigatorPredicate">关联模型属性过滤条件表达式，参数形式：x=>x.Department.Id == "5dd4b62d-1175-43d1-917d-66f427b5c537" </param>
        /// <param name="includeProperties">关联实体模型属性定义的lambda 表达式数组，参数形式：x=>x.User, y=>y.Dempartment ...</param>
        /// <returns></returns>
        Task<List<TViewModel>> GetBoVMCollectionAsyn(ListSinglePageParameter listPageParameter, Expression<Func<TEntityModel, bool>> navigatorPredicate, params Expression<Func<TEntityModel, object>>[] includeProperties);
        #endregion

        #region 6.获取经过层次外观处理之后的视图模型对象集合
        /// <summary>
        /// 获取视图模型对象集合，并且将视图模型集合设置为带有层次形式外观的集合，其基本的处理方法是将 Name 或者其他的属性值通过添加中文空格的方式做缩进层次处理
        /// </summary>
        /// <returns></returns>
        Task<List<TViewModel>> GetBoVMCollectionWithHierarchicalStyleAsyn();

        /// <summary>
        /// 获取视图模型对象集合，并且将视图模型集合设置为带有层次形式外观的集合，其基本的处理方法是将 Name 或者其他的属性值通过添加中文空格的方式做缩进层次处理
        /// </summary>
        /// <param name="listPageParameter">单页数据列表对象集合约束模型对象</param>
        /// <returns></returns>
        Task<List<TViewModel>> GetBoVMCollectionWithHierarchicalStyleAsyn(ListSinglePageParameter listPageParameter);

        /// <summary>
        /// 获取视图模型对象集合，并且将视图模型集合设置为带有层次形式外观的集合，其基本的处理方法是将 Name 或者其他的属性值通过添加中文空格的方式做缩进层次处理
        /// </summary>
        /// <param name="listPageParameter">单页数据列表对象集合约束模型对象</param>
        /// <param name="includeProperties">关联实体模型属性定义的lambda 表达式数组，参数形式：x=>x.User, y=>y.Dempartment ...</param>
        /// <returns></returns>
        Task<List<TViewModel>> GetBoVMCollectionWithHierarchicalStyleAsyn(ListSinglePageParameter listPageParameter, params Expression<Func<TEntityModel, object>>[] includeProperties);

        /// <summary>
        /// 获取视图模型对象集合，并且将视图模型集合设置为带有层次形式外观的集合，其基本的处理方法是将 Name 或者其他的属性值通过添加中文空格的方式做缩进层次处理
        /// </summary>
        /// <param name="listPageParameter">单页数据列表对象集合约束模型对象</param>
        /// <param name="navigatorPredicate">关联模型属性过滤条件表达式，参数形式：x=>x.Department.Id == "5dd4b62d-1175-43d1-917d-66f427b5c537" </param>
        /// <param name="includeProperty">关联实体模型属性定义的lambda 表达式，参数形式：x=>x.User</param>
        /// <returns></returns>
        Task<List<TViewModel>> GetBoVMCollectionWithHierarchicalStyleAsyn(ListSinglePageParameter listPageParameter, Expression<Func<TEntityModel, bool>> navigatorPredicate, Expression<Func<TEntityModel, object>> includeProperty);
        #endregion

        #region 7.获取指定泛型的视图模型的单个或者多个对象
        /// <summary>
        /// 根据实体模型类型 TOrther 和其数据对象的 id，获取对应类型 TOtherViewModel 的视图模型对象
        /// </summary>
        /// <typeparam name="TOrther"></typeparam>
        /// <typeparam name="TOtherViewModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TOtherViewModel> GetOtherBoVM<TOrther, TOtherViewModel>(Guid id)
            where TOrther : class, IEntity, new()
            where TOtherViewModel : class, IEntityViewModel, new();

        /// <summary>
        /// 根据实体模型类型 TOrther，获取对应类型 TOtherViewModel 的视图模型对象集合
        /// </summary>
        /// <typeparam name="TOrther"></typeparam>
        /// <typeparam name="TOtherViewModel"></typeparam>
        /// <returns></returns>
        Task<List<TOtherViewModel>> GetOtherBoVMCollection<TOrther, TOtherViewModel>()
            where TOrther : class, IEntity, new()
            where TOtherViewModel : class, IEntityViewModel, new();

        /// <summary>
        /// 根据指定实体模型类型 TOrther ，获取对应类型 TOtherViewModel 的视图模型对象集合
        /// </summary>
        /// <typeparam name="TOrther">实体模型类型</typeparam>
        /// <typeparam name="TOtherViewModel">视图模型类型</typeparam>
        /// <param name="predicate">数据过滤表达式</param>
        /// <returns></returns>
        Task<List<TOtherViewModel>> GetOtherBoVMCollection<TOrther, TOtherViewModel>(Expression<Func<TOrther, bool>> predicate)
            where TOrther : class, IEntity, new()
            where TOtherViewModel : class, IEntityViewModel, new();

        /// <summary>
        /// 根据指定实体模型类型 TOrther ，获取对应类型 TOrther 的视图模型对象集合
        /// </summary>
        /// <typeparam name="TOrther">实体模型类型</typeparam>
        /// <typeparam name="TOtherViewModel">API 模型类型</typeparam>
        /// <param name="predicate">数据过滤表达式</param>
        /// <param name="includeProperties">关联实体模型属性定义的lambda 表达式数组，参数形式：x=>x.User, y=>y.Dempartment ...</param>
        /// <returns></returns>
        Task<List<TOtherViewModel>> GetOtherBoVMCollection<TOrther, TOtherViewModel>(Expression<Func<TOrther, bool>> predicate, params Expression<Func<TOrther, object>>[] includeProperties)
            where TOrther : class, IEntity, new()
            where TOtherViewModel : class, IEntityViewModel, new();
        #endregion

        #region 8.数据持久化处理方法
        /// <summary>
        /// 保存数据，将传入的视图模型数据转换成实体模型对象，然后通过 Repository 处理持久化（新增或者更新） 
        /// </summary>
        /// <param name="boVM"></param>
        /// <returns></returns>
        Task<SaveStatusModel> SaveBoAsyn(TViewModel boVM);

        /// <summary>
        /// 删除数据，根据的 Id 查询实体模型对象，然后通过 Repository 处理持久化（删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeleteStatusModel> DeleteBoAsyn(Guid id);
        #endregion

        #region 9.获取业务实体对象数据状态的方法
        /// <summary>
        /// 根据指定的条件表达式，判断持久层数据对象的唯一性,返回 true 表示唯一，false 表示已经存在了
        /// </summary>
        /// <param name="uniquePredicate">判断条件的 Lambda 表达式</param>
        /// <returns></returns>
        Task<bool> IsUnique(Expression<Func<TEntityModel, bool>> uniquePredicate); 
        #endregion

        #region 10.针对当前应用开发项目定制的接口方法
        /// <summary>
        /// 设置附件相关的属性
        /// </summary>
        /// <param name="boVM"></param>
        /// <returns></returns>
        Task SetAttachmentFileItem(TViewModel boVM);
        Task SetAttachmentFileItemForOtherBoVM<TOtherViewModel>(TOtherViewModel boVM) where TOtherViewModel : class, IEntityViewModel, new();

        /// <summary>
        /// 获取用于导航树插件 jsTree 节点数据集合的方法
        /// </summary>
        /// <typeparam name="SelfReferentialEntity">具有自引用关系而且继承自 IEntity 的实体模型类型</typeparam>
        /// <returns></returns>
        Task<List<TreeNodeForJsTree>> GetTreeViewNodeForJsTreeCollectionAsyn<SelfReferentialEntity>(Expression<Func<SelfReferentialEntity, object>> includeProperty) where SelfReferentialEntity : class, IEntity, new();

        /// <summary>
        /// 获取用于导航树插件 BootStrap-TreeView 节点数据集合的方法
        /// </summary>
        /// <typeparam name="SelfReferentialEntity">具有自引用关系而且继承自 IEntity 的实体模型类型</typeparam>
        /// <param name="includeProperty">包含的关联对象的关系</param>
        /// <returns></returns>
        Task<List<TreeNodeForBootStrapTreeView>> GetTreeViewNodeForBootStrapTreeViewCollectionAsyn<SelfReferentialEntity>(Expression<Func<SelfReferentialEntity, object>> includeProperty) where SelfReferentialEntity : class, IEntity, new();

        /// <summary>
        /// 获取用于导航树插件 BootStrap-TreeView 节点数据集合的方法
        /// </summary>
        /// <typeparam name="SelfReferentialEntity">具有自引用关系而且继承自 IEntity 的实体模型类型</typeparam>
        /// <param name="predicate">过滤条件</param>
        /// <param name="includeProperty">包含的关联对象的关系</param>
        /// <returns></returns>
        Task<List<TreeNodeForBootStrapTreeView>> GetTreeViewNodeForBootStrapTreeViewCollectionAsyn<SelfReferentialEntity>(Expression<Func<SelfReferentialEntity, bool>> predicate, Expression<Func<SelfReferentialEntity, object>> includeProperty) where SelfReferentialEntity : class, IEntity, new();

        Task<List<TreeNodeForBootStrapTreeView>> GetTreeViewNodeForBootStrapTreeViewWithPlainFacadeItemCollectionAsyn<PlainFacadeItemEntity>() where PlainFacadeItemEntity : class, IEntity, new();

        /// <summary>
        /// 获取当前菜单分组 Id
        /// </summary>
        /// <param name="urlString"></param>
        /// <returns></returns>
        string GetApplicationMenuGroupId(string urlString);

        /// <summary>
        /// 获取当前活动的菜单条目 Id
        /// </summary>
        /// <param name="urlString"></param>
        /// <returns></returns>
        string GetGetApplicationMenuItemActiveId(string urlString);

        /// <summary>
        /// 根据当前登录的用户名和指定的员工类型，返回当前用户与组织机构关联的联机信息视图模型
        ///   1. 对于指定 Employee、VenderEmployee、PurchaserEmployee，提供完整的组织机构关联信息；
        ///   2. 对于其它未处理关联信息的，只返回基础性息
        /// </summary>
        /// <typeparam name="TEmployee"></typeparam>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<UserOnlineInformationVM> GetUserOnlineInformationVM(string userName);

        /// <summary>
        /// 获取平台营运管理子系统通用的单位选择器元素
        /// </summary>
        /// <returns></returns>
        Task<List<OrganizationSelectorVM>> GetOrganizationSelectorItemCollection(string organzationId = null);

        /// <summary>
        /// 根据用户名获取用户归属单位的 Id
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        Task<Guid> CommonGetOrganizationId(string userName);

        /// <summary>
        /// 通过用户名获取用户归属的供应商单位 Id
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        Task<Guid> CommonGetPurchaserId(string userName);

        /// <summary>
        /// 通过用户名获取用户归属的供应商单位 Id
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        Task<Guid> CommonGetVenderId(string userName);

        /// <summary>
        /// 设置用户的登录令牌的声明
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="claimEnum">令牌声明名称</param>
        /// <param name="claimValue">声明对应的值</param>
        /// <returns></returns>
        Task CommonSetUserClarm(string userName, UserCommonClaimEnum claimName, string claimValue);
        #endregion
    }
}

using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.ApplicationCommon.Attachments;
using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.Foundation.SpecificationsForEntityModel;
using LPFW.ORM;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LPFW.DataAccess
{
    /// <summary>
    /// 通用的数据持久层访问处理接口定义：
    ///   1.获取指定类型实体数据对象数量的方法，2个重载；
    ///   2.常规获取指定类型 T 单个实体对象的方法，4个重载；
    ///   3.常规的获取指定的类型 T 实体对象集合的方法，5 个重载；
    ///   4.使用单页数据传输处理模型 ListSinglePageParameter 获取指定类型的 T 的实体对象集合的方法，3个重载；
    ///   5.直接指定分页参数，获取指定类型 T 实体对象分页后的单页数据集合 <see cref="PaginatedList{T}" />，3个重载；
    ///   6.使用分页数据传输模型 ListPageParameter 获取指定型 T 实体对象分页后的单页数据集合 <see cref="PaginatedList{T}" />，4个重载
    ///   7.获取指定类型实体对象个数的方法，2个重载
    ///   8.获取指定类型 TOther 单个实体对象的方法，4个重载；
    ///   9.获取指定类型 TOther 实体对象集合的方法，3个重载；
    ///  10.获取和普通文件、图片、头像、视频文件相关的方法；
    ///  11.判断是否存在指定条件的对象的方法，2个重载；
    ///  12.持久化（保存、删除）实体对象的方法
    /// </summary>
    public interface IEntityRepository<T> where T : class, IEntityBase, new()
    {

        LpDbContext EntitiesContext { get; }
        UserManager<ApplicationUser> ApplicationUserManager { get; }
        RoleManager<ApplicationRole> ApplicationRoleManager { get; }



        #region 1.获取指定类型实体数据对象数量的方法，2个重载
        /// <summary>
        /// 获取指定类型 T 的集合元素数量
        /// </summary>
        /// <returns></returns>
        Task<int> GetBoAmountAsyn();
        Task<int> GetBoAmountAsyn(Expression<Func<T, bool>> predicate);

        #endregion

        #region 2.常规获取指定类型 T 单个实体对象的方法，5个重载
        /// <summary>
        /// 获取指定类型 T 集合元素的第一个
        /// </summary>
        /// <returns></returns>
        Task<T> GetBoAsyn();

        /// <summary>
        /// 根据对象的 Id 值获取类型 T 的单个业务对象
        /// </summary>
        /// <param name="id">业务对象 Id 的值</param>
        /// <returns></returns>
        Task<T> GetBoAsyn(Guid id);

        /// <summary>
        /// 根据对象的 Id 值和指定的对象实体模型中的关联实体参数，获取类型 T 的单个业务对象，同时包含了关联对象的值
        /// </summary>
        /// <param name="id">业务对象 Id 的值</param>
        /// <param name="includeProperties">业务对象模型中指定关联的实体属性的 Lamabda 表达式，例如：x=x.Name,... </param>
        /// <returns></returns>
        Task<T> GetBoAsyn(Guid id, params Expression<Func<T, object>>[] includeProperties); 

        /// <summary>
        /// 根据 Lambda 表达式的条件，获取满足条件的类型 T 集合元素的第一个元素
        /// </summary>
        /// <param name="predicate">使用 Lambada 表达式提交的查询条件</param>
        /// <returns></returns>
        Task<T> GetBoAsyn(Expression<Func<T, bool>> predicate);
        Task<T> GetBoAsyn(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        #endregion

        #region 3.常规的获取指定的类型 T 实体对象集合的方法，5 个重载
        /// <summary>
        /// 获取指定类型 T 的集合的全部元素
        /// </summary>
        /// <returns></returns>
        Task<IQueryable<T>> GetBoCollectionAsyn();

        /// <summary>
        /// 根据关键词，获取指定类型 T 的集合的全部元素
        /// </summary>
        /// <returns></returns>
        Task<IQueryable<T>> GetBoCollectionAsyn(string keyword);

        /// <summary>
        /// 根据指定的对象实体模型中的关联实体参数，获取类型 T 集合的全部业务对象，每个业务对象同时包含了关联对象的值
        /// </summary>
        /// <param name="includeProperties">业务对象模型中管关联的实体的 Lamabda 表达式</param>
        /// <returns></returns>
        Task<IQueryable<T>> GetBoCollectionAsyn(params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// 根据查询条件 Lambda 表达式获取指定类型 T 满足条件的全部元素
        /// </summary>
        /// <param name="predicate">使用 Lambada 表达式提交的查询条件</param>
        /// <returns></returns>
        Task<IQueryable<T>> GetBoCollectionAsyn(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 根据查询条件 Lambda 表达式和指定的对象实体模型中的关联实体参数，获取指定类型 T 满足条件的全部元素
        /// </summary>
        /// <param name="predicate">使用 Lambada 表达式提交的查询条件</param>
        /// <param name="includeProperties">业务对象模型中管关联的实体的 Lamabda 表达式</param>
        /// <returns></returns>
        Task<IQueryable<T>> GetBoCollectionAsyn(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        #endregion

        #region 4.使用单页数据传输处理模型 ListSinglePageParameter 获取指定类型的 T 的实体对象集合的方法，3个重载
        /// <summary>
        /// 根据条件规约模型返回指定类型 T 的全部对象
        /// </summary>
        /// <param name="listPageParameter"></param>
        /// <returns></returns>
        Task<IQueryable<T>> GetBoCollectionAsyn(ListSinglePageParameter listPageParameter);

        /// <summary>
        /// 根据条件规约模型返回指定类型 T 的全部对象，并包括 includeProperties 指定的关联对象的集合
        /// </summary>
        /// <param name="listPageParameter"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<IQueryable<T>> GetBoCollectionAsyn(ListSinglePageParameter listPageParameter, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// 带有导航条件的处理
        /// </summary>
        /// <param name="listPageParameter"></param>
        /// <param name="navigatorPredicate"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<IQueryable<T>> GetBoCollectionAsyn(ListSinglePageParameter listPageParameter, Expression<Func<T, bool>> navigatorPredicate, Expression<Func<T, object>> includePropertiy);

        Task<IQueryable<T>> GetBoCollectionAsyn(ListSinglePageParameter listPageParameter, Expression<Func<T, bool>> navigatorPredicate, params Expression<Func<T, object>>[] includeProperties);

        #endregion

        #region 5.直接指定分页参数，获取指定类型 T 实体对象分页后的单页数据集合 PaginatedList<T>，3个重载
        /// <summary>
        /// 按照指定的属性进行分页，提取分页后的对象集合，在本框架中，通常使用 SortCode
        /// </summary>
        /// <typeparam name="TKey">分页所依赖的属性</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页对象的数量</param>
        /// <param name="keySelector">指定分页依赖属性的 Lambda 表达式</param>
        /// <returns></returns>
        Task<PaginatedList<T>> GetBoPaginateAsyn<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector);

        /// <summary>
        /// 按照指定的属性、查询条件、关联属性进行分页，提取分页后的对象集合，在本框架中，通常使用 SortCode
        /// </summary>
        /// <typeparam name="TKey">分页所依赖的属性</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页的对象数量</param>
        /// <param name="keySelector">指定分页依赖属性的 Lambda 表达式</param>
        /// <param name="predicate">使用 Lambada 表达式提交的查询条件</param>
        /// <param name="includeProperties">业务对象模型中管关联的实体的 Lamabda 表达式</param>
        /// <returns></returns>
        Task<PaginatedList<T>> GetBoPaginateAsyn<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// 按照给定的排序属性、升降序要求、指定的属性、查询条件、关联属性进行分页，提取分页后的对象集合
        /// </summary>
        /// <typeparam name="TKey">分页所依赖的属性</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页的对象数量</param>
        /// <param name="sortProperty">排序的属性</param>
        /// <param name="isDescens">升序/降序</param>
        /// <param name="predicate">使用 Lambada 表达式提交的查询条件</param>
        /// <param name="includeProperties">业务对象模型中管关联的实体的 Lamabda 表达式</param>
        /// <returns></returns>
        Task<PaginatedList<T>> GetBoPaginateAsyn<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> sortProperty, bool isDescens, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        #endregion

        #region 6.使用分页数据传输模型 ListPageParameter 获取指定型 T 实体对象分页后的单页数据集合 PaginatedList<T>，4个重载
        /// <summary>
        /// 使用页面参数定义的方式处理分页数据
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="listPageParameter"></param>
        /// <param name="predicate"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<PaginatedList<T>> GetBoPaginateAsyn(ListPageParameter listPageParameter);
        Task<PaginatedList<T>> GetBoPaginateAsyn(ListPageParameter listPageParameter, params Expression<Func<T, object>>[] includeProperties);
        Task<PaginatedList<T>> GetBoPaginateAsyn(ListPageParameter listPageParameter, Expression<Func<T, bool>> predicate);
        Task<PaginatedList<T>> GetBoPaginateAsyn(ListPageParameter listPageParameter, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        #endregion

        #region 7.获取指定类型实体对象个数的方法，2个重载
        Task<int> GetOtherBoAmountAsyn<TOther>() where TOther : class, IEntityBase, new();
        Task<int> GetOtherBoAmountAsyn<TOther>(Expression<Func<TOther, bool>> predicate) where TOther : class, IEntityBase, new();
        Task<int> GetOtherBoAmountAsyn<TOther>(Expression<Func<TOther, bool>> predicate, params Expression<Func<TOther, object>>[] includeProperties) where TOther : class, IEntityBase, new();
        #endregion

        #region 8.获取指定类型 TOther 单个实体对象的方法，4个重载
        /// <summary>
        /// 根据 id 获取指定类型 TOther 的对象，其中 TOther 指定的实体模型也必须是实现 IEntityBase 的类型
        /// </summary>
        /// <typeparam name="TOther"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TOther> GetOtherBoAsyn<TOther>(Guid id) where TOther : class, IEntityBase, new();
        Task<TOther> GetOtherBoAsyn<TOther>(Guid id, params Expression<Func<TOther, object>>[] includeProperties) where TOther : class, IEntityBase, new();
        Task<TOther> GetOtherBoAsyn<TOther>(Expression<Func<TOther, bool>> predicate) where TOther : class, IEntityBase, new();
        Task<TOther> GetOtherBoAsyn<TOther>(Expression<Func<TOther, bool>> predicate, params Expression<Func<TOther, object>>[] includeProperties) where TOther : class, IEntityBase, new();
        #endregion

        #region 9.获取指定类型 TOther 实体对象集合的方法，3个重载
        /// <summary>
        /// 获取指定类型 TOther 的全部对象，其中 TOther 指定的实体模型也必须是实现 IEntityBase 的类型
        /// </summary>
        /// <typeparam name="TOther"></typeparam>
        /// <returns></returns>
        Task<IQueryable<TOther>> GetOtherBoCollectionAsyn<TOther>() where TOther : class, IEntityBase, new();
        Task<IQueryable<TOther>> GetOtherBoCollectionAsyn<TOther>(Expression<Func<TOther, bool>> predicate) where TOther : class, IEntityBase, new();
        Task<IQueryable<TOther>> GetOtherBoCollectionAsyn<TOther>(Expression<Func<TOther, bool>> predicate, params Expression<Func<TOther, object>>[] includeProperties) where TOther : class, IEntityBase, new();
        
        Task<PaginatedList<TOther>> GetOrtherBoPaginateAsyn<TOther>(ListPageParameter listPageParameter, Expression<Func<TOther, bool>> predicate, params Expression<Func<TOther, object>>[] includeProperties) where TOther : class, IEntityBase, new();

        #endregion

        #region 10.获取和普通文件、图片、头像、视频文件相关的方法
        /// <summary>
        /// 关联上传附件相关的处理方法
        /// </summary>
        /// <param name="relevanceObjectID">与附件文件关联的业务实体对象额 Id</param>
        /// <returns></returns>
        Task<BusinessFile> GetBusinessFileAsyn(Guid relevanceObjectID);
        Task<List<BusinessFile>> GetBusinessFileCollectionAsyn(Guid relevanceObjectID);
        Task<BusinessImage> GetAvatar(Guid relevanceObjectID);
        Task<BusinessImage> GetBusinessImageAsyn(Guid relevanceObjectID);
        Task<List<BusinessImage>> GetBusinessImageCollectionAsyn(Guid relevanceObjectID);
        Task<BusinessVideo> GetBusinessVideoAsyn(Guid relevanceObjectID);
        Task<List<BusinessVideo>> GetBusinessVideoCollectionAsyn(Guid relevanceObjectID);

        #endregion

        #region 11.判断是否存在指定条件的对象的方法，2个重载
        /// <summary>
        /// 根据指定的 Id 的值，判断是否存在相应的对象
        /// </summary>
        /// <param name="id">指定对象的 Id 的值</param>
        /// <returns></returns>
        Task<bool> HasBoAsyn(Guid id);

        /// <summary>
        /// 根据指定的查询条件的值，判断是否存在相应的对象
        /// </summary>
        /// <param name="predicate">>使用 Lambada 表达式提交的查询条件</param>
        /// <returns></returns>
        Task<bool> HasBoAsyn(Expression<Func<T, bool>> predicate);

        Task<bool> HasOtherBoAsyn<TOther>(Guid id) where TOther : class, IEntityBase, new();
        Task<bool> HasOtherBoAsyn<TOther>(Expression<Func<TOther, bool>> predicate) where TOther : class, IEntityBase, new();

        #endregion

        #region 12.持久化（保存、删除）实体对象的方法

        /// <summary>
        /// 保存或者修改指定对象
        /// </summary>
        /// <param name="entity">待处理的对象</param>
        /// <returns>持久化之后的对象</returns>
        Task<SaveStatusModel> SaveBoAsyn(T entity);
        Task<SaveStatusModel> SaveOtherBoWithStatusAsyn<TOther>(TOther entity) where TOther : class, IEntityBase, new();

        /// <summary>
        /// 持久化指定类型 TOther 的数据对象，并从持久层返回相应的对象
        /// </summary>
        /// <typeparam name="TOther"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TOther> SaveOtherBoAsyn<TOther>(TOther entity) where TOther : class, IEntityBase, new();

        /// <summary>
        /// 删除指定 Id 的对象
        /// </summary>
        /// <param name="id">指定对象的 Id 的值</param>
        /// <returns></returns>
        Task<DeleteStatusModel> DeleteBoAsyn(Guid id);

        /// <summary>
        /// 根据查询条件和关联关系限制，批量删除实体对象数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<DeleteStatusModel> DeleteBoAsyn(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// 删除指定对象 Id 和对象的类型，删除对应的类型对象
        /// </summary>
        /// <typeparam name="TOther"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeleteStatusModel> DeleteOtherBoAsyn<TOther>(Guid id) where TOther : class, IEntityBase, new();

        /// <summary>
        /// 根据查询条件和对象类型，批量删除实体对象数据
        /// </summary>
        /// <typeparam name="TOther"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<DeleteStatusModel> DeleteOtherBoAsyn<TOther>(Expression<Func<TOther, bool>> predicate, params Expression<Func<TOther, object>>[] includeProperties) where TOther : class, IEntityBase, new();

        #endregion
    }
}

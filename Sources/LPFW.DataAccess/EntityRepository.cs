using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.ApplicationCommon.Attachments;
using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.Foundation.SpecificationsForEntityModel;
using LPFW.ORM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LPFW.Foundation.Tools;

namespace LPFW.DataAccess
{
    /// <summary>
    /// 泛型仓储接口  <see cref="IEntityRepository{T}" /> 的具体实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityRepository<T> : IEntityRepository<T> where T : class, IEntityBase, new()
    {
        #region 基础属性约束
        private readonly LpDbContext _entitiesContext;               // 数据映射
        private readonly UserManager<ApplicationUser> _userManager;  // 用户管理，统一在这里注入，方便后续应用中直接处理
        private readonly RoleManager<ApplicationRole> _roleManager;  // 角色管理

        // 以下公开的三个只读属性，是为了方便扩展 IEntityRepository<T> 方法时候使用的
        public LpDbContext EntitiesContext { get { return _entitiesContext; } }
        public UserManager<ApplicationUser> ApplicationUserManager { get { return _userManager; } }
        public RoleManager<ApplicationRole> ApplicationRoleManager { get { return _roleManager; } } 
        #endregion

        /// <summary>
        /// 构造函数，负责相关参数的注入和初始化几个基本参数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public EntityRepository(LpDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _entitiesContext = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region 1.获取指定类型实体数据对象数量的方法，2个重载
        /// <summary>
        /// 获取指定类型 T 的集合元素数量
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> GetBoAmountAsyn() => await _entitiesContext.Set<T>().CountAsync();
        public virtual async Task<int> GetBoAmountAsyn(Expression<Func<T, bool>> predicate) => await _entitiesContext.Set<T>().CountAsync(predicate);

        #endregion

        #region 2.常规获取指定类型 T 单个实体对象的方法，4个重载
        /// <summary>
        /// 获取指定类型 T 集合元素的第一个
        /// </summary>
        /// <returns></returns>
        public virtual async Task<T> GetBoAsyn() => await _entitiesContext.Set<T>().FirstOrDefaultAsync();

        /// <summary>
        /// 根据对象的 Id 值获取类型 T 的单个业务对象
        /// </summary>
        /// <param name="id">业务对象 Id 的值</param>
        /// <returns></returns>
        public virtual async Task<T> GetBoAsyn(Guid id) => await _entitiesContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

        /// <summary>
        /// 根据对象的 Id 值和指定的对象实体模型中的关联实体参数，获取类型 T 的单个业务对象，同时包含了关联对象的值
        /// </summary>
        /// <param name="id">业务对象 Id 的值</param>
        /// <param name="includeProperties">业务对象模型中管关联的实体的 Lamabda 表达式</param>
        /// <returns></returns>
        public virtual async Task<T> GetBoAsyn(Guid id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> dbSet = _entitiesContext.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    dbSet = dbSet.Include(includeProperty);
                }
            }

            return await dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// 根据 Lambda 表达式的条件，获取满足条件的类型 T 集合元素的第一个元素
        /// </summary>
        /// <param name="predicate">使用 Lambada 表达式提交的查询条件</param>
        /// <returns></returns>
        public virtual async Task<T> GetBoAsyn(Expression<Func<T, bool>> predicate) => await _entitiesContext.Set<T>().FirstOrDefaultAsync(predicate);

        public virtual async Task<T> GetBoAsyn(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> dbSet = _entitiesContext.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    dbSet = dbSet.Include(includeProperty);
                }
            }

            return await dbSet.FirstOrDefaultAsync(predicate);
        }

        #endregion

        #region 3.常规的获取指定的类型 T 实体对象集合的方法，5 个重载
        /// <summary>
        /// 获取指定类型 T 的集合的全部元素
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IQueryable<T>> GetBoCollectionAsyn()
        {
            var dbSet = _entitiesContext.Set<T>();
            var result = await dbSet.ToListAsync();
            return result.AsQueryable<T>();
        }

        /// <summary>
        /// 根据关键词，获取指定类型 T 的集合的全部元素
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IQueryable<T>> GetBoCollectionAsyn(string keyword)
        {
            if (String.IsNullOrEmpty(keyword))
                return await GetBoCollectionAsyn();
            else
            {
                // 查询条件表达式
                Expression<Func<T, bool>> containsExpession = LambdaHelper.GetConditionExpression<T>(keyword);
                return await GetBoCollectionAsyn(containsExpession);
            }
        }

        /// <summary>
        /// 根据指定的对象实体模型中的关联实体参数，获取类型 T 集合的全部业务对象，每个业务对象同时包含了关联对象的值
        /// </summary>
        /// <param name="includeProperties">业务对象模型中管关联的实体的 Lamabda 表达式</param>
        /// <returns></returns>
        public virtual async Task<IQueryable<T>> GetBoCollectionAsyn(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _entitiesContext.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            var result = await query.ToListAsync();
            return result.AsQueryable();
        }

        /// <summary>
        /// 根据查询条件 Lambda 表达式获取指定类型 T 满足条件的全部元素
        /// </summary>
        /// <param name="predicate">使用 Lambada 表达式提交的查询条件</param>
        /// <returns></returns>
        public virtual async Task<IQueryable<T>> GetBoCollectionAsyn(Expression<Func<T, bool>> predicate)
        {
            var result = await _entitiesContext.Set<T>().Where(predicate).ToListAsync();
            return result.AsQueryable();
        } 

        /// <summary>
        /// 根据查询条件 Lambda 表达式和指定的对象实体模型中的关联实体参数，获取指定类型 T 满足条件的全部元素
        /// </summary>
        /// <param name="predicate">使用 Lambada 表达式提交的查询条件</param>
        /// <param name="includeProperties">业务对象模型中管关联的实体的 Lamabda 表达式</param>
        /// <returns></returns>
        public virtual async Task<IQueryable<T>> GetBoCollectionAsyn(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _entitiesContext.Set<T>().Where(predicate);
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            var result = await query.ToListAsync();
            return result.AsQueryable();
        }
        #endregion

        #region 4.使用单页数据传输处理模型 ListSinglePageParameter 获取指定类型的 T 的实体对象集合的方法，3个重载
        public virtual async Task<IQueryable<T>> GetBoCollectionAsyn(ListSinglePageParameter listPageParameter)
        {
            var keyword = "";
            if (!String.IsNullOrEmpty(listPageParameter.Keyword))
                keyword = listPageParameter.Keyword;

            Expression<Func<T, bool>> predicateExpession = null;

            // 查询条件表达式
            Expression<Func<T, bool>> containsExpession = LambdaHelper.GetConditionExpression<T>(keyword);

            // 类型处理条件表达式
            if (!String.IsNullOrEmpty(listPageParameter.TypeID) && !String.IsNullOrEmpty(listPageParameter.TypeName))
            {
                Expression<Func<T, bool>> typeExpession = LambdaHelper.GetEqual<T>(listPageParameter.TypeName + ".Id", listPageParameter.TypeID);
                predicateExpession = containsExpession.And(typeExpession);
            }
            else
                predicateExpession = containsExpession;

            // 排序属性表达式
            var sortExpession = LambdaHelper.GetPropertyExpression<T,object>(listPageParameter.SortProperty);

            var boCollection = await GetBoCollectionAsyn(predicateExpession);
            // 排序
            if (listPageParameter.SortDesc.ToLower() == "ascend")
                boCollection = boCollection.OrderBy(sortExpession);
            if (listPageParameter.SortDesc.ToLower() == "descend")
                boCollection = boCollection.OrderByDescending(sortExpession);

            return boCollection;
        }

        public virtual async Task<IQueryable<T>> GetBoCollectionAsyn(ListSinglePageParameter listPageParameter, params Expression<Func<T, object>>[] includeProperties)
        {
            // 处理关键词
            var keyword = "";
            if (!String.IsNullOrEmpty(listPageParameter.Keyword))
                keyword = listPageParameter.Keyword;

            Expression<Func<T, bool>> predicateExpession = null;

            // 查询条件表达式
            Expression<Func<T, bool>> containsExpession = LambdaHelper.GetConditionExpression<T>(keyword);

            // 类型处理条件表达式
            // todo 这个处理存在问题
            if (!String.IsNullOrEmpty(listPageParameter.TypeID) && !String.IsNullOrEmpty(listPageParameter.TypeName))
            {
                Expression<Func<T, bool>> typeExpession = LambdaHelper.GetEqual<T>(listPageParameter.TypeName + ".Id", listPageParameter.TypeID);
                predicateExpession = containsExpession.And(typeExpession);
            }
            else
                predicateExpession = containsExpession;

            // 排序属性表达式
            var sortExpession = LambdaHelper.GetPropertyExpression<T,object>(listPageParameter.SortProperty);

            IQueryable<T> boCollection = _entitiesContext.Set<T>().Where(predicateExpession);
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    boCollection = boCollection.Include(includeProperty);
                }
            }

            // 排序
            if (String.IsNullOrEmpty(listPageParameter.SortDesc))
                boCollection = boCollection.OrderBy(sortExpession);
            else
                boCollection = boCollection.OrderByDescending(sortExpession);

            var result = await boCollection.ToListAsync();
            return result.AsQueryable();
        }

        public virtual async Task<IQueryable<T>> GetBoCollectionAsyn(ListSinglePageParameter listPageParameter, Expression<Func<T, bool>> navigatorPredicate, Expression<Func<T, object>> includeProperty)
        {
            // 处理关键词
            var keyword = "";
            if (!String.IsNullOrEmpty(listPageParameter.Keyword))
                keyword = listPageParameter.Keyword;

            Expression<Func<T, bool>> predicateExpession = null;

            // 查询条件表达式
            Expression<Func<T, bool>> containsExpession = LambdaHelper.GetConditionExpression<T>(keyword);

            // 添加导航过滤的表达式
            if (navigatorPredicate != null)
                predicateExpession = containsExpession.And(navigatorPredicate);

            IQueryable<T> boCollection = await GetBoCollectionAsyn(predicateExpession, includeProperty);

            // 排序
            var sortExpession = LambdaHelper.GetPropertyExpression<T,object>(listPageParameter.SortProperty);
            if (String.IsNullOrEmpty(listPageParameter.SortDesc))
                boCollection = boCollection.OrderBy(sortExpession);
            else
                boCollection = boCollection.OrderByDescending(sortExpession);

            return boCollection;
        }

        public virtual async Task<IQueryable<T>> GetBoCollectionAsyn(ListSinglePageParameter listPageParameter, Expression<Func<T, bool>> navigatorPredicate, params Expression<Func<T, object>>[] includeProperties)
        {
            // 处理关键词
            var keyword = "";
            if (!String.IsNullOrEmpty(listPageParameter.Keyword))
                keyword = listPageParameter.Keyword;

            Expression<Func<T, bool>> predicateExpession = null;

            // 查询条件表达式
            Expression<Func<T, bool>> containsExpession = LambdaHelper.GetConditionExpression<T>(keyword);

            // 添加导航过滤的表达式
            if (navigatorPredicate != null)
                predicateExpession = containsExpession.And(navigatorPredicate);

            IQueryable<T> boCollection = await GetBoCollectionAsyn(predicateExpession, includeProperties);

            // 排序
            var sortExpession = LambdaHelper.GetPropertyExpression<T, object>(listPageParameter.SortProperty);
            if (String.IsNullOrEmpty(listPageParameter.SortDesc))
                boCollection = boCollection.OrderBy(sortExpession);
            else
                boCollection = boCollection.OrderByDescending(sortExpession);

            return boCollection;
        }

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
        public virtual async Task<PaginatedList<T>> GetBoPaginateAsyn<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector)
        {
            var result = await GetBoPaginateAsyn(pageIndex, pageSize, keySelector, null);
            return result;
        }

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
        public virtual async Task<PaginatedList<T>> GetBoPaginateAsyn<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = await GetBoCollectionAsyn(includeProperties);
            query = query.OrderBy(keySelector);
            query = (predicate == null) ? query : query.Where(predicate);
            return query.ToPaginatedList(pageIndex, pageSize);
        }

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
        public virtual async Task<PaginatedList<T>> GetBoPaginateAsyn<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> sortProperty, bool isDescend, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = await GetBoCollectionAsyn(includeProperties);

            query = (isDescend == true) ? query.OrderBy(sortProperty) : query.OrderByDescending(sortProperty);
            query = (predicate == null) ? query : query.Where(predicate);

            return query.ToPaginatedList(pageIndex, pageSize);
        }
        #endregion

        #region 6.使用分页数据传输模型 ListPageParameter 获取指定型 T 实体对象分页后的单页数据集合 PaginatedList<T>，4个重载
        public virtual async Task<PaginatedList<T>> GetBoPaginateAsyn(ListPageParameter listPageParameter)
        {
            var pageIndex = Int16.Parse(listPageParameter.PageIndex);
            var pageSize = Int16.Parse(listPageParameter.PageSize);
            var typeID = "";
            var typeName = "";
            var keyword = "";

            if (!String.IsNullOrEmpty(listPageParameter.TypeID))
                typeID = listPageParameter.TypeID;
            if (!String.IsNullOrEmpty(listPageParameter.Keyword))
                keyword = listPageParameter.Keyword;

            // 查询条件表达式
            Expression<Func<T, bool>> predicateExpession = LambdaHelper.GetConditionExpression<T>(keyword);

            // todo 类型处理条件表达式
            //if (!String.IsNullOrEmpty(listPageParameter.TypeID) && !String.IsNullOrEmpty(listPageParameter.TypeName))
            //{
            //    Expression<Func<T, bool>> typeExpession = _GetEqual(listPageParameter.TypeName + ".Id", listPageParameter.TypeID);
            //    predicateExpession = containsExpession.And(typeExpession);
            //}
            //else
            //    predicateExpession = containsExpession;

            // 排序属性表达式
            var sortExpession = LambdaHelper.GetPropertyExpression<T,object>(listPageParameter.SortProperty);

            // 临时数据集合
            var tempCollection = _entitiesContext.Set<T>().Where(predicateExpession);

            // 排序
            if (listPageParameter.SortDesc.ToLower() == "ascend")
                tempCollection = tempCollection.OrderBy(sortExpession);
            if (listPageParameter.SortDesc.ToLower() == "descend")
                tempCollection = tempCollection.OrderByDescending(sortExpession);

            //var isDescend = String.IsNullOrEmpty(listPageParameter.SortProperty);

            // 分页
            var result = await tempCollection.ToPaginatedListAsync(pageIndex, pageSize);

            listPageParameter.PageAmount = result.TotalPageCount.ToString();
            listPageParameter.ObjectAmount = result.TotalCount.ToString();
            listPageParameter.PagenateGroup = PagenateGroupRepository.GetItem<T>(result, 10, pageIndex);
            listPageParameter.Keyword = keyword;
            listPageParameter.TypeName = typeName;

            return result;
        }

        public virtual async Task<PaginatedList<T>> GetBoPaginateAsyn(ListPageParameter listPageParameter, params Expression<Func<T, object>>[] includeProperties)
        {
            var pageIndex = Int16.Parse(listPageParameter.PageIndex);
            var pageSize = Int16.Parse(listPageParameter.PageSize);
            var typeID = "";
            var typeName = "";
            var keyword = "";

            if (!String.IsNullOrEmpty(listPageParameter.TypeID))
                typeID = listPageParameter.TypeID;
            if (!String.IsNullOrEmpty(listPageParameter.Keyword))
                keyword = listPageParameter.Keyword;

            Expression<Func<T, bool>> predicateExpession = null;

            // 查询条件表达式
            Expression<Func<T, bool>> containsExpession = LambdaHelper.GetConditionExpression<T>(keyword);

            // 类型处理条件表达式
            if (!String.IsNullOrEmpty(listPageParameter.TypeID) && !String.IsNullOrEmpty(listPageParameter.TypeName))
            {
                Expression<Func<T, bool>> typeExpession = LambdaHelper.GetEqual<T>(listPageParameter.TypeName + ".Id", listPageParameter.TypeID);
                predicateExpession = containsExpession.And(typeExpession);
            }
            else
                predicateExpession = containsExpession;

            // 排序属性表达式
            var sortExpession = LambdaHelper.GetPropertyExpression<T,object>(listPageParameter.SortProperty);

            // 临时数据集合
            IQueryable<T> query = _entitiesContext.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            var tempCollection = query.Where(predicateExpession);

            // 排序
            if (String.IsNullOrEmpty(listPageParameter.SortProperty))
                tempCollection = tempCollection.OrderBy(sortExpession);
            else
                tempCollection = tempCollection.OrderByDescending(sortExpession);

            var isDescend = String.IsNullOrEmpty(listPageParameter.SortProperty);

            // 分页
            var result = await tempCollection.ToPaginatedListAsync(pageIndex, pageSize);

            listPageParameter.PageAmount = result.TotalPageCount.ToString();
            listPageParameter.ObjectAmount = result.TotalCount.ToString();
            listPageParameter.PagenateGroup = PagenateGroupRepository.GetItem<T>(result, 10, pageIndex);
            listPageParameter.Keyword = keyword;
            listPageParameter.TypeName = typeName;

            return result;

        }

        public virtual async Task<PaginatedList<T>> GetBoPaginateAsyn(ListPageParameter listPageParameter, Expression<Func<T, bool>> predicate)
        {
            var pageIndex = Int16.Parse(listPageParameter.PageIndex);
            var pageSize = Int16.Parse(listPageParameter.PageSize);
            var typeID = "";
            var typeName = "";
            var keyword = "";

            if (!String.IsNullOrEmpty(listPageParameter.TypeID))
                typeID = listPageParameter.TypeID;
            if (!String.IsNullOrEmpty(listPageParameter.Keyword))
                keyword = listPageParameter.Keyword;

            var tempCollection = _entitiesContext.Set<T>().Where(predicate);

            // 排序
            var sortExpession = LambdaHelper.GetPropertyExpression<T,object>(listPageParameter.SortProperty);
            if (String.IsNullOrEmpty(listPageParameter.SortProperty))
                tempCollection = tempCollection.OrderBy(sortExpession);
            else
                tempCollection = tempCollection.OrderByDescending(sortExpession);

            var isDescend = String.IsNullOrEmpty(listPageParameter.SortProperty);

            // 分页
            var result = await tempCollection.ToPaginatedListAsync(pageIndex, pageSize);

            listPageParameter.PageAmount = result.TotalPageCount.ToString();
            listPageParameter.ObjectAmount = result.TotalCount.ToString();
            listPageParameter.PagenateGroup = PagenateGroupRepository.GetItem<T>(result, 10, pageIndex);
            listPageParameter.Keyword = keyword;
            listPageParameter.TypeName = typeName;

            return result;

        }

        public virtual async Task<PaginatedList<T>> GetBoPaginateAsyn(ListPageParameter listPageParameter, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            // 处理关键词
            var keyword = "";
            if (!String.IsNullOrEmpty(listPageParameter.Keyword))
                keyword = listPageParameter.Keyword;

            Expression<Func<T, bool>> predicateExpession = null;

            // 查询条件表达式
            Expression<Func<T, bool>> containsExpession = LambdaHelper.GetConditionExpression<T>(keyword);

            // 添加导航过滤的表达式
            predicateExpession = containsExpession.And(predicate);

            IQueryable<T> dbSet = _entitiesContext.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    dbSet = dbSet.Include(includeProperty);
                }
            }

            IQueryable<T> boCollection = dbSet.Where(predicate); 

            // 排序
            var sortExpession = LambdaHelper.GetPropertyExpression<T,object>(listPageParameter.SortProperty);
            if (String.IsNullOrEmpty(listPageParameter.SortDesc))
                boCollection = boCollection.OrderBy(sortExpession);
            else
                boCollection = boCollection.OrderByDescending(sortExpession);

            // 分页
            var pageIndex = Int16.Parse(listPageParameter.PageIndex);
            var pageSize = Int16.Parse(listPageParameter.PageSize);

            var result = await boCollection.ToPaginatedListAsync(pageIndex, pageSize);

            listPageParameter.PageAmount = result.TotalPageCount.ToString();
            listPageParameter.ObjectAmount = result.TotalCount.ToString();
            listPageParameter.PagenateGroup = PagenateGroupRepository.GetItem<T>(result, 10, pageIndex);

            return result;
        }

        #endregion

        #region 7.获取指定类型实体对象个数的方法，3个重载
        public virtual async Task<int> GetOtherBoAmountAsyn<TOther>() where TOther : class, IEntityBase, new() 
            => await _entitiesContext.Set<TOther>().CountAsync();

        public virtual async Task<int> GetOtherBoAmountAsyn<TOther>(Expression<Func<TOther, bool>> predicate) where TOther : class, IEntityBase, new()
             => await _entitiesContext.Set<TOther>().CountAsync(predicate);

        public virtual async Task<int> GetOtherBoAmountAsyn<TOther>(Expression<Func<TOther, bool>> predicate, params Expression<Func<TOther, object>>[] includeProperties) where TOther : class, IEntityBase, new()
        {
            IQueryable<TOther> dbSet = _entitiesContext.Set<TOther>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    dbSet = dbSet.Include(includeProperty);
                }
            }
            return await dbSet.CountAsync(predicate);
        }
        #endregion

        #region 8.获取指定类型 TOther 单个实体对象的方法，4个重载
        /// <summary>
        /// 根据 id 获取指定类型 TOther 的对象
        /// </summary>
        /// <typeparam name="TOther"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TOther> GetOtherBoAsyn<TOther>(Guid id) where TOther : class, IEntityBase, new() => await _entitiesContext.Set<TOther>().FirstOrDefaultAsync(x => x.Id == id);

        public virtual async Task<TOther> GetOtherBoAsyn<TOther>(Guid id, params Expression<Func<TOther, object>>[] includeProperties) where TOther : class, IEntityBase, new()
        {
            IQueryable<TOther> dbSet = _entitiesContext.Set<TOther>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    dbSet = dbSet.Include(includeProperty);
                }
            }
            return await dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<TOther> GetOtherBoAsyn<TOther>(Expression<Func<TOther, bool>> predicate) where TOther : class, IEntityBase, new()
        {
            return await _entitiesContext.Set<TOther>().FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<TOther> GetOtherBoAsyn<TOther>(Expression<Func<TOther, bool>> predicate, params Expression<Func<TOther, object>>[] includeProperties) where TOther : class, IEntityBase, new()
        {
            IQueryable<TOther> dbSet = _entitiesContext.Set<TOther>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    dbSet = dbSet.Include(includeProperty);
                }
            }
            return await dbSet.FirstOrDefaultAsync(predicate);
        }
        #endregion

        #region 9.获取指定类型 TOther 实体对象集合的方法，3个重载
        /// <summary>
        /// 获取指定类型 TOther 的全部对象
        /// </summary>
        /// <typeparam name="TOther"></typeparam>
        /// <returns></returns>
        public virtual async Task<IQueryable<TOther>> GetOtherBoCollectionAsyn<TOther>() where TOther : class, IEntityBase, new()
        {
            var dbSet = _entitiesContext.Set<TOther>();
            var result = await dbSet.ToListAsync();
            return result.AsQueryable<TOther>();
        }

        public virtual async Task<IQueryable<TOther>> GetOtherBoCollectionAsyn<TOther>(Expression<Func<TOther, bool>> predicate) where TOther : class, IEntityBase, new()
        {
            var dbSet = _entitiesContext.Set<TOther>();
            var result = await dbSet.ToListAsync();
            IQueryable<TOther> query = result.AsQueryable().Where(predicate);
            return query;
        }

        public virtual async Task<IQueryable<TOther>> GetOtherBoCollectionAsyn<TOther>(Expression<Func<TOther, bool>> predicate, params Expression<Func<TOther, object>>[] includeProperties) where TOther : class, IEntityBase, new()
        {
            IQueryable<TOther> dbSet = _entitiesContext.Set<TOther>().Where(predicate);
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    dbSet = dbSet.Include(includeProperty);
                }
            }

            var result = await dbSet.ToListAsync();
            IQueryable<TOther> query = result.AsQueryable();
            return query;
        }

        public virtual async Task<PaginatedList<TOther>> GetOrtherBoPaginateAsyn<TOther>(ListPageParameter listPageParameter, Expression<Func<TOther, bool>> predicate, params Expression<Func<TOther, object>>[] includeProperties) where TOther : class, IEntityBase, new()
        {
            // 处理关键词
            var keyword = "";
            if (!String.IsNullOrEmpty(listPageParameter.Keyword))
                keyword = listPageParameter.Keyword;

            Expression<Func<TOther, bool>> predicateExpession = null;

            // 查询条件表达式
            Expression<Func<TOther, bool>> containsExpession = LambdaHelper.GetConditionExpression<TOther>(keyword);

            // 添加导航过滤的表达式
            predicateExpession = containsExpession.And(predicate);

            IQueryable<TOther> dbSet = _entitiesContext.Set<TOther>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    dbSet = dbSet.Include(includeProperty);
                }
            }

            IQueryable<TOther> boCollection = dbSet.Where(predicate);

            // 排序
            var sortExpession = LambdaHelper.GetPropertyExpression<TOther, object>(listPageParameter.SortProperty);
            if (String.IsNullOrEmpty(listPageParameter.SortDesc))
                boCollection = boCollection.OrderBy(sortExpession);
            else
                boCollection = boCollection.OrderByDescending(sortExpession);

            // 分页
            var pageIndex = Int16.Parse(listPageParameter.PageIndex);
            var pageSize = Int16.Parse(listPageParameter.PageSize);

            var result = await boCollection.ToPaginatedListAsync(pageIndex, pageSize);

            listPageParameter.PageAmount = result.TotalPageCount.ToString();
            listPageParameter.ObjectAmount = result.TotalCount.ToString();
            listPageParameter.PagenateGroup = PagenateGroupRepository.GetItem<TOther>(result, 10, pageIndex);

            return result;
        }

        #endregion

        #region 10.获取和普通文件、图片、头像、视频文件相关的方法
        public virtual async Task<BusinessFile> GetBusinessFileAsyn(Guid relevanceObjectID) =>
            await _entitiesContext.Set<BusinessFile>().FirstOrDefaultAsync(x => x.RelevanceObjectID == relevanceObjectID && x.IsUnique == true);

        public virtual async Task<List<BusinessFile>> GetBusinessFileCollectionAsyn(Guid relevanceObjectID) =>
            await _entitiesContext.Set<BusinessFile>().Where(x => x.RelevanceObjectID == relevanceObjectID && x.IsUnique == false).ToListAsync();

        public virtual async Task<BusinessImage> GetAvatar(Guid relevanceObjectID) =>
            await _entitiesContext.Set<BusinessImage>().FirstOrDefaultAsync(x => x.RelevanceObjectID == relevanceObjectID && x.IsUnique == true && x.IsAvatar == true);

        public virtual async Task<BusinessImage> GetBusinessImageAsyn(Guid relevanceObjectID) =>
            await _entitiesContext.Set<BusinessImage>().FirstOrDefaultAsync(x => x.RelevanceObjectID == relevanceObjectID && x.IsUnique == true && x.IsAvatar == false);

        public virtual async Task<List<BusinessImage>> GetBusinessImageCollectionAsyn(Guid relevanceObjectID) =>
            await _entitiesContext.Set<BusinessImage>().Where(x => x.RelevanceObjectID == relevanceObjectID && x.IsUnique == false).ToListAsync();

        public virtual async Task<BusinessVideo> GetBusinessVideoAsyn(Guid relevanceObjectID) =>
            await _entitiesContext.Set<BusinessVideo>().FirstOrDefaultAsync(x => x.RelevanceObjectID == relevanceObjectID && x.IsUnique == true);

        public virtual async Task<List<BusinessVideo>> GetBusinessVideoCollectionAsyn(Guid relevanceObjectID) =>
            await _entitiesContext.Set<BusinessVideo>().Where(x => x.RelevanceObjectID == relevanceObjectID && x.IsUnique == false).ToListAsync();

        #endregion

        #region 11.判断是否存在指定条件的对象的方法，2个重载
        /// <summary>
        /// 根据指定的 Id 的值，判断是否存在相应的对象
        /// </summary>
        /// <param name="id">指定对象的 Id 的值</param>
        /// <returns></returns>
        public virtual async Task<bool> HasBoAsyn(Guid id) => await _entitiesContext.Set<T>().AnyAsync(x => x.Id == id);

        /// <summary>
        /// 根据指定的查询条件的值，判断是否存在相应的对象
        /// </summary>
        /// <param name="predicate">>使用 Lambada 表达式提交的查询条件</param>
        /// <returns></returns>
        public virtual async Task<bool> HasBoAsyn(Expression<Func<T, bool>> predicate) => await _entitiesContext.Set<T>().AnyAsync(predicate);

        public virtual async Task<bool> HasOtherBoAsyn<TOther>(Guid id) where TOther : class, IEntityBase, new() 
            => await _entitiesContext.Set<TOther>().AnyAsync(x => x.Id == id);

        public virtual async Task<bool> HasOtherBoAsyn<TOther>(Expression<Func<TOther, bool>> predicate) where TOther : class, IEntityBase, new() 
            => await _entitiesContext.Set<TOther>().AnyAsync(predicate);

        #endregion

        #region 12.持久化（保存、删除）实体对象的方法
        /// <summary>
        /// 保存或者修改指定对象
        /// </summary>
        /// <param name="entity">待处理的对象</param>
        /// <returns></returns>
        public virtual async Task<SaveStatusModel> SaveBoAsyn(T entity)
        {
            var dbSet = _entitiesContext.Set<T>();
            var result = new SaveStatusModel() { SaveSatus = true, Message = "保存操作成功！" };
            var hasInstance = await HasBoAsyn(entity.Id);
            if (hasInstance)
                dbSet.Update(entity);
            else
                await dbSet.AddAsync(entity);

            try
            {
                await _entitiesContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                result.SaveSatus = false;
                result.Message = "[调试时捕获的错误：]" + ex.InnerException.ToString();
            }

            return result;
        }

        public virtual async Task<SaveStatusModel> SaveOtherBoWithStatusAsyn<TOther>(TOther entity) where TOther : class, IEntityBase, new()
        {
            var result = new SaveStatusModel() { SaveSatus = true, Message = "保存操作成功！" };
            var dbSet = _entitiesContext.Set<TOther>();

            var hasInstance = await _entitiesContext.Set<TOther>().AnyAsync(x => x.Id == entity.Id);

            if (hasInstance)
                dbSet.Update(entity);
            else
                await dbSet.AddAsync(entity);
            try
            {
                await _entitiesContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                result.SaveSatus = false;
                result.Message = "[调试时捕获的错误：]" + ex.InnerException.ToString();
            }

            return result;
        }

        public virtual async Task<TOther> SaveOtherBoAsyn<TOther>(TOther entity) where TOther : class, IEntityBase, new()
        {
            var dbSet = _entitiesContext.Set<TOther>();

            //var hasInstance = await HasBoAsyn(entity.Id);// 不能调用，表对应T类型而不是TOther
            var hasInstance = await _entitiesContext.Set<TOther>().AnyAsync(x => x.Id == entity.Id);// 手动实现

            if (hasInstance)
                dbSet.Update(entity);
            else
                await dbSet.AddAsync(entity);
            await _entitiesContext.SaveChangesAsync();

            return _entitiesContext.Set<TOther>().FirstOrDefault(x => x.Id == entity.Id);
           
        }

        /// <summary>
        /// 删除指定 Id 值的对象
        /// </summary>
        /// <param name="id">指定对象的 Id 的值</param>
        /// <returns></returns>
        public virtual async Task<DeleteStatusModel> DeleteBoAsyn(Guid id)
        {
            var result = new DeleteStatusModel() { DeleteSatus = true, Message = "删除操作成功！" };
            var hasIstance = await HasBoAsyn(id);
            if (!hasIstance)
            {
                result.DeleteSatus = false;
                result.Message = "不存在所指定的数据，无法执行删除操作！";
            }
            else
            {
                var tobeDeleteItem = await GetBoAsyn(id);
                try
                {
                    _entitiesContext.Set<T>().Remove(tobeDeleteItem);
                    _entitiesContext.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    result.DeleteSatus = false;
                    result.Message = "你所删除的数据，已经在其它相关业务中使用了，不能执行删除。除非你检查相关的关联数据，清理删除与此依赖的其他数据，否则后续的删除依然不能进行。";
                }
            }
            return result;
        }

        public virtual async Task<DeleteStatusModel> DeleteBoAsyn(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) 
        {
            var result = new DeleteStatusModel() { DeleteSatus = true, Message = "删除操作成功！" };
            var boCollection = await GetBoCollectionAsyn(predicate, includeProperties);
            if (boCollection.Any()) 
            {
                try
                {
                    _entitiesContext.Set<T>().RemoveRange(boCollection);
                    _entitiesContext.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    result.DeleteSatus = false;
                    result.Message = "你所删除的数据，已经在其它相关业务中使用了，不能执行删除。除非你检查相关的关联数据，清理删除与此依赖的其他数据，否则后续的删除依然不能进行。";
                }

            }
            return result;
        }


        public virtual async Task<DeleteStatusModel> DeleteOtherBoAsyn<TOther>(Guid id) where TOther : class, IEntityBase, new()
        {
            var result = new DeleteStatusModel() { DeleteSatus = true, Message = "删除操作成功！" };
            var hasIstance = await _entitiesContext.Set<TOther>().AnyAsync(x => x.Id == id);
            if (!hasIstance)
            {
                result.DeleteSatus = false;
                result.Message = "不存在所指定的数据，无法执行删除操作！";
            }
            else
            {
                var tobeDeleteItem = await GetOtherBoAsyn<TOther>(id);
                try
                {
                    _entitiesContext.Set<TOther>().Remove(tobeDeleteItem);
                    _entitiesContext.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    result.DeleteSatus = false;
                    result.Message = "你所删除的数据，已经在其它相关业务中使用了，不能执行删除。除非你检查相关的关联数据，清理删除与此依赖的其他数据，否则后续的删除依然不能进行。";
                }
            }
            return result;
        }

        public virtual async Task<DeleteStatusModel> DeleteOtherBoAsyn<TOther>(Expression<Func<TOther, bool>> predicate, params Expression<Func<TOther, object>>[] includeProperties) where TOther : class, IEntityBase, new() 
        {
            var result = new DeleteStatusModel() { DeleteSatus = true, Message = "删除操作成功！" };
            var boCollection = await GetOtherBoCollectionAsyn(predicate, includeProperties);
            try
            {
                _entitiesContext.Set<TOther>().RemoveRange(boCollection);
                _entitiesContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                result.DeleteSatus = false;
                result.Message = "你所删除的数据，已经在其它相关业务中使用了，不能执行删除。除非你检查相关的关联数据，清理删除与此依赖的其他数据，否则后续的删除依然不能进行。";
            }
            return result;
        }

        #endregion
    }
}

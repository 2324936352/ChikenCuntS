using LPFW.DataAccess.Tools;
using LPFW.Foundation.SpecificationsForEntityModel;
using LPFW.ViewModels;
using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPFW.ViewModelServices.Tools
{
    /// <summary>
    /// 常规的视图模型与实体模型之间转换助理方法
    /// </summary>
    public static class ConversionForViewModelHelper
    {
        /// <summary>
        /// 根据指定实体模型和视图模型的对象进行转换，返回一个指定类型的视图模型对象
        /// </summary>
        /// <typeparam name="TEntityModel">实体模型对象类型</typeparam>
        /// <typeparam name="TViewModel"> 实体模型对象类型</typeparam>
        /// <param name="bo">实体模型对象</param>
        /// <returns></returns>
        public static TViewModel GetFromEntity<TEntityModel, TViewModel>(TEntityModel bo)
            where TEntityModel : class, IEntityBase, new()
            where TViewModel : class, IEntityViewModel, new()
        {
            var isNew = false;
            if (bo == null)
            {
                bo = new TEntityModel();
                isNew = true;
            }
            var boVM = new TViewModel();
            boVM.IsNew = isNew;

            // 映射基本属性值
            bo.MapToViewModel<TEntityModel, TViewModel>(boVM);
            return boVM;
        }

        /// <summary>
        /// 根据指定实体模型和API模型的对象进行转换，返回一个指定类型的 API 模型对象集合
        /// </summary>
        /// <typeparam name="TEntityModel">实体模型对象类型</typeparam>
        /// <typeparam name="TViewModel">实体模型对象类型</typeparam>
        /// <param name="entityCollection">实体对象集合</param>
        /// <returns></returns>
        public static List<TViewModel> GetFromEntityCollection<TEntityModel, TViewModel>(IQueryable<TEntityModel> entityCollection)
            where TEntityModel : class, IEntityBase, new()
            where TViewModel : class, IEntityViewModel, new()
        {
            var boVMCollection = new List<TViewModel>();
            int count = 0;
            foreach (var item in entityCollection)
            {
                var boVM = new TViewModel();
                item.MapToViewModel<TEntityModel, TViewModel>(boVM);
                boVM.OrderNumber = (++count).ToString();
                boVMCollection.Add(boVM);
            }
            return boVMCollection;
        }

        /// <summary>
        /// 根据实体模型对象集合和分页约束模型对象生成视图模型对象集合
        /// </summary>
        /// <param name="entityCollection">实体模型集合</param>
        /// <param name="listPageParameter">分页约束模型对象</param>
        /// <returns></returns>
        public static List<TViewModel> GetFromEntityCollection<TEntityModel, TViewModel>(PaginatedList<TEntityModel> entityCollection, ListPageParameter listPageParameter)
            where TEntityModel : class, IEntityBase, new()
            where TViewModel : class, IEntityViewModel, new()
        {
            var boVMCollection = new List<TViewModel>();
            // 初始化视图模型起始序号
            int count = (int.Parse(listPageParameter.PageIndex) - 1) * int.Parse(listPageParameter.PageSize);
            foreach (var item in entityCollection)
            {
                var boVM = new TViewModel();
                item.MapToViewModel<TEntityModel, TViewModel>(boVM);
                boVM.OrderNumber = (++count).ToString();
                boVMCollection.Add(boVM);
            }
            return boVMCollection;
        }

        /// <summary>
        /// 根据实体模型对象集合生成视图模型带有层次形式外观的对象集合
        /// </summary>
        /// <param name="entityCollection">实体模型集合</param>
        /// <returns></returns>
        public static List<TViewModel> GetFromEntityCollectionWithHierarchicalStyle<TEntityModel, TViewModel>(IQueryable<TEntityModel> entityCollection)
            where TEntityModel : class, IEntity, new()
            where TViewModel : class, IEntityViewModel, new()
        {
            var selfReferentialItemCollection = SelfReferentialItemFactory<TEntityModel>.GetCollection(entityCollection.ToList(), true);
            var boVMCollection = new List<TViewModel>();
            int count = 0;
            foreach (var item in entityCollection.OrderBy(x => x.SortCode))
            {
                var boVM = new TViewModel();
                item.MapToViewModel<TEntityModel, TViewModel>(boVM);
                boVM.OrderNumber = (++count).ToString();
                var sItem = selfReferentialItemCollection.FirstOrDefault(x => x.ID == item.Id.ToString());
                if (sItem != null)
                    boVM.Name = sItem.DisplayName;

                boVMCollection.Add(boVM);
            }
            return boVMCollection;
        }
    }
}

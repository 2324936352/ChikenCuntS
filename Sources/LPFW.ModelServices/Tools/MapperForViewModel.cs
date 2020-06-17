using LPFW.Foundation.SpecificationsForEntityModel;
using LPFW.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LPFW.ViewModelServices.Tools
{
    /// <summary>
    /// 实体模型和视图模型映射器扩展方法
    /// </summary>
    public static class MapperForViewModel
    {
        /// <summary>
        /// 为继承自 IEntity 的实体创建扩展方法，将其常规属性的值映射到指定的视图模型
        /// 1. 将常规类型的属性进行映射；
        /// 2. 将关联属性与视图模型进行映射 
        /// </summary>
        /// <typeparam name="TEntity">继承自 IEntity 的实体模型的类型</typeparam>
        /// <typeparam name="TViewModel">继承自 IEntityViewModel 的视图模型的类型 </typeparam>
        /// <param name="bo"></param>
        /// <param name="boVM">待映射处理的视图模型</param>
        public static void MapToViewModel<TEntity, TViewModel>(this TEntity bo, TViewModel boVM)
            where TEntity : class, IEntityBase, new()
            where TViewModel : class, IEntityViewModel, new()
        {
            boVM.Id = bo.Id;

            PropertyInfo[] boProperties = typeof(TEntity).GetProperties();
            PropertyInfo[] boVMProperties = typeof(TViewModel).GetProperties();

            foreach (var item in boProperties)
            {
                var itemTypeName = item.PropertyType.Name;
                var itemTypeFullName = item.PropertyType.FullName;

                // 排除自定义的属性类型
                if (!itemTypeFullName.Contains("LPFW"))
                {
                    // 提取对应的视图模型的属性
                    var boVM_Property = boVMProperties.Where(x => x.Name == item.Name).FirstOrDefault();
                    if (boVM_Property != null)
                    {
                        // 为视图模型指定的属性，使用 Bo 对应的属性赋值
                        boVM_Property.SetValue(boVM, item.GetValue(bo));
                    }
                }
                // 处理自定义的属性值
                else
                {
                    // 提取属性的值
                    var relevanceObject = item.GetValue(bo);
                    if (relevanceObject != null)
                    {
                        // 排除枚举
                        if (relevanceObject.GetType().IsEnum == false)
                        {
                            // 获取自定义类型的属性集合
                            PropertyInfo[] relevanceObjectProperties = relevanceObject.GetType().GetProperties();

                            // 处理 Id 相关的值
                            var relevanceObjectIdProperty = relevanceObjectProperties.FirstOrDefault(x => x.Name == "Id");
                            if (relevanceObjectIdProperty != null)
                            {
                                var boVM_RelevanceObjectIdProperty = boVMProperties.Where(x => x.Name == item.Name + "Id").FirstOrDefault();
                                if (boVM_RelevanceObjectIdProperty != null) 
                                {
                                    var typeName = boVM_RelevanceObjectIdProperty.PropertyType.Name.ToString().ToLower();
                                    if (typeName == "string")
                                        boVM_RelevanceObjectIdProperty.SetValue(boVM, relevanceObjectIdProperty.GetValue(relevanceObject).ToString());
                                    else
                                    {
                                        boVM_RelevanceObjectIdProperty.SetValue(boVM, relevanceObjectIdProperty.GetValue(relevanceObject));
                                    }
                                }
                            }
                            // 处理 Name 相关的值
                            var relevanceObjectNameProperty = relevanceObjectProperties.FirstOrDefault(x => x.Name == "Name");
                            if (relevanceObjectNameProperty != null)
                            {
                                var boVM_RelevanceObjectNameProperty = boVMProperties.Where(x => x.Name == item.Name + "Name").FirstOrDefault();
                                if (boVM_RelevanceObjectNameProperty != null)
                                    boVM_RelevanceObjectNameProperty.SetValue(boVM, relevanceObjectNameProperty.GetValue(relevanceObject).ToString());
                            }
                        }
                        // 处理枚举
                        else
                        {
                            // 处理对应的视图模型的枚举属性
                            var boVM_EnumProperty = boVMProperties.Where(x => x.Name == item.Name).FirstOrDefault();
                            if (boVM_EnumProperty != null)
                                boVM_EnumProperty.SetValue(boVM, relevanceObject);

                            // 处理对应的视图模型的枚举属性+Name的属性
                            var boVM_EnumNameProperty = boVMProperties.Where(x => x.Name == item.Name+"Name").FirstOrDefault();
                            if (boVM_EnumNameProperty != null)
                                boVM_EnumNameProperty.SetValue(boVM, relevanceObject.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 为继承自 IEntityViewModel 的视图模型创建扩展方法，以便将视图模型的数据映射至对应的实体模型
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="boVM"></param>
        /// <param name="bo"></param>
        public static void MapToEntityModel<TViewModel,TEntity>(this TViewModel boVM,TEntity bo)
            where TViewModel : class, IEntityViewModel, new()
            where TEntity : class, IEntityBase, new()
        {
            PropertyInfo[] boVMProperties = typeof(TViewModel).GetProperties();
            PropertyInfo[] boProperties = typeof(TEntity).GetProperties();

            foreach (var item in boVMProperties)
            {
                var itemTypeName = item.PropertyType.Name;
                var itemTypeFullName = item.PropertyType.FullName;

                // 排除自定义的属性类型
                if (!itemTypeFullName.Contains("LPFW"))
                {
                    // 提取对应的视图模型的属性
                    var bo_Property = boProperties.Where(x => x.Name == item.Name).FirstOrDefault();
                    if (bo_Property != null)
                    {
                        if (bo_Property.Name.ToLower() == "datetime")
                        {
                            var tempValue = (DateTime)item.GetValue(boVM);
                            bo_Property.SetValue(bo, tempValue);
                        }
                        else
                        {
                            try
                            {
                                bo_Property.SetValue(bo, item.GetValue(boVM));
                            }
                            catch(Exception ex)
                            {
                                var a = ex;
                            }

                        }
                    }
                }
            }
        }
    }
}

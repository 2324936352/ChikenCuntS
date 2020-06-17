using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.Demo;
using LPFW.ViewModelServices.Tools;
using LPFW.ViewModels.Common;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels.Demo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using LPFW.DataAccess.Extensions.Demo;
using System.Linq;

namespace LPFW.ViewModelServices.Extensions.Demo
{
    /// <summary>
    /// 针对 <see cref="IViewModelService{TPrimaryKey,DemoEntityVM}" /> 的扩展方法，这个扩展的方式对很多实体模型适用。
    /// </summary>
    public static class DemoEntityServiceExtension
    {
        /// <summary>
        /// 初始化配置关联属性相关的数据，几乎在所有存在关联关系的属性，都应该使用这个方法，包括名称都应该一致。
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boId"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task GetboVMRelevanceData(this IViewModelService<DemoEntity, DemoEntityVM> service,DemoEntityVM boVM)
        {
            var boId = boVM.Id;

            var bo = await service.EntityRepository.GetBoAsyn(boId, x => x.DemoEntityParent, y=>y.DemoCommon);
            if (bo == null)
                bo= new DemoEntity();

            // 获取与枚举值相关的处理
            if (!boVM.IsNew)
            {
                boVM.DemoEntityEnum = bo.DemoEntityEnum;
                boVM.DemoEntityEnumName = bo.DemoEntityEnum.ToString();
            }
            boVM.DemoEntityEnumItemCollection = PlainFacadeItemFactory<DemoEntity>.GetByEnum(bo.DemoEntityEnum);

            // 设置上级关系的视图模型数据
            if (bo.DemoEntityParent != null)
            {
                boVM.DemoEntityParentId = bo.DemoEntityParent.Id.ToString();
                boVM.DemoEntityParentName = bo.DemoEntityParent.Name;
            }
            // 设置供给前端视图模型选择上级元素时的下拉选项集合
            var sourceItems01 = await service.EntityRepository.GetOtherBoCollectionAsyn<DemoEntityParent>();
            boVM.DemoEntityParentItemCollection = SelfReferentialItemFactory<DemoEntityParent>.GetCollection(sourceItems01.ToList(), true);
            
            // 设置选择关联的多选一关系
            if (bo.DemoCommon != null)
            {
                boVM.DemoCommonId = bo.DemoCommon.Id.ToString();
                boVM.DemoCommonName = bo.DemoCommon.Name;
            }
            var sourceItems02 = await service.EntityRepository.GetOtherBoCollectionAsyn<DemoCommon>();
            boVM.DemoCommonItemCollection = PlainFacadeItemFactory<DemoCommon>.Get(sourceItems02.ToList());

            //// 设置多选关系
            //if (bo.DemoItemForMultiSelects != null)
            //{
            //    boVM.DemoItemForMultiSelectId = new string[bo.DemoItemForMultiSelects.Count];
            //    boVM.DemoItemForMultiSelectName = new string[bo.DemoItemForMultiSelects.Count];
            //    for (int i = 0; i < bo.DemoItemForMultiSelects.Count; i++)
            //    {
            //        boVM.DemoItemForMultiSelectId[i] = bo.DemoItemForMultiSelects[i].Id.ToString();
            //        boVM.DemoItemForMultiSelectName[i] = bo.DemoItemForMultiSelects[i].Name;
            //    }
            //}
            //else
            //{
            //    boVM.DemoItemForMultiSelectId = new string[0];
            //    boVM.DemoItemForMultiSelectName = new string[0];
            //}
            var sourceItems03 = await service.EntityRepository.GetOtherBoCollectionAsyn<DemoItemForMultiSelect>();
            boVM.DemoItemForMultiSelectItemCollection = PlainFacadeItemFactory<DemoItemForMultiSelect>.Get(sourceItems03.ToList());
        }

        /// <summary>
        /// 存储具有关联关系的业务实体模型对象
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task<SaveStatusModel> SaveBoWithRelevanceDataAsyn(this IViewModelService<DemoEntity, DemoEntityVM> service, DemoEntityVM boVM)
        {
            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.GetBoAsyn(boVM.Id);
            if (bo == null)
                bo = new DemoEntity();

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel<DemoEntityVM, DemoEntity>(bo);

            // 处理关联的 DemoEntityParent
            if (!String.IsNullOrEmpty(boVM.DemoEntityParentId))
            {
                var parentId = Guid.Parse(boVM.DemoEntityParentId);
                var parentItem = await service.EntityRepository.GetOtherBoAsyn<DemoEntityParent>(parentId);
                bo.DemoEntityParent = parentItem;
            }

            // 处理关联的 DemoCommon
            if (!String.IsNullOrEmpty(boVM.DemoCommonId))
            {
                var commonId = Guid.Parse(boVM.DemoCommonId);
                var commonItem = await service.EntityRepository.GetOtherBoAsyn<DemoCommon>(commonId);
                bo.DemoCommon = commonItem;
            }

            // 处理枚举
            bo.DemoEntityEnum = boVM.DemoEntityEnum;

                
            // 执行持久化处理，如果失败，直接返回
            var saveStatus = await service.EntityRepository.SaveBoAsyn(bo);
            if (saveStatus.SaveSatus != true)
                return saveStatus;
        
            // 处理关联的示例订单产品
            if (boVM.DemoEntityItemVMCollection != null)
            {
                // 更新数据
                foreach (var item in boVM.DemoEntityItemVMCollection)
                {
                    // 提取订单条目对象
                    var itemObject = await service.EntityRepository.GetOtherBoAsyn<DemoEntityItem>(item.Id);
                    if (itemObject == null)
                        itemObject = new DemoEntityItem();

                    // 映射数据
                    item.MapToEntityModel(itemObject);

                    // 处理关联订单
                    itemObject.DemoEntity = bo;

                    // 持久化, 如果失败直接返回
                    saveStatus = await service.EntityRepository.SaveOtherBoWithStatusAsyn<DemoEntityItem>(itemObject);
                    if (saveStatus.SaveSatus != true)
                        return saveStatus;

                }
            }

            return saveStatus;
        }

        /// <summary>
        /// 获取关联的普通文件数据，用于处理上传后刷新数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boId"></param>
        /// <returns></returns>
        public static async Task<List<BusinessFileVM>> GetBusinessFileVMCollection(this IViewModelService<DemoEntity, DemoEntityVM> service, Guid boId)
        {
            var result = new List<BusinessFileVM>();

            var businessFileCollection = await service.EntityRepository.GetBusinessFileCollectionAsyn(boId);
            foreach (var item in businessFileCollection)
                result.Add(new BusinessFileVM(item));

            return result;
        }

        public static async Task<List<BusinessImageVM>> GetBusinessImageVMCollection(this IViewModelService<DemoEntity, DemoEntityVM> service, Guid boId)
        {
            var result = new List<BusinessImageVM>();

            var businessFileCollection = await service.EntityRepository.GetBusinessImageCollectionAsyn(boId);
            foreach (var item in businessFileCollection)
                result.Add(new BusinessImageVM(item));

            return result;
        }

        /// <summary>
        /// 为传入的视图模型设置归属于其中的子元素的视图模型集合，用于在前端进行设置管理
        /// </summary>
        /// <param name="service"></param>
        /// <param name="bovm"></param>
        /// <returns></returns>
        public static async Task SetDemoEntityItemVMCollection(this IViewModelService<DemoEntity, DemoEntityVM> service, DemoEntityVM boVM)
        {
            // 提取订单条目
            var demoEntityItemVMCollection  = await service.GetOtherBoVMCollection<DemoEntityItem, DemoEntityItemVM>(x => x.DemoEntity.Id == boVM.Id, x => x.DemoEntity);
            // 处理关联订单 DemoEntity 的 ID
            foreach (var item in demoEntityItemVMCollection)
            {
                item.DemoEntityId = boVM.Id.ToString();
                item.DemoEntityName = boVM.Name;
            }
            boVM.DemoEntityItemVMCollection = demoEntityItemVMCollection;
        }

    }
}

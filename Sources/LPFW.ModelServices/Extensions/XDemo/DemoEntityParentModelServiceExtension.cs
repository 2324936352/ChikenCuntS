using LPFW.DataAccess;
using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.Demo;
using LPFW.ViewModelServices.Tools;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels.Demo;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LPFW.ViewModelServices.Extensions.Demo
{
    /// <summary>
    /// 针对  <see cref="IViewModelService{DemoEntityParent,DemoEntityParentVM}" /> 的视图模型服务的扩展方法
    /// </summary>
    public static class DemoEntityParentServiceExtension
    {
        /// <summary>
        /// 获取与 DemoEntityParentViewModel 相关的关联属性的值
        /// </summary>
        /// <param name="viewModelService"></param>
        /// <param name="boId"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task GetboVMRelevanceData(this IViewModelService<DemoEntityParent, DemoEntityParentVM> service,DemoEntityParentVM boVM)
        {
            var bo = await service.EntityRepository.GetBoAsyn(boVM.Id, x => x.Parent);

            // 设置上级关系的视图模型数据
            if (bo != null)
            {
                if (bo.Parent != null)
                {
                    boVM.ParentId = bo.Parent.Id.ToString();
                    boVM.ParentName = bo.Parent.Name;
                }
            }

            // 设置供给前端视图模型选择上级元素时的下拉选项集合
            boVM.ParentItemCollection = await SelfReferentialItemFactory<DemoEntityParent>.GetCollectionAsyn(service.EntityRepository, true);
        }

        /// <summary>
        /// 在持久化保存 DemoEntityParent 时，处理存在关联关系的数据，然后执行保存
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task<SaveStatusModel> SaveBoWithRelevanceDataAsyn(this IViewModelService<DemoEntityParent, DemoEntityParentVM> service,
            DemoEntityParentVM boVM)
        {
            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.GetBoAsyn(boVM.Id);
            if (bo == null)
                bo = new DemoEntityParent();
            
            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel<DemoEntityParentVM, DemoEntityParent>(bo);

            // 处理关联的上级节点
            if (String.IsNullOrEmpty(boVM.ParentId))
            {
                bo.Parent = bo;
            }
            else
            {
                var parentId = Guid.Parse(boVM.ParentId);
                bo.Parent = await service.EntityRepository.GetBoAsyn(parentId);
            }
            
            // 执行持久化处理
            var saveStatus = await service.EntityRepository.SaveBoAsyn(bo);

            return saveStatus;
        }
    }
}

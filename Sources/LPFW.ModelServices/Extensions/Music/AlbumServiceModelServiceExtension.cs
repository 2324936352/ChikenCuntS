using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.MusicEntity;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels.MusicViewModel;
using LPFW.ViewModelServices.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LPFW.ViewModelServices.Extensions.Music
{
   public static class AlbumServiceModelServiceExtension
    {
        public static async Task GetboVMRelevanceData(this IViewModelService<Album, AlbumViewModel> service, AlbumViewModel boVM)
        {
            var bo = await service.EntityRepository.GetBoAsyn(boVM.Id);

            // 设置上级关系的视图模型数据
            if (bo != null)
            {
                if (bo.MusicEntity != null)
                {
                    boVM.MusicId = bo.MusicEntity.Id.ToString();
                    boVM.Music = bo.MusicEntity.Name;
                }
            }

            // 设置供给前端视图模型选择上级元素时的下拉选项集合
            boVM.AlbumItemCollection = await SelfReferentialItemFactory<Album>.GetCollectionAsyn(service.EntityRepository, true);
        }

        /// <summary>
        /// 存储具有关联关系的业务实体模型对象
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task<SaveStatusModel> SaveBoWithRelevanceDataAsyn(this IViewModelService<Album, AlbumViewModel> service, AlbumViewModel boVM)
        {
            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.GetBoAsyn(boVM.Id);
            if (bo == null)
                bo = new Album();

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel<AlbumViewModel, Album>(bo);

            // 处理关联的 DemoEntityParent
            if (String.IsNullOrEmpty(boVM.Music))
            {

                bo.MusicEntity = bo;
            }
            else
            {
                var MusicTypeId = Guid.Parse(boVM.MusicId);
                bo.MusicEntity = await service.EntityRepository.GetBoAsyn(MusicTypeId);
            }

            // 执行持久化处理，如果失败，直接返回
            var saveStatus = await service.EntityRepository.SaveBoAsyn(bo);
            if (saveStatus.SaveSatus != true)
                return saveStatus;
            return saveStatus;
        }

    }
}

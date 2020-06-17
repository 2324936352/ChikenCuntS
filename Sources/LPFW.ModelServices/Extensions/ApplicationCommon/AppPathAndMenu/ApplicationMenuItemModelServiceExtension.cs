using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.ApplicationCommon.AppPathAndMenu;
using LPFW.ViewModels.ApplicationCommon.AppPathAndMenu;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModelServices.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPFW.ViewModelServices.Extensions.ApplicationCommon.AppPathAndMenu
{
    public static class ApplicationMenuItemModelServiceExtension
    {
        public static async Task GetboVMRelevanceData(this IViewModelService<ApplicationMenuItem, ApplicationMenuItemVM> service, ApplicationMenuItemVM boVM)
        {
            var boId = boVM.Id;

            var bo = await service.EntityRepository.GetBoAsyn(boId, x => x.ParentItem, y => y.ApplicationMenuGroup);
            if (bo == null)
                bo = new ApplicationMenuItem();

            // 设置上级关系的视图模型数据
            if (bo.ParentItem != null)
            {
                boVM.ParentItemId = bo.ParentItem.Id.ToString();
                boVM.ParentItemName = bo.ParentItem.Name;
            }
            // 设置供给前端视图模型选择上级元素时的下拉选项集合
            var sourceItems01 = await service.EntityRepository.GetOtherBoCollectionAsyn<ApplicationMenuItem>();
            boVM.ParentItemItemCollection = SelfReferentialItemFactory<ApplicationMenuItem>.GetCollection(sourceItems01.OrderBy(x=>x.SortCode).ToList(), true);

            // 设置选择关联的多选一关系
            if (bo.ApplicationMenuGroup != null)
            {
                boVM.ApplicationMenuGroupId = bo.ApplicationMenuGroup.Id.ToString();
                boVM.ApplicationMenuGroupName = bo.ApplicationMenuGroup.Name;
            }
            var sourceItems02 = await service.EntityRepository.GetOtherBoCollectionAsyn<ApplicationMenuGroup>();
            boVM.ApplicationMenuGroupItemCollection = PlainFacadeItemFactory<ApplicationMenuGroup>.Get(sourceItems02.OrderBy(x=>x.SortCode).ToList());

        }

        public static async Task GetboVMRelevanceData(this IViewModelService<ApplicationMenuItem, ApplicationMenuItemVM> service, ApplicationMenuItemVM boVM,string groupId)
        {
            var boId = boVM.Id;

            var bo = await service.EntityRepository.GetBoAsyn(boId, x => x.ParentItem, y => y.ApplicationMenuGroup);
            if (bo == null)
                bo = new ApplicationMenuItem();

            // 设置上级关系的视图模型数据
            if (bo.ParentItem != null)
            {
                boVM.ParentItemId = bo.ParentItem.Id.ToString();
                boVM.ParentItemName = bo.ParentItem.Name;
            }
            // 设置供给前端视图模型选择上级元素时的下拉选项集合，这时候限制其可选的下拉数据，都是归属指定 groupId 的菜单条
            var sourceItems01 = await service.EntityRepository.GetOtherBoCollectionAsyn<ApplicationMenuItem>(x => x.ApplicationMenuGroup.Id == Guid.Parse(groupId), y => y.ApplicationMenuGroup);
            boVM.ParentItemItemCollection = SelfReferentialItemFactory<ApplicationMenuItem>.GetCollection(sourceItems01.OrderBy(x => x.SortCode).ToList(), true);

            // 设置选择关联的菜单分组属性
            var menuGroup = await service.EntityRepository.GetOtherBoAsyn<ApplicationMenuGroup>(Guid.Parse(groupId));
            bo.ApplicationMenuGroup = menuGroup;
            if (bo.ApplicationMenuGroup != null)
            {
                boVM.ApplicationMenuGroupId = bo.ApplicationMenuGroup.Id.ToString();
                boVM.ApplicationMenuGroupName = bo.ApplicationMenuGroup.Name;
            }
            var sourceItems02 = await service.EntityRepository.GetOtherBoCollectionAsyn<ApplicationMenuGroup>();
            boVM.ApplicationMenuGroupItemCollection = PlainFacadeItemFactory<ApplicationMenuGroup>.Get(sourceItems02.OrderBy(x => x.SortCode).ToList());

        }

        public static async Task<SaveStatusModel> SaveBoWithRelevanceDataAsyn(this IViewModelService<ApplicationMenuItem, ApplicationMenuItemVM> service, ApplicationMenuItemVM boVM)
        {
            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.GetBoAsyn(boVM.Id);
            if (bo == null)
                bo = new ApplicationMenuItem();

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel<ApplicationMenuItemVM, ApplicationMenuItem>(bo);

            // 处理关联的 ParentItem
            if (!String.IsNullOrEmpty(boVM.ParentItemId))
            {
                var parentId = Guid.Parse(boVM.ParentItemId);
                var parentItem = await service.EntityRepository.GetOtherBoAsyn<ApplicationMenuItem>(parentId);
                bo.ParentItem = parentItem;
            }
            else
                bo.ParentItem = bo;


            // 处理关联的 ApplicationMenuGroup
            if (!String.IsNullOrEmpty(boVM.ApplicationMenuGroupId))
            {
                var id = Guid.Parse(boVM.ApplicationMenuGroupId);
                var item = await service.EntityRepository.GetOtherBoAsyn<ApplicationMenuGroup>(id);
                bo.ApplicationMenuGroup = item;
            }

            // 执行持久化处理
            var saveStatus = await service.EntityRepository.SaveBoAsyn(bo);

            return saveStatus;
        }

        public static async Task<List<TreeNodeForBootStrapTreeView>> GetTreeViewNodeByApplicationMenuGroup(this IViewModelService<ApplicationMenuItem, ApplicationMenuItemVM> service)
        {
            var menuGroupCollection = await service.EntityRepository.GetOtherBoCollectionAsyn<ApplicationMenuGroup>();
            var selfReferentialItemCollection = new List<SelfReferentialItem>();
            foreach (var item in menuGroupCollection.OrderBy(x => x.SortCode))
            {
                var sItem = new SelfReferentialItem()
                {
                    ID = item.Id.ToString(),
                    ParentID = item.Id.ToString(),
                    DisplayName = item.Name,
                    SortCode = item.SortCode
                };
                selfReferentialItemCollection.Add(sItem);
            }
            // 构建树节点集合
            var result = TreeViewFactoryForBootSrapTreeView.GetTreeNodes(selfReferentialItemCollection);

            return result;
        }

    }
}

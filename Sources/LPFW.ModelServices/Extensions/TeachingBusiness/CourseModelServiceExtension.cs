using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.EntitiyModels.TeachingBusiness;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels.TeachingBusiness;
using LPFW.ViewModelServices.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LPFW.ViewModelServices.Extensions.TeachingBusiness
{
    public static class CourseModelServiceExtension
    {
        #region 课程数据维护部份
        public static async Task<List<CourseVM>> GetCourseVMCollection(this IViewModelService<Course, CourseVM> service, ListPageParameter listPageParameter, Expression<Func<Course, bool>> navigatorPredicate, params Expression<Func<Course, object>>[] includeProperties)
        {
            var boCollection = await service.EntityRepository.GetBoPaginateAsyn(listPageParameter, navigatorPredicate, includeProperties);
            var boVMCollection = new List<CourseVM>();

            // 初始化视图模型起始序号
            int count = (int.Parse(listPageParameter.PageIndex) - 1) * int.Parse(listPageParameter.PageSize);
            foreach (var bo in boCollection)
            {
                var boVM = new CourseVM();
                bo.MapToViewModel<Course, CourseVM>(boVM);
                boVM.OrderNumber = (++count).ToString();
                if (bo.CourseContainer != null)
                {
                    boVM.CourseContainerId = bo.CourseContainer.Id.ToString();
                    boVM.CourseContainerName = bo.CourseContainer.Name;
                }

                if (bo.Creator != null)
                {
                    boVM.CreatorId = bo.Creator.Id;
                    boVM.CreatorName = bo.Creator.Name;
                }

                if (bo.CourseAdministrator != null)
                {
                    boVM.CourseAdministratorId = bo.CourseAdministrator.Id;
                    boVM.CourseAdministratorName = bo.CourseAdministrator.Name;
                }

                boVMCollection.Add(boVM);
            }
            return boVMCollection;
        }

        public static async Task GetboVMRelevanceData(this IViewModelService<Course, CourseVM> service, CourseVM boVM)
        {
            var boId = boVM.Id;

            var bo = await service.EntityRepository.GetBoAsyn(boId, i => i.CourseContainer, i => i.Creator, i => i.CourseAdministrator);
            if (bo == null)
                bo = new Course();

            if (bo.CourseContainer != null)
            {
                boVM.CourseContainerId = bo.CourseContainer.Id.ToString();
                boVM.CourseContainerName = bo.CourseContainer.Name;
            }
            var sourceItems = await service.EntityRepository.GetOtherBoCollectionAsyn<CourseContainer>();
            boVM.CourseContainerItemCollection = PlainFacadeItemFactory<CourseContainer>.Get(sourceItems.OrderBy(x => x.SortCode).ToList());

            if (bo.Creator != null)
            {
                boVM.CreatorId = bo.Creator.Id;
                boVM.CreatorName = bo.Creator.Name;
            }

            if (bo.CourseAdministrator != null)
            {
                boVM.CourseAdministratorId = bo.CourseAdministrator.Id;
                boVM.CourseAdministratorName = bo.CourseAdministrator.Name;
            }
        }

        public static async Task<SaveStatusModel> SaveBoWithRelevanceDataAsyn(this IViewModelService<Course, CourseVM> service, CourseVM boVM)
        {
            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.GetBoAsyn(boVM.Id);
            if (bo == null)
                bo = new Course();

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel<CourseVM, Course>(bo);

            // 处理关联的 ParentItem
            if (!String.IsNullOrEmpty(boVM.CourseContainerId))
            {
                var parentId = Guid.Parse(boVM.CourseContainerId);
                var parentItem = await service.EntityRepository.GetOtherBoAsyn<CourseContainer>(parentId);
                bo.CourseContainer = parentItem;
            }

            if (boVM.CreatorId != Guid.Empty)
            {
                var item = await service.EntityRepository.GetOtherBoAsyn<ApplicationUser>(boVM.CreatorId);
                bo.Creator = item;
            }

            if (boVM.CourseAdministratorId != Guid.Empty)
            {
                var item = await service.EntityRepository.GetOtherBoAsyn<ApplicationUser>(boVM.CourseAdministratorId);
                bo.CourseAdministrator = item;
            }

            // 执行持久化处理
            var saveStatus = await service.EntityRepository.SaveBoAsyn(bo);

            return saveStatus;
        }
        #endregion

        #region 课程单元数据维护部份
        /// <summary>
        /// 根据 Id 返回新建或者编辑的课程单元视图对象
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id">课程单元 Id</param>
        /// <returns></returns>
        public static async Task<CourseItemVM> GetCourseItemVMAsync(this IViewModelService<Course, CourseVM> service, Guid id, Guid courseId, Guid userId)
        {
            var bo = await service.EntityRepository.GetOtherBoAsyn<CourseItem>(id, i => i.Parent, i => i.Course, i => i.Creator);
            if (bo == null)
                bo = new CourseItem();

            var boVM = ConversionForViewModelHelper.GetFromEntity<CourseItem, CourseItemVM>(bo);

            if (bo.Parent != null)
            {
                boVM.ParentId = bo.Parent.Id.ToString();
                boVM.ParentName = bo.Parent.Name;
            }
            var sourceItems = await service.EntityRepository.GetOtherBoCollectionAsyn<CourseItem>(x => x.Course.Id == courseId, i => i.Parent, i => i.Course);
            boVM.ParentItemCollection = SelfReferentialItemFactory<CourseItem>.GetCollection(sourceItems.OrderBy(x => x.SortCode).ToList(), true);

            if (bo.Course != null)
            {
                boVM.CourseId = bo.Course.Id;
                boVM.CourseName = bo.Course.Name;
            }
            else
            {
                var course = await service.EntityRepository.GetBoAsyn(courseId);
                boVM.CourseId = course.Id;
                boVM.CourseName = course.Name;
            }

            if (bo.Creator != null)
            {
                boVM.CreatorId = bo.Creator.Id;
                boVM.CreatorName = bo.Creator.Name;
            }
            else
            {
                var creator = await service.EntityRepository.ApplicationUserManager.FindByIdAsync(userId.ToString());
                boVM.CreatorId = creator.Id;
                boVM.CreatorName = creator.Name;
            }

            return boVM;
        }

        /// <summary>
        /// 设置课程单元的关联属性
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task SetCourseItemVMAsync(this IViewModelService<Course, CourseVM> service, CourseItemVM boVM)
        {
            var boId = boVM.Id;
            var bo = await service.EntityRepository.GetOtherBoAsyn<CourseItem>(boId, i => i.Course, i => i.Parent, i => i.Creator);
            if (bo == null)
                bo = new CourseItem();

            if (bo.Parent != null)
            {
                boVM.ParentId = bo.Parent.Id.ToString();
                boVM.ParentName = bo.Parent.Name;
            }
            var sourceItems = await service.EntityRepository.GetOtherBoCollectionAsyn<CourseItem>(x => x.Course.Id == boVM.CourseId, i => i.Parent, i => i.Course);
            boVM.ParentItemCollection = SelfReferentialItemFactory<CourseItem>.GetCollection(sourceItems.OrderBy(x => x.SortCode).ToList(), true);
        }

        /// <summary>
        /// 保存课程单元
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task<SaveStatusModel> SaveCourseItemVMAsync(this IViewModelService<Course, CourseVM> service, CourseItemVM boVM)
        {
            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.GetOtherBoAsyn<CourseItem>(boVM.Id, i => i.Course, i => i.Parent, i => i.Creator, i => i.CourseItemContent);
            if (bo == null)
                bo = new CourseItem();

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel<CourseItemVM, CourseItem>(bo);

            // 处理关联的 ParentItem
            if (!String.IsNullOrEmpty(boVM.ParentId))
            {
                var parentId = Guid.Parse(boVM.ParentId);
                var parentItem = await service.EntityRepository.GetOtherBoAsyn<CourseItem>(parentId);
                bo.Parent = parentItem;
            }
            else
            {
                bo.Parent = bo;
            }

            if (boVM.CourseId != Guid.Empty)
            {
                var item = await service.EntityRepository.GetBoAsyn(boVM.CourseId);
                bo.Course = item;
            }

            if (boVM.CreatorId != Guid.Empty)
            {
                var item = await service.EntityRepository.GetOtherBoAsyn<ApplicationUser>(boVM.CreatorId);
                bo.Creator = item;
            }

            if (bo.CourseItemContent == null)
            {
                bo.CourseItemContent = new CourseItemContent()
                {
                    Name = bo.Name,
                    SortCode = bo.SortCode,
                    Description = bo.Description
                };
            }

            // 执行持久化处理
            var saveStatus = await service.EntityRepository.SaveOtherBoWithStatusAsyn(bo);

            return saveStatus;
        }

        public static async Task<DeleteStatusModel> DeleteCourseItemAsync(this IViewModelService<Course, CourseVM> service, Guid itemId) 
        {
            var courseItem = await service.EntityRepository.GetOtherBoAsyn<CourseItem>(itemId, i=>i.CourseItemContent);
            var contentId = courseItem.CourseItemContent.Id;

            var deleteStatusModel = await service.EntityRepository.DeleteOtherBoAsyn<CourseItem>(itemId);
            deleteStatusModel= await service.EntityRepository.DeleteOtherBoAsyn<CourseItemContent>(contentId);

            return deleteStatusModel;
        }

        #endregion

        #region 课程单元教学内容数据维护部份
        /// <summary>
        /// 根据课程 Id，提取缺省的课程单元内容视图模型
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<CourseItemContentVM> GetDefaultCourseItemContentVM(this IViewModelService<Course, CourseVM> service, Guid id)
        {
            var result = new CourseItemContentVM();

            var itemCollection = await service.EntityRepository.GetOtherBoCollectionAsyn<CourseItem>(x => x.Course.Id == id, i => i.Course, i => i.CourseItemContent, i => i.Creator, i => i.Course.CourseContainer);
            var item = itemCollection.OrderBy(x => x.SortCode).FirstOrDefault();
            if (item != null)
            {
                result.Id = item.CourseItemContent.Id;
                result.Name = item.CourseItemContent.Name;
                result.SortCode = item.CourseItemContent.SortCode;
                result.Description = item.CourseItemContent.Description;
                result.SecondTitle = item.CourseItemContent.SecondTitle;
                result.HeadContent = item.CourseItemContent.HeadContent;
                result.FootContent = item.CourseItemContent.FootContent;
                result.UpdateDate = item.CourseItemContent.UpdateDate;
                result.BodyContent = item.CourseItemContent.BodyContent;

                result.CourseItemId = item.Id;
                result.CourseItemName = item.Name;

                if (item.Course != null)
                {
                    result.CourseId = item.Course.Id;
                    result.CourseName = item.Course.Name;

                    if (item.Course.CourseContainer != null)
                    {
                        result.CourseContainerId = item.Course.CourseContainer.Id;
                        result.CourseContainerName = item.Course.CourseContainer.Name;
                    }
                }

                if (item.CourseItemContent.Editor != null)
                {
                    result.EditorId = item.CourseItemContent.Editor.Id;
                    result.EditorName = item.CourseItemContent.Editor.Name;
                }
            }
            else
            {
                result.CourseId = id;
                var course = await service.EntityRepository.GetBoAsyn(id, i => i.CourseContainer);
                result.CourseName = course.Name;
                if (course.CourseContainer != null)
                {
                    result.CourseContainerId = course.CourseContainer.Id;
                    result.CourseContainerName = course.CourseContainer.Name;
                }
            }
            return result;
        }

        /// <summary>
        /// 根据课程单元 Id，提取课程单元内容编辑器
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id">课程单元 Id</param>
        /// <returns></returns>
        public static async Task<CourseItemContentVM> GetCourseItemContentVM(this IViewModelService<Course, CourseVM> service, Guid id)
        {
            var result = new CourseItemContentVM();

            var item = await service.EntityRepository.GetOtherBoAsyn<CourseItem>(id, i => i.Course, i => i.CourseItemContent, i => i.Creator, i => i.Course.CourseContainer);
            if (item != null)
            {
                result.Id = item.CourseItemContent.Id;
                result.Name = item.CourseItemContent.Name;
                result.SortCode = item.CourseItemContent.SortCode;
                result.Description = item.CourseItemContent.Description;
                result.SecondTitle = item.CourseItemContent.SecondTitle;
                result.HeadContent = item.CourseItemContent.HeadContent;
                result.FootContent = item.CourseItemContent.FootContent;
                result.UpdateDate = item.CourseItemContent.UpdateDate;
                result.BodyContent = item.CourseItemContent.BodyContent;

                result.CourseItemId = item.Id;
                result.CourseItemName = item.Name;

                if (item.Course != null)
                {
                    result.CourseId = item.Course.Id;
                    result.CourseName = item.Course.Name;

                    if (item.Course.CourseContainer != null)
                    {
                        result.CourseContainerId = item.Course.CourseContainer.Id;
                        result.CourseContainerName = item.Course.CourseContainer.Name;
                    }
                }

                if (item.CourseItemContent.Editor != null)
                {
                    result.EditorId = item.CourseItemContent.Editor.Id;
                    result.EditorName = item.CourseItemContent.Editor.Name;
                }
            }
            else
            {
                result.CourseId = id;
                var course = await service.GetBoVMAsyn(id);
                result.CourseName = course.Name;
            }
            return result;
        }

        public static async Task<SaveStatusModel> SaveCourseItemContentVM(this IViewModelService<Course, CourseVM> service, CourseItemContentVM boVM)
        {
            var bo = await service.EntityRepository.GetOtherBoAsyn<CourseItemContent>(boVM.Id);
            if (bo == null)
                bo = new CourseItemContent();

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel<CourseItemContentVM, CourseItemContent>(bo);

            var saveStatus = await service.EntityRepository.SaveOtherBoWithStatusAsyn(bo);
            return saveStatus;

        }
        #endregion

        #region 视图导航树节点数据处理部分
        public static async Task<List<TreeNodeForBootStrapTreeView>> GetTreeViewNodeByCourseContainer(this IViewModelService<Course, CourseVM> service)
        {
            var menuGroupCollection = await service.EntityRepository.GetOtherBoCollectionAsyn<CourseContainer>();
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

        public static async Task<List<TreeNodeForBootStrapTreeView>> GetTreeViewNodeForCourseContentItem(this IViewModelService<Course, CourseVM> service, Guid id)
        {
            var sourceItems = await service.EntityRepository.GetOtherBoCollectionAsyn<CourseItem>(x => x.Course.Id == id, i => i.Parent, i => i.Course);
            var selfReferentialItemCollection = SelfReferentialItemFactory<CourseItem>.GetCollection(sourceItems.OrderBy(x => x.SortCode).ToList(), false);

            // 构建树节点集合
            var result = TreeViewFactoryForBootSrapTreeView.GetTreeNodes(selfReferentialItemCollection);

            return result;

        } 
        #endregion
    }
}

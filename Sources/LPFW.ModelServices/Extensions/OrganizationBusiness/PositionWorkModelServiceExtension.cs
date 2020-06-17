using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.OrganzationBusiness;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels.OrganizationBusiness;
using LPFW.ViewModelServices.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPFW.ViewModelServices.Extensions.OrganizationBusiness
{
    /// <summary>
    /// 针对  <see cref="IViewModelService{PositionWork, PositionWorkVM}" /> 的视图模型服务的扩展方法
    /// </summary>
    public static class PositionWorkModelServiceExtension
    {
        /// <summary>
        /// 根据部门的 Id 返回全部岗位之下的全部作业数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public static async Task<List<PositionWorkListVM>> GetPositionWorkListVMAsync(this IViewModelService<PositionWork, PositionWorkVM> service, Guid deptId) 
        {
            var result = new List<PositionWorkListVM>();
            
            var positionCollection = await service.EntityRepository.GetOtherBoCollectionAsyn<Position>(x => x.Department.Id == deptId, y => y.Department);
            var countX = 0;
            foreach (var item in positionCollection.OrderBy(x=>x.SortCode)) 
            {
                var positionWorkListVM = new PositionWorkListVM()
                {
                    Id = item.Id,
                    Name = item.Name,
                    SortCode = item.SortCode, 
                    OrderNumber = (++countX).ToString(), 
                    PositionWorkVMCollection = new List<PositionWorkVM>()
                };

                var workCollection = await service.EntityRepository.GetBoCollectionAsyn(x => x.Position.Id == item.Id, y => y.Position);
                var countY = 0;
                foreach (var wItem in workCollection.OrderBy(x=>x.SortCode)) 
                {
                    var positionWorkVM = ConversionForViewModelHelper.GetFromEntity<PositionWork, PositionWorkVM>(wItem);
                    positionWorkVM.OrderNumber = positionWorkListVM.OrderNumber + "." + (++countY).ToString();
                    positionWorkListVM.PositionWorkVMCollection.Add(positionWorkVM);
                }
                result.Add(positionWorkListVM);
            }
            return result;
        }

        /// <summary>
        /// 为简单创建的岗位视图模型对象配置关联属性，以及在编辑员工信息时提供相关的选项数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task GetboVMRelevanceData(this IViewModelService<PositionWork, PositionWorkVM> service, PositionWorkVM boVM, Guid deptId)
        {
            var boId = boVM.Id;

            var bo = await service.EntityRepository.GetBoAsyn(boId, y => y.Position);
            if (bo == null)
                bo = new PositionWork();

            // 设置当前对象的上级部门的视图模型数据
            if (bo.Position != null)
            {
                boVM.PositionId   = bo.Position.Id.ToString();
                boVM.PositionName = bo.Position.Name;
            }
            boVM.DepartemntId = deptId;
            // 设置供给前端视图模型选择上级元素时的下拉选项集合
            var sourceItems = await service.EntityRepository.GetOtherBoCollectionAsyn<Position>(x => x.Department.Id == deptId, y => y.Department);
            boVM.PositionItemCollection = PlainFacadeItemFactory<Position>.Get(sourceItems.OrderBy(x => x.SortCode).ToList()); ;
        }

        /// <summary>
        /// 保存岗位作业视图对象数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task<SaveStatusModel> SaveBoWithRelevanceDataAsyn(this IViewModelService<PositionWork, PositionWorkVM> service, PositionWorkVM boVM)
        {
            var saveStatus = new SaveStatusModel() { SaveSatus = true, Message = "" };
            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.GetBoAsyn(boVM.Id);
            if (bo == null)
                bo = new PositionWork();

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel(bo);

            // 处理关联的岗位
            if (!String.IsNullOrEmpty(boVM.PositionId))
            {
                var parentId = Guid.Parse(boVM.PositionId);
                var parentItem = await service.EntityRepository.GetOtherBoAsyn<Position>(parentId);
                bo.Position = parentItem;
            }

            // 执行持久化处理
            saveStatus = await service.EntityRepository.SaveBoAsyn(bo);
            return saveStatus;
        }
    }
}

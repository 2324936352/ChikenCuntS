using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.OrganzationBusiness;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels.OrganizationBusiness;
using LPFW.ViewModelServices.Tools;

namespace LPFW.ViewModelServices.Extensions.OrganizationBusiness
{
    /// <summary>
    /// 针对  <see cref="IViewModelService{PositionWorkKPI, PositionWorkKPIVM}" /> 的视图模型服务的扩展方法
    /// </summary>
    public static class PositionWorkKpiModelServiceExtension
    {
        /// <summary>
        /// 根据部门的 Id 返回全部岗位之下的全部作业绩效指标数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public static async Task<List<PositionWorkKpiListVM>> GetPositionWorkKpiListVMAsync(this IViewModelService<PositionWorkKPI,PositionWorkKPIVM> service, Guid deptId)
        {
            var result = new List<PositionWorkKpiListVM>();

            var positionCollection = await service.EntityRepository.GetOtherBoCollectionAsyn<Position>(x => x.Department.Id == deptId, y => y.Department);
            var countX = 0;
            foreach (var item in positionCollection.OrderBy(x => x.SortCode))
            {
                var positionWorkKpiListVM = new PositionWorkKpiListVM()
                {
                    Id = item.Id,
                    Name = item.Name,
                    SortCode = item.SortCode,
                    OrderNumber = (++countX).ToString(),
                    PositionWorkKPIVMCollection = new List<PositionWorkKPIVM>()
                };

                var workKpiCollection = await service.EntityRepository.GetBoCollectionAsyn(x => x.Position.Id == item.Id, y => y.Position);
                var countY = 0;
                foreach (var wItem in workKpiCollection.OrderBy(x => x.SortCode))
                {
                    var positionWorkKpiVM = ConversionForViewModelHelper.GetFromEntity<PositionWorkKPI, PositionWorkKPIVM>(wItem);
                    positionWorkKpiVM.OrderNumber = positionWorkKpiListVM.OrderNumber + "." + (++countY).ToString();
                    positionWorkKpiListVM.PositionWorkKPIVMCollection.Add(positionWorkKpiVM);
                }
                result.Add(positionWorkKpiListVM);
            }
            return result;
        }

        /// <summary>
        /// 为简单创建的岗位绩效指标视图模型对象配置关联属性
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task GetboVMRelevanceData(this IViewModelService<PositionWorkKPI, PositionWorkKPIVM> service, PositionWorkKPIVM boVM, Guid deptId)
        {
            var boId = boVM.Id;

            var bo = await service.EntityRepository.GetBoAsyn(boId, y => y.Position);
            if (bo == null)
                bo = new PositionWorkKPI();

            // 设置当前对象的上级部门的视图模型数据
            if (bo.Position != null)
            {
                boVM.PositionId = bo.Position.Id.ToString();
                boVM.PositionName = bo.Position.Name;
            }

            // 设置供给前端视图模型选择上级元素时的下拉选项集合
            var sourceItems = await service.EntityRepository.GetOtherBoCollectionAsyn<Position>(x => x.Department.Id == deptId, y => y.Department);
            boVM.PositionItemCollection = PlainFacadeItemFactory<Position>.Get(sourceItems.OrderBy(x => x.SortCode).ToList());

            // 设置当前对象的指标类型的视图模型数据
            if (bo.KPIType != null)
            {
                boVM.OrganizationKPITypeId = bo.KPIType.Id.ToString();
                boVM.OrganizationKPITypeName = bo.KPIType.Name;
            }
            // 设置供给前端视图模型选择上级元素时的下拉选项集合
            var sourceItems01 = await service.EntityRepository.GetOtherBoCollectionAsyn<OrganizationKPIType>();
            boVM.OrganizationKPITypeItemCollection = PlainFacadeItemFactory<OrganizationKPIType>.Get(sourceItems01.OrderBy(x => x.SortCode).ToList());

            boVM.DepartemntId = deptId;
        }

        /// <summary>
        /// 保存岗位作业绩效指标视图对象数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task<SaveStatusModel> SaveBoWithRelevanceDataAsyn(this IViewModelService<PositionWorkKPI, PositionWorkKPIVM> service, PositionWorkKPIVM boVM)
        {
            var saveStatus = new SaveStatusModel() { SaveSatus = true, Message = "" };
            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.GetBoAsyn(boVM.Id);
            if (bo == null)
                bo = new PositionWorkKPI();

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel(bo);

            // 处理关联的指标类型
            if (!String.IsNullOrEmpty(boVM.OrganizationKPITypeId))
            {
                var parentId = Guid.Parse(boVM.OrganizationKPITypeId);
                var parentItem = await service.EntityRepository.GetOtherBoAsyn<OrganizationKPIType>(parentId);
                bo.KPIType = parentItem;
            }

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

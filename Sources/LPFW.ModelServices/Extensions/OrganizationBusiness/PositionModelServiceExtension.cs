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
    /// 针对  <see cref="IViewModelService{Position, PositionVM}" /> 的视图模型服务的扩展方法
    /// </summary>
    public static class PositionModelServiceExtension
    {
        /// <summary>
        /// 为简单创建的岗位视图模型对象配置关联属性，以及在编辑员工信息时提供相关的选项数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task GetboVMRelevanceData(this IViewModelService<Position, PositionVM> service, PositionVM boVM)
        {
            var boId = boVM.Id;

            var bo = await service.EntityRepository.GetBoAsyn(boId,y => y.Department);
            if (bo == null)
                bo = new Position();

            // 设置当前对象的上级部门的视图模型数据
            if (bo.Department != null)
            {
                boVM.DepartmentId = bo.Department.Id.ToString();
                boVM.DepartmentName = bo.Department.Name;
            }
            // 设置供给前端视图模型选择上级元素时的下拉选项集合
            var sourceItems01 = await service.EntityRepository.GetOtherBoCollectionAsyn<Department>();
            boVM.DepartmentItemCollection = SelfReferentialItemFactory<Department>.GetCollection(sourceItems01.OrderBy(x => x.SortCode).ToList(), true);
        }

        /// <summary>
        /// 根据岗位关联的部门处理关联数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <param name="groupId">部门分区对象 Id</param>
        /// <returns></returns>
        public static async Task GetboVMRelevanceData(this IViewModelService<Position, PositionVM> service, PositionVM boVM, string typeId)
        {
            var boId = boVM.Id;

            var bo = await service.EntityRepository.GetBoAsyn(boId, x => x.Department);
            if (bo == null)
                bo = new Position();

            // 设置上级关系的视图模型数据
            if (bo.Department != null)
            {
                boVM.DepartmentId = bo.Department.Id.ToString();
                boVM.DepartmentName = bo.Department.Name;
            }
            else
            {
                // 为新建的视图模型对象直接指定归属实体对象
                var deaultType = await service.EntityRepository.GetOtherBoAsyn<Department>(Guid.Parse(typeId));
                boVM.DepartmentId = deaultType.Id.ToString();
                boVM.DepartmentName = deaultType.Name;
            }
            // 设置供给前端视图模型选择上级元素时的下拉选项集合
            var sourceItems01 = await service.EntityRepository.GetOtherBoCollectionAsyn<Department>();
            boVM.DepartmentItemCollection = SelfReferentialItemFactory<Department>.GetCollection(sourceItems01.OrderBy(x => x.SortCode).ToList(), true);
        }

        /// <summary>
        /// 保存岗位视图对象数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task<SaveStatusModel> SaveBoWithRelevanceDataAsyn(this IViewModelService<Position, PositionVM> service, PositionVM boVM)
        {
            var saveStatus = new SaveStatusModel() { SaveSatus = true, Message = "" };
            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.GetBoAsyn(boVM.Id);
            if (bo == null)
                bo = new Position();

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel(bo);

            // 处理关联的上级单位
            if (!String.IsNullOrEmpty(boVM.DepartmentId))
            {
                var parentId = Guid.Parse(boVM.DepartmentId);
                var parentItem = await service.EntityRepository.GetOtherBoAsyn<Department>(parentId);
                bo.Department = parentItem;
            }

            // 执行持久化处理
            saveStatus = await service.EntityRepository.SaveBoAsyn(bo);
            return saveStatus;
        }
    }
}

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
    /// 针对  <see cref="IViewModelService{OrganizationBusinessProcess, OrganizationBusinessProcessVM}" /> 的视图模型服务的扩展方法
    /// </summary>
    public static class OrganizationBusinessProcessModelServiceExtension
    {
        /// <summary>
        /// 根据业务过程归属的单位处理业务过程视图模型的关联数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <param name="organizationId">单位对象 Id</param>
        /// <returns></returns>
        public static async Task GetboVMRelevanceData(this IViewModelService<OrganizationBusinessProcess, OrganizationBusinessProcessVM> service, OrganizationBusinessProcessVM boVM, string organizationId)
        {
            var boId = boVM.Id;

            var bo = await service.EntityRepository.GetBoAsyn(boId, y => y.Organization);
            if (bo == null)
                bo = new OrganizationBusinessProcess();

            // 设置选择关联的部门属性
            var organization = await service.EntityRepository.GetOtherBoAsyn<Organization>(Guid.Parse(organizationId));
            bo.Organization = organization;
            if (bo.Organization != null)
            {
                boVM.OrganizationId = bo.Organization.Id.ToString();
                boVM.OrganizationName = bo.Organization.Name;
            }
            var sourceItems = await service.EntityRepository.GetOtherBoCollectionAsyn<Organization>();
            boVM.OrganizationItemCollection = PlainFacadeItemFactory<Organization>.Get(sourceItems.OrderBy(x => x.SortCode).ToList());

        }

        /// <summary>
        /// 保存过程定义模型数据，并检查头节点情况，没有的话直接创建
        /// </summary>
        /// <param name="service"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static async Task<SaveStatusModel> SaveBoWithRelevanceDataAsyn(this IViewModelService<OrganizationBusinessProcess, OrganizationBusinessProcessVM> service, OrganizationBusinessProcessVM boVM)
        {
            var saveStatus = new SaveStatusModel() { SaveSatus = true, Message = "" };
            // 根据视图模型的 Id 获取实体对象
            var bo = await service.EntityRepository.GetBoAsyn(boVM.Id);
            if (bo == null)
                bo = new OrganizationBusinessProcess();

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel(bo);

            // 处理关联的归属单位
            if (!String.IsNullOrEmpty(boVM.OrganizationId))
            {
                var id = Guid.Parse(boVM.OrganizationId);
                var item = await service.EntityRepository.GetOtherBoAsyn<Organization>(id);
                bo.Organization = item;
            }

            // 执行持久化处理
            saveStatus = await service.EntityRepository.SaveBoAsyn(bo);
            if (saveStatus.SaveSatus) 
            {
                // 处理头节点，如果没有，则需要创建
                var header = await service.EntityRepository.GetOtherBoAsyn<OrganizationBusinessProcessActivity>(x => x.OrganizationBusinessProcessId == bo.Id && x.PositionWork == null, y => y.PositionWork);
                if (header == null)
                {
                    header = new OrganizationBusinessProcessActivity()
                    {
                        Name = "开始：" + bo.Name,
                        Description = "", 
                        OrganizationBusinessProcessId = bo.Id
                    };
                    await service.EntityRepository.SaveOtherBoAsyn<OrganizationBusinessProcessActivity>(header);
                }
            }

            return saveStatus;
        }

        /// <summary>
        /// 根据业务过程 Id，获取业务过程集合（单链表）
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<List<OrganizationBusinessProcessActivityVM>> GetActivityCollection(this IViewModelService<OrganizationBusinessProcess, OrganizationBusinessProcessVM> service, Guid id) 
        {
            var result = new List<OrganizationBusinessProcessActivityVM>();
            var activityCollection = await service.EntityRepository.GetOtherBoCollectionAsyn<OrganizationBusinessProcessActivity>(x=>x.OrganizationBusinessProcessId==id);
            
            // 提取链表头元素
            var header = await service.EntityRepository.GetOtherBoAsyn<OrganizationBusinessProcessActivity>(x => x.OrganizationBusinessProcessId == id && x.PreviousNodeId==Guid.Empty);
            var headerVM = new OrganizationBusinessProcessActivityVM(header);

            _GetOrganizationBusinessProcessActivityLinkTable(activityCollection.ToList(), result, headerVM);

            return result;
        }

        /// <summary>
        /// 创建附加或者向下插入的节点视图模型
        /// </summary>
        /// <param name="service"></param>
        /// <param name="sourceNodeId"></param>
        /// <returns></returns>
        public static async Task<OrganizationBusinessProcessActivityVM> GetAppendOrDownInsertNode(this IViewModelService<OrganizationBusinessProcess, OrganizationBusinessProcessVM> service, Guid sourceNodeId) 
        {
            // 提取当前节点
            var sourceNode = await service.EntityRepository.GetOtherBoAsyn<OrganizationBusinessProcessActivity>(sourceNodeId);

            // 新建节点，并将前置节点指向 sourceNode 节点
            var result = new OrganizationBusinessProcessActivityVM();
            result.PreviousNodeId = sourceNode.Id;
            result.OrganizationBusinessProcessId = sourceNode.OrganizationBusinessProcessId;

            // 配置待选的岗位作业

            return result;
        }

        /// <summary>
        /// 保存附加或者向下插入的节点视图模型
        /// </summary>
        /// <param name="service"></param>
        /// <param name="activityVM"></param>
        /// <returns></returns>
        public static async Task<SaveStatusModel> AppendOrDownInsertNodeSave(
            this IViewModelService<OrganizationBusinessProcess, OrganizationBusinessProcessVM> service,
            OrganizationBusinessProcessActivityVM activityVM)
        {
            var saveStatus = new SaveStatusModel() { SaveSatus = true, Message = "" };

            // 提取待保存的节点的前置节点
            var previousNode = await service.EntityRepository.GetOtherBoAsyn<OrganizationBusinessProcessActivity>(activityVM.PreviousNodeId);

            // 检查前置节点原来的后置节点情况
            if (previousNode.NextNodeId == Guid.Empty)
            {
                // 没有后置节点，则直接将 activityVM 作为尾节点附加
                var activity = new OrganizationBusinessProcessActivity()
                {
                    Id = activityVM.Id,
                    OrganizationBusinessProcessId = activityVM.OrganizationBusinessProcessId,
                    PreviousNodeId = previousNode.Id,
                    Name = activityVM.Name,
                    Description = activityVM.Description,
                    //PositionWork                = work
                };
                // 将前置节点的后置节点指向当前节点
                previousNode.NextNodeId = activity.Id;

                // 持久化
                await service.EntityRepository.SaveOtherBoAsyn<OrganizationBusinessProcessActivity>(previousNode);
                await service.EntityRepository.SaveOtherBoAsyn<OrganizationBusinessProcessActivity>(activity);
            }
            else 
            {
                // 提取前置节点的原后置节点
                var nextNode = await service.EntityRepository.GetOtherBoAsyn<OrganizationBusinessProcessActivity>(previousNode.NextNodeId);
                // 处理当前节点
                var activity = new OrganizationBusinessProcessActivity()
                {
                    Id = activityVM.Id,
                    OrganizationBusinessProcessId = activityVM.OrganizationBusinessProcessId,
                    PreviousNodeId = previousNode.Id, 
                    NextNodeId= nextNode.Id,
                    Name = activityVM.Name,
                    Description = activityVM.Description,
                    //PositionWork                = work
                };

                // 将前置节点的后置节点指向当前节点
                previousNode.NextNodeId = activity.Id;
                nextNode.PreviousNodeId = activity.Id;
                // 持久化
                await service.EntityRepository.SaveOtherBoAsyn<OrganizationBusinessProcessActivity>(previousNode);
                await service.EntityRepository.SaveOtherBoAsyn<OrganizationBusinessProcessActivity>(activity);
                await service.EntityRepository.SaveOtherBoAsyn<OrganizationBusinessProcessActivity>(nextNode);

            }
            return saveStatus;
        }


        /// <summary>
        /// 创建向上插入节点的视图模型
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <param name="workId"></param>
        /// <returns></returns>
        public static async Task<OrganizationBusinessProcessActivityVM> GetUpInsertNode(this IViewModelService<OrganizationBusinessProcess, OrganizationBusinessProcessVM> service, Guid sourceNodeId) 
        {
            // 提取当前节点
            var sourceNode = await service.EntityRepository.GetOtherBoAsyn<OrganizationBusinessProcessActivity>(sourceNodeId);

            // 新建节点，并将前置节点指向 sourceNode 前置节点，后置节点指向 sourceNode
            var result = new OrganizationBusinessProcessActivityVM();
            result.PreviousNodeId = sourceNode.PreviousNodeId;
            result.NextNodeId = sourceNode.Id;
            result.OrganizationBusinessProcessId = sourceNode.OrganizationBusinessProcessId;

            // 配置待选的岗位作业

            return result;
        }

        /// <summary>
        /// 保存向上插入的节点
        /// </summary>
        /// <param name="service"></param>
        /// <param name="activityVM"></param>
        /// <returns></returns>
        public static async Task<SaveStatusModel> UpInsertNodeSave(
            this IViewModelService<OrganizationBusinessProcess, OrganizationBusinessProcessVM> service,
            OrganizationBusinessProcessActivityVM activityVM)
        {
            var saveStatus = new SaveStatusModel() { SaveSatus = true, Message = "" };

            // 提取待保存的节点的前置节点
            var previousNode = await service.EntityRepository.GetOtherBoAsyn<OrganizationBusinessProcessActivity>(activityVM.PreviousNodeId);
            // 提取前置节点的原后置节点
            var nextNode = await service.EntityRepository.GetOtherBoAsyn<OrganizationBusinessProcessActivity>(activityVM.NextNodeId);
            
            // 处理当前节点
            var activity = new OrganizationBusinessProcessActivity()
            {
                Id = activityVM.Id,
                OrganizationBusinessProcessId = activityVM.OrganizationBusinessProcessId,

                PreviousNodeId = previousNode.Id,
                NextNodeId = nextNode.Id,

                Name = activityVM.Name,
                Description = activityVM.Description,
                //PositionWork                = work
            };

            // 将前置节点的后置节点指向当前节点
            previousNode.NextNodeId = activity.Id;
            nextNode.PreviousNodeId = activity.Id;
            // 持久化
            await service.EntityRepository.SaveOtherBoAsyn<OrganizationBusinessProcessActivity>(previousNode);
            await service.EntityRepository.SaveOtherBoAsyn<OrganizationBusinessProcessActivity>(activity);
            await service.EntityRepository.SaveOtherBoAsyn<OrganizationBusinessProcessActivity>(nextNode);

            return saveStatus;
        }

        /// <summary>
        /// 移动节点
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <param name="up">是否向上</param>
        /// <returns></returns>
        public static async Task<SaveStatusModel> MoveActivity(this IViewModelService<OrganizationBusinessProcess, OrganizationBusinessProcessVM> service, Guid id, bool up) 
        {
            var saveStatus = new SaveStatusModel() { SaveSatus = true, Message = "" };
            // 提取当前节点
            var currentNode = await service.EntityRepository.GetOtherBoAsyn<OrganizationBusinessProcessActivity>(id);
            // 提取待保存的节点的前置节点
            var previousNode = await service.EntityRepository.GetOtherBoAsyn<OrganizationBusinessProcessActivity>(currentNode.PreviousNodeId);
            // 提取前置节点的原后置节点
            var nextNode = await service.EntityRepository.GetOtherBoAsyn<OrganizationBusinessProcessActivity>(currentNode.NextNodeId);

            if (up)
            {
                // 是否是根节点
                if (previousNode.PreviousNodeId != Guid.Empty) 
                {
                    var tempId = previousNode.PreviousNodeId;
                    var previousPreviousNode= await service.EntityRepository.GetOtherBoAsyn<OrganizationBusinessProcessActivity>(tempId);
                    previousPreviousNode.NextNodeId = currentNode.Id;

                    currentNode.PreviousNodeId = previousPreviousNode.Id;
                    currentNode.NextNodeId = previousNode.Id;

                    previousNode.PreviousNodeId = currentNode.Id;
                    previousNode.NextNodeId = nextNode.Id;

                    nextNode.PreviousNodeId = previousNode.Id;
                    
                    // 持久化
                    await service.EntityRepository.SaveOtherBoAsyn<OrganizationBusinessProcessActivity>(previousPreviousNode);
                    await service.EntityRepository.SaveOtherBoAsyn<OrganizationBusinessProcessActivity>(previousNode);
                    await service.EntityRepository.SaveOtherBoAsyn<OrganizationBusinessProcessActivity>(currentNode);
                    await service.EntityRepository.SaveOtherBoAsyn<OrganizationBusinessProcessActivity>(nextNode);
                }
            }
            else 
            {
                // 是否是尾节点
                if (nextNode != null)
                {
                    var tempId = nextNode.NextNodeId;
                    var nextNextNode = await service.EntityRepository.GetOtherBoAsyn<OrganizationBusinessProcessActivity>(tempId);
                    
                    nextNode.NextNodeId = currentNode.Id;
                    nextNode.PreviousNodeId = previousNode.Id;

                    currentNode.PreviousNodeId = nextNode.Id;
                    currentNode.NextNodeId = nextNextNode.Id;

                    nextNextNode.PreviousNodeId = currentNode.Id;
                    previousNode.NextNodeId = nextNode.Id;

                    // 持久化
                    await service.EntityRepository.SaveOtherBoAsyn<OrganizationBusinessProcessActivity>(nextNextNode);
                    await service.EntityRepository.SaveOtherBoAsyn<OrganizationBusinessProcessActivity>(previousNode);
                    await service.EntityRepository.SaveOtherBoAsyn<OrganizationBusinessProcessActivity>(currentNode);
                    await service.EntityRepository.SaveOtherBoAsyn<OrganizationBusinessProcessActivity>(nextNode);
                }
            }
            return saveStatus;
        }

        public static async Task<SaveStatusModel> DeleteActivity(this IViewModelService<OrganizationBusinessProcess, OrganizationBusinessProcessVM> service, Guid id) 
        {
            var saveStatus = new SaveStatusModel() { SaveSatus = true, Message = "" };
            // 提取当前节点
            var currentNode = await service.EntityRepository.GetOtherBoAsyn<OrganizationBusinessProcessActivity>(id);
            // 提取待保存的节点的前置节点
            var previousNode = await service.EntityRepository.GetOtherBoAsyn<OrganizationBusinessProcessActivity>(currentNode.PreviousNodeId);
            // 提取前置节点的原后置节点
            var nextNode = await service.EntityRepository.GetOtherBoAsyn<OrganizationBusinessProcessActivity>(currentNode.NextNodeId);

            if (nextNode != null)
            {
                previousNode.NextNodeId = nextNode.Id;
                nextNode.PreviousNodeId = previousNode.PreviousNodeId;
                await service.EntityRepository.DeleteOtherBoAsyn<OrganizationBusinessProcessActivity>(currentNode.Id);
                await service.EntityRepository.SaveOtherBoAsyn(previousNode);
                await service.EntityRepository.SaveOtherBoAsyn(nextNode);
            }
            else 
            {
                previousNode.NextNodeId = Guid.Empty;
                await service.EntityRepository.DeleteOtherBoAsyn<OrganizationBusinessProcessActivity>(currentNode.Id);
                await service.EntityRepository.SaveOtherBoAsyn(previousNode);
            }

            return saveStatus;
        }

        /// <summary>
        /// 提取 Organization 的所有数据作为前端导航树的数据节点
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static async Task<List<TreeNodeForBootStrapTreeView>> GetTreeViewNodeByOrganization(this IViewModelService<OrganizationBusinessProcess, OrganizationBusinessProcessVM> service, Guid id)
        {
            var itemCollection = await service.EntityRepository.GetOtherBoCollectionAsyn<Organization>(x => x.TransactionCenterRegister.Id == id, t => t.TransactionCenterRegister);
            var selfReferentialItemCollection = new List<SelfReferentialItem>();
            foreach (var item in itemCollection.OrderBy(x => x.SortCode))
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

        /// <summary>
        /// 获取指定 id 的
        /// </summary>
        /// <param name="service"></param>
        /// <param name="sourceNodeId"></param>
        /// <returns></returns>
        public static async Task<OrganizationBusinessProcessActivityVM> GetActivityVM(this IViewModelService<OrganizationBusinessProcess, OrganizationBusinessProcessVM> service, Guid sourceNodeId)
        {
            // 提取当前节点
            var sourceNode = await service.EntityRepository.GetOtherBoAsyn<OrganizationBusinessProcessActivity>(sourceNodeId);
            var result = new OrganizationBusinessProcessActivityVM(sourceNode);

            // 配置待选的岗位作业

            return result;
        }

        /// <summary>
        /// 保存附加节点视图模型
        /// </summary>
        /// <param name="service"></param>
        /// <param name="activityVM"></param>
        /// <returns></returns>
        public static async Task<SaveStatusModel> SaveActivityVM(
            this IViewModelService<OrganizationBusinessProcess, OrganizationBusinessProcessVM> service,
            OrganizationBusinessProcessActivityVM activityVM)
        {
            var saveStatus = new SaveStatusModel() { SaveSatus = true, Message = "" };
            // 提取待保存的节点的前置节点
            var activity = await service.EntityRepository.GetOtherBoAsyn<OrganizationBusinessProcessActivity>(activityVM.Id);

            activity.Name = activityVM.Name;
            activity.Description = activityVM.Description;
            activity.SortCode = activityVM.SortCode;

            saveStatus = await service.EntityRepository.SaveOtherBoWithStatusAsyn(activity);
            return saveStatus;
        }



        /// <summary>
        /// 构建节点集合
        /// </summary>
        /// <param name="sourceItems"></param>
        /// <param name="resultItems"></param>
        /// <param name="activityItem"></param>
        private static void _GetOrganizationBusinessProcessActivityLinkTable(
            List<OrganizationBusinessProcessActivity> sourceItems,
            List<OrganizationBusinessProcessActivityVM> resultItems,
            OrganizationBusinessProcessActivityVM activityItem
            ) 
        {
            resultItems.Add(activityItem);
            if (activityItem.NextNodeId != Guid.Empty) 
            {
                var item = sourceItems.FirstOrDefault(x => x.Id == activityItem.NextNodeId);
                var itemVM = new OrganizationBusinessProcessActivityVM(item);
                _GetOrganizationBusinessProcessActivityLinkTable(sourceItems, resultItems, itemVM);
            }
        }
    }
}

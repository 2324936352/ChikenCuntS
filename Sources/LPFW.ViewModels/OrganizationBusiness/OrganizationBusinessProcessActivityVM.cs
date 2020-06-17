using LPFW.EntitiyModels.OrganzationBusiness;
using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.OrganizationBusiness
{
    /// <summary>
    /// 单位业务过程节点视图模型
    /// </summary>
    public class OrganizationBusinessProcessActivityVM : EntityViewModel
    {
        public Guid OrganizationBusinessProcessId { get; set; }  // 关联过程 Id
        public Guid PreviousNodeId { get; set; }                 // 前置节点 Id
        public Guid NextNodeId { get; set; }                     // 后置节点 Id，后置节点为 null 时，是尾节点

        [Display(Name = "岗位作业")]
        public Guid PositionWorkId { get; set; }                 // 岗位工作 Id
        [Display(Name = "岗位作业")]
        public string PositionWorkName { get; set; }
        public List<PlainFacadeItem> PositionWorkItemCollection { get; set; }

        public OrganizationBusinessProcessActivityVM() 
        {
            this.Id = Guid.NewGuid();
        }

        public OrganizationBusinessProcessActivityVM(OrganizationBusinessProcessActivity bo)
        {
            this.Id = bo.Id;
            this.Name = bo.Name;
            this.Description = bo.Description;
            this.SortCode = bo.SortCode;

            this.OrganizationBusinessProcessId = bo.OrganizationBusinessProcessId;
            this.PreviousNodeId = bo.PreviousNodeId;
            this.NextNodeId = bo.NextNodeId;

            if (bo.PositionWork != null) 
            {
                this.PositionWorkId = bo.PositionWork.Id;
                this.PositionWorkName = bo.PositionWork.Name;
            }
        }

    }
}

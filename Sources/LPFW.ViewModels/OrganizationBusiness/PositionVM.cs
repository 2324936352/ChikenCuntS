using LPFW.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.OrganizationBusiness
{
    public class PositionVM : EntityViewModel
    {
        [Display(Name = "归属部门")]
        public string DepartmentId { get; set; }
        [Display(Name = "归属部门")]
        public string DepartmentName { get; set; }
        public List<SelfReferentialItem> DepartmentItemCollection { get; set; }

        public List<PositionWorkKPIVM> PositionWorkKpiVMCollection { get; set; }
    }
}

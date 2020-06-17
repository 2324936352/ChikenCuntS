using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.MusicViewModel
{
   public  class CreateRoleViewModel
    {

        [Required]
        [Display(Name = "角色")]
        public string RoleName { get; set; }
    }
}

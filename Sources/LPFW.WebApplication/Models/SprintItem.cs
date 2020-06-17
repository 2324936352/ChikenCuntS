using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPFW.WebApplication.Models
{
    public class SprintItem
    {
        public int Counter { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string FinishedDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.MusicEntity
{
    /// <summary>
    /// 权限
    /// </summary>
    public class Power
    {
        public int Id { get; set; }
        public string Admin { get; set; } //管理员
        public string User { get; set; }//用户

       
    }
}

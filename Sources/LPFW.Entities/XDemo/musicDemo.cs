using LPFW.EntitiyModels.Demo;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.EntitiyModels.XDemo
{
    public class MusicDemo:Entity
    {

        public string UndertakerName { get; set; }//任务承担人名称
        public DateTime EndTime { get; set; }//完成结束时间
        public bool IsFinished { get; set; }//完成状态

        public MusicDemo()
        {
            this.Id = Guid.NewGuid();
        }
    }
}

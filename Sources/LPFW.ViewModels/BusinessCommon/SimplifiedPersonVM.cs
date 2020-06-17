using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.ViewModels.BusinessCommon
{
    /// <summary>
    /// 简化的人员数据视图模型，主要用于类似名片样式呈现人员信息的处理
    /// </summary>
    public class SimplifiedPersonVM
    {
        public string Id { get; set; }   
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Summary { get; set; }         // 简要描述
        public string AvatarPath { get; set; }      // 人员头像路径
        public string JobTitle { get; set; }        // 工作头衔
        public string Address { get; set; }         // 联系地址
        public Guid RelevanceObjectId { get; set; } // 关联对象 Id
    }
}

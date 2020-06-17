using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.EntitiyModels.BusinessCommon
{
    /// <summary>
    /// 地址的抽象类，可作为系统中所有的人员的定义的基类，在具体的使用中：
    ///   Name 属性直接赋值国家，SortCode 属性用于存储邮编。
    ///   Description 在存储是，将全部的地址信息合并成详细地址
    ///   具体在前端进行赋值的时候，国家、省份、城市、区县要实现采用级联下拉的方式进行选择配置
    ///   简单处理时，也可以直接输入，可以根据情况自定使用
    /// </summary>
    public abstract class AddressBase : Entity
    {
        [StringLength(20)]
        public string ProvinceName { get; set; } // 省份名称
        [StringLength(20)]
        public string CityName { get; set; }     // 城市名称
        [StringLength(20)]
        public string CountyName { get; set; }   // 区县名称
        [StringLength(200)]
        public string DetailName { get; set; }   // 具体的路、街道 ...

        public AddressBase()
        {
            this.Id = Guid.NewGuid();
        }
    }
}

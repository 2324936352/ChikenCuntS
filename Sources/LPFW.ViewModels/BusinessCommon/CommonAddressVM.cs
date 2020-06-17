using LPFW.EntitiyModels.BusinessCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPFW.ViewModels.BusinessCommon
{
    /// <summary>
    /// 地址基类视图模型，在实际使用中，派生的实体模型很少增加这个基类的实行和关系，
    ///   所以可以考虑在相关的视图模型中直接组合这个。
    /// </summary>
    public class CommonAddressVM
    {
        public Guid Id { get; set; }

        [Display(Name = "国家")]
        [StringLength(20, ErrorMessage = "你输入的数据超出限制20个字符的长度。")]
        public string Name { get; set; }

        [Display(Name = "完整地址")]
        [StringLength(1000, ErrorMessage = "你输入的数据超出限制1000个字符的长度。")]
        public string Description { get; set; }

        [Display(Name = "邮政编码")]
        public string SortCode { get; set; }

        [Display(Name = "省份")]
        [StringLength(20, ErrorMessage = "你输入的数据超出限制20个字符的长度。")]
        public string ProvinceName { get; set; }

        [Display(Name = "城市")]
        [StringLength(20, ErrorMessage = "你输入的数据超出限制20个字符的长度。")]
        public string CityName { get; set; } 

        [Display(Name = "区县")]
        [StringLength(20, ErrorMessage = "你输入的数据超出限制20个字符的长度。")]
        public string CountyName { get; set; }

        [Display(Name = "详细地址")]
        [StringLength(200, ErrorMessage = "你输入的数据超出限制1000个字符的长度。")]
        public string DetailName { get; set; }

        public CommonAddressVM()
        {
            Id = Guid.NewGuid();
            SortCode = "510000";
            Name = "中国";
            ProvinceName = "广西";
            CityName = "";
            CountyName = "";
            DetailName = "";
        }

        public CommonAddressVM(CommonAddress bo)
        {
            Id           = bo.Id;
            SortCode     = bo.SortCode;
            Name         = bo.Name;
            ProvinceName = bo.ProvinceName;
            CityName     = bo.CityName;
            CountyName   = bo.CountyName;
            DetailName   = bo.DetailName;
            Description  = bo.Description;
        }

        public void MapToBo(CommonAddress bo)
        {
            bo.Id           = Id;
            bo.SortCode     = SortCode;     
            bo.Name         = Name;        
            bo.ProvinceName = ProvinceName;
            bo.CityName     = CityName;    
            bo.CountyName   = CountyName;  
            bo.DetailName   = DetailName;
            Description     = bo.Description;
        }

        public void UpdateCommonAddress(CommonAddress bo) 
        {
            if (bo != null)
            {
                this.MapToBo(bo);
                bo.Description = bo.ProvinceName + " " + bo.CityName + " " + bo.CountyName + " " + bo.DetailName;
            }
            else
            {
                bo = new CommonAddress();
                this.MapToBo(bo);
                bo.Description = bo.ProvinceName + " " + bo.CityName + " " + bo.CountyName + " " + bo.DetailName;
            }
        }
    }
}

using LPFW.ViewModels.BusinessCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.HtmlHelper
{
    public static class CommonAddressHelper
    {
        /// <summary>
        /// 水平通用地址选择器,提交form前请先触发id为getValueId按钮的click事件，即先调用getCommonAddressValue()，使用本地址控件的前提是引入area.js <script class="resources library" src="~/js/area.js" type="text/javascript"></script>
        /// 如果是模态对话框使用本选择器，请在渲染模态对话框后调用 _init_area()初始化地址选择器，必须调用
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="getValueId">隐藏按钮的id，用于点击设置个input值方便form提交</param>
        /// <param name="getValueName">隐藏按钮的name</param>
        /// <returns></returns>
        public static HtmlString CommonAddressHorizontalHtml(this IHtmlHelper helper, string getValueId, string getValueName, CommonAddressVM boVM)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append(
               "<div class='card-header'>"+
                   "<div class='card-header-form'>"+                       
                        "<div class='input-group'>" +
                            "<input type = 'text' id = 'Name' name = 'Name' placeholder= '中国' value ='中国'    />" +
                            "<select id = 's_province' name='s_province' value=\"" + boVM.ProvinceName + "\"></select>" +
                            "<select id = 's_city' name='s_city' value=\"" + boVM.CityName + "\"></select>" +
                            "<select id = 's_county' name='s_county' value=\"" + boVM.CountyName + "\"></select>" +
                            "<input type = 'text' id = 'ProvinceName' name = 'ProvinceName' hidden value=\"" + boVM.ProvinceName + "\"/>" +
                            "<input type = 'text' id = 'CityName' name = 'CityName' value=\"" + boVM.CityName + "\" hidden />" +
                            "<input type = 'text' id = 'CountyName' name = 'CountyName' value=\"" + boVM.CountyName + "\" hidden />" +
                            "<input type = 'text' id = 'DetailName' name = 'DetailName' placeholder = '详细地址' value=\"" + boVM.DetailName + "\" style = 'width:200px' />" +
                            "<input type = 'text' id = 'SortCode' name = 'SortCode' placeholder = '邮政编码' value=\"" + boVM.SortCode + "\" />" +
                        "</div>"+
                        "<button id = '"+getValueId+"' name = '"+getValueName+ "' onclick='getCommonAddressValue()' hidden > </ button >" +
                    "</div>" +
        
                    "<div id = 'show' ></ div >"+
                "</ div >"
                );

            return new HtmlString(htmlContent.ToString());
        }


        /// <summary>
        /// 垂直通用地址选择器,提交form前请先触发id为getValueId按钮的click事件,即先调用getCommonAddressValue(),使用本地址控件的前提是引入area.js <script class="resources library" src="~/js/area.js" type="text/javascript"></script>
        ///  如果是模态对话框使用本选择器，请在渲染模态对话框后调用 _init_area()初始化地址选择器，必须调用
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="getValueId">隐藏按钮的id，用于点击设置个input值方便form提交</param>
        /// <param name="getValueName">隐藏按钮的name</param>
        /// <returns></returns>
        public static HtmlString CommonAddressVerticalHtml(this IHtmlHelper helper, string getValueId, string getValueName,CommonAddressVM boVM)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append(
                  
                  
                       
                            "<div class='form-group'>"+
                                "<input type = 'text' id = 'Name' name = 'Name' placeholder= '中国' value ='中国'  class='form-control'  />" +
                            "</div>" +
                             "<div class='form-group'>" +
                                "<select id = 's_province' name='s_province' value=\"" + boVM.ProvinceName + "\" class='form-control'></select>" +
                            "</div>" +
                            "<div class='form-group'>" +
                                "<select id = 's_city' name='s_city'  value=\"" + boVM.CityName + "\" class='form-control'></select>" +
                            "</div>" +
                            "<div class='form-group'>" +
                                "<select id = 's_county' name='s_county' value=\"" + boVM.CountyName + "\" class='form-control' ></select>" +
                            "</div>" +
                            "<div class='form-group'>" +
                                "<input type = 'text' id = 'ProvinceName' name = 'ProvinceName'  class='form-control' value=\"" + boVM.ProvinceName+"\"  hidden />" +
                            "</div>" +
                            "<div class='form-group'>" +
                                "<input type = 'text' id = 'CityName' name = 'CityName' hidden  value=\"" + boVM.CityName + "\" class='form-control' />" +
                            "</div>" +
                             "<div class='form-group'>" +
                                "<input type = 'text' id = 'CountyName' name = 'CountyName' hidden value=\"" + boVM.CountyName + "\" class='form-control' />" +
                            "</div>" +
                             "<div class='form-group'>" +
                                "<input type = 'text' id = 'DetailName' name = 'DetailName' placeholder = '详细地址'  value=\"" + boVM.DetailName + "\" class='form-control' />" +
                            "</div>" +
                             "<div class='form-group'>" +
                                "<input type = 'text' id = 'SortCode' name = 'SortCode' placeholder = '邮政编码' value=\"" + boVM.SortCode + "\" class='form-control' />" +
                            "</div>" +            
                             "<button id = '" + getValueId + "' name = '" + getValueName + "' onclick='getCommonAddressValue()' hidden > </button >" +
                            "<div id = 'show' ></div >"
                );

            return new HtmlString(htmlContent.ToString());
        }
    }
}

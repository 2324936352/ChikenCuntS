using LPFW.ViewModels.ControlModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LPFW.HtmlHelper
{
    public static class BoVMDetailHelper
    {
        #region 标题

        /// <summary>
        /// 明细页标题，包含一个退回到列表的操作按钮
        /// </summary>
        /// <param name="helpe"></param>
        /// <param name="titleName">标题名称</param>
        /// <param name="defualtUrlString">退出按钮的导航路径</param>
        /// <returns></returns>
        public static HtmlString PaginateDetailHeader(this IHtmlHelper helper, string titleName, string defualtUrlString)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append(
                "<div class='card-header'>" +
                "<h4 style = 'font-size:22px;font-weight:normal;color:dimgray'>" + titleName + "</h4>" +
                "<div class='card-header-form'>" +
                "<button class='btn btn-info' onclick='appGotoDefaultPaginatePage(\"" + defualtUrlString + "\",\"mainWorkPlaceArea\")'><i class='fas fa-times'></i> 退出 </button>" +
                "</div>" +
                "</div>");
            return new HtmlString(htmlContent.ToString());
        }

        public static HtmlString PaginateDetailWithModalHeader(this IHtmlHelper helper, string titleName, string defualtUrlString, string modalId)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append(
                "<div class='card-header'>" +
                "<h4 style = 'font-size:22px;font-weight:normal;color:dimgray'>" + titleName + "</h4>" +
                "<div class='card-header-form'>" +
                "<button class='btn btn-info' onclick='appGotoDefaultPaginatePageForModal(\"" + defualtUrlString + "\",\"mainWorkPlaceArea\",\"" + modalId + "\")'><i class='fas fa-times'></i> 退出 </button>" +
                "</div>" +
                "</div>");
            return new HtmlString(htmlContent.ToString());
        }

        public static HtmlString CommonDetaiHeader(this IHtmlHelper helper, string titleName, string defualtUrlString)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append(
                "<div class='card-header'>" +
                "<h4 style = 'font-size:22px;font-weight:normal;color:dimgray'>" + titleName + "</h4>" +
                "<div class='card-header-form'>" +
                "<button class='btn btn-info' onclick='appGotoDefaultCommonPage(\"" + defualtUrlString + "\",\"mainWorkPlaceArea\")'><i class='fas fa-times'></i> 退出 </button>" +
                "</div>" +
                "</div>");
            return new HtmlString(htmlContent.ToString());
        }

        public static HtmlString CommonDetaiWithModalHeader(this IHtmlHelper helper, string titleName, string defualtUrlString, string modalId)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append(
                "<div class='card-header'>" +
                "<h4 style = 'font-size:22px;font-weight:normal;color:dimgray'>" + titleName + "</h4>" +
                "<div class='card-header-form'>" +
                "<button class='btn btn-info' onclick='appGotoDefaultCommonPageForModal(\"" + defualtUrlString + "\",\"mainWorkPlaceArea\",\"" + modalId + "\")'><i class='fas fa-times'></i> 退出 </button>" +
                "</div>" +
                "</div>");
            return new HtmlString(htmlContent.ToString());
        }

        #endregion

        /// <summary>
        /// 用于处理单行输入普通文本数据 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="itemDisplay">显示名</param>
        /// <param name="itemName">属性名</param>
        /// <param name="itemValue">属性值</param>
        /// <param name="placeholderString">输入框提示文字</param>
        /// <param name="modelState">属性校验结果</param>
        /// <param name="modelStateValue">属性校验错误数据</param>
        /// <returns></returns>
        public static HtmlString DetailTextItem(this IHtmlHelper helper, string itemName,object itemValue)
        {
            string itemDisplay = helper.DisplayName(itemName);
            var htmlContent = new StringBuilder();
            htmlContent.Append(
                _GetStartString(itemDisplay) +
                itemValue + 
                 _GetEndString());
            return new HtmlString(htmlContent.ToString());
        }

        /// <summary>
        /// 根据视图模型创建常规的数据处理输入的内容
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static HtmlString GetHtmlStringForDetailItems(this IHtmlHelper helper, List<DetailItem> createOrEditItems, object boVM)
        {
            var htmlContent = new StringBuilder();

            PropertyInfo[] properties = boVM.GetType().GetProperties();
            foreach (var item in createOrEditItems)
            {
                var itemName = item.PropertyName;
                var property = properties.FirstOrDefault(x => x.Name == itemName);
                if (property != null)
                {
                    // 提取属性数据种类
                    var itemKind = item.DataType;

                    // 提取属性类型名称
                    var itemTypeName = property.PropertyType.Name;
                    // 提取属性的值
                    var itemValue = "";
                    if (property.GetValue(boVM) != null)
                    {
                        // 在这里处理类型转换
                        switch (itemKind)
                        {
                            case ViewModelDataType.日期:
                                var date = (DateTime)property.GetValue(boVM);
                                itemValue = date.ToString("yyyy年MM月dd日");
                                break;
                            case ViewModelDataType.日期时间:
                                var time = (DateTime)property.GetValue(boVM);
                                itemValue = time.ToString("yyyy年MM月dd日 hh-mm-ss");
                                break;
                            case ViewModelDataType.性别:
                                if (property.GetValue(boVM).ToString().ToLower() == "true")
                                    itemValue = "女";
                                else
                                    itemValue = "男";
                                break;
                            case ViewModelDataType.是否:
                                if (property.GetValue(boVM).ToString().ToLower() == "true")
                                    itemValue = "是";
                                else
                                    itemValue = "否";
                                break;
                            case ViewModelDataType.图标:
                                itemValue = "<i class=\"" + property.GetValue(boVM).ToString() + "\"></i>";
                                break;
                            default:
                                itemValue = property.GetValue(boVM).ToString();
                                break;
                        }
                    }
                    // 提取属性显示名
                    string itemDisplay = helper.DisplayName(itemName);

                    htmlContent.Append(_GetStartString(itemDisplay) + itemValue + _GetEndString());

                }
            }

            return new HtmlString(htmlContent.ToString());
        }


        /// <summary>
        /// 处理显示名称所需要的 html 字符串
        /// </summary>
        /// <param name="itemDisplay"></param>
        /// <returns></returns>
        private static string _GetStartString(string itemDisplay)
        {
            return
                "<div class='form-group form-row' style='margin-top:-3px'>" +
                "    <div class='col-12 col-md-2 col-sm-2 text-lg-right' style='font-size:14px;font-weight:normal'>" + itemDisplay +"：</div>" +
                "    <div class='col-12 col-md-9 col-sm-9' style='border-bottom-style:solid;border-bottom-color:darkgray;border-bottom-width:1px'>";
        }

        /// <summary>
        /// 处理后端字符串
        /// </summary>
        /// <param name="errorTip"></param>
        /// <returns></returns>
        private static string _GetEndString()
        {
            return
                "    </div>" +
                "</div>";
        }

        /// <summary>
        /// 根据视图模型创建常规的数据处理输入的内容
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static HtmlString GetModalHtmlStringForDetailItems(this IHtmlHelper helper, List<DetailItem> createOrEditItems, object boVM)
        {
            var htmlContent = new StringBuilder();

            PropertyInfo[] properties = boVM.GetType().GetProperties();
            foreach (var item in createOrEditItems)
            {
                var itemName = item.PropertyName;
                var property = properties.FirstOrDefault(x => x.Name == itemName);
                if (property != null)
                {
                    // 提取属性数据种类
                    var itemKind = item.DataType;

                    // 提取属性类型名称
                    var itemTypeName = property.PropertyType.Name;
                    // 提取属性的值
                    var itemValue = "";
                    if (property.GetValue(boVM) != null)
                    {
                        // 在这里处理类型转换
                        switch (itemKind)
                        {
                            case ViewModelDataType.日期:
                                var date = (DateTime)property.GetValue(boVM);
                                itemValue = date.ToString("yyyy年MM月dd日");
                                break;
                            case ViewModelDataType.日期时间:
                                var time = (DateTime)property.GetValue(boVM);
                                itemValue = time.ToString("yyyy年MM月dd日 hh-mm-ss");
                                break;
                            case ViewModelDataType.性别:
                                if (property.GetValue(boVM).ToString().ToLower() == "true")
                                    itemValue = "女";
                                else
                                    itemValue = "男";
                                break;
                            case ViewModelDataType.是否:
                                if (property.GetValue(boVM).ToString().ToLower() == "true")
                                    itemValue = "是";
                                else
                                    itemValue = "否";
                                break;
                            case ViewModelDataType.图标:
                                itemValue = "<i class=\"" + property.GetValue(boVM).ToString() + "\"></i>";
                                break;
                            default:
                                itemValue = property.GetValue(boVM).ToString();
                                break;
                        }
                    }
                    // 提取属性显示名
                    string itemDisplay = helper.DisplayName(itemName);

                    htmlContent.Append(_GetModalStartString(itemDisplay) + itemValue + _GetModalEndString());

                }
            }

            return new HtmlString(htmlContent.ToString());
        }

        /// <summary>
        /// 处理显示名称所需要的 html 字符串
        /// </summary>
        /// <param name="itemDisplay"></param>
        /// <returns></returns>
        private static string _GetModalStartString(string itemDisplay)
        {
            return
                "<div class='form-group form-row' style='margin-top:-12px'>" +
                "    <div class='col-12 col-md-12 col-sm-12' style='font-weight:normal'><small class='form-text text-info'>" + itemDisplay + "</small></div>" +
                "    <div class='col-12 col-md-12 col-sm-12' style='font-size:14px;border-top-style:solid;border-top-color:darkgray;border-top-width:1px;padding-top:2px'>";
        }

        /// <summary>
        /// 处理后端字符串
        /// </summary>
        /// <param name="errorTip"></param>
        /// <returns></returns>
        private static string _GetModalEndString()
        {
            return
                "    </div>" +
                "</div>";
        }

    }
}

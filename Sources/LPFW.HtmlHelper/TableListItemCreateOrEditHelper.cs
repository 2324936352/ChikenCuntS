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
    /// <summary>
    /// 列表编辑器助理程序
    /// </summary>
    public static class TableListItemCreateOrEditHelper
    {
        public static HtmlString GetTableListItemInput(this IHtmlHelper helper, CreateOrEditItem createOrEditItem, object boVM,string itemNamePrefix)
        {
            var htmlContent = new StringBuilder();
            var itemKind = createOrEditItem.DataType;
            // 需要显示处理的所需参数
            var htmlItemPara = _GetHtmlItemPara(helper, createOrEditItem, boVM,itemNamePrefix);

            switch (itemKind)
            {
                case ViewModelDataType.隐藏:
                    htmlContent.Append("<input type='hidden' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' value='" + htmlItemPara.ItemValue + "' />");
                    break;
                case ViewModelDataType.单行文本:
                    htmlContent.Append(_GetTableListItemInputString(htmlItemPara));
                    break;
                default:
                    break;
            }

            return new HtmlString(htmlContent.ToString());
        }

        private static string _GetTableListItemInputString(HtmlItemPara htmlItemPara)
        {
            var readOnly = "";
            var isReadonlyPlainText = "";

            if (htmlItemPara.IsPlainText)
                isReadonlyPlainText = "-plaintext";

            if (htmlItemPara.IsReadonly)
                readOnly = "readonly ";

            if (htmlItemPara.IsReadonlyPlainText)
            {
                readOnly = "readonly ";
                isReadonlyPlainText = "-plaintext";
            }

            var defaultString =
                "<input type = 'text' class='form-control"+isReadonlyPlainText+" form-control-sm " + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' value='" + htmlItemPara.ItemValue + "' required "+readOnly+" />";
            if (!String.IsNullOrEmpty(htmlItemPara.DescValue))
            {
                var descName = htmlItemPara.ItemName + "Help";
                defaultString =
                "<input type='text' class='form-control"+isReadonlyPlainText+" form-control-sm " + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' aria-describedby='" + descName + "' value='" + htmlItemPara.ItemValue + "' required "+readOnly+"/>" +
                "<small id='" + descName + "' class='form-text text-muted'>" + htmlItemPara.DescValue + "</small>";
            }
            var result = _GetModalDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetModalValidateString(htmlItemPara.ErrorTip);
            return result;
        }

        /// <summary>
        /// 处理显示名称所需要的 html 字符串
        /// </summary>
        /// <param name="itemDisplay"></param>
        /// <returns></returns>
        private static string _GetModalDispalyNameString(string itemDisplay)
        {
            return
                "<div class='form-group form-row' style='padding:0px;margin:0px'>" +
                //"    <div class='text-info text-left' style='font-size:11px;margin-left:4px'>" + itemDisplay + "</div> " +
                "    <div class='col-12 col-md-12 col-sm-12'>";
        }

        /// <summary>
        /// 处理校验错误信息所需要的字符串
        /// </summary>
        /// <param name="errorTip"></param>
        /// <returns></returns>
        private static string _GetModalValidateString(string errorTip)
        {
            return
                "    <small class='form-text text-danger text-right' style='font-size:11px'>" + errorTip + "</small>" +
                "    </div>" +
                "</div>";
        }


        private static HtmlItemPara _GetHtmlItemPara(IHtmlHelper helper, CreateOrEditItem item, object boVM,string itemNamePrefix)
        {
            var htmlItemPara = new HtmlItemPara();

            PropertyInfo[] properties = boVM.GetType().GetProperties();
            var itemName = item.PropertyName;
            var property = properties.FirstOrDefault(x => x.Name == itemName);
            if (property != null)
            {
                var tempName = item.PropertyName;
                itemName = itemNamePrefix + "." + itemName;
                // 提取属性类型名称
                var itemTypeName = property.PropertyType.Name;
                // 提取属性的值
                object itemValue = "";
                if (property.GetValue(boVM) != null)
                    if (itemTypeName != "String[]")
                        itemValue = property.GetValue(boVM).ToString();
                    else
                        itemValue = property.GetValue(boVM);

                // 提取属性显示名
                string itemDisplay = helper.DisplayName(tempName);
                // 构建
                string placeholderString = "请输入" + itemDisplay + "数据。";

                // 定义和获取校验结果
                ModelValidationState modelState = helper.ViewData.ModelState.GetValidationState(itemName);

                // 获取及获取校验信息
                ModelStateEntry modelStateValue;
                helper.ViewData.ModelState.TryGetValue(itemName, out modelStateValue);

                var errorcalss = "";
                var errorTip = "";

                if (modelState == ModelValidationState.Invalid)
                {
                    errorcalss = "is-invalid";
                    errorTip = modelStateValue.Errors.FirstOrDefault().ErrorMessage;
                }

                // 编辑单元数据处理要求描述
                string descValue = item.TipsString;

                // 简单封装一下需要传输处理的参数
                htmlItemPara = new HtmlItemPara()
                {
                    ItemDisplay       = itemDisplay,
                    Errorclass        = errorcalss,
                    ItemName          = itemName,
                    PlaceholderString = placeholderString,
                    ErrorTip          = errorTip,
                    ItemValue         = itemValue,
                    DescValue         = descValue
                };
            }

            htmlItemPara.IsPlainText = item.IsPlainText;
            htmlItemPara.IsReadonly = item.IsReadonly;
            htmlItemPara.IsReadonlyPlainText = item.IsReadonlyPlainText;
            return htmlItemPara;

        }

    }
}

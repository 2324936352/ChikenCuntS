using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels;
using System.Reflection;
using LPFW.ViewModels.BusinessCommon;

namespace LPFW.HtmlHelper
{
    /// <summary>
    /// 常规的处理 **CreateOrEditForm 相关的 html 表单元素助理
    /// </summary>
    public static class BoVMCreateOrEditFormHelper
    {
        #region 卡片标题部分
        /// <summary>
        /// 普通卡片标题，包含一个退回到列表的操作按钮 appGotoDefaultCommonPage
        /// </summary>
        /// <param name="helpe"></param>
        /// <param name="titleName">标题名称</param>
        /// <param name="defualtUrlString">退出按钮的导航路径</param>
        /// <returns></returns>
        public static HtmlString CommonCreateOrEditCardHeader(this IHtmlHelper helper, string titleName, string defualtUrlString)
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


        /// <summary>
        /// 在分页的列表元素的表单标题，包含一个退回到列表的操作按钮 appGotoDefaultPaginatePage
        /// </summary>
        /// <param name="helpe"></param>
        /// <param name="titleName">标题名称</param>
        /// <param name="defualtUrlString">退出按钮的导航路径</param>
        /// <returns></returns>
        public static HtmlString PaginateCreateOrEditCardHeader(this IHtmlHelper helper, string titleName, string defualtUrlString)
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

        #endregion

        #region 根据传入的视图模型生成数据处理表单元素的方法
        /// <summary>
        /// 一个简单的根据视图模型创建常规的数据处理输入的内容
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static HtmlString GetHtmlStringForVM(this IHtmlHelper helper, object boVM)
        {
            var htmlContent = new StringBuilder();

            PropertyInfo[] properties = boVM.GetType().GetProperties();
            foreach (var item in properties)
            {
                var itemTypeName = item.PropertyType.Name;
                if (itemTypeName.ToLower() == "string" && item.Name != "OrderNumber" && !item.Name.Contains("Id"))
                {
                    var itemName = item.Name;
                    var itemValue = item.GetValue(boVM);
                    string itemDisplay = helper.DisplayName(itemName);
                    string placeholderString = "请输入" + itemDisplay + "数据。";

                    var itemHtmlString = helper.FormItemForInputText(itemName, itemValue, "请输入" + itemDisplay + "数据。");
                    ModelStateEntry modelStateValue;
                    ModelValidationState modelState = helper.ViewData.ModelState.GetValidationState(itemName);
                    helper.ViewData.ModelState.TryGetValue(itemName, out modelStateValue);

                    var errorcalss = "";
                    var errorTip = "";

                    if (modelState == ModelValidationState.Invalid)
                    {
                        errorcalss = "is-invalid";
                        errorTip = modelStateValue.Errors.FirstOrDefault().ErrorMessage; ;
                    }

                    htmlContent.Append(
                        _GetDispalyNameString(itemDisplay) +
                        "<input type='text' class='form-control " + errorcalss + "' id='" + itemName + "' name='" + itemName + "' placeholder='" + placeholderString + "' value='" + itemValue + "' required />" +
                         _GetValidateString(errorTip));
                }
            }
            return new HtmlString(htmlContent.ToString());
        }

        #endregion

        #region 适用于常规的视图，根据传入的表单元素清单 List<CreateOrEditItem> 生成处理数据处理表单的方法
        /// <summary>
        /// 根据视图模型创建常规的数据处理输入的内容
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static HtmlString GetHtmlStringForCreateOrEditItemCollection(this IHtmlHelper helper, List<CreateOrEditItem> createOrEditItems, object boVM)
        {
            var htmlContent = new StringBuilder();
            foreach (var item in createOrEditItems)
            {
                var itemKind = item.DataType;

                // 需要显示处理的所需参数
                var htmlItemPara = _GetHtmlItemPara(helper, item, boVM);

                switch (itemKind)
                {
                    case ViewModelDataType.隐藏:
                        htmlContent.Append("<input type='hidden' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' value='" + htmlItemPara.ItemValue + "' />");
                        break;
                    case ViewModelDataType.单行文本:
                        htmlContent.Append(_GetInputItemString(htmlItemPara));
                        break;
                    case ViewModelDataType.多行文本:
                        htmlContent.Append(_GetAreaString(htmlItemPara));
                        break;
                    case ViewModelDataType.富文本:
                        htmlContent.Append(_GetAreaString(htmlItemPara));
                        break;
                    case ViewModelDataType.密码:
                        htmlContent.Append(_GetPasswordString(htmlItemPara));
                        break;
                    case ViewModelDataType.普通下拉单选一:
                        // 提取关联属性可选项集合
                        var plainFacadelItems =  _GetSelectOptionsWithPlainFacadelItem(htmlItemPara.ItemName, boVM);
                        htmlContent.Append(_GetSelectWithPlainFacadeItemString(htmlItemPara,plainFacadelItems));
                        break;
                    case ViewModelDataType.层次下拉单选一:
                        var selfReferentialItems = _GetSelectOptionsWithSelfReferentialItem(htmlItemPara.ItemName, boVM);
                        htmlContent.Append(_GetSelectWithSelfReferentialItemString(htmlItemPara,selfReferentialItems));
                        break;
                    case ViewModelDataType.枚举下拉单选一:
                        // 提取关联属性可选项集合
                        var enumItems = _GetSelectOptionsWithEnumItem(htmlItemPara.ItemName, boVM);
                        htmlContent.Append(_GetSelectWithPlainFacadeItemString(htmlItemPara, enumItems));
                        break;
                    case ViewModelDataType.Select多选:
                        // 提取关联属性可选项集合
                        var selectItems = _GetSelectOptionsWithPlainFacadelItem(htmlItemPara.ItemName, boVM);
                        htmlContent.Append(_GetMultiSelectWithPlainFacadeItemString(htmlItemPara, selectItems));
                        break;
                    case ViewModelDataType.Select层次多选:
                        // 提取关联属性可选项集合
                        var selfReferentialSelectItems = _GetSelectOptionsWithSelfReferentialItem(htmlItemPara.ItemName, boVM);
                        htmlContent.Append(_GetMultiSelectSelfReferentialItemString(htmlItemPara, selfReferentialSelectItems));
                        break;
                    case ViewModelDataType.CheckBox多选:
                        // 提取关联属性可选项集合
                        var checkItems = _GetSelectOptionsWithPlainFacadelItem(htmlItemPara.ItemName, boVM);
                        htmlContent.Append(_GetMultiCheckWithPlainFacadeItemString(htmlItemPara, checkItems));
                        break;
                    case ViewModelDataType.日期:
                        htmlContent.Append(_GetDateItemString(htmlItemPara));
                        break;
                    case ViewModelDataType.日期时间:
                        htmlContent.Append(_GetDateTimeItemString(htmlItemPara));
                        break;
                    case ViewModelDataType.是否:
                        htmlContent.Append(_GetYesOrNoString(htmlItemPara));
                        break;
                    case ViewModelDataType.性别:
                        htmlContent.Append(_GetSexString(htmlItemPara));
                        break;
                    case ViewModelDataType.联系地址:
                        htmlContent.Append(_GetAddressVMString(htmlItemPara));
                        break;
                    default:
                        break;
                }
            }

            return new HtmlString(htmlContent.ToString());
        }

        public static HtmlString GetHtmlStringForCreateOrEditItem(this IHtmlHelper helper, CreateOrEditItem createOrEditItem, object boVM)
        {
            var htmlContent = new StringBuilder();
            var itemKind = createOrEditItem.DataType;

            // 需要显示处理的所需参数
            var htmlItemPara = _GetHtmlItemPara(helper, createOrEditItem, boVM);

            switch (itemKind)
            {
                case ViewModelDataType.隐藏:
                    htmlContent.Append("<input type='hidden' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' value='" + htmlItemPara.ItemValue + "' />");
                    break;
                case ViewModelDataType.单行文本:
                    htmlContent.Append(_GetInputItemString(htmlItemPara));
                    break;
                case ViewModelDataType.多行文本:
                    htmlContent.Append(_GetAreaString(htmlItemPara));
                    break;
                case ViewModelDataType.富文本:
                    htmlContent.Append(_GetAreaString(htmlItemPara));
                    break;
                case ViewModelDataType.密码:
                    htmlContent.Append(_GetPasswordString(htmlItemPara));
                    break;
                case ViewModelDataType.普通下拉单选一:
                    // 提取关联属性可选项集合
                    var plainFacadelItems = _GetSelectOptionsWithPlainFacadelItem(htmlItemPara.ItemName, boVM);
                    htmlContent.Append(_GetSelectWithPlainFacadeItemString(htmlItemPara, plainFacadelItems));
                    break;
                case ViewModelDataType.层次下拉单选一:
                    var selfReferentialItems = _GetSelectOptionsWithSelfReferentialItem(htmlItemPara.ItemName, boVM);
                    htmlContent.Append(_GetSelectWithSelfReferentialItemString(htmlItemPara, selfReferentialItems));
                    break;
                case ViewModelDataType.枚举下拉单选一:
                    // 提取关联属性可选项集合
                    var enumItems = _GetSelectOptionsWithEnumItem(htmlItemPara.ItemName, boVM);
                    htmlContent.Append(_GetSelectWithPlainFacadeItemString(htmlItemPara, enumItems));
                    break;
                case ViewModelDataType.Select多选:
                    // 提取关联属性可选项集合
                    var selectItems = _GetSelectOptionsWithPlainFacadelItem(htmlItemPara.ItemName, boVM);
                    htmlContent.Append(_GetMultiSelectWithPlainFacadeItemString(htmlItemPara, selectItems));
                    break;
                case ViewModelDataType.CheckBox多选:
                    // 提取关联属性可选项集合
                    var checkItems = _GetSelectOptionsWithPlainFacadelItem(htmlItemPara.ItemName, boVM);
                    htmlContent.Append(_GetMultiCheckWithPlainFacadeItemString(htmlItemPara, checkItems));
                    break;
                case ViewModelDataType.日期:
                    htmlContent.Append(_GetDateItemString(htmlItemPara));
                    break;
                case ViewModelDataType.日期时间:
                    htmlContent.Append(_GetDateTimeItemString(htmlItemPara));
                    break;
                case ViewModelDataType.是否:
                    htmlContent.Append(_GetYesOrNoString(htmlItemPara));
                    break;
                case ViewModelDataType.性别:
                    htmlContent.Append(_GetSexString(htmlItemPara));
                    break;
                default:
                    break;
            }

            return new HtmlString(htmlContent.ToString());
        }

        private static string _GetInputItemString(HtmlItemPara htmlItemPara)
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
                "<input type = 'text' class='form-control"+isReadonlyPlainText+" "+ htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' value='" + htmlItemPara.ItemValue + "' required "+readOnly+" />";
            if (!String.IsNullOrEmpty(htmlItemPara.DescValue))
            {
                var descName = htmlItemPara.ItemName + "Help";
                defaultString =
                "<input type='text' class='form-control"+isReadonlyPlainText+ " "+ htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' aria-describedby='" + descName + "' value='" + htmlItemPara.ItemValue + "' required "+readOnly+" />" +
                "<small id='" + descName + "' class='form-text text-muted'>" + htmlItemPara.DescValue + "</small>";
            }
            var result = _GetDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetValidateString(htmlItemPara.ErrorTip);
            return result;
        }

        private static string _GetDateItemString(HtmlItemPara htmlItemPara)
        {
            var dateValue = DateTime.Parse(htmlItemPara.ItemValue.ToString()).ToString("yyyy-MM-dd");
            var defaultString =
                "<input type = 'date' class='form-control " + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' value='" + dateValue + "' required />";
            if (!String.IsNullOrEmpty(htmlItemPara.DescValue))
            {
                var descName = htmlItemPara.ItemName + "Help";
                defaultString =
                "<input type='date' class='form-control " + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' aria-describedby='" + descName + "' value='" + dateValue + "' required />" +
                "<small id='" + descName + "' class='form-text text-muted'>" + htmlItemPara.DescValue + "</small>";
            }
            var result = _GetDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetValidateString(htmlItemPara.ErrorTip);
            return result;
        }

        private static string _GetDateTimeItemString(HtmlItemPara htmlItemPara)
        {
            var dateTimeValue = DateTime.Parse(htmlItemPara.ItemValue.ToString()).ToString("yyyy-MM-ddThh:mm");
            var defaultString =
                "<input type = 'datetime-local' class='form-control " + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' value='" + dateTimeValue + "' required />";
            if (!String.IsNullOrEmpty(htmlItemPara.DescValue))
            {
                var descName = htmlItemPara.ItemName + "Help";
                defaultString =
                "<input type='datetime-local' class='form-control " + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' aria-describedby='" + descName + "' value='" + dateTimeValue + "' required />" +
                "<small id='" + descName + "' class='form-text text-muted'>" + htmlItemPara.DescValue + "</small>";
            }
            var result = _GetDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetValidateString(htmlItemPara.ErrorTip);
            return result;
        }

        private static string _GetPasswordString(HtmlItemPara htmlItemPara)
        {
            var result = "";
            var defaultString = "<input type = 'password' class='form-control " + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' value='" + htmlItemPara.ItemValue + "' required />";
            if (!String.IsNullOrEmpty(htmlItemPara.DescValue))
            {
                var descName = htmlItemPara.ItemName + "Help";
                defaultString =
                "<input type='password' class='form-control " + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' aria-describedby='" + descName + "' value='" + htmlItemPara.ItemValue + "' required />" +
                "<small id='" + descName + "' class='form-text text-muted'>" + htmlItemPara.DescValue + "</small>";
            }
            result = _GetDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetValidateString(htmlItemPara.ErrorTip);
            return result;
        }

        private static string _GetAreaString(HtmlItemPara htmlItemPara)
        {
            var defaultString =
                "<textarea class='form-control" + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' rows='5'>" + htmlItemPara.ItemValue + "</textarea>";
            if (!String.IsNullOrEmpty(htmlItemPara.DescValue))
            {
                var descName = htmlItemPara.ItemName + "Help";
                defaultString =
                "<textarea class='form-control" + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' aria-describedby='" + descName + "' rows='5'>" + htmlItemPara.ItemValue + "</textarea>" +
                "<small id='" + descName + "' class='form-text text-muted'>" + htmlItemPara.DescValue + "</small>";
            }

            string result = _GetDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetValidateString(htmlItemPara.ErrorTip);
            return result;
        }

        private static string _GetSelectWithSelfReferentialItemString(HtmlItemPara htmlItemPara, List<SelfReferentialItem> optionItems)
        {
            var result = "";
            if (optionItems == null)
            {
                result = _GetDispalyNameString(htmlItemPara.ItemDisplay) + "<div class='text-danger' style='margin-top:7px'>设计时错误：没有初始化供选择的数据。</div>" + _GetValidateString(htmlItemPara.ErrorTip);
            }
            else
            {

                var optionString = "";
                foreach (var item in optionItems)
                {
                    if (item.ID == htmlItemPara.ItemValue.ToString())
                        optionString = optionString + "<option value = " + item.ID + " selected> " + item.DisplayName + " </option>";
                    else
                        optionString = optionString + "<option value = " + item.ID + "> " + item.DisplayName + " </option>";
                }


                var defaultString =
                    "<select class='form-control " + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "'>" +
                    "<option value=''></option>" +
                    optionString +
                    "</select>";

                if (!String.IsNullOrEmpty(htmlItemPara.DescValue))
                {
                    var descName = htmlItemPara.ItemName + "Help";
                    defaultString =
                    "<select class='form-control " + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' aria-describedby='" + descName + "' >" +
                    "<option value=''></option>" +
                    optionString +
                    "</select>" +
                    "<small id='" + descName + "' class='form-text text-muted'>" + htmlItemPara.DescValue + "</small>";
                }
                result = _GetDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetValidateString(htmlItemPara.ErrorTip);
            }
            
                return result;
        }

        private static string _GetSelectWithPlainFacadeItemString(HtmlItemPara htmlItemPara, List<PlainFacadeItem> optionItems)
        {
            var result = "";
            if (optionItems == null)
            {
                result = _GetDispalyNameString(htmlItemPara.ItemDisplay) + "<div class='text-danger' style='margin-top:7px'>设计时错误：没有初始化供选择的数据。</div>" + _GetValidateString(htmlItemPara.ErrorTip);
            }
            else
            {
                var optionString = "";
                foreach (var item in optionItems)
                {
                    if (item.ID == htmlItemPara.ItemValue.ToString())
                        optionString = optionString + "<option value = " + item.ID + " selected> " + item.DisplayName + " </option>";
                    else
                        optionString = optionString + "<option value = " + item.ID + "> " + item.DisplayName + " </option>";
                }


                var defaultString =
                    "<select class='form-control " + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "'>" +
                    "<option value=''></option>" +
                    optionString +
                    "</select>";

                if (!String.IsNullOrEmpty(htmlItemPara.DescValue))
                {
                    var descName = htmlItemPara.ItemName + "Help";
                    defaultString =
                    "<select class='form-control " + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' aria-describedby='" + descName + "' >" +
                    "<option value=''></option>" +
                    optionString +
                    "</select>" +
                    "<small id='" + descName + "' class='form-text text-muted'>" + htmlItemPara.DescValue + "</small>";
                }

                result = _GetDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetValidateString(htmlItemPara.ErrorTip);
            }
            return result;
        }

        private static string _GetMultiSelectWithPlainFacadeItemString(HtmlItemPara htmlItemPara, List<PlainFacadeItem> optionItems)
        {
            var result = "";
            if (optionItems == null)
            {
                result = _GetDispalyNameString(htmlItemPara.ItemDisplay) + "<div class='text-danger' style='margin-top:7px'>设计时错误：没有初始化供选择的数据。</div>" + _GetValidateString(htmlItemPara.ErrorTip);
            }
            else
            {
                string[] selectItems = htmlItemPara.ItemValue as string[];
                var optionString = "";

                foreach (var item in optionItems)
                {
                    if (selectItems.Contains(item.ID))
                        optionString = optionString + "<option value = " + item.ID + " selected='selected'> " + item.DisplayName + " </option>";
                    else
                        optionString = optionString + "<option value = " + item.ID + "> " + item.DisplayName + " </option>";
                }


                var defaultString =
                    "<select class='form-control " + htmlItemPara.Errorclass + "' multiple='multiple' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "[]' placeholder='" + htmlItemPara.PlaceholderString + "'>" +
                    optionString +
                    "</select>";

                if (!String.IsNullOrEmpty(htmlItemPara.DescValue))
                {
                    var descName = htmlItemPara.ItemName + "Help";
                    defaultString =
                    "<select class='form-control " + htmlItemPara.Errorclass + "' multiple='multiple' data-height='100%'  id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "[]' placeholder='" + htmlItemPara.PlaceholderString + "' aria-describedby='" + descName + "' >" +
                    "<option value=''></option>" +
                    optionString +
                    "</select>" +
                    "<small id='" + descName + "' class='form-text text-muted'>" + htmlItemPara.DescValue + "</small>";
                }
                result = _GetDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetValidateString(htmlItemPara.ErrorTip);
            }
            return result;
        }

        private static string _GetMultiSelectSelfReferentialItemString(HtmlItemPara htmlItemPara, List<SelfReferentialItem> optionItems)
        {
            var result = "";
            if (optionItems == null)
            {
                result = _GetDispalyNameString(htmlItemPara.ItemDisplay) + "<div class='text-danger' style='margin-top:7px'>设计时错误：没有初始化供选择的数据。</div>" + _GetValidateString(htmlItemPara.ErrorTip);
            }
            else
            {
                string[] selectItems = htmlItemPara.ItemValue as string[];
                var optionString = "";

                foreach (var item in optionItems)
                {
                    if (selectItems.Contains(item.ID))
                        optionString = optionString + "<option value = " + item.ID + " selected='selected'> " + item.DisplayName + " </option>";
                    else
                        optionString = optionString + "<option value = " + item.ID + "> " + item.DisplayName + " </option>";
                }


                var defaultString =
                    "<select class='form-control " + htmlItemPara.Errorclass + "' multiple='multiple' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "[]' placeholder='" + htmlItemPara.PlaceholderString + "'>" +
                    optionString +
                    "</select>";

                if (!String.IsNullOrEmpty(htmlItemPara.DescValue))
                {
                    var descName = htmlItemPara.ItemName + "Help";
                    defaultString =
                    "<select class='form-control " + htmlItemPara.Errorclass + "' multiple='multiple' data-height='100%'  id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "[]' placeholder='" + htmlItemPara.PlaceholderString + "' aria-describedby='" + descName + "' >" +
                    "<option value=''></option>" +
                    optionString +
                    "</select>" +
                    "<small id='" + descName + "' class='form-text text-muted'>" + htmlItemPara.DescValue + "</small>";
                }
                result = _GetDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetValidateString(htmlItemPara.ErrorTip);
            }
            return result;
        }

        private static string _GetMultiCheckWithPlainFacadeItemString(HtmlItemPara htmlItemPara, List<PlainFacadeItem> optionItems)
        {
            var result = "";
            if (optionItems == null)
            {
                result = _GetDispalyNameString(htmlItemPara.ItemDisplay) + "<div class='text-danger' style='margin-top:7px'>设计时错误：没有初始化供选择的数据。</div>" + _GetValidateString(htmlItemPara.ErrorTip);
            }
            else
            {

                var checkString = "";
                var counter = 0;
                foreach (var item in optionItems)
                {
                    if (item.ID == htmlItemPara.ItemValue.ToString())
                        checkString = checkString +
                            "<div class='custom-control custom-checkbox'>" +
                            "<input type = 'checkbox' class='custom-control-input' name='" + htmlItemPara.ItemName + "' id='" + item.ID + "' value='" + item.ID + "' checked>" +
                            "<label class='custom-control-label' for='" + item.ID + "'>" + item.DisplayName + "</label>" +
                            "</div>";
                    else
                        checkString = checkString +
                            "<div class='custom-control custom-checkbox'>" +
                            "<input type = 'checkbox' class='custom-control-input' name='" + htmlItemPara.ItemName + "' id='" + item.ID + "' value='" + item.ID + "'>" +
                            "<label class='custom-control-label' for='" + item.ID + "'>" + item.DisplayName + "</label>" +
                            "</div>";
                    counter++;
                }

                result = _GetDispalyNameString(htmlItemPara.ItemDisplay) + checkString + _GetValidateString(htmlItemPara.ErrorTip);
            }
            return result;
        }

        private static string _GetSexString(HtmlItemPara htmlItemPara)
        {
            var result = "";
            var defaultString = "";
            if (htmlItemPara.ItemValue.ToString().ToLower() == "false")
            {
                defaultString =
                    "<div class='custom-control custom-radio custom-control-inline' style='margin-top:7px'>" +
                    "<input class='custom-control-input' type='radio' name='" + htmlItemPara.ItemName + "' id='" + htmlItemPara.ItemName +"01' value='false' checked>" +
                    "<label class='custom-control-label' for='" + htmlItemPara.ItemName +"01'>男</label>" +
                    "</div>" +
                    "<div class='custom-control custom-radio custom-control-inline'>" +
                    "<input class='custom-control-input' type='radio' name='" + htmlItemPara.ItemName + "' id='" + htmlItemPara.ItemName +"02' value='true'>" + "" +
                    "<label class='custom-control-label' for='" + htmlItemPara.ItemName +"02'>女</label>" +
                    "</div>";
            }
            else
            {
                defaultString =
                    "<div class='custom-control custom-radio custom-control-inline' style='margin-top:7px'>" +
                    "<input class='custom-control-input' type='radio' name='" + htmlItemPara.ItemName + "' id='" + htmlItemPara.ItemName +"01' value='false'>" +
                    "<label class='custom-control-label' for='" + htmlItemPara.ItemName +"01'>男</label>" +
                    "</div>" +
                    "<div class='custom-control custom-radio custom-control-inline'>" +
                    "<input class='custom-control-input' type='radio' name='" + htmlItemPara.ItemName + "' id='" + htmlItemPara.ItemName +"02' value='true' checked>" + "" +
                    "<label class='custom-control-label' for='" + htmlItemPara.ItemName +"02'>女</label>" +
                    "</div>";
            }
            result = _GetDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetValidateString(htmlItemPara.ErrorTip);
            return result;
        }

        private static string _GetYesOrNoString(HtmlItemPara htmlItemPara)
        {
            var result = "";
            var defaultString = "";
            if (htmlItemPara.ItemValue.ToString().ToLower() == "false")
            {
                defaultString =
                    "<div class='custom-control custom-radio custom-control-inline' style='margin-top:7px'>" +
                    "<input class='custom-control-input' type='radio' name='" + htmlItemPara.ItemName + "' id='" + htmlItemPara.ItemName +"01' value='true'>" +
                    "<label class='custom-control-label' for='" + htmlItemPara.ItemName +"01'>是</label>" +
                    "</div>" +
                    "<div class='custom-control custom-radio custom-control-inline'>" +
                    "<input class='custom-control-input' type='radio' name='" + htmlItemPara.ItemName + "' id='" + htmlItemPara.ItemName +"02' value='false' checked>" + "" +
                    "<label class='custom-control-label' for='" + htmlItemPara.ItemName +"02'>否</label>" +
                    "</div>";
            }
            else
            {
                defaultString =
                    "<div class='custom-control custom-radio custom-control-inline' style='margin-top:7px'>" +
                    "<input class='custom-control-input' type='radio' name='" + htmlItemPara.ItemName + "' id='" + htmlItemPara.ItemName +"01' value='true' checked>" +
                    "<label class='custom-control-label' for='" + htmlItemPara.ItemName +"01'>是</label>" +
                    "</div>" +
                    "<div class='custom-control custom-radio custom-control-inline'>" +
                    "<input class='custom-control-input' type='radio' name='" + htmlItemPara.ItemName + "' id='" + htmlItemPara.ItemName +"02' value='false'>" + "" +
                    "<label class='custom-control-label' for='" + htmlItemPara.ItemName +"02'>否</label>" +
                    "</div>";
            }
            result = _GetDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetValidateString(htmlItemPara.ErrorTip);
            return result;
        }

        private static string _GetAddressVMString(HtmlItemPara htmlItemPara)
        {
            var addressVM = htmlItemPara.ItemValue as CommonAddressVM;
            var result = "";
            if (addressVM == null)
            {
                result =
                    "<div class='form-group form-row' style='margin-top:-18px'>    " +
                    "    <div class='col-12 col-md-2 col-sm-2 text-lg-right' style='margin-top:8px'>" +
                    "        <label class='control-label' style='font-size:16px;font-weight:normal'>联系地址：</label>" +
                    "    </div>  " +
                    "    <div class='col-12 col-md-7 col-sm-7'> 运行时错误，未对地址变量 AddressVM 进行初始化。" +
                    "    </div>   " +
                    "    <div class='col-12 col-md-3 col-sm-3' style='margin-top:8px'>        " +
                    "        <span class='text-danger'></span>    " +
                    "    </div>" +
                    "</div>";
            }
            else
            {
                result =
                    "<div class='form-group form-row' style='margin-top:-18px'>    " +
                    "    <div class='col-12 col-md-2 col-sm-2 text-lg-right' style='margin-top:8px'>" +
                    "        <label class='control-label' style='font-size:16px;font-weight:normal'>联系地址：</label>" +
                    "    </div>  " +
                    "    <div class='col-12 col-md-7 col-sm-7'>" +
                    "        <div class='row'>" +
                    "            <input type='hidden' id='AddressVM.Id' name='AddressVM.Id' value='" + addressVM.Id + "' />" +
                    "            <input type='hidden' id='AddressVM.Name' name='AddressVM.Name' value='" + addressVM.Name + "' />" +
                    "            <input type='hidden' id='AddressVM_ProvinceName' name='AddressVM_ProvinceName' value='" + addressVM.ProvinceName + "' />" +
                    "            <input type='hidden' id='AddressVM_CityName' name='AddressVM_CityName' value='" + addressVM.CityName + "' />" +
                    "            <input type='hidden' id='AddressVM_CountyName' name='AddressVM_CountyName' value='" + addressVM.CountyName + "' />" +
                    "            <div class='col-sm' style='padding-right:1px'>" +
                    "                <select id = 'AddressVM.ProvinceName' name='AddressVM.ProvinceName' class='form-control'></select>" +  
                    "            </div>" +
                    "            <div class='col-sm' style='padding-left:1px;padding-right:1px'>" +
                    "                <select id = 'AddressVM.CityName' name='AddressVM.CityName' class='form-control'></select> " +
                    "            </div>" +
                    "            <div class='col-sm' style='padding-left:1px'>" +
                    "                <select id = 'AddressVM.CountyName' name='AddressVM.CountyName' class='form-control'></select> " +
                    "            </div>" +
                    "        </div>" +
                    "        <div class='row' style='margin-top:5px'>" +
                    "            <div class='col-9' style='padding-right:1px'>" +
                    "                <input type='text' class='form-control' id='AddressVM.DetailName' name='AddressVM.DetailName' placeholder='详细地址' value='" + addressVM.DetailName + "' required=''>" +
                    "            </div>" +
                    "            <div class='col-3' style='padding-left:1px'>" +
                    "                <input type='text' class='form-control' id='AddressVM.SortCode' name='AddressVM.SortCode' placeholder='邮编' value='" + addressVM.SortCode + "' required=''>" +
                    "            </div>" +
                    "        </div>" +
                    "    </div>   " +
                    "    <div class='col-12 col-md-3 col-sm-3' style='margin-top:8px'>" +
                    "        <span class='text-danger'></span>    " +
                    "    </div>" +
                    "</div>";
            }
            return result;
        }
        #endregion

        #region 单个的数据表单元素的方法
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
        public static HtmlString FormItemForInputText(this IHtmlHelper helper, string itemName, object itemValue, string placeholderString)
        {
            string itemDisplay = helper.DisplayName(itemName);
            ModelStateEntry modelStateValue;
            ModelValidationState modelState = helper.ViewData.ModelState.GetValidationState(itemName);
            helper.ViewData.ModelState.TryGetValue(itemName, out modelStateValue);

            var errorcalss = "";
            var errorTip = "";

            if (modelState == ModelValidationState.Invalid)
            {
                errorcalss = "is-invalid";
                errorTip = modelStateValue.Errors.FirstOrDefault().ErrorMessage; ;
            }

            var htmlContent = new StringBuilder();
            htmlContent.Append(
                _GetDispalyNameString(itemDisplay) +
                "<input type='text' class='form-control " + errorcalss + "' id='" + itemName + "' name='" + itemName + "' placeholder='" + placeholderString + "' value='" + itemValue + "' required />" +
                 _GetValidateString(errorTip));
            return new HtmlString(htmlContent.ToString());
        }

        /// <summary>
        /// 用于处理单行输入普通文本数据 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="itemDisplay">显示名</param>
        /// <param name="itemName">属性名</param>
        /// <param name="itemValue">属性值</param>
        /// <param name="placeholderString">输入框提示文字</param>
        /// <param name="descName">输入框底部提示标识名</param>
        /// <param name="descValue">输入框底部提示标识文字说明</param>
        /// <param name="modelState">属性校验结果</param>
        /// <param name="modelStateValue">属性校验错误数据</param>
        /// <returns></returns>
        public static HtmlString FormItemForInputText(this IHtmlHelper helper, string itemName, object itemValue, string placeholderString, string descValue)
        {
            string itemDisplay = helper.DisplayName(itemName);
            string descName = itemName + "Help";
            ModelStateEntry modelStateValue;
            ModelValidationState modelState = helper.ViewData.ModelState.GetValidationState(itemName);
            helper.ViewData.ModelState.TryGetValue(itemName, out modelStateValue);

            var errorcalss = "";
            var errorTip = "";

            if (modelState == ModelValidationState.Invalid)
            {
                errorcalss = "is-invalid";
                errorTip = modelStateValue.Errors.FirstOrDefault().ErrorMessage; ;
            }

            var htmlContent = new StringBuilder();
            htmlContent.Append
                (

                _GetDispalyNameString(itemDisplay) +

                "<input type='text' class='form-control " + errorcalss + "' id='" + itemName + "' name='" + itemName + "' placeholder='" + placeholderString + "' aria-describedby='" + descName + "' value='" + itemValue + "' required />" +
                "<small id='" + descName + "' class='form-text text-muted'>" + descValue + "</small>" +

                _GetValidateString(errorTip)

                );

            return new HtmlString(htmlContent.ToString());
        }

        /// <summary>
        /// 用于处理 textarea 标签的
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="itemName"></param>
        /// <param name="itemValue"></param>
        /// <param name="placeholderString"></param>
        /// <returns></returns>
        public static HtmlString FormItemForTextArea(this IHtmlHelper helper, string itemName, object itemValue, string placeholderString)
        {
            string itemDisplay = helper.DisplayName(itemName);
            ModelStateEntry modelStateValue;
            ModelValidationState modelState = helper.ViewData.ModelState.GetValidationState(itemName);
            helper.ViewData.ModelState.TryGetValue(itemName, out modelStateValue);
            var errorcalss = "";
            var errorTip = "";

            if (modelState == ModelValidationState.Invalid)
            {
                errorcalss = "is-invalid";
                errorTip = modelStateValue.Errors.FirstOrDefault().ErrorMessage; ;
            }

            var htmlContent = new StringBuilder();
            htmlContent.Append(
                _GetDispalyNameString(itemDisplay) +
                "<textarea class='form-control " + errorcalss + "' id='" + itemName + "' name='" + itemName + "' placeholder='" + placeholderString + "'>" + itemValue + "</textarea>" +
                 _GetValidateString(errorTip));
            return new HtmlString(htmlContent.ToString());
        }

        /// <summary>
        /// 使用 SelfReferentialItem 类型的元素集合作为下拉选项，下拉的条目内容以层次外观的方式呈现
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="itemName"></param>
        /// <param name="itemValue"></param>
        /// <param name="placeholderString"></param>
        /// <param name="optionItems"></param>
        /// <returns></returns>
        public static HtmlString FormItemForSelectWithSelfReferentialItem(this IHtmlHelper helper, string itemName, object itemValue, string placeholderString, List<SelfReferentialItem> optionItems)
        {
            string itemDisplay = helper.DisplayName(itemName);
            ModelStateEntry modelStateValue;
            ModelValidationState modelState = helper.ViewData.ModelState.GetValidationState(itemName);
            helper.ViewData.ModelState.TryGetValue(itemName, out modelStateValue);
            var errorcalss = "";
            var errorTip = "";

            if (modelState == ModelValidationState.Invalid)
            {
                errorcalss = "is-invalid";
                errorTip = modelStateValue.Errors.FirstOrDefault().ErrorMessage; ;
            }

            var optionString = "";
            foreach (var item in optionItems)
            {
                if (item.ID == itemValue.ToString())
                    optionString = optionString + "<option value = " + item.ID + " selected> " + item.DisplayName + " </option>";
                else
                    optionString = optionString + "<option value = " + item.ID + "> " + item.DisplayName + " </option>";
            }

            var htmlContent = new StringBuilder();
            htmlContent.Append(
                _GetDispalyNameString(itemDisplay) +
                "<select class='form-control " + errorcalss + "' id='" + itemName + "' name='" + itemName + "' placeholder='" + placeholderString + "'>" +
                "<option value=''></option>" +
                optionString +
                "</select>" +
                 _GetValidateString(errorTip));

            return new HtmlString(htmlContent.ToString());
        }

        /// <summary>
        /// 使用 SelfReferentialItem 类型的元素集合作为下拉选项，下拉的条目内容以层次外观的方式呈现
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="itemName"></param>
        /// <param name="itemValue"></param>
        /// <param name="placeholderString"></param>
        /// <param name="descValue">提示说明</param>
        /// <param name="optionItems"></param>
        /// <returns></returns>
        public static HtmlString FormItemForSelectWithSelfReferentialItem(this IHtmlHelper helper, string itemName, object itemValue, string placeholderString, string descValue, List<SelfReferentialItem> optionItems)
        {
            string descName = itemName + "Help";
            string itemDisplay = helper.DisplayName(itemName);
            ModelStateEntry modelStateValue;
            ModelValidationState modelState = helper.ViewData.ModelState.GetValidationState(itemName);
            helper.ViewData.ModelState.TryGetValue(itemName, out modelStateValue);
            var errorcalss = "";
            var errorTip = "";

            if (modelState == ModelValidationState.Invalid)
            {
                errorcalss = "is-invalid";
                errorTip = modelStateValue.Errors.FirstOrDefault().ErrorMessage; ;
            }

            var optionString = "";
            foreach (var item in optionItems)
            {
                if (item.ID == itemValue.ToString())
                    optionString = optionString + "<option value = " + item.ID + " selected> " + item.DisplayName + " </option>";
                else
                    optionString = optionString + "<option value = " + item.ID + "> " + item.DisplayName + " </option>";
            }

            var htmlContent = new StringBuilder();
            htmlContent.Append(
                _GetDispalyNameString(itemDisplay) +
                "<select class='form-control " + errorcalss + "' id='" + itemName + "' name='" + itemName + "' placeholder='" + placeholderString + "' aria-describedby='" + descName + "' >" +
                "<option value=''></option>" +
                optionString +
                "</select>" +
                "<small id='" + descName + "' class='form-text text-muted'>" + descValue + "</small>" +
                 _GetValidateString(errorTip));

            return new HtmlString(htmlContent.ToString());
        }

        #endregion

        #region 共用的一些 html 字符串生成处理服务
        /// <summary>
        /// 根据类似 ParentID 属性，获取下拉层次节点数据，一般供给 select 标签的 option 集合使用
        /// </summary>
        /// <param name="parentIdName"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        private static List<SelfReferentialItem> _GetSelectOptionsWithSelfReferentialItem(string parentIdName, object boVM)
        {
            List<SelfReferentialItem> resultItems = null;
            PropertyInfo[] properties = boVM.GetType().GetProperties();

            // 提取关联数据集合的属性名称
            var relevancePropertyName = parentIdName.Replace("Id", "ItemCollection");
            // 提取关联数据集合的属性
            var relevanceProperty = properties.FirstOrDefault(x => x.Name == relevancePropertyName);
            // 提取关联数据集合
            if (relevanceProperty != null)
            {
                resultItems = relevanceProperty.GetValue(boVM) as List<SelfReferentialItem>;
            }

            return resultItems;
        }

        private static List<PlainFacadeItem> _GetSelectOptionsWithPlainFacadelItem(string itemName, object boVM)
        {
            List<PlainFacadeItem> resultItems = null;
            PropertyInfo[] properties = boVM.GetType().GetProperties();

            // 提取关联数据集合的属性名称
            var relevancePropertyName = itemName.Replace("Id", "ItemCollection");
            // 提取关联数据集合的属性
            var relevanceProperty = properties.FirstOrDefault(x => x.Name == relevancePropertyName);
            // 提取关联数据集合
            if (relevanceProperty != null)
            {
                resultItems = relevanceProperty.GetValue(boVM) as List<PlainFacadeItem>;
            }

            return resultItems;
        }

        private static List<PlainFacadeItem> _GetSelectOptionsWithEnumItem(string itemName, object boVM)
        {
            List<PlainFacadeItem> resultItems = null;
            PropertyInfo[] properties = boVM.GetType().GetProperties();

            // 提取关联数据集合的属性名称
            var relevancePropertyName = itemName+"ItemCollection";
            // 提取关联数据集合的属性
            var relevanceProperty = properties.FirstOrDefault(x => x.Name == relevancePropertyName);
            // 提取关联数据集合
            if (relevanceProperty != null)
            {
                resultItems = relevanceProperty.GetValue(boVM) as List<PlainFacadeItem>;
            }

            return resultItems;
        }


        /// <summary>
        /// 处理显示名称所需要的 html 字符串
        /// </summary>
        /// <param name="itemDisplay"></param>
        /// <returns></returns>
        private static string _GetDispalyNameString(string itemDisplay)
        {
            return
                "<div class='form-group form-row' style='margin-top:-18px'>" +
                "    <div class='col-12 col-md-2 col-sm-2 text-lg-right' style='margin-top:8px'>" +
                "        <label class='control-label' style='font-size:16px;font-weight:normal'>" + itemDisplay + "：</label>" +
                "    </div>" +
                "    <div class='col-12 col-md-7 col-sm-7'>";
        }

        /// <summary>
        /// 处理校验错误信息所需要的字符串
        /// </summary>
        /// <param name="errorTip"></param>
        /// <returns></returns>
        private static string _GetValidateString(string errorTip)
        {
            return
                "    </div>" +
                "    <div class='col-12 col-md-3 col-sm-3' style='margin-top:8px'>" +
                "        <span class='text-danger'>" + errorTip + "</span>" +
                "    </div>" +
                "</div>";
        }

        #endregion

        private static HtmlItemPara _GetHtmlItemPara(IHtmlHelper helper, CreateOrEditItem item,object boVM)
        {
            var htmlItemPara = new HtmlItemPara();
            PropertyInfo[] properties = boVM.GetType().GetProperties();
            var itemName = item.PropertyName;
            var property = properties.FirstOrDefault(x => x.Name == itemName);
            if (property != null)
            {
                // 提取属性类型名称
                var itemTypeName = property.PropertyType.Name;
                var itemTypeFullName = property.PropertyType.FullName;
                
                // 提取属性的值
                object itemValue = "";
                if (property.GetValue(boVM) != null)
                    if (itemTypeFullName.Contains("LPFW"))
                    {
                        itemValue = property.GetValue(boVM);
                    }
                    else
                    {
                        if (itemTypeName != "String[]")
                            itemValue = property.GetValue(boVM).ToString();
                        else
                            itemValue = property.GetValue(boVM);
                    }

                // 提取属性显示名
                string itemDisplay = helper.DisplayName(itemName);
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
                    ItemDisplay = itemDisplay,
                    Errorclass = errorcalss,
                    ItemName = itemName,
                    PlaceholderString = placeholderString,
                    ErrorTip = errorTip,
                    ItemValue = itemValue,
                    DescValue = descValue
                };
            }
            htmlItemPara.IsPlainText = item.IsPlainText;
            htmlItemPara.IsReadonly = item.IsReadonly;
            htmlItemPara.IsReadonlyPlainText = item.IsReadonlyPlainText;
            return htmlItemPara;

        }
    }


}

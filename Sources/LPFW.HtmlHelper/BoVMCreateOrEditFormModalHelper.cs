using LPFW.ViewModels.BusinessCommon;
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
    /// 为使用 Modal 会话框的 CreateOrEdit 表单处理的 Html 助理
    /// </summary>
    public static class BoVMCreateOrEditFormModalHelper
    {
        /// <summary>
        /// 与上一个方法类似，适用于使用 Modal 来加载处理数据的卡片标题
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="titleName"></param>
        /// <param name="defualtUrlString"></param>
        /// <param name="modalId"></param>
        /// <returns></returns>
        public static HtmlString CommonCreateOrEditWithModalCardHeader(this IHtmlHelper helper, string titleName, string defualtUrlString, string modalId)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append(
                "<div class='card-header' style=''>" +
                "<h4 style = 'font-size:22px;font-weight:normal;color:dimgray'>" + titleName + "</h4>" +
                "<div class='card-header-form'>" +
                "<button class='btn btn-info' onclick='appGotoDefaultCommonPageForModal(\"" + defualtUrlString + "\",\"mainWorkPlaceArea\",\"" + modalId + "\")'><i class='fas fa-times'></i> 退出 </button>" +
                "</div>" +
                "</div>");

            return new HtmlString(htmlContent.ToString());
        }

        /// <summary>
        /// 与上一个方法类似，适用于使用 Modal 来加载处理数据的卡片标题
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="titleName"></param>
        /// <param name="defualtUrlString"></param>
        /// <param name="modalId"></param>
        /// <returns></returns>
        public static HtmlString PaginateCreateOrEditWithModalCardHeader(this IHtmlHelper helper, string titleName, string defualtUrlString, string modalId)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append(
                "<div class='card-header' style=''>" +
                "<h4 style = 'font-size:22px;font-weight:normal;color:dimgray'>" + titleName + "</h4>" +
                "<div class='card-header-form'>" +
                "<button class='btn btn-info' onclick='appGotoDefaultPaginatePageForModal(\"" + defualtUrlString + "\",\"mainWorkPlaceArea\",\"" + modalId + "\")'><i class='fas fa-times'></i> 退出 </button>" +
                "</div>" +
                "</div>");

            return new HtmlString(htmlContent.ToString());
        }


        #region 适用于 Modal 的视图，根据传入的表单元素清单 List<CreateOrEditItem> 生成处理数据处理表单的方法
        /// <summary>
        /// 根据视图模型创建适用于 Modal 的数据处理输入的内容
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public static HtmlString GetModalHtmlStringForCreateOrEditItemCollection(this IHtmlHelper helper, List<CreateOrEditItem> createOrEditItems, object boVM)
        {
            var htmlContent = new StringBuilder();
            foreach (var item in createOrEditItems)
            {
                // 提取属性数据种类
                var itemKind = item.DataType;
                // 需要显示处理的所需参数
                var htmlItemPara = _GetHtmlItemPara(helper, item, boVM);

                switch (itemKind)
                {
                    case ViewModelDataType.隐藏:
                        htmlContent.Append("<input type='hidden' id='" + htmlItemPara.ItemName + "' name='" +  htmlItemPara.ItemName + "' value='" + htmlItemPara.ItemValue + "' />");
                        break;
                    case ViewModelDataType.单行文本:
                        htmlContent.Append(_GetModalInputItemString(htmlItemPara));
                        break;
                    case ViewModelDataType.多行文本:
                        htmlContent.Append(_GetModalAreaString(htmlItemPara));
                        break;
                    case ViewModelDataType.富文本:
                        htmlContent.Append(_GetModalAreaString(htmlItemPara));
                        break;
                    case ViewModelDataType.密码:
                        htmlContent.Append(_GetModalPasswordString(htmlItemPara));
                        break;
                    case ViewModelDataType.普通下拉单选一:
                        // 提取关联属性可选项集合
                        var plainFacadelItems = _GetSelectOptionsWithPlainFacadelItem(htmlItemPara.ItemName, boVM);
                        htmlContent.Append(_GetSelectWithPlainFacadeItemString(htmlItemPara, plainFacadelItems));
                        break;
                    case ViewModelDataType.层次下拉单选一:
                        htmlContent.Append(_GetSelectWithSelfReferentialItemString(htmlItemPara, _GetSelectOptionsWithSelfReferentialItem(htmlItemPara.ItemName, boVM)));
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
                    case ViewModelDataType.联系地址:
                        htmlContent.Append(_GetAddressVMString(htmlItemPara));
                        break;
                    default:
                        break;
                }
            }

            return new HtmlString(htmlContent.ToString());
        }

        private static string _GetModalInputItemString(HtmlItemPara htmlItemPara)
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
                "<input type='text' class='form-control"+isReadonlyPlainText+" "+ htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' aria-describedby='" + descName + "' value='" + htmlItemPara.ItemValue + "' required "+readOnly+" />" +
                "<small id='" + descName + "' class='form-text text-muted'>" + htmlItemPara.DescValue + "</small>";
            }
            var result = _GetModalDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetModalValidateString(htmlItemPara.ErrorTip);
            return result;
        }

        private static string _GetModalAreaString(HtmlItemPara htmlItemPara)
        {
            var result = "";
            var defaultString =
                "<textarea class='form-control " + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "'>" + htmlItemPara.ItemValue + "</textarea>";
            if (!String.IsNullOrEmpty(htmlItemPara.DescValue))
            {
                var descName = htmlItemPara.ItemName + "Help";
                defaultString =
                "<input type='text' class='form-control " + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' aria-describedby='" + descName + "' value='" + htmlItemPara.ItemValue + "' required />" +
                "<small id='" + descName + "' class='form-text text-muted'>" + htmlItemPara.DescValue + "</small>";
            }
            result = _GetModalDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetModalValidateString(htmlItemPara.ErrorTip);
            return result;
        }

        private static string _GetModalPasswordString(HtmlItemPara htmlItemPara)
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
            result = _GetModalDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetModalValidateString(htmlItemPara.ErrorTip);
            return result;
        }

        private static string _GetModalSelectWithSelfReferentialItemString(HtmlItemPara htmlItemPara, List<SelfReferentialItem> optionItems)
        {
            var result = "";
            if (optionItems == null)
            {
                result = _GetModalDispalyNameString(htmlItemPara.ItemDisplay) + "<div class='text-danger' style='margin-top:7px'>设计时错误：没有初始化供选择的数据。</div>" + _GetModalValidateString(htmlItemPara.ErrorTip);
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
                result = _GetModalDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetModalValidateString(htmlItemPara.ErrorTip);
            }
            return result;
        }

        private static string _GetDateItemString(HtmlItemPara htmlItemPara)
        {
            var dateValue = DateTime.Parse(htmlItemPara.ItemValue.ToString()).ToString("yyyy-MM-dd");
            var result = "";
            var defaultString =
                "<input type = 'date' class='form-control " + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' value='" + dateValue + "' required />";
            if (!String.IsNullOrEmpty(htmlItemPara.DescValue))
            {
                var descName = htmlItemPara.ItemName + "Help";
                defaultString =
                "<input type='date' class='form-control " + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' aria-describedby='" + descName + "' value='" + dateValue + "' required />" +
                "<small id='" + descName + "' class='form-text text-muted'>" + htmlItemPara.DescValue + "</small>";
            }
            result = _GetModalDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetModalValidateString(htmlItemPara.ErrorTip);
            return result;
        }

        private static string _GetDateTimeItemString(HtmlItemPara htmlItemPara)
        {
            var dateTimeValue = DateTime.Parse(htmlItemPara.ItemValue.ToString()).ToString("yyyy-MM-ddThh:mm");
            var result = "";
            var defaultString =
                "<input type = 'datetime-local' class='form-control " + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' value='" + dateTimeValue + "' required />";
            if (!String.IsNullOrEmpty(htmlItemPara.DescValue))
            {
                var descName = htmlItemPara.ItemName + "Help";
                defaultString =
                "<input type='datetime-local' class='form-control " + htmlItemPara.Errorclass + "' id='" + htmlItemPara.ItemName + "' name='" + htmlItemPara.ItemName + "' placeholder='" + htmlItemPara.PlaceholderString + "' aria-describedby='" + descName + "' value='" + dateTimeValue + "' required />" +
                "<small id='" + descName + "' class='form-text text-muted'>" + htmlItemPara.DescValue + "</small>";
            }
            result = _GetModalDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetModalValidateString(htmlItemPara.ErrorTip);
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
                    "    <div class='text-info text-left' style='font-size:11px;margin-left:4px'>联系地址</div>  " +
                    "    <div class='col-12 col-md-12 col-sm-12'> 运行时错误，未对地址变量 AddressVM 进行初始化。" +
                    "    </div>   " +
                    "</div>";
            }
            else
            {
                result =
                    "<div class='form-group form-row' style='margin-top:-18px'>    " +
                    "    <div class='text-info text-left' style='font-size:11px;margin-left:4px'>联系地址</div>  " +
                    "    <div class='col-12 col-md-12 col-sm-12'>" +
                    "        <div class='row'>" +
                    "            <input type='hidden' id='AddressVM.Id' name='AddressVM.Id' value='" + addressVM.Id + "' />" +
                    "            <input type='hidden' id='AddressVM.Name' name='AddressVM.Name' value='" + addressVM.Name + "'>" +
                    "            <input type='hidden' id='AddressVM_ProvinceName' name='AddressVM_ProvinceName' value='" + addressVM.ProvinceName + "' />" +
                    "            <input type='hidden' id='AddressVM_CityName' name='AddressVM_CityName' value='" + addressVM.CityName + "' />" +
                    "            <input type='hidden' id='AddressVM_CountyName' name='AddressVM_CountyName' value='" + addressVM.CountyName + "' />" +
                    "            <div class='col-sm' style='padding-right:1px'>" +
                    "                <select id = 'AddressVM.ProvinceName' name='AddressVM.ProvinceName' class='form-control'>" +  
                    "                </select> " +
                    "            </div>" +
                    "            <div class='col-sm' style='padding-left:1px;padding-right:1px'>" +
                    "                <select id = 'AddressVM.CityName' name='AddressVM.CityName' value='" + addressVM.CityName + "' class='form-control'></select> " +
                    "            </div>" +
                    "            <div class='col-sm' style='padding-left:1px'>" +
                    "                <select id = 'AddressVM.CountyName' name='AddressVM.CountyName' value='" + addressVM.CountyName + "' class='form-control'></select> " +
                    "            </div>" +
                    "        </div>" +
                    "        <div class='row' style='margin-top:5px'>" +
                    "            <div class='col-9' style='padding-right:1px'>" +
                    "                <input type='text' class='form-control ' id='AddressVM.DetailName' name='AddressVM.DetailName' placeholder='详细地址' value='" + addressVM.DetailName + "' required='' />" +
                    "            </div>" +
                    "            <div class='col-3' style='padding-left:1px'>" +
                    "                <input type='text' class='form-control ' id='AddressVM.SortCode' name='SortCode' placeholder='邮编' value='" + addressVM.SortCode + "' required='' />" +
                    "            </div>" +
                    "        </div>" +
                    "    </div>   " +
                    "    <small class='form-text text-danger text-right' style='font-size:11px'>" + htmlItemPara.ErrorTip + "</small> " +
                    "</div>";
            }
            return result;
        }

        #endregion

        /// <summary>
        /// 处理显示名称所需要的 html 字符串
        /// </summary>
        /// <param name="itemDisplay"></param>
        /// <returns></returns>
        private static string _GetModalDispalyNameString(string itemDisplay)
        {
            return
                "<div class='form-group form-row' style='margin-top:-13px'>" +
                "    <div class='text-info text-left' style='font-size:11px;margin-left:4px'>"+itemDisplay+"</div> "+
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

        private static string _GetSelectWithSelfReferentialItemString(HtmlItemPara htmlItemPara, List<SelfReferentialItem> optionItems)
        {
            var result = "";
            if (optionItems == null)
            {
                result = _GetModalDispalyNameString(htmlItemPara.ItemDisplay) + "<div class='text-danger' style='margin-top:7px'>设计时错误：没有初始化供选择的数据。</div>" + _GetModalValidateString(htmlItemPara.ErrorTip);
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
                result = _GetModalDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetModalValidateString(htmlItemPara.ErrorTip);
            }
            return result;
        }


        private static List<PlainFacadeItem> _GetSelectOptionsWithEnumItem(string itemName, object boVM)
        {
            List<PlainFacadeItem> resultItems = null;
            PropertyInfo[] properties = boVM.GetType().GetProperties();

            // 提取关联数据集合的属性名称
            var relevancePropertyName = itemName + "ItemCollection";
            // 提取关联数据集合的属性
            var relevanceProperty = properties.FirstOrDefault(x => x.Name == relevancePropertyName);
            // 提取关联数据集合
            if (relevanceProperty != null)
            {
                resultItems = relevanceProperty.GetValue(boVM) as List<PlainFacadeItem>;
            }

            return resultItems;
        }

        private static string _GetMultiSelectWithPlainFacadeItemString(HtmlItemPara htmlItemPara, List<PlainFacadeItem> optionItems)
        {
            var result = "";
            if (optionItems == null)
            {
                result = _GetModalDispalyNameString(htmlItemPara.ItemDisplay) + "<div class='text-danger' style='margin-top:7px'>设计时错误：没有初始化供选择的数据。</div>" + _GetModalValidateString(htmlItemPara.ErrorTip);
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
                result = _GetModalDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetModalValidateString(htmlItemPara.ErrorTip);
            }
            return result;
        }


        private static string _GetSelectWithPlainFacadeItemString(HtmlItemPara htmlItemPara, List<PlainFacadeItem> optionItems)
        {
            var result = "";
            if (optionItems == null)
            {
                result = _GetModalDispalyNameString(htmlItemPara.ItemDisplay) + "<div class='text-danger' style='margin-top:7px'>设计时错误：没有初始化供选择的数据。</div>" + _GetModalValidateString(htmlItemPara.ErrorTip);
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

                result = _GetModalDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetModalValidateString(htmlItemPara.ErrorTip);
            }
            return result;
        }

        private static string _GetMultiCheckWithPlainFacadeItemString(HtmlItemPara htmlItemPara, List<PlainFacadeItem> optionItems)
        {
            var result = "";
            if (optionItems == null)
            {
                result = _GetModalDispalyNameString(htmlItemPara.ItemDisplay) + "<div class='text-danger' style='margin-top:7px'>设计时错误：没有初始化供选择的数据。</div>" + _GetModalValidateString(htmlItemPara.ErrorTip);
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

                result = _GetModalDispalyNameString(htmlItemPara.ItemDisplay) + checkString + _GetModalValidateString(htmlItemPara.ErrorTip);
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
            result = _GetModalDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetModalValidateString(htmlItemPara.ErrorTip);
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
            result = _GetModalDispalyNameString(htmlItemPara.ItemDisplay) + defaultString + _GetModalValidateString(htmlItemPara.ErrorTip);
            return result;
        }


        private static HtmlItemPara _GetHtmlItemPara(IHtmlHelper helper, CreateOrEditItem item, object boVM)
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

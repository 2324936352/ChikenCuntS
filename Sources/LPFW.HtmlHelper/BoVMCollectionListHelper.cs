using LPFW.ViewModels.ControlModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LPFW.HtmlHelper
{
    /// <summary>
    /// 常规视图模型列表显示的 MVC HTML 代码助理
    /// </summary>
    public static class BoVMCollectionListHelper
    {
        /// <summary>
        /// 常规的标题行，以及查询、新建操作
        /// </summary>
        /// <param name="helpe"></param>
        /// <param name="titleName"></param>
        /// <param name="creatOrEditUrlString"></param>
        /// <param name="searchWithKeywordUrlString"></param>
        /// <returns></returns>
        public static HtmlString CommonListCardHeader(this IHtmlHelper helper, string titleName, string creatOrEditUrlString, string searchWithKeywordUrlString)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append(
                "<div class='card-header'>" + //  style='background-color:whitesmoke; border-bottom-color:darkgray;border-bottom-width:1px;border-bottom-style:solid'
                "<h4 style = 'font-size:22px;font-weight:normal;color:dimgray'>" + titleName + "</h4>" +
                "<div class='card-header-form'>" +
                "<div class='input-group'>" +
                "<input id='keyword' type = 'text' class='form-control' placeholder='请输入关键词...'>" +
                "<div class='input-group-btn'>" +
                "<button class='btn btn-info' onclick='appGotoCommonSearchPage($(\"#keyword\").val(),\"" + searchWithKeywordUrlString + "\",\"mainWorkPlaceArea\")'><i class='fas fa-search'></i></button>" +
                "</div>" +
                "<button class='btn btn-info' onclick='appGotoCreateOrEditWithPartialView(\"" + Guid.NewGuid() + "\",\"" + creatOrEditUrlString + "\",\"mainWorkPlaceArea\")'><i class='fas fa-plus'></i> 新建数据</button>" +
                "</div>" +
                "</div>" +
                "</div>");

            return new HtmlString(htmlContent.ToString());
        }

        /// <summary>
        /// 常规的标题行，以及查询、新建操作
        /// </summary>
        /// <param name="helpe"></param>
        /// <param name="titleName"></param>
        /// <param name="creatOrEditUrlString"></param>
        /// <param name="searchWithKeywordUrlString"></param>
        /// <returns></returns>
        public static HtmlString CommonListNoCreateCardHeader(this IHtmlHelper helper, string titleName, string searchWithKeywordUrlString)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append(
                "<div class='card-header'>" + //  style='background-color:whitesmoke; border-bottom-color:darkgray;border-bottom-width:1px;border-bottom-style:solid'
                "<h4 style = 'font-size:22px;font-weight:normal;color:dimgray'>" + titleName + "</h4>" +
                "<div class='card-header-form'>" +
                "<div class='input-group'>" +
                "<input id='keyword' type = 'text' class='form-control' placeholder='请输入关键词...'>" +
                "<div class='input-group-btn'>" +
                "<button class='btn btn-info' onclick='appGotoCommonSearchPage($(\"#keyword\").val(),\"" + searchWithKeywordUrlString + "\",\"mainWorkPlaceArea\")'><i class='fas fa-search'></i></button>" +
                "</div>" +
                "</div>" +
                "</div>" +
                "</div>");

            return new HtmlString(htmlContent.ToString());
        }

        public static HtmlString CommonListByTypeCardHeader(this IHtmlHelper helper, string titleName, string creatOrEditUrlString, string searchWithKeywordUrlString)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append(
                "<div class='card-header'>" + //  style='background-color:whitesmoke; border-bottom-color:darkgray;border-bottom-width:1px;border-bottom-style:solid'
                "<h4 style = 'font-size:22px;font-weight:normal;color:dimgray'>" + titleName + "</h4>" +
                "<div class='card-header-form'>" +
                "<div class='input-group'>" +
                "<input id='keyword' type = 'text' class='form-control' placeholder='请输入关键词...'>" +
                "<div class='input-group-btn'>" +
                "<button class='btn btn-info' onclick='appGotoCommonSearchPage($(\"#keyword\").val(),\"" + searchWithKeywordUrlString + "\",\"mainWorkPlaceArea\")'><i class='fas fa-search'></i></button>" +
                "</div>" +
                "<button class='btn btn-info' onclick='appGotoCreateOrEditByTypeIdWithPartialView(\"" + Guid.NewGuid() + "\",\"" + creatOrEditUrlString + "\",\"mainWorkPlaceArea\")'><i class='fas fa-plus'></i> 新建数据</button>" +
                "</div>" +
                "</div>" +
                "</div>");

            return new HtmlString(htmlContent.ToString());
        }

        /// <summary>
        /// 使用模态框作为查询、新建操作导航的标题行
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="titleName"></param>
        /// <param name="creatOrEditUrlString"></param>
        /// <param name="searchWithKeywordUrlString"></param>
        /// <param name="modalId"></param>
        /// <param name="modalContentId"></param>
        /// <returns></returns>
        public static HtmlString CommonListWithModalCardHeader(this IHtmlHelper helper, string titleName, string creatOrEditUrlString, string searchWithKeywordUrlString, string modalId,string modalContentId)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append(
                "<div class='card-header'>" +
                "<h4 style = 'font-size:22px;font-weight:normal;color:dimgray'>" + titleName + "</h4>" +
                "<div class='card-header-form'>" +
                "<div class='input-group'>" +
                "<input id='keyword' type = 'text' class='form-control' placeholder='请输入关键词...'>" +
                "<div class='input-group-btn'>" +
                "<button class='btn btn-info' onclick='appGotoCommonSearchPage($(\"#keyword\").val(),\"" + searchWithKeywordUrlString + "\",\"mainWorkPlaceArea\")'><i class='fas fa-search'></i></button>" +
                "</div>" +
                "<button class='btn btn-info' onclick='appGotoCreateOrEditWithModal(\"" + Guid.NewGuid() + "\",\"" + creatOrEditUrlString + "\",\""+modalId+"\",\""+modalContentId+"\")'><i class='fas fa-plus'></i> 新建数据</button>" +
                "</div>" +
                "</div>" +
                "</div>");

            return new HtmlString(htmlContent.ToString());
        }

        public static HtmlString CommonListByTypeWithModalCardHeader(this IHtmlHelper helper, string titleName, string creatOrEditUrlString, string searchWithKeywordUrlString, string modalId, string modalContentId)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append(
                "<div class='card-header'>" +
                "<h4 style = 'font-size:22px;font-weight:normal;color:dimgray'>" + titleName + "</h4>" +
                "<div class='card-header-form'>" +
                "<div class='input-group'>" +
                "<input id='keyword' type = 'text' class='form-control' placeholder='请输入关键词...'>" +
                "<div class='input-group-btn'>" +
                "<button class='btn btn-info' onclick='appGotoCommonSearchPage($(\"#keyword\").val(),\"" + searchWithKeywordUrlString + "\",\"mainWorkPlaceArea\")'><i class='fas fa-search'></i></button>" +
                "</div>" +
                "<button class='btn btn-info' onclick='appGotoCreateOrEditByTypeWithModal(\"" + Guid.NewGuid() + "\",\"" + creatOrEditUrlString + "\",\"" + modalId + "\",\"" + modalContentId + "\")'><i class='fas fa-plus'></i> 新建数据</button>" +
                "</div>" +
                "</div>" +
                "</div>");

            return new HtmlString(htmlContent.ToString());
        }

        /// <summary>
        /// 使用分页器的标题行，以及查询、新建操作
        /// </summary>
        /// <param name="helpe"></param>
        /// <param name="titleName"></param>
        /// <param name="creatOrEditUrlString"></param>
        /// <param name="searchWithKeywordUrlString"></param>
        /// <returns></returns>
        public static HtmlString PaginateListCardHeader(this IHtmlHelper helper,string titleName,string creatOrEditUrlString, string searchWithKeywordUrlString)
        {
            var htmlContent = new StringBuilder();
            var createOrEditButton = "<button class='btn btn-info' onclick='appGotoCreateOrEditWithPartialView(\"" + Guid.NewGuid() + "\",\"" + creatOrEditUrlString + "\",\"mainWorkPlaceArea\")'><i class='fas fa-plus'></i> 新建数据</button>";
            if (String.IsNullOrEmpty(creatOrEditUrlString))
                createOrEditButton = "";

            htmlContent.Append(
                "<div class='card-header'>" +
                "<h4 style = 'font-size:22px;font-weight:normal;color:dimgray'>" + titleName + "</h4>" +
                "<div class='card-header-form'>" +
                "<div class='input-group'>" +
                "<input id='keyword' type = 'text' class='form-control' placeholder='请输入关键词...'>" +
                "<div class='input-group-btn'>" +
                "<button class='btn btn-info' onclick='appGotoSearchPaginatePage($(\"#keyword\").val(),\"" + searchWithKeywordUrlString + "\",\"mainWorkPlaceArea\")'><i class='fas fa-search'></i></button>" +
                "</div>" + createOrEditButton+
                "</div>" +
                "</div>" +
                "</div>");

            return new HtmlString(htmlContent.ToString());
        }

        public static HtmlString PaginateListWithoutCreateCardHeader(this IHtmlHelper helper, string titleName, string creatOrEditUrlString, string searchWithKeywordUrlString)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append(
                "<div class='card-header'>" +
                "<h4 style = 'font-size:22px;font-weight:normal;color:dimgray'>" + titleName + "</h4>" +
                "<div class='card-header-form'>" +
                "<div class='input-group'>" +
                "<input id='keyword' type = 'text' class='form-control' placeholder='请输入关键词...'>" +
                "<div class='input-group-btn'>" +
                "<button class='btn btn-info' onclick='appGotoSearchPaginatePage($(\"#keyword\").val(),\"" + searchWithKeywordUrlString + "\",\"mainWorkPlaceArea\")'><i class='fas fa-search'></i></button>" +
                "</div>" +
                // "<button class='btn btn-info' onclick='appGotoCreateOrEditWithPartialView(\"" + Guid.NewGuid() + "\",\"" + creatOrEditUrlString + "\",\"mainWorkPlaceArea\")'><i class='fas fa-plus'></i> 新建数据</button>" +
                "</div>" +
                "</div>" +
                "</div>");

            return new HtmlString(htmlContent.ToString());
        }

        public static HtmlString PaginateListByTypeCardHeader(this IHtmlHelper helper, string titleName, string creatOrEditUrlString, string searchWithKeywordUrlString)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append(
                "<div class='card-header'>" +
                "<h4 style = 'font-size:22px;font-weight:normal;color:dimgray'>" + titleName + "</h4>" +
                "<div class='card-header-form'>" +
                "<div class='input-group'>" +
                "<input id='keyword' type = 'text' class='form-control' placeholder='请输入关键词...'>" +
                "<div class='input-group-btn'>" +
                "<button class='btn btn-info' onclick='appGotoSearchPaginatePage($(\"#keyword\").val(),\"" + searchWithKeywordUrlString + "\",\"mainWorkPlaceArea\")'><i class='fas fa-search'></i></button>" +
                "</div>" +
                "<button class='btn btn-info' onclick='appGotoCreateOrEditByTypeIdWithPartialView(\"" + Guid.NewGuid() + "\",\"" + creatOrEditUrlString + "\",\"mainWorkPlaceArea\")'><i class='fas fa-plus'></i> 新建数据</button>" +
                "</div>" +
                "</div>" +
                "</div>");

            return new HtmlString(htmlContent.ToString());
        }

        public static HtmlString PaginateListWithModalCardHeader(this IHtmlHelper helper, string titleName, string creatOrEditUrlString, string searchWithKeywordUrlString, string modalId,string modalContentId)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append(
                "<div class='card-header'>" +
                "<h4 style = 'font-size:22px;font-weight:normal;color:dimgray'>" + titleName + "</h4>" +
                "<div class='card-header-form'>" +
                "<div class='input-group'>" +
                "<input id='keyword' type = 'text' class='form-control' placeholder='请输入关键词...'>" +
                "<div class='input-group-btn'>" +
                "<button class='btn btn-info' onclick='appGotoSearchPaginatePage($(\"#keyword\").val(),\"" + searchWithKeywordUrlString + "\",\"mainWorkPlaceArea\")'><i class='fas fa-search'></i></button>" +
                "</div>" +
                //"<button class='btn btn-info' onclick='appGotoCreateOrEditWithPartialView(\"" + Guid.NewGuid() + "\",\"" + creatOrEditUrlString + "\",\"mainWorkPlaceArea\")'><i class='fas fa-plus'></i> 新建数据</button>" +
                "<button class='btn btn-info' onclick='appGotoCreateOrEditWithModal(\"" + Guid.NewGuid() + "\",\"" + creatOrEditUrlString + "\",\"" + modalId + "\",\"" + modalContentId + "\")'><i class='fas fa-plus'></i> 新建数据</button>" +
                "</div>" +
                "</div>" +
                "</div>");

            return new HtmlString(htmlContent.ToString());
        }

        /// <summary>
        /// 表头列
        /// </summary>
        /// <param name="helpe"></param>
        /// <param name="titleName"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static HtmlString ListTableHeader(this IHtmlHelper helper, string titleName,int width)
        {
            var widthString = "";
            if (width > 0)
                widthString = "width='" + width.ToString() + "px'";

            var htmlContent = new StringBuilder();
            htmlContent.Append("<th style='padding-bottom:10px; border-color:darkgray;border-bottom-style: solid;border-bottom-width:2px;border-bottom-color:darkslategray;border-top-style:solid;border-top-width:2px;border-top-color:darkslategray'" + widthString + ">" + titleName + "</th>");

            return new HtmlString(htmlContent.ToString());
        }

        /// <summary>
        /// 根据表头列的元素集合生成列表表头
        /// </summary>
        /// <param name="helpe"></param>
        /// <returns></returns>
        public static HtmlString ListTableHeaderForListItems(this IHtmlHelper helper, List<TableListItem> listItems, string defaultUrlString, string mainWorkPlaceAreaId)
        {
            var htmlContent = new StringBuilder();
            foreach (var item in listItems)
            {
                var widthString = "";
                // 处理宽度数据
                if (item.Width > 0)
                    widthString = "width='" + item.Width.ToString() + "px'";

                if (item.IsSort)
                {
                    var sortString = "";
                    // 处理排序，未排序使用 fa-sort，升序使用 fa-sort-down，降序使用 fa-sort-up
                    switch (item.SortDesc.ToLower())
                    {
                        case "":
                            sortString = "<a class='float-right text-info'  href='javascript:void(0)' onclick='appGotoCommonSortPage(\"" + item.PropertyName + "\",\"Ascend\",\"" + defaultUrlString + "\",\"" + mainWorkPlaceAreaId + "\")'><i class='fa fa-sort' aria-hidden='true'></i></a>";
                            break;
                        case "ascend":
                            sortString = "<a class='float-right text-info'  href='javascript:void(0)' onclick='appGotoCommonSortPage(\"" + item.PropertyName + "\",\"Descend\",\"" + defaultUrlString + "\",\"" + mainWorkPlaceAreaId + "\")'><i class='fa fa-sort-down' aria-hidden='true'></i></a>";
                            break;
                        case "descend":
                            sortString = "<a class='float-right text-info'  href='javascript:void(0)' onclick='appGotoCommonSortPage(\"" + item.PropertyName + "\",\"Ascend\",\"" + defaultUrlString + "\",\"" + mainWorkPlaceAreaId + "\")'><i class='fa fa-sort-up' aria-hidden='true'></i></a>";
                            break;
                        default:
                            break;
                    }

                    htmlContent.Append("<th style='padding-bottom:10px; border-color:darkgray; border-bottom-style: solid;border-bottom-width:2px;border-bottom-color:darkslategray;border-top-style:solid;border-top-width:2px;border-top-color:darkslategray' " + widthString + ">" + item.DsipalyName +
                        sortString +
                        "</th>");
                }
                else
                    htmlContent.Append("<th style='padding-bottom:10px; border-color:darkgray;border-bottom-style: solid;border-bottom-width:2px;border-bottom-color:darkslategray;border-top-style:solid;border-top-width:2px;border-top-color:darkslategray' " + widthString + ">" + item.DsipalyName + "</th>");
            }

            return new HtmlString(htmlContent.ToString());
        }

        /// <summary>
        /// 列表的 <td></td> 数据
        /// </summary>
        /// <param name="helpe"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static HtmlString ListTableData(this IHtmlHelper helper, string data)
        {
            var htmlContent = new StringBuilder();
            htmlContent.Append("<td style='background-color:white;border-color:darkgray'>" + data + "</td>");

            return new HtmlString(htmlContent.ToString());
        }

        /// <summary>
        /// 处理列表的 <td></td> 数据
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static HtmlString ListTableDataForListItems(this IHtmlHelper helper,List<TableListItem> listItems, object data)
        {
            var htmlContent = new StringBuilder();
            PropertyInfo[] properties = data.GetType().GetProperties();

            foreach (var item in listItems)
            {
                var property = properties.FirstOrDefault(x => x.Name == item.PropertyName);
                var valueString = "";
                if (property != null)
                {
                    var itemObject = property.GetValue(data);
                    if (itemObject != null)
                    {
                        switch (item.DataType)
                        {
                            case ViewModelDataType.日期:
                                var dateObj = (DateTime)itemObject;
                                valueString = dateObj.ToString("yyyy年MM月dd日");
                                break;
                            case ViewModelDataType.日期时间:
                                var dateTimeObj = (DateTime)itemObject;
                                valueString = dateTimeObj.ToString("yyyy年MM月dd日 HH:mm:ss");
                                break;
                            case ViewModelDataType.性别:
                                if (itemObject.ToString().ToLower() == "true")
                                    valueString = "女";
                                else
                                    valueString = "男";
                                break;
                            case ViewModelDataType.是否:
                                if (itemObject.ToString().ToLower() == "true")
                                    valueString = "是";
                                else
                                    valueString = "否";
                                break;
                            case ViewModelDataType.图标:
                                valueString = "<i class=\"" + itemObject.ToString() + "\"></i>";
                                break;
                            case ViewModelDataType.货币:
                                valueString = string.Format("{0:C}", itemObject);
                                break;
                            default:
                                valueString = itemObject.ToString();
                                break;
                        }
                    }
                }

                if(item.DataType== ViewModelDataType.货币) // 右对齐处理
                    htmlContent.Append("<td style='background-color:white;border-color:darkgray'><div style='margin-top:3px' class='text-right'>" + valueString + "</div></td>");
                else
                    htmlContent.Append("<td style='background-color:white;border-color:darkgray'><div style='margin-top:3px'>" + valueString + "</div></td>");
            }
            return new HtmlString(htmlContent.ToString());
        }

        /// <summary>
        /// 列表附加的空行
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="rowAmount"></param>
        /// <param name="colAmount"></param>
        /// <returns></returns>
        public static HtmlString SetAdditionalRowForTable(this IHtmlHelper helper, int rowAmount, int colAmount)
        {
            var htmlContent = new StringBuilder();
            for (int i = 0; i < rowAmount; i++)
            {
                htmlContent.Append("<tr style='background-color:white'>");
                for (int j = 0; j < colAmount; j++)
                {
                    if (j == 0)
                        htmlContent.Append("<td style='background-color:white;border-color:darkgray'>　</td>");
                    else
                        htmlContent.Append("<td style='background-color:white;border-color:darkgray'> </td>");
                }
                htmlContent.Append("<tr>");
            }
            return new HtmlString(htmlContent.ToString());
        }
    }
}

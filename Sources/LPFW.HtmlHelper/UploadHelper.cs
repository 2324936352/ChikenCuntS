using LPFW.ViewModels.Common;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.HtmlHelper
{
    /// <summary>
    /// 文件上传 Html 助理
    /// </summary>
    public static class UploadHelper
    {
        /// <summary>
        /// 创建普通文件上传的 Html 代码 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id">文件归属数据对象 Id</param>
        /// <param name="fileVMCollection">文件视图模型集合</param>
        /// <returns></returns>
        public static HtmlString UploadBusinessFileCollection(this IHtmlHelper helper, Guid id,List<BusinessFileVM> fileVMCollection)
        {
            string htmlString =
                "<div class='form-group form-row' style='margin-top:-18px'> " +
                "    <div class='col-12 col-md-12 col-sm-12'>" +
                "        <div class='card'>" +
                "            <div style='padding:7px;padding-bottom:2px;border-bottom-color:lightgray;border-bottom-style:dotted;border-bottom-width:1px'>" +
                "                <span style='font-size:16px;font-weight:bold;color:dimgray'>普通上传文件管理</span> " +
                "                <div class='float-right'>" +
                "                    <a href='javascript:void(0)' class='btn btn-outline-info btn-sm' onclick='appOpenCommonBusinessFileCollectionSelectorModal(\"" + id.ToString() + "\")'><i class='fa fa-upload'></i> 上传文件</a>" +
                "                </div>" +
                "            </div>" +
                "            <div class='card-body' style='padding:3px' id='commonBusinessFileCollectionArea'>" +
                             StringHelperForUpload.UploadFileListTable(fileVMCollection) +
                "            </div>" +
                "        </div>" +
                "    </div>  " +
                "</div>";
            return new HtmlString(htmlString);
        }


        /// <summary>
        /// 创建普通图片文件上传的 Html 代码 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id"></param>
        /// <param name="imageVMCollection"></param>
        /// <returns></returns>
        public static HtmlString UploadBusinessImageCollection(this IHtmlHelper helper, Guid id, List<BusinessImageVM> imageVMCollection)
        {
            string htmlString =
                "<div class='form-group form-row' style='margin-top:-18px'> " +
                "    <div class='col-12 col-md-12 col-sm-12'>" +
                "        <div class='card'>" +
                "            <div style='padding:7px;padding-bottom:2px;border-bottom-color:lightgray;border-bottom-style:dotted;border-bottom-width:1px'>" +
                "                <span style='font-size:16px;font-weight:bold;color:dimgray'>图片文件上传管理</span> " +
                "                <div class='float-right'>" +
                "                    <a href='javascript:void(0)' class='btn btn-outline-danger btn-sm' onclick='appDeleteBusinessImageCollection(\""+id.ToString()+"\")'><i class='fa fa-times'></i> 删除勾选的图片</a>" +
                "                    <a href='javascript:void(0)' class='btn btn-outline-info btn-sm' onclick='appOpenCommonBusinessImageCollectionSelectorModal(\"" + id.ToString() + "\")'><i class='fa fa-upload'></i> 上传图片</a>"+
                "                </div>" +
                "            </div>" +
                "            <div class='card-body' style='padding:3px'><div class='row gutters-sm' id='commonImageFileCollectionArea'>" +
                             StringHelperForUpload.UploadImageList(imageVMCollection) +
                "            </div></div>" +
                "        </div>" +
                "    </div>  " +
                "</div>";
            return new HtmlString(htmlString);
        }

        public static HtmlString UploadAvatar(this IHtmlHelper helper, Guid id, BusinessImageVM imageVM)
        {
            var avatarPath = "/images/avatar/avatar-1.png";
            if (imageVM != null)
                if (!String.IsNullOrEmpty(imageVM.FilePath))
                    avatarPath = imageVM.FilePath;

            string htmlString =
                "<div class='form-group form-row' style='margin-top:-18px'>" +
                "    <div class='col-12 col-md-12 col-sm-12'>" +
                "        <a href='javascript:void(0)' onclick='appOpenCommonAvatarSelectorModal(\"" + id.ToString() + "\")'>" +
                "            <figure class='avatar mr-2 avatar-xl'>" +
                "                <img id='AvartarImage' src='" + avatarPath + "' alt='...'>" +
                "            </figure>" +
                "        </a>" +
                "    </div>" +
                "</div>";
            return new HtmlString(htmlString);
        }

        public static HtmlString UploadVideoAndCover(this IHtmlHelper helper, BusinessVideoVM videoVM, BusinessImageVM imageVM)
        {
            var result = "" +
                "<div class='form-group form-row' style='margin-top:-18px'>" +
                "    <div class='col-12 col-md-12 col-sm-12'>" +
                "        <div class='row'>" +
                "            <div class='col-12 col-md-6 col-sm-6'>" + StringHelperForUpload.UploadImage(imageVM) +
                "            </div>" +
                "            <div class='col-12 col-md-6 col-sm-6'>" + StringHelperForUpload.UploadVideo(videoVM) +
                "            </div>" +
                "        </div>" +
                "    </div>" +
                "</div>";

            return new HtmlString(result);

        }

    }
}

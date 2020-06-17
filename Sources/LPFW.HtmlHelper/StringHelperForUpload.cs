using LPFW.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.HtmlHelper
{
    /// <summary>
    /// 构建前端使用的 Html 字符串助理
    /// </summary>
    public static class StringHelperForUpload
    {
        /// <summary>
        /// 根据上传文件视图模型集合，创建列表 html 字符串
        /// </summary>
        /// <param name="fileVMCollection"></param>
        /// <returns></returns>
        public static string UploadFileListTable(List<BusinessFileVM> fileVMCollection)
        {
            var trString = "";
            var counter = 0;
            foreach (var item in fileVMCollection)
            {
                trString = trString +
                    "<tr>" +
                    "    <td width='50'>" + (++counter).ToString() + "</td>" +
                    "    <td>" + item.FileDisplayName + "</td>" +
                    "    <td width='110'>" +
                    "        <a href='" + item.FilePath + "' target='_blank'> <i class='fas fa-download text-info'></i> 下载</a> " +
                    "        <a href='javascript:void(0);' onclick='appDeleteBusinessFile(\"" + item.Id + "\",\"" + item.ObjectId + "\")'><i class='fas fa-times text-warning'></i> 删除</a>" +
                    "    </td>" +
                    "</tr>";
            }

            var result ="<table class='table table-bordered table-sm'>" + trString + "</table>";
            return result;
        }

        public static string UploadImageList(List<BusinessImageVM> imageVMCollection)
        {
            var result = "";
            foreach (var item in imageVMCollection)
            {
                result = result +
                    "<div class='col-6 col-sm-2'>" +
                    "    <label class='imagecheck mb-2'>" +
                    "        <input name='imageVMCollectionName' type='checkbox' value='" + item.Id.ToString() + "' class='imagecheck-input' />" +
                    "        <figure class='imagecheck-figure'>" +
                    "            <img src='" + item.FilePath + "' alt='}' class='imagecheck-image'>" +
                    "        </figure>" +
                    "    </label>" +
                    "</div>";
            }

            return result;
        }

        public static string UploadImage(BusinessImageVM imageVM)
        {
            var imagePath = "/images/empty_16_9.jpg";
            if (!String.IsNullOrEmpty(imageVM.FilePath))
                imagePath = imageVM.FilePath;

            var result = "" +
                "<div class='card'>" +
                "    <div class='card-body' style='padding:0px'>" +
                "        <img id='commonBusinessVideoCover' name='commonBusinessVideoCover' src='" + imagePath + "' controls style='clear:both;display:block;margin:auto; width:100%; height:auto; object-fit:fill' />" +
                "    </div>" +
                "    <div class='card-footer' style='padding:10px;border-top-color:lightgray;border-top-style:dotted;border-top-width:1px'>" +
                "        <div class='float-right'>" +
                "            <a href='javascript:void(0)' onclick='appDeleteCommonBusinessVideoCover(\"" + imageVM.ObjectId + "\")'  class='btn btn-outline-danger btn-sm'>清除标题图片</a>" +
                "            <a href='javascript:void(0)' onclick='appOpenCommonBusinessVideoCoverSelectorModal(\"" + imageVM.ObjectId + "\")'  class='btn btn-outline-info btn-sm'>上传标题图片</a>" +
                "        </div>" +
                "    </div>" +
                "</div>";
            return result;
        }

        public static string UploadVideo(BusinessVideoVM videoVM)
        {
            var videoPath = "";
            if (!String.IsNullOrEmpty(videoVM.FilePath))
                videoPath = videoVM.FilePath;

            var result = "" +
                "<div class='card'>" +
                "    <div class='card-body' style='padding:0px'>" +
                "        <video id='commonBusinessVideo' name='commonBusinessVideo' src='"+videoPath+"' controls style='clear:both;display:block;margin:auto; width:100%; height:100%; object-fit:fill'></video>" +
                "    </div>" +
                "    <div class='card-footer' style='padding:10px;border-top-color:lightgray;border-top-style:dotted;border-top-width:1px'>" +
                "        <div class='float-right'>" +
                "            <a href='javascript:void(0)' onclick='appDeleteCommonBusinessVideo(\"" + videoVM.ObjectId + "\")' class='btn btn-outline-danger btn-sm' id='commonBusinessVideoDelete'>删除视频文件</a>" +
                "            <a href='javascript:void(0)' onclick='appOpenCommonBusinessVideoSelectorModal(\"" + videoVM.ObjectId + "\")' class='btn btn-outline-info btn-sm'>上传视频文件</a>" +
                "        </div>" +
                "    </div>" +
                "</div>";
            return result;
        }
    }
}

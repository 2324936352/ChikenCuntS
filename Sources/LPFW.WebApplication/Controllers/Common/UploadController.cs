using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LPFW.DataAccess;
using LPFW.EntitiyModels.ApplicationCommon.Attachments;
using LPFW.HtmlHelper;
using LPFW.ViewModels.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace LPFW.WebApplication.Controllers.Common
{
    /// <summary>
    /// 常规的上传文件的控制器，在这个框架中，所有的上传相关的操作，都是用这个控制器进行处理。
    ///   在服务器端创建 uploadFiles 文件夹，其中包含 avatar、commonFiles、images、videoes 四个文件夹，
    ///   分别存储头像、普通文件、图片和视频文件。
    /// 前端使用 Bootstrap Fileinput 插件
    /// </summary>
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment _HostingEnvironment;  // 系统驻留环境

        private readonly IEntityRepository<BusinessFile> _businessFileRepository;
        private readonly IEntityRepository<BusinessImage> _businessImageRepository;
        private readonly IEntityRepository<BusinessVideo> _businessVideoRepository;

        public UploadController(
            IWebHostEnvironment hostingEnvironment,
            IEntityRepository<BusinessImage> image,
            IEntityRepository<BusinessFile> file,
            IEntityRepository<BusinessVideo> video
            )
        {
            _HostingEnvironment = hostingEnvironment;
            _businessFileRepository = file;
            _businessImageRepository = image;
            _businessVideoRepository = video;
        }

        /// <summary>
        /// 接受上传并保存普通文件
        /// </summary>
        /// <param name="id">图像关联的数据元素对象的 id</param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> FileCollectionSave(string id) 
        {
            // 读取提交的上传文件表单中的文件
            var files= Request.Form.Files;
            // 获取文件的字节数
            long size = files.Sum(f => f.Length);
            // 获取服务器的根目录的绝对地址
            string webRootPath = _HostingEnvironment.WebRootPath;
            // 获取文件包含应用系统文件的绝对地址
            string contentRootPath = _HostingEnvironment.ContentRootPath;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // 获取文件的扩展名，扩展名不含 "."
                    string fileExt = _GetFileSuffix(formFile.FileName);
                    // 获取文件字节数
                    long fileSize = formFile.Length;
                    // 更新存储文件的文件名称，形式为： id_上传的文件物理名称
                    string newFileName = id+"_" + formFile.FileName;
                    // 设置上传文件的存储位置
                    var filePath = webRootPath + "/uploadFiles/commonfiles/"+ newFileName;
                    // 保存文件
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    // 创建文件信息存储对象
                    var fileBo = new BusinessFile
                    {
                        RelevanceObjectID = Guid.Parse(id),
                        Name = formFile.FileName,
                        UploadPath = "/uploadFiles/commonfiles/" + newFileName
                    };

                    // 持久化文件信息存储对象
                    await _businessFileRepository.SaveBoAsyn(fileBo);
                }
            }

            return Ok(new { count = files.Count, size });
        }

        /// <summary>
        /// 根据传入的文件 id ，删除文件记录和相应的物理文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> DeleteBusinessFile(Guid id,Guid relevanceObjectId)
        {
            // 提取文件路径信息
            var file = await _businessFileRepository.GetBoAsyn(id);
            string filePath = _HostingEnvironment.WebRootPath + file.UploadPath;
            
            // 删除文件记录
            await _businessFileRepository.DeleteBoAsyn(id);
            // 删除物理文件
            System.IO.File.Delete(filePath);

            // 更新文件列表数据
            var fileVMCollection = new List<BusinessFileVM>();
            var businessFileCollection = await _businessFileRepository.GetBusinessFileCollectionAsyn(relevanceObjectId);
            foreach (var item in businessFileCollection)
                fileVMCollection.Add(new BusinessFileVM(item));

            return StringHelperForUpload.UploadFileListTable(fileVMCollection);
        }

        /// <summary>
        /// 接受上传并保存图像文件
        /// </summary>
        /// <param name="id">图像关联的数据元素对象的 id</param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> ImageCollectionSave(string id)
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string webRootPath = _HostingEnvironment.WebRootPath;
            string contentRootPath = _HostingEnvironment.ContentRootPath;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {

                    string fileExt = _GetFileSuffix(formFile.FileName);     // 文件扩展名，不含“.”
                    long fileSize = formFile.Length;                        // 获得文件大小，以字节为单位
                    string newFileName = id + "_" + formFile.FileName;      // 生成新的文件名
                    var filePath = webRootPath + "/uploadFiles/images/" + newFileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    var imageBo = new BusinessImage();
                    imageBo.RelevanceObjectID = Guid.Parse(id);
                    imageBo.Name = formFile.FileName;
                    imageBo.UploadPath = "/uploadFiles/images/" + newFileName;
                    imageBo.DisplayName = formFile.FileName;
                    await _businessImageRepository.SaveBoAsyn(imageBo);
                }
            }

            return Ok(new { count = files.Count, size });
        }

        public async Task<string> DeleteBusinessImageCollection(Guid id, string idCollection)
        {
            if (!String.IsNullOrWhiteSpace(idCollection))
            {
                var imageIdCollection = idCollection.Split(',');
                foreach (var item in imageIdCollection)
                {
                    // 提取文件路径信息
                    var file = await _businessImageRepository.GetBoAsyn(Guid.Parse(item));
                    string filePath = _HostingEnvironment.WebRootPath + file.UploadPath;

                    // 删除文件记录
                    await _businessImageRepository.DeleteBoAsyn(Guid.Parse(item));
                    // 删除物理文件
                    System.IO.File.Delete(filePath);

                }
            }

            // 刷新图片列表
            var imageVMCollection = new List<BusinessImageVM>();
            var businessImgageCollection = await _businessImageRepository.GetBusinessImageCollectionAsyn(id);
            foreach (var item in businessImgageCollection)
                imageVMCollection.Add(new BusinessImageVM(item));

            return StringHelperForUpload.UploadImageList(imageVMCollection);

        }

        /// <summary>
        /// 接受上传并保存头像文件
        /// </summary>
        /// <param name="id">头像关联的数据元素对象的 id</param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> AvatarSave(string id)
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string webRootPath = _HostingEnvironment.WebRootPath;
            string contentRootPath = _HostingEnvironment.ContentRootPath;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {

                    string fileExt = _GetFileSuffix(formFile.FileName);     // 文件后缀，不含“.”
                    long fileSize = formFile.Length;                        // 获得文件大小，以字节为单位
                    string newFileName = id + "_" + formFile.FileName;      // 生成新的文件名
                    var filePath = webRootPath + "/uploadFiles/avatars/" + newFileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    // 提取 avatar 图片，并处理存储的唯一性
                    var imageBo = await _businessImageRepository.GetBoAsyn(x => x.RelevanceObjectID == Guid.Parse(id) && x.IsUnique == true && x.IsAvatar == true);
                    if (imageBo == null)
                    {
                        imageBo = new BusinessImage
                        {
                            RelevanceObjectID = Guid.Parse(id),
                            Name = formFile.FileName,
                            Description = "",
                            UploadFileSuffix = fileExt,
                            UploadPath = "/uploadFiles/avatars/" + newFileName,
                            DisplayName = "头像",
                            IsAvatar = true,
                            IsUnique = true
                        };
                    }
                    else
                    {
                        // 删除之前的物理文件
                        System.IO.File.Delete(webRootPath + imageBo.UploadPath);
                        // 更新数据记录
                        imageBo.RelevanceObjectID = Guid.Parse(id);
                        imageBo.Name = formFile.FileName;
                        imageBo.Description = "";
                        imageBo.UploadFileSuffix = fileExt;
                        imageBo.UploadPath = "/uploadFiles/avatars/" + newFileName;
                        imageBo.DisplayName = "头像";
                    }
                    await _businessImageRepository.SaveBoAsyn(imageBo);
                }
            }

            return Ok(new { count = files.Count, size });
        }

        public async Task<IActionResult> VideoCoverSave(string id)
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string webRootPath = _HostingEnvironment.WebRootPath;
            string contentRootPath = _HostingEnvironment.ContentRootPath;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string fileExt = _GetFileSuffix(formFile.FileName);     // 文件后缀，不含“.”
                    long fileSize = formFile.Length;                        // 获得文件大小，以字节为单位
                    string newFileName = id + "_" + formFile.FileName;      // 生成新的文件名
                    var filePath = webRootPath + "/uploadFiles/images/" + newFileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    // 提取封面图片，并处理存储的唯一性
                    var imageBo = await _businessImageRepository.GetBoAsyn(x => x.RelevanceObjectID == Guid.Parse(id) && x.IsUnique == true);
                    if (imageBo == null)
                    {
                        imageBo = new BusinessImage
                        {
                            RelevanceObjectID = Guid.Parse(id),
                            Name = formFile.FileName,
                            Description = "",
                            UploadFileSuffix = fileExt,
                            UploadPath = "/uploadFiles/images/" + newFileName,
                            DisplayName = "头像",
                            IsUnique = true
                        };
                    }
                    else
                    {
                        // 删除之前的物理文件
                        System.IO.File.Delete(webRootPath + imageBo.UploadPath);
                        // 更新数据记录
                        imageBo.RelevanceObjectID = Guid.Parse(id);
                        imageBo.Name = formFile.FileName;
                        imageBo.Description = "";
                        imageBo.UploadFileSuffix = fileExt;
                        imageBo.UploadPath = "/uploadFiles/images/" + newFileName;
                        imageBo.DisplayName = "头像";
                    }
                    await _businessImageRepository.SaveBoAsyn(imageBo);
                }
            }

            return Ok(new { count = files.Count, size });

        }

        /// <summary>
        /// 清楚视频封面对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> DeleteVideoCover(Guid id)
        {
            // 提取文件路径信息
            var file = await _businessFileRepository.GetBoAsyn(x => x.RelevanceObjectID == id && x.IsUnique == true);
            if (file != null)
            {
                string filePath = _HostingEnvironment.WebRootPath + file.UploadPath;
                // 删除文件记录
                await _businessFileRepository.DeleteBoAsyn(file.Id);
                // 删除物理文件
                System.IO.File.Delete(filePath);
            }
            return "/images/empty_16_9.jpg";
        }

        /// <summary>
        /// 接受并保存视频文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> VideoSave(string id)
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string webRootPath = _HostingEnvironment.WebRootPath;
            string contentRootPath = _HostingEnvironment.ContentRootPath;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string fileExt = _GetFileSuffix(formFile.FileName);     // 文件扩展名，不含“.”
                    long fileSize = formFile.Length;                        // 获得文件大小，以字节为单位
                    string newFileName = id + "_" + formFile.FileName;      // 生成新的文件名
                    var filePath = webRootPath + "/uploadFiles/videos/" + newFileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    // 处理一个关联对象只能拥有一个视频
                    var videoBo =await _businessVideoRepository.GetBoAsyn(x => x.RelevanceObjectID == Guid.Parse(id) && x.IsUnique == true);
                    if (videoBo == null)
                    {
                        videoBo = new BusinessVideo();
                        videoBo.RelevanceObjectID = Guid.Parse(id);
                        videoBo.Name = formFile.FileName;
                        videoBo.UploadFileSuffix = fileExt;
                        videoBo.UploadPath = "/uploadFiles/videos/" + newFileName;
                        videoBo.IsUnique = true;
                    }
                    else
                    {
                        //todo 需要处理删除原来的物理文件
                        videoBo.RelevanceObjectID = Guid.Parse(id);
                        videoBo.Name = formFile.FileName;
                        videoBo.UploadFileSuffix = fileExt;
                        videoBo.UploadPath = "/uploadFiles/videos/" + newFileName;
                    }
                    await _businessVideoRepository.SaveBoAsyn(videoBo);
                }

            }

            return Ok(new { count = files.Count, size });
        }

        public async Task<string> DeleteVideo(Guid id)
        {
            // 提取文件路径信息
            var file = await _businessVideoRepository.GetBoAsyn(x => x.RelevanceObjectID == id && x.IsUnique == true);
            if (file != null)
            {
                string filePath = _HostingEnvironment.WebRootPath + file.UploadPath;
                // 删除文件记录
                await _businessVideoRepository.DeleteBoAsyn(file.Id);
                // 删除物理文件
                System.IO.File.Delete(filePath);
            }
            return "#";
        }


        #region 获取上传文件数据常规维护管理 Html 字符串的控制器方法
        public async Task<string> DataForUploadFile(Guid id)
        {
            var file = await _businessFileRepository.GetBoAsyn(id);
            var fileVMCollection = new List<BusinessFileVM>() { new BusinessFileVM(file) };

            return StringHelperForUpload.UploadFileListTable(fileVMCollection);
        }

        /// <summary>
        /// 返回关联的上传文件的 html 字符串，直接在相关的 div 中渲染
        /// </summary>
        /// <param name="id">关联对象的 Id，不是上传文件的 Id</param>
        /// <returns></returns>
        public virtual async Task<string> DataForUploadFileCollection(Guid id)
        {
            var fileVMCollection = new List<BusinessFileVM>();

            var businessFileCollection = await _businessFileRepository.GetBusinessFileCollectionAsyn(id);
            foreach (var item in businessFileCollection)
                fileVMCollection.Add(new BusinessFileVM(item));

            return StringHelperForUpload.UploadFileListTable(fileVMCollection);
        }

        public virtual async Task<string> DataForAvatar(Guid id)
        {
            var result = "";
            var image = await _businessImageRepository.GetBoAsyn(x => x.RelevanceObjectID == id && x.IsUnique == true && x.IsAvatar == true);
            if(image!=null)
                result=image.UploadPath;

            return result;
        }

        /// <summary>
        /// 返回关联的上传图片的 html 字符串，直接在相关的 div 中渲染
        /// </summary>
        /// <param name="id">关联对象的 Id，不是上传文件的 Id</param>
        /// <returns></returns>
        public virtual async Task<string> DataForUploadImageCollection(Guid id)
        {
            var imageVMCollection = new List<BusinessImageVM>();

            var businessImgageCollection = await _businessImageRepository.GetBusinessImageCollectionAsyn(id);
            foreach (var item in businessImgageCollection)
                imageVMCollection.Add(new BusinessImageVM(item));

            return StringHelperForUpload.UploadImageList(imageVMCollection);
        }

        public virtual async Task<string> DataForUploadVideoCover(Guid id)
        {
            var result = "";
            var image = await _businessImageRepository.GetBoAsyn(x => x.RelevanceObjectID == id && x.IsUnique == true);
            if (image != null)
                result = image.UploadPath;

            return result;
        }

        public virtual async Task<string> DataForUploadVideo(Guid id)
        {
            var result = "";
            var image = await _businessVideoRepository.GetBoAsyn(x => x.RelevanceObjectID == id && x.IsUnique == true);
            if (image != null)
                result = image.UploadPath;

            return result;
        }
        #endregion


        /// <summary>
        /// 获取文件后缀名
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        private string _GetFileSuffix(string fileName)
        {
            if (fileName.IndexOf(".") == -1)
                  return "";
            string[] temp = fileName.Split('.');
            return temp[temp.Length - 1].ToLower();
        }
    }
}
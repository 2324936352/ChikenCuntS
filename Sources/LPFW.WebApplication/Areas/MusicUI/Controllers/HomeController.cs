using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LPFW.EntitiyModels.MusicUIEntity;
using LPFW.ViewModels.MusicViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace LPFW.WebApplication.Areas.MusicUI.Controllers
{
    [Area("MusicUI")]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private IStudentRepository _studentRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
       



        //使用构造函数注入的方式注入IStudentRepository
        public HomeController(IStudentRepository studentRepository, IHostingEnvironment hostingEnvironment)
        {
            _studentRepository = studentRepository;
            this._hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {






            //  throw new Exception("此异常发生在Details视图中");


            MusicCore student = _studentRepository.GetStudent(id);

            if (student == null)
            {
                Response.StatusCode = 404;
                return View("StudentNotFound", id);

            }

            //实例化HomeDetailsViewModel并存储音乐详细信息和PageTitle
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Student = student,
                PageTitle = "歌曲详细信息"
            };
            return View(homeDetailsViewModel);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(MusicCoreViewModel model)
        {
            if (ModelState.IsValid)
            {

                string uniqueFileName = null;

                if (model.Photos != null && model.Photos.Count > 0)
                {
                    uniqueFileName = ProcessUploadedFile(model);

                }
                string MusicFileName = null;

                if (model.MusicPath != null && model.MusicPath.Count > 0)
                {
                    MusicFileName = MusicPathUploadedFiles(model);

                }
                string LyricFileName = null;

                if (model.lyricPath != null && model.lyricPath.Count > 0)
                {
                    LyricFileName = LyricPathUploadedFiles(model);

                }
                MusicCore newMusic = new MusicCore
                {
                    Name = model.Name,
                    TypeName = model.TypeName,
                    SingerName = model.SingerName,
                    MusicPath = MusicFileName,
                    lyricPath = LyricFileName,
                    PhotoPath = uniqueFileName
                };
                StreamWriter sw = new StreamWriter("wwwroot/Music/music/musicList.txt", false, System.Text.Encoding.Default);    //一定在写绝对路径
                sw.WriteLine($"[{model.Name},{model.Name}]");
                sw.WriteLine(); //输入空行  
                sw.Close();//关闭文件


                _studentRepository.Add(newMusic);
                return RedirectToAction("Index");
            }
            return View();
        }
      
#region 上传文件
//private readonly string _targetFilePath = "C:\\Users\\Administrator\\Desktop\\项目\\ChikenCuntS\\Sources\\LPFW.WebApplication\\wwwroot\\Music\\music\\songImg";
///// <summary>
///// 流式文件上传
///// </summary>
///// <returns></returns>
//[HttpPost("Create")]
//public async Task<IActionResult> UploadingStream()
//{

//    //获取boundary
//    var boundary = HeaderUtilities.RemoveQuotes(MediaTypeHeaderValue.Parse(Request.ContentType).Boundary).Value;
//    //得到reader
//    var reader = new MultipartReader(boundary, HttpContext.Request.Body);
//    //{ BodyLengthLimit = 2000 };//
//    var section = await reader.ReadNextSectionAsync();

//    //读取section
//    while (section != null)
//    {
//        var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);
//        if (hasContentDispositionHeader)
//        {
//            var trustedFileNameForFileStorage = Path.GetRandomFileName();
//            await WriteFileAsync(section.Body, Path.Combine(_targetFilePath, trustedFileNameForFileStorage));
//        }
//        section = await reader.ReadNextSectionAsync();
//    }
//    return Created(nameof(HomeController), null);
//}

///// <summary>
///// 缓存式文件上传
///// </summary>
///// <param name=""></param>
///// <returns></returns>
//[HttpPost("UploadingFormFile")]
//public async Task<IActionResult> UploadingFormFile(IFormFile file)
//{
//    using (var stream = file.OpenReadStream())
//    {
//        var trustedFileNameForFileStorage = Path.GetRandomFileName();
//        await WriteFileAsync(stream, Path.Combine(_targetFilePath, trustedFileNameForFileStorage));
//    }
//    return Created(nameof(HomeController), null);
//}


///// <summary>
///// 写文件导到磁盘
///// </summary>
///// <param name="stream">流</param>
///// <param name="path">文件保存路径</param>
///// <returns></returns>
//public static async Task<int> WriteFileAsync(System.IO.Stream stream, string path)
//{
//    const int FILE_WRITE_SIZE = 84975;//写出缓冲区大小
//    int writeCount = 0;
//    using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write, FILE_WRITE_SIZE, true))
//    {
//        byte[] byteArr = new byte[FILE_WRITE_SIZE];
//        int readCount = 0;
//        while ((readCount = await stream.ReadAsync(byteArr, 0, byteArr.Length)) > 0)
//        {
//            await fileStream.WriteAsync(byteArr, 0, readCount);
//            writeCount += readCount;
//        }
//    }
//    return writeCount;
//} 
#endregion


/// <summary>
/// 将照片保存到指定的路径中，并返回唯一的文件名
/// </summary>
/// <returns></returns>
private string ProcessUploadedFile(MusicCoreViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photos.Count > 0)
            {
                foreach (var photo in model.Photos)
                {
                    //必须将图像上传到wwwroot中的images文件夹
                    //而要获取wwwroot文件夹的路径，我们需要注入 ASP.NET Core提供的HostingEnvironment服务
                    //通过HostingEnvironment服务去获取wwwroot文件夹的路径
                    string uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/Music/music/songimg");
                    //为了确保文件名是唯一的，我们在文件名后附加一个新的GUID值和一个下划线
                    //uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    uniqueFileName = photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    //因为使用了非托管资源，所以需要手动进行释放
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        //使用IFormFile接口提供的CopyTo()方法将文件复制到wwwroot/images文件夹
                        photo.CopyTo(fileStream);
                    }



                }
            }

            return uniqueFileName;

            
        }


        /// <summary>
        /// 将歌曲保存到指定的路径中，并返回唯一的文件名
        /// </summary>
        /// <returns></returns>
        private string MusicPathUploadedFiles(MusicCoreViewModel model)
        {


            string MusicFileName = null;

            if (model.MusicPath.Count > 0)
            {
                foreach (var music in model.MusicPath)
                {
                    //必须将图像上传到wwwroot中的images文件夹
                    //而要获取wwwroot文件夹的路径，我们需要注入 ASP.NET Core提供的HostingEnvironment服务
                    //通过HostingEnvironment服务去获取wwwroot文件夹的路径
                    string uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/Music/music");
                    //为了确保文件名是唯一的，我们在文件名后附加一个新的GUID值和一个下划线
                    //uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    MusicFileName = music.FileName;
                    string filePath = Path.Combine(uploadsFolder, MusicFileName);

                    //因为使用了非托管资源，所以需要手动进行释放
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        //使用IFormFile接口提供的CopyTo()方法将文件复制到wwwroot/images文件夹
                        music.CopyTo(fileStream);
                    }



                }
            }

            return MusicFileName;


        }

        private string LyricPathUploadedFiles(MusicCoreViewModel model)
        {


            string LyricFileName = null;

            if (model.lyricPath.Count > 0)
            {
                foreach (var lyric in model.lyricPath)
                {
                    //必须将图像上传到wwwroot中的images文件夹
                    //而要获取wwwroot文件夹的路径，我们需要注入 ASP.NET Core提供的HostingEnvironment服务
                    //通过HostingEnvironment服务去获取wwwroot文件夹的路径
                    string uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/Music/music/Lyric");
                    //为了确保文件名是唯一的，我们在文件名后附加一个新的GUID值和一个下划线
                    //uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    LyricFileName = lyric.FileName;
                    string filePath = Path.Combine(uploadsFolder, LyricFileName);

                    //因为使用了非托管资源，所以需要手动进行释放
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        //使用IFormFile接口提供的CopyTo()方法将文件复制到wwwroot/images文件夹
                        lyric.CopyTo(fileStream);
                    }



                }
            }

            return LyricFileName;


        }
       
      
    }
}

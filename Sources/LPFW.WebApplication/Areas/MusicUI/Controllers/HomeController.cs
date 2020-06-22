using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LPFW.EntitiyModels.MusicUIEntity;
using LPFW.ViewModels.MusicViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;

namespace LPFW.WebApplication.Areas.MusicUI.Controllers
{
    [Area("MusicUI")]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private IStudentRepository _studentRepository;
        //private readonly HostingEnvironment hostingEnvironment;
        private readonly ILogger logger;



        //使用构造函数注入的方式注入IStudentRepository
        public HomeController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
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

            //实例化HomeDetailsViewModel并存储Student详细信息和PageTitle
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Student = student,
                PageTitle = "学生详细信息"
            };
            return View(homeDetailsViewModel);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {

                //string uniqueFileName = null;

                //if (model.Photos != null && model.Photos.Count > 0)
                //{
                //    uniqueFileName = ProcessUploadedFile(model);

                //}
                MusicCore newMusic = new MusicCore
                {
                    Name = model.Name,
                  
                    TypeName = model.TypeName,
                    //PhotoPath = uniqueFileName
                };

                _studentRepository.Add(newMusic);
                return RedirectToAction("Details", new { id = newMusic.Id });
            }
            return View();
        }

        /// <summary>
        /// 将照片保存到指定的路径中，并返回唯一的文件名
        /// </summary>
        /// <returns></returns>
        private string ProcessUploadedFile(StudentCreateViewModel model)
        {


            string uniqueFileName = null;

            if (model.Photos.Count > 0)
            {
                foreach (var photo in model.Photos)
                {
                    //必须将图像上传到wwwroot中的images文件夹
                    //而要获取wwwroot文件夹的路径，我们需要注入 ASP.NET Core提供的HostingEnvironment服务
                    //通过HostingEnvironment服务去获取wwwroot文件夹的路径
                    string uploadsFolder = Path.Combine(HostingEnvironment.ApplicationName, "images");
                    //为了确保文件名是唯一的，我们在文件名后附加一个新的GUID值和一个下划线

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
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
    }
}

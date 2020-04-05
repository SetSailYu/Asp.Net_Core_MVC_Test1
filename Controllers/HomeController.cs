using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebCore测试1VS2019.Models;
using WebCore测试1VS2019.ViewModels;

using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WebCore测试1VS2019.Controllers
{
    /// <summary>
    /// 视图Home控制器
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        ///  学生类仓储服务接口
        /// </summary>
        private readonly IStudentRepository _studentRepository;
        /// <summary>
        /// 获取或设置www目录路径
        /// </summary>
        private readonly IWebHostEnvironment webHostEnvironment;

        // 使用构造函数注入的方式注入IStudentRepository,   
        // 获取或设置www目录路径 --> using Microsoft.AspNetCore.Hosting;
        //      IHostingEnvironment.WebRootPath (已过时)
        //      IWebHostEnvironment.WebRootPath (Core 3.0 之后推荐)
        public HomeController(IStudentRepository studentRepository, IWebHostEnvironment webHostEnvironment)
        {
            _studentRepository = studentRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// 默认设置首页视图
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            IEnumerable<Student> model = _studentRepository.GetAllStudents();
            return View(model);
        }

        public ObjectResult Details(int id)
        {
            Student model = _studentRepository.GetStudent(id);
            return new ObjectResult(model);

            // return $"id == {id},并且名字为{name}";
            // return Json(new { id="1",name="张三" });
        }

        public IActionResult Deta(int? id)
        {
            //Student model = _studentRepository.GetStudent(1);

            //ViewData["PageTitle"] = "学生详情";
            //ViewData["Student"] = model;

            //ViewBag.PageTitle = "学生详情";
            //ViewBag.Student = model;

            HomeDetaViewModel homeDetaViewModel = new HomeDetaViewModel()
            {
                Student = _studentRepository.GetStudent(id??1),
                //PageTitle = "学生详情（视图Home下Deta视图的数据模型提供）"
            };

            return View(homeDetaViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 添加学生页面重载 form表单post数据提交获取
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                //if (model.Photos != null && model.Photos.Count > 0)
                if (model.Photo != null)
                {
                    //foreach (var photo in model.Photos)
                    //{
                    //    // 必须将图像上传到wwwroot中的images文件夹
                    //    // 而要获取wwwroot文件夹的路径，我们需要注入 ASP.NET Core提供的HostingEnvironment服务
                    //    // 通过HostingEnvironment服务去获取wwwroot文件夹的路径
                    //    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                    //    // 获取上传的文件名
                    //    string untrustedFileName = Path.GetFileName(photo.FileName);
                    //    // 返回指定图片的后缀
                    //    //string fileext1 = System.IO.Path.GetExtension(untrustedFileName);
                    //    // 编码命名文件
                    //    //uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    //    uniqueFileName = DateTime.Now.ToString("yyyyMMddHHffss") + "_" + untrustedFileName;
                    //    // 获取文件路径
                    //    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    //    // 使用IFormFile接口提供的CopyTo()方法将文件复制到wwwroot/images文件夹
                    //    photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    //}

                    // 必须将图像上传到wwwroot中的images文件夹
                    // 而要获取wwwroot文件夹的路径，我们需要注入 ASP.NET Core提供的HostingEnvironment服务
                    // 通过HostingEnvironment服务去获取wwwroot文件夹的路径
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                    // 获取上传的文件名
                    string untrustedFileName = Path.GetFileName(model.Photo.FileName);
                    // 返回指定图片的后缀
                    //string fileext1 = System.IO.Path.GetExtension(untrustedFileName);
                    // 编码命名文件
                    //uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    uniqueFileName = DateTime.Now.ToString("yyyyMMddHHffss") + "_" + untrustedFileName;
                    // 获取文件路径
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    // 使用IFormFile接口提供的CopyTo()方法将文件复制到wwwroot/images文件夹
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                Student newStudent = new Student
                {
                    Name = model.Name,
                    ClassName = model.ClassName,
                    Email = model.Email,
                    PhotoPath = uniqueFileName
                };

                Student new_student = _studentRepository.Add(newStudent);
                // 重定向到详情页面
                return RedirectToAction("Deta", new { id = new_student.Id });
            }
            return View();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebCore测试1VS2019.Models;
using WebCore测试1VS2019.ViewModels;

using Microsoft.AspNetCore.Cors;

using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WebCore测试1VS2019.Controllers
{
    
    //[Route("api/[controller]")]
    //[ApiController]
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
            // 模型验证
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                //if (model.Photos != null && model.Photos.Count > 0)
                if (model.Photo != null)
                {
                    uniqueFileName = ProcessUploadedFile(model);
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

        /// <summary>
        /// 编辑学生信息页面加载 获取学生信息
        /// </summary>
        /// <param name="id">该学生的Id</param>
        /// <returns></returns>
        [HttpGet]
        public ViewResult Edit(int id)
        {
            Student student = _studentRepository.GetStudent(id);
            if (student != null)
            {
                // 将该学生的信息存入视图模型中
                StudentEditViewModel studentEditViewModel = new StudentEditViewModel
                {
                    Id = student.Id,
                    Name = student.Name,
                    ClassName = student.ClassName,
                    Email = student.Email,
                    ExistingPhotoPath = student.PhotoPath
                };

                return View(studentEditViewModel);
            }

            throw new Exception("查询不到这个学生信息");
            //return View();
        }

        [HttpPost]
        public IActionResult Edit(StudentEditViewModel model)
        {
            // 模型验证
            // 检查提供的数据是否有效，如果没有通过验证，需要重新编辑学生信息
            // 这样用户就可以更正并重新提交编辑表单
            if (ModelState.IsValid)
            {
                // 从视图模型中获取学生信息到领域模型中
                Student student = _studentRepository.GetStudent(model.Id);
                student.Email = model.Email;
                student.Name = model.Name;
                student.ClassName = model.ClassName;
                // 判断是否修改前就存在图片
                if (model.Photo != null)
                {
                    // 判断用户是否有上传图片
                    if (model.ExistingPhotoPath != null)
                    {
                        // 如果用户有上传图片，则删除原图片
                        string filePahth = Path.Combine(webHostEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePahth);

                        student.PhotoPath = ProcessUploadedFile(model);
                    }
                }
                // 更新学生信息
                Student updateStudent = _studentRepository.Update(student);
                // 重定向页面到首页
                return RedirectToAction("Index");
            }
            // 提供的数据无效，重新编辑学生信息
            return View(model);
        }

        /// <summary>
        /// 图片上传方法，返回重新编辑完成的上传文件名
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string ProcessUploadedFile(StudentCreateViewModel model)
        {
            string uniqueFileName = null;

            //// 多文件上传方法
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
            string fileext1 = System.IO.Path.GetExtension(untrustedFileName);
            // 编码命名文件
            //uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
            uniqueFileName = DateTime.Now.ToString("yyyyMMddHHffss") + "_" + Guid.NewGuid().ToString() + fileext1;
            // 获取文件路径
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            // 因为使用了非托管资源，所以需要手动进行释放
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                // 使用IFormFile接口提供的CopyTo()方法将文件复制到wwwroot/images文件夹
                model.Photo.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
    }
}
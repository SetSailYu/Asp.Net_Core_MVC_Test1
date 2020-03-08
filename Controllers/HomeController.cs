using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebCore测试1VS2019.Models;
using WebCore测试1VS2019.ViewModels;

namespace WebCore测试1VS2019.Controllers
{
    /// <summary>
    /// 视图Home控制器
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        // 使用构造函数注入的方式注入IStudentRepository
        public HomeController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
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

        public ObjectResult Details()
        {
            Student model = _studentRepository.GetStudent(1);
            return new ObjectResult(model);

            // return View();
            //return Json(new { id="1",name="张三" });
        }

        public IActionResult Deta()
        {
            //Student model = _studentRepository.GetStudent(1);

            //ViewData["PageTitle"] = "学生详情";

            //ViewData["Student"] = model;

            //ViewBag.PageTitle = "学生详情";

            //ViewBag.Student = model;

            HomeDetaViewModel homeDetaViewModel = new HomeDetaViewModel()
            {
                Student = _studentRepository.GetStudent(1),
                PageTitle = "学生详情（Home下Deta视图的数据模型提供）"
            };

            return View(homeDetaViewModel);
        }

    }
}
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
        /// <summary>
        ///  学生类仓储服务接口
        /// </summary>
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
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                Student new_student = _studentRepository.Add(student);
                // 重定向到详情页面
                return RedirectToAction("Deta", new { id = new_student.Id });
            }
            return View();
        }

    }
}
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebCore测试1VS2019.Models;

namespace WebCore测试1VS2019.ViewModels
{
    public class StudentCreateViewModel
    {
        // Display 显示

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "请输入名字"), MaxLength(50, ErrorMessage = "名字的长度不能超过50个字符")]
        public string Name { get; set; }

        /// <summary>
        /// 班级  ?为可空
        /// </summary>
        [Display(Name = "班级信息")]
        [Required(ErrorMessage = "请选择班级信息")]
        public ClassNameEnum? ClassName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Display(Name = "邮箱地址")]
        [Required(ErrorMessage = "请输入邮箱地址")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "邮箱的格式不正确")]
        public string Email { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        [Display(Name = "图片")]
        public IFormFile Photo { get; set; }

        //[Display(Name = "图片（多文件）")]
        //public List<IFormFile> Photos { get; set; }
    }
}

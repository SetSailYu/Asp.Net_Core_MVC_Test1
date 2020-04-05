using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore测试1VS2019.Models
{
    /// <summary>
    /// 学生模型
    /// </summary>
    public class Student
    {
        // Display 显示

        /// <summary>
        /// 学号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "请输入名字"),MaxLength(50,ErrorMessage = "名字的长度不能超过50个字符")]
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
        /// 图片地址
        /// </summary>
        [Display(Name = "图片地址")]
        public string PhotoPath { get; set; }
    }

    /***                  模型验证
     *      Required                        指定该字段是必填的
     *      Range                           指定允许的最小值和最大值
     *      MinLength                       只用MinLength指定字符串的最小长度
     *      MaxLength                       使用MaxLength指定字符串的最大长度
     *      Compare                         比较模型的2个属性。例如比较Email和ComfirmEmail属性
     *      RegularExpression               正则表达式 验证提供的值是否与正则表达式指定的模式匹配
     */



}

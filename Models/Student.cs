using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore测试1VS2019.Models
{
    /// <summary>
    /// 学生模型
    /// </summary>
    public class Student
    {
        /// <summary>
        /// 学号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 班级
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
    }
}

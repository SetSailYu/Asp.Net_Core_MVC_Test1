using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore测试1VS2019.Models
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// ModelBuilder扩展方法  种子数据
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    Name = "张三",
                    ClassName = ClassNameEnum.FirstGrade,
                    Email = "zhangsan@ddxc.org"
                },
                new Student
                {
                    Id = 2,
                    Name = "李四",
                    ClassName = ClassNameEnum.GradeThree,
                    Email = "lisi@ddxc.org"
                }
                );
        }
    }
}

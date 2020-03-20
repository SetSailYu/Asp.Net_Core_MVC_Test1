using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore测试1VS2019.Models
{
    /// <summary>
    /// 学生类仓储服务接口
    /// </summary>
    public interface IStudentRepository
    {
        /// <summary>
        /// 通过学号获取该学生信息
        /// </summary>
        /// <param name="id">学号</param>
        /// <returns></returns>
        Student GetStudent(int id);

        /// <summary>
        /// 获取所有学生信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<Student> GetAllStudents();

        /// <summary>
        /// 添加一名学生信息
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        Student Add(Student student);

        /// <summary>
        /// 修改一名学生的信息
        /// </summary>
        /// <param name="updateStudent"></param>
        /// <returns></returns>
        Student Update(Student updateStudent);

        /// <summary>
        /// 删除一名学生的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Student Delete(int id);
    }
}

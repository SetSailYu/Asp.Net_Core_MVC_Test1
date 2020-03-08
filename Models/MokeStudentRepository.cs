﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore测试1VS2019.Models
{
    public class MockStudentRepository : IStudentRepository
    {
        private List<Student> _studentList;

        public MockStudentRepository()
        {
            _studentList = new List<Student>()
            {
                new Student() { Id = 1, Name = "张三", ClassName = "一年级", Email = "Tony-zhang@52abp.com"},
                new Student() { Id = 2, Name = "李四", ClassName = "二年级", Email = "lisi@52abp.com"},
                new Student() { Id = 3, Name = "王二麻子", ClassName = "二年级", Email = "wang@52abp.com"}
            };
        }


        public Student GetStudent(int id)
        {
            return _studentList.FirstOrDefault(a => a.Id == id);
            //throw new NotImplementedException();
        }


        public IEnumerable<Student> GetAllStudents()
        {
            return _studentList;
        }
    }
}

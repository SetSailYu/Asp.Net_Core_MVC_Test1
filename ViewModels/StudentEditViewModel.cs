using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore测试1VS2019.ViewModels
{
    public class StudentEditViewModel: StudentCreateViewModel
    {
        /// <summary>
        /// 学号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 存在的图片地址
        /// </summary>
        public string ExistingPhotoPath { get; set; }
    }
}

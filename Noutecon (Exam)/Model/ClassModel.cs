using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noutecon__Exam_.Model
{
    public class ClassModel
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }
        public int CuratorId { get; set; }
        public int NumOfStudents { get; set; }
        public TeacherAccountModel ClassTeacher { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noutecon__Exam_.Model
{
    public class AssignedClassWithStudentsClass
    {
        public ClassModel AlreadySelectedClass { get; set; }
        public List<StudentAccountModel>? SelectedStudents { get; set; }
    }
}

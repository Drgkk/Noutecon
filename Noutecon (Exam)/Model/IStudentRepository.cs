using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Noutecon__Exam_.Model
{
    public interface IStudentRepository
    {
        bool AuthenticateUser(NetworkCredential credential);
        void Add(StudentModel studentModel);
        void Edit(StudentModel studentModel);
        void Remove(StudentModel studentModel);
        StudentModel GetById(int id);
        StudentModel GetByUsername(string username);
        IEnumerable<StudentModel> GetAll();
    }
}

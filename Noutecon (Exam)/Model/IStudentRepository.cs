using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        void RemoveById(int id);
        StudentModel GetById(int id);
        StudentModel GetByUsername(string username);
        ObservableCollection<StudentAccountModel> GetStudentsAccountsByClassId(int Id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Noutecon__Exam_.Model
{
    public interface ITeacherRepository
    {
        bool AuthenticateUser(NetworkCredential credential);
        void Add(TeacherModel teacherModel);
        void Edit(TeacherModel teacherModel);
        void Remove(TeacherModel teacherModel);
        TeacherModel GetById(int id);
        TeacherModel GetByUsername(string username);
        IEnumerable<TeacherModel> GetAll();
    }
}

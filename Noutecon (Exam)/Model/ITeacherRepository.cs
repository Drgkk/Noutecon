using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Noutecon__Exam_.Model
{
    public interface ITeacherRepository
    {
        bool AuthenticateUser(NetworkCredential credential);
        void Add(TeacherModel teacherModel);
        void EditPfpById(int id, string pfpPath);
        void Remove(TeacherModel teacherModel);
        TeacherAccountModel GetAccountById(int id);
        TeacherModel GetByUsername(string username);
        IEnumerable<TeacherModel> GetAll();
        void EditTeacherUsername(int id, string userName);
        void EditTeacherFirstName(int id, string firstName);
        void EditTeacherLastName(int id, string lastName);
        void EditPassword(int id, NetworkCredential nc);
        NetworkCredential GetNetworkCredential(int id);
    }
}

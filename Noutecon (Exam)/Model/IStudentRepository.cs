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
        void RemoveFromClassById(int studentId, int classId);
        StudentModel GetById(int id);
        StudentAccountModel GetAccountById(int id);
        StudentModel GetByUsername(string username);
        ObservableCollection<StudentAccountModel> GetStudentsAccountsByClassId(int Id);
        void AddStudentToClassById(int studentId, int classId);
        int GetStudentIdByUsername(string username);
        bool IsStudentInClass(int studentId, int classId);
        void EditStudentUsername(int id, string userName);
        void EditStudentFirstName(int id, string firstName);
        void EditStudentLastName(int id, string lastName);
        void EditPfpById(int id, string pfpPath);
        void EditPassword(int id, NetworkCredential nc);
        NetworkCredential GetNetworkCredential(int id);
    }
}

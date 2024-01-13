using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noutecon__Exam_.Model
{
    public interface ITestRepository
    {
        int Add(TestModel testModel);
        void Remove(TestModel testModel);
        TestModel GetById(int id);
        ObservableCollection<TestModel> GetTestsByTeacherId(int teacherId);
        ObservableCollection<TestModel> GetTestsByStudentId(int studentId);
        ObservableCollection<TestModel> GetTestsByStudentIdAndTeacherId(int studentId, int teacherId);
        int GetTestId(string name);
        bool ValidateTest(string name);
        void EditImageAndAudioById(string imagePath, string audioPath, int testId, int questionNum);
        void Edit(int testId, TestModel testModel);
        string GetNameById(int testId);
        (int, double) GetStudentTriesAndResult(int studentId, int testId);
        void SetStudentResult(double result, int studentId, int testId);
        TeacherAccountModel GetTestTeacher(int testId);
        void UpdateTestMainValuesAndStudents(int testId, TestModel testModel);
    }
}

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
        int GetTestId(string name);
        bool ValidateTest(string name);
        void EditImageAndAudioById(string imagePath, string audioPath, int testId, int questionNum);
        void Edit(int testId, TestModel testModel);
        string GetNameById(int testId);
    }
}

using Noutecon__Exam_.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noutecon__Exam_.Repositories
{
    public class TestRepository : RepositoryBase, ITestRepository
    {
        public void Add(TestModel testModel)
        {
            using(var conn = GetConnection())
            {
                using(var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "insert into [Test] ([Name], [TeacherId], [NumberOfTries], [Category]) output INSERTED.ID values (@name, @teacherId, @numOfTries, @category)";
                    command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar).Value = testModel.Name;
                    command.Parameters.Add("@teacherId", System.Data.SqlDbType.Int).Value = testModel.TeacherId;
                    command.Parameters.Add("@numOfTries", System.Data.SqlDbType.Int).Value = testModel.NumberOfTries;
                    command.Parameters.Add("@category", System.Data.SqlDbType.NVarChar).Value = testModel.Category;
                    int testId = (int)command.ExecuteScalar();
                    foreach (QuestionModel questionModel in testModel.Questions)
                    {
                        command.CommandText = "insert into [Question] ([TestId], [QuestionText], [ImagePath], [AudioPath], [AnswerType]) output INSERTED.ID values (@testId, @questionText, @imagePath, @audioPath, @answerType)";
                        command.Parameters.Clear();
                        command.Parameters.Add("@testId", System.Data.SqlDbType.Int).Value = testId;
                        command.Parameters.Add("@questionText", System.Data.SqlDbType.NVarChar).Value = questionModel.QuestionText;
                        command.Parameters.Add("@imagePath", System.Data.SqlDbType.NVarChar).Value = questionModel.ImagePath;
                        if(string.IsNullOrEmpty(questionModel.AudioPath))
                        {
                            command.Parameters.Add("@audioPath", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.Add("@audioPath", System.Data.SqlDbType.NVarChar).Value = questionModel.AudioPath;
                        }
                        int answerType = 0;
                        if(questionModel is IOneAnswer oneAnswer)
                        {
                            answerType = 0;
                            command.Parameters.Add("@answerType", System.Data.SqlDbType.Int).Value = answerType;
                            int questionId = (int)command.ExecuteScalar();
                            int i = 0;
                            foreach (string answer in oneAnswer.Answers)
                            {
                                command.CommandText = "insert into [Answer] ([QuestionId], [Answer], [IsRight]) values (@questionId, @answer, @isRight)";
                                command.Parameters.Clear();
                                command.Parameters.Add("@questionId", System.Data.SqlDbType.Int).Value = questionId;
                                command.Parameters.Add("@answer", System.Data.SqlDbType.NVarChar).Value = answer;
                                bool isRight = false;
                                if(i == oneAnswer.RightAnswer)
                                {
                                    isRight = true;
                                }
                                command.Parameters.Add("@isRight", System.Data.SqlDbType.Bit).Value = isRight;
                                command.ExecuteNonQuery();
                                i++;
                            }
                        }
                        else if (questionModel is IMultipleAnswer multipleAnswer)
                        {
                            answerType = 1;
                            command.Parameters.Add("@answerType", System.Data.SqlDbType.Int).Value = answerType;
                            int questionId = (int)command.ExecuteScalar();
                            int i = 0;
                            foreach (string answer in multipleAnswer.Answers)
                            {
                                command.CommandText = "insert into [Answer] ([QuestionId], [Answer], [IsRight]) values (@questionId, @answer, @isRight)";
                                command.Parameters.Clear();
                                command.Parameters.Add("@questionId", System.Data.SqlDbType.Int).Value = questionId;
                                command.Parameters.Add("@answer", System.Data.SqlDbType.NVarChar).Value = answer;
                                bool isRight = false;
                                if (multipleAnswer.RightAnswers.Contains(i))
                                {
                                    isRight = true;
                                }
                                command.Parameters.Add("@isRight", System.Data.SqlDbType.Bit).Value = isRight;
                                command.ExecuteNonQuery();
                                i++;
                            }
                        }
                        else if (questionModel is IManualAnswer manualAnswer)
                        {
                            answerType = 2;
                            command.Parameters.Add("@answerType", System.Data.SqlDbType.Int).Value = answerType;
                            int questionId = (int)command.ExecuteScalar();
                            command.CommandText = "insert into [Answer] ([QuestionId], [Answer], [IsRight]) values (@questionId, @answer, @isRight)";
                            command.Parameters.Clear();
                            command.Parameters.Add("@questionId", System.Data.SqlDbType.Int).Value = questionId;
                            command.Parameters.Add("@answer", System.Data.SqlDbType.NVarChar).Value = manualAnswer.RightAnswer;
                            command.Parameters.Add("@isRight", System.Data.SqlDbType.Bit).Value = true;
                            command.ExecuteNonQuery();
                        }
                    }

                    foreach(StudentAccountModel student in testModel.Students)
                    {
                        command.CommandText = "insert into [TestStudent] ([TestId], [StudentId], [Result], [NumberOfTries]) values (@testId, @studentId, @result, @numOfTriesStud)";
                        command.Parameters.Clear();
                        command.Parameters.Add("@testId", System.Data.SqlDbType.Int).Value = testId;
                        command.Parameters.Add("@studentId", System.Data.SqlDbType.Int).Value = student.Id;
                        command.Parameters.Add("@result", System.Data.SqlDbType.Int).Value = 0;
                        command.Parameters.Add("@numOfTriesStud", System.Data.SqlDbType.Int).Value = 0;
                        command.ExecuteNonQuery();
                    }
                    

                }
            }
        }

        public TestModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetTestId(string name)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<TestModel> GetTestsByStudentId(int studentId)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<TestModel> GetTestsByTeacherId(int teacherId)
        {
            ObservableCollection<TestModel> tests = new ObservableCollection<TestModel>();
            using(var conn = GetConnection())
            {
                using(var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "";
                }
            }
            return tests;
        }

        public void Remove(TestModel testModel)
        {
            throw new NotImplementedException();
        }

        public bool ValidateTest(string name)
        {
            throw new NotImplementedException();
        }
    }
}

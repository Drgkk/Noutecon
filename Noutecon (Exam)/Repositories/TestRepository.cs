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
                    command.CommandText = "select * from [Test] where [TeacherId] = @teacherId";
                    command.Parameters.Add("@teacherId", System.Data.SqlDbType.Int).Value = teacherId;
                    using(var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TestModel test = new TestModel();
                            test.Id = reader.GetInt32(0);
                            test.Name = reader.GetString(1);
                            test.TeacherId = reader.GetInt32(2);    
                            test.NumberOfTries = reader.GetInt32(3);
                            test.Category = reader.GetString(4);
                            test.Questions = new List<QuestionModel>();
                            var commandQuestion = new SqlCommand();
                            commandQuestion.Connection = conn;
                            commandQuestion.CommandText = "select * from [Question] where [TestId] = @testId";
                            commandQuestion.Parameters.Add("@testId", System.Data.SqlDbType.Int).Value = test.Id;
                            using(var readerQuestion  = commandQuestion.ExecuteReader())
                            {
                                while(readerQuestion.Read())
                                {
                                    int answerType = readerQuestion.GetInt32(5);
                                    if(answerType == 0)
                                    {
                                        OneAnswerQuestionModel oneAnswerQuestionModel = new OneAnswerQuestionModel();
                                        oneAnswerQuestionModel.Id = readerQuestion.GetInt32(0);
                                        oneAnswerQuestionModel.TestId = readerQuestion.GetInt32(1);
                                        oneAnswerQuestionModel.QuestionText = readerQuestion.GetString(2);
                                        oneAnswerQuestionModel.ImagePath = readerQuestion.GetString(3);
                                        oneAnswerQuestionModel.Answers = new List<string>();
                                        if(!readerQuestion.IsDBNull(4))
                                        {
                                            oneAnswerQuestionModel.AudioPath = readerQuestion.GetString(4);
                                        }
                                        var commandAnswer = new SqlCommand();
                                        commandAnswer.Connection = conn;
                                        commandAnswer.CommandText = "select * from [Answer] where [QuestionId] = @questionId";
                                        commandAnswer.Parameters.Add("@questionId", System.Data.SqlDbType.Int).Value = oneAnswerQuestionModel.Id;
                                        using(var readerAnswer = commandAnswer.ExecuteReader())
                                        {
                                            int i = 0;
                                            while(readerAnswer.Read())
                                            {
                                                oneAnswerQuestionModel.Answers.Add(readerAnswer.GetString(2));
                                                if(readerAnswer.GetBoolean(3))
                                                {
                                                    oneAnswerQuestionModel.RightAnswer = i;
                                                }
                                                i++;
                                            }
                                        }
                                        test.Questions.Add(oneAnswerQuestionModel);
                                    }
                                    else if (answerType == 1)
                                    {
                                        MultipleAnswerQuestionModel multipleAnswerQuestionModel = new MultipleAnswerQuestionModel();
                                        multipleAnswerQuestionModel.Id = readerQuestion.GetInt32(0);
                                        multipleAnswerQuestionModel.TestId = readerQuestion.GetInt32(1);
                                        multipleAnswerQuestionModel.QuestionText= readerQuestion.GetString(2);
                                        multipleAnswerQuestionModel.ImagePath= readerQuestion.GetString(3);
                                        if (!readerQuestion.IsDBNull(4))
                                        {
                                            multipleAnswerQuestionModel.AudioPath = readerQuestion.GetString(4);
                                        }
                                        multipleAnswerQuestionModel.Answers = new List<string>();
                                        multipleAnswerQuestionModel.RightAnswers = new List<int>();
                                        var commandAnswer = new SqlCommand();
                                        commandAnswer.Connection = conn;
                                        commandAnswer.CommandText = "select * from [Answer] where [QuestionId] = @questionId";
                                        commandAnswer.Parameters.Add("@questionId", System.Data.SqlDbType.Int).Value = multipleAnswerQuestionModel.Id;
                                        using (var readerAnswer = commandAnswer.ExecuteReader())
                                        {
                                            int i = 0;
                                            while (readerAnswer.Read())
                                            {
                                                multipleAnswerQuestionModel.Answers.Add(readerAnswer.GetString(2));
                                                if (readerAnswer.GetBoolean(3))
                                                {
                                                    multipleAnswerQuestionModel.RightAnswers.Add(i);
                                                }
                                                i++;
                                            }
                                        }
                                        test.Questions.Add(multipleAnswerQuestionModel);
                                    }
                                    else if (answerType == 2)
                                    {
                                        ManualQuestionModel manualQuestionModel = new ManualQuestionModel();
                                        manualQuestionModel.Id = readerQuestion.GetInt32(0);
                                        manualQuestionModel.TestId= readerQuestion.GetInt32(1);
                                        manualQuestionModel.QuestionText= readerQuestion.GetString(2);
                                        manualQuestionModel.ImagePath= readerQuestion.GetString(3);
                                        if (!readerQuestion.IsDBNull(4))
                                        {
                                            manualQuestionModel.AudioPath = readerQuestion.GetString(4);
                                        }
                                        var commandAnswer = new SqlCommand();
                                        commandAnswer.Connection = conn;
                                        commandAnswer.CommandText = "select * from [Answer] where [QuestionId] = @questionId";
                                        commandAnswer.Parameters.Add("@questionId", System.Data.SqlDbType.Int).Value = manualQuestionModel.Id;
                                        using (var readerAnswer = commandAnswer.ExecuteReader())
                                        {
                                            while (readerAnswer.Read())
                                            {
                                                manualQuestionModel.RightAnswer = readerAnswer.GetString(2);
                                            }
                                        }
                                        test.Questions.Add(manualQuestionModel);
                                    }
                                }
                            }

                            var commandStudent = new SqlCommand();
                            commandStudent.Connection = conn;
                            commandStudent.CommandText = "select * from Student where Id in (select StudentId from [TestStudent] where [TestId] = @testId)";
                            commandStudent.Parameters.Add("@testId", System.Data.SqlDbType.Int).Value = test.Id;
                            test.Students = new List<StudentAccountModel>();
                            using(var readerStudent = commandStudent.ExecuteReader())
                            {
                                while (readerStudent.Read())
                                {
                                    StudentAccountModel student = new StudentAccountModel();
                                    student.Id = readerStudent.GetInt32(0);
                                    student.Username = readerStudent.GetString(1);
                                    student.FirstName = readerStudent.GetString(3);
                                    student.LastName = readerStudent.GetString(4);
                                    student.ProfilePicturePath = readerStudent.GetString(5);
                                    test.Students.Add(student);
                                }
                            }
                            tests.Add(test);
                        }
                    }
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

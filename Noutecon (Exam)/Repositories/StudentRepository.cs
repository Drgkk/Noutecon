using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Noutecon__Exam_.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Noutecon__Exam_.Repositories
{
    class StudentRepository : RepositoryBase, IStudentRepository
    {
        public void Add(StudentModel studentModel)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "insert into [Student] ([Username], [Password], [FirstName], [LastName], [ClassId], [ProfilePicturePath]) values (@username, @password, @firstname, @lastname, @classId, @pfp)";
                    command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = studentModel.Username;
                    command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar).Value = studentModel.Password;
                    command.Parameters.Add("@firstname", System.Data.SqlDbType.NVarChar).Value = studentModel.FirstName;
                    command.Parameters.Add("@lastname", System.Data.SqlDbType.NVarChar).Value = studentModel.LastName;
                    command.Parameters.Add("@classId", System.Data.SqlDbType.NVarChar).Value = studentModel.ClassId;
                    command.Parameters.Add("@pfp", System.Data.SqlDbType.NVarChar).Value = studentModel.ProfilePicturePath;
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using(var connection = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from [student] where username = @username and [password] = @password";
                    command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = credential.UserName;
                    command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar).Value = credential.Password;
                    validUser = command.ExecuteScalar() == null ? false : true;
                }
            }
            return validUser;
        }

        public void Edit(StudentModel studentModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StudentModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public StudentModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public StudentModel GetByUsername(string username)
        {
            StudentModel student = null;
            using(var connection = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection= connection;
                    command.CommandText = "select * from [Student] where username = @username";
                    command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = username;
                    using(var reader =  command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            student = new StudentModel()
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Password = string.Empty,
                                FirstName = reader.GetString(3),
                                LastName = reader.GetString(4),
                                ClassId = reader.GetInt32(5),
                                ProfilePicturePath = reader.GetString(6),
                            };
                        }
                    }
                }
            }
            return student;
        }

        public void Remove(StudentModel studentModel)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<StudentAccountModel> GetStudentsAccountsByClassId(int Id)
        {
            ObservableCollection<StudentAccountModel> students = new ObservableCollection<StudentAccountModel>();
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from [Student] where ClassId = @Id";
                    command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = Id;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new StudentAccountModel()
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                FirstName = reader.GetString(3),
                                LastName = reader.GetString(4),
                                ClassId = reader.GetInt32(5),
                                ProfilePicturePath = reader.GetString(6),
                            });
                        }
                    }
                }
            }
            return students;
        }

        public void RemoveById(int id)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "delete from [Student] where Id = @Id";
                    command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}

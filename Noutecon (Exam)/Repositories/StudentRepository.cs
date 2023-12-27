using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Noutecon__Exam_.Model;

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
                    command.CommandText = "insert into [Student] ([Username], [Password], [FirstName], [LastName], [ClassId]) values (@username, @password, @firstname, @lastname, @classId)";
                    command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = studentModel.Username;
                    command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar).Value = studentModel.Password;
                    command.Parameters.Add("@firstname", System.Data.SqlDbType.NVarChar).Value = studentModel.FirstName;
                    command.Parameters.Add("@lastname", System.Data.SqlDbType.NVarChar).Value = studentModel.LastName;
                    command.Parameters.Add("@classId", System.Data.SqlDbType.NVarChar).Value = studentModel.ClassId;
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
                                ClassId = reader.GetInt32(5)
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
    }
}

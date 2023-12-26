using Noutecon__Exam_.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Noutecon__Exam_.Repositories
{
    class TeacherRepository : RepositoryBase, ITeacherRepository
    {
        public void Add(TeacherModel teacherModel)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "insert into [Teacher] values (@username, @password, @firstname, @lastname, @school)";
                    command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = teacherModel.Username;
                    command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar).Value = teacherModel.Password;
                    command.Parameters.Add("@firstname", System.Data.SqlDbType.NVarChar).Value = teacherModel.FirstName;
                    command.Parameters.Add("@lastname", System.Data.SqlDbType.NVarChar).Value = teacherModel.LastName;
                    command.Parameters.Add("@school", System.Data.SqlDbType.NVarChar).Value = teacherModel.School;
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool isValidTeacher = true;
            using(var conn = GetConnection())
            {
                using(var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "select * from [Teacher] where username = @username and password = @password";
                    command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = credential.UserName;
                    command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar).Value = credential.Password;
                    isValidTeacher = command.ExecuteScalar() == null ? false : true;
                }
            }
            return isValidTeacher;
        }

        public void Edit(TeacherModel teacherModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TeacherModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public TeacherModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public TeacherModel GetByUsername(string username)
        {
            TeacherModel teacher = null;
            using (var conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "select * from [Teacher] where username = @username";
                    command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = username;
                    using(var reader =  command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            teacher = new TeacherModel()
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Password = string.Empty,
                                FirstName = reader.GetString(3),
                                LastName = reader.GetString(4)
                            };
                        }
                    }
                }
            }
            return teacher;
        }

        public void Remove(TeacherModel teacherModel)
        {
            throw new NotImplementedException();
        }
    }
}

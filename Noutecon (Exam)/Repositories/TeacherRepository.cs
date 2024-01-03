using Noutecon__Exam_.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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
                    command.CommandText = "insert into [Teacher] ([Username], [Password], [FirstName], [LastName], [School], [ProfilePicturePath]) values (@username, @password, @firstname, @lastname, @school, @pfp)";
                    command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = teacherModel.Username;
                    command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar).Value = teacherModel.Password;
                    command.Parameters.Add("@firstname", System.Data.SqlDbType.NVarChar).Value = teacherModel.FirstName;
                    command.Parameters.Add("@lastname", System.Data.SqlDbType.NVarChar).Value = teacherModel.LastName;
                    command.Parameters.Add("@school", System.Data.SqlDbType.NVarChar).Value = teacherModel.School;
                    command.Parameters.Add("@pfp", System.Data.SqlDbType.NVarChar).Value = teacherModel.ProfilePicturePath;
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

        public void EditPfpById(int id, string pfpPath)
        {
            using (var conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "update [Teacher] set ProfilePicturePath = @pfp where Id = @id";
                    command.Parameters.Add("@pfp", System.Data.SqlDbType.NVarChar).Value = pfpPath;
                    command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<TeacherModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public TeacherAccountModel GetAccountById(int id)
        {
            TeacherAccountModel teacherAccountModel = null;
            using (var conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "select * from [Teacher] where Id = @id";
                    command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            teacherAccountModel = new TeacherAccountModel()
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                FirstName = reader.GetString(3),
                                LastName = reader.GetString(4),
                                School = reader.GetString(5),
                                ProfilePicturePath = reader.GetString(6)
                            };
                        }
                    }
                }
            }
            return teacherAccountModel;
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
                                LastName = reader.GetString(4),
                                School = reader.GetString(5),
                                ProfilePicturePath = reader.GetString(6)
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

        public void EditTeacherUsername(int id, string userName)
        {
            using (var conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "update [Teacher] set Username = @username where Id = @id";
                    command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = userName;
                    command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void EditTeacherFirstName(int id, string firstName)
        {
            using (var conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "update [Teacher] set FirstName = @firstname where Id = @id";
                    command.Parameters.Add("@firstname", System.Data.SqlDbType.NVarChar).Value = firstName;
                    command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void EditTeacherLastName(int id, string lastName)
        {
            using (var conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "update [Teacher] set LastName = @lastname where Id = @id";
                    command.Parameters.Add("@lastname", System.Data.SqlDbType.NVarChar).Value = lastName;
                    command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void EditPassword(int id, NetworkCredential nc)
        {
            using (var conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "update [Teacher] set Password = @password where Id = @id";
                    command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar).Value = nc.Password;
                    command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }

        public NetworkCredential GetNetworkCredential(int id)
        {
            NetworkCredential nc = null;
            using (var conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "select Username, Password from [Teacher] where Id = @id";
                    command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                    using (var reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            nc = new NetworkCredential(reader.GetString(0), reader.GetString(1));
                        }
                    }
                }
            }
            return nc;
        }

    }
}

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
                    command.CommandText = "insert into [Student] ([Username], [Password], [FirstName], [LastName], [ProfilePicturePath]) values (@username, @password, @firstname, @lastname, @pfp)";
                    command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = studentModel.Username;
                    command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar).Value = studentModel.Password;
                    command.Parameters.Add("@firstname", System.Data.SqlDbType.NVarChar).Value = studentModel.FirstName;
                    command.Parameters.Add("@lastname", System.Data.SqlDbType.NVarChar).Value = studentModel.LastName;
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
        public StudentAccountModel GetAccountById(int id)
        {
            StudentAccountModel studentAccountModel = null;
            using (var conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "select * from [Student] where Id = @id";
                    command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            studentAccountModel = new StudentAccountModel()
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                FirstName = reader.GetString(3),
                                LastName = reader.GetString(4),
                                ProfilePicturePath = reader.GetString(5)
                            };
                        }
                    }
                }
            }
            return studentAccountModel;
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
                                ProfilePicturePath = reader.GetString(5),
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
                    command.CommandText = "select * from [Student] where Id in (select StudentId from [StudentClass] Where ClassId = @Id)";
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
                                ProfilePicturePath = reader.GetString(5),
                            });
                        }
                    }
                }
            }
            return students;
        }

        public void RemoveFromClassById(int studentId, int classId)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "delete from [StudentClass] where StudentId = @studentId and ClassId = @classId";
                    command.Parameters.Add("@studentId", System.Data.SqlDbType.Int).Value = studentId;
                    command.Parameters.Add("@classId", System.Data.SqlDbType.Int).Value = classId;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddStudentToClassById(int studentId, int classId)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "insert into [StudentClass] ([ClassId], [StudentId]) values (@classId, @studentId)";
                    command.Parameters.Add("@classId", System.Data.SqlDbType.Int).Value = classId;
                    command.Parameters.Add("@studentId", System.Data.SqlDbType.Int).Value = studentId;
                    command.ExecuteNonQuery();
                }
            }
        }

        public int GetStudentIdByUsername(string username)
        {
            int studentId = 0;
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select Id from [Student] where username = @username";
                    command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = username;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            studentId = reader.GetInt32(0);
                        }
                    }
                }
            }
            return studentId;
        }

        public bool IsStudentInClass(int studentId, int classId)
        {
            bool isStudentInClass = false;
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from [StudentClass] Where StudentId = @studentId and ClassId = @classId";
                    command.Parameters.Add("@classId", System.Data.SqlDbType.Int).Value = classId;
                    command.Parameters.Add("@studentId", System.Data.SqlDbType.Int).Value = studentId;
                    isStudentInClass = command.ExecuteScalar() == null ?  false : true;
                }
            }
            return isStudentInClass;
        }

        public void EditStudentUsername(int id, string userName)
        {
            using (var conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "update [Student] set Username = @username where Id = @id";
                    command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = userName;
                    command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void EditStudentFirstName(int id, string firstName)
        {
            using (var conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "update [Student] set FirstName = @firstname where Id = @id";
                    command.Parameters.Add("@firstname", System.Data.SqlDbType.NVarChar).Value = firstName;
                    command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void EditStudentLastName(int id, string lastName)
        {
            using (var conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "update [Student] set LastName = @lastname where Id = @id";
                    command.Parameters.Add("@lastname", System.Data.SqlDbType.NVarChar).Value = lastName;
                    command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void EditPfpById(int id, string pfpPath)
        {
            using (var conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "update [Student] set ProfilePicturePath = @pfp where Id = @id";
                    command.Parameters.Add("@pfp", System.Data.SqlDbType.NVarChar).Value = pfpPath;
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
                    command.CommandText = "update [Student] set Password = @password where Id = @id";
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
                    command.CommandText = "select Username, Password from [Student] where Id = @id";
                    command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            nc = new NetworkCredential(reader.GetString(0), reader.GetString(1));
                        }
                    }
                }
            }
            return nc;
        }

        public ObservableCollection<double> GetAllGradesFromTeacher(int teacherId)
        {
            ObservableCollection<double> grades = new ObservableCollection<double>();
            using (var conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "select Result from [TestStudent] where TestId in (select Id from [Test] where TeacherId = @teachedId)";
                    command.Parameters.Add("@teachedId", System.Data.SqlDbType.Int).Value = teacherId;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            grades.Add(reader.GetDouble(0));
                        }
                    }
                }
            }
            return grades;
        }

        public ObservableCollection<double> GetAllGradesFromStudentOfTeacher(int studentId, int teacherId)
        {
            ObservableCollection<double> grades = new ObservableCollection<double>();
            using (var conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "select Result from [TestStudent] where TestId in (select Id from [Test] where TeacherId = @teachedId) and StudentId = @studentId";
                    command.Parameters.Add("@teachedId", System.Data.SqlDbType.Int).Value = teacherId;
                    command.Parameters.Add("@studentId", System.Data.SqlDbType.Int).Value = studentId;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            grades.Add(reader.GetDouble(0));
                        }
                    }
                }
            }
            return grades;
        }

    }
}

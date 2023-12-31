using Noutecon__Exam_.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;

namespace Noutecon__Exam_.Repositories
{
    public class ClassRepository : RepositoryBase, IClassRepository
    {
        public bool ValidateClass(string uniqueId)
        {
            bool isValid;
            using (SqlConnection conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "select * from [Class] where [UniqueId] = @uniqueId COLLATE SQL_Latin1_General_CP1_CS_AS";
                    command.Parameters.Add("@uniqueId", System.Data.SqlDbType.NVarChar).Value = uniqueId;
                    isValid = command.ExecuteScalar() == null ? false : true;
                }
            }
            return isValid;
        }
        public void Add(ClassModel classModel)
        {
            using(SqlConnection conn = GetConnection())
            {
                using(var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "insert into [Class] ([UniqueId], [Name], [Grade], [CuratorId]) values (@uniqueId, @name, @grade, @curatorId)";
                    command.Parameters.Add("@uniqueId", System.Data.SqlDbType.NVarChar).Value = classModel.UniqueId;
                    command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar).Value = classModel.Name;
                    command.Parameters.Add("@grade", System.Data.SqlDbType.Int).Value = classModel.Grade;
                    command.Parameters.Add("@curatorId", System.Data.SqlDbType.Int).Value = classModel.CuratorId;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateUniqueIdOfClassWithId(int id, string uniqueId)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "update [Class] set uniqueId = @uniqueId where Id = @id";
                    command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                    command.Parameters.Add("@uniqueId", System.Data.SqlDbType.NVarChar).Value = uniqueId;
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<ClassModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public ClassModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ClassModel GetByUniqueId(string uniqueId)
        {
            ClassModel classModel = null;
            using (SqlConnection conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "select * from [Class] where [UniqueId] = @uniqueId";
                    command.Parameters.Add("@uniqueId", System.Data.SqlDbType.NVarChar).Value = uniqueId;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            classModel = new ClassModel()
                            {
                                Id = reader.GetInt32(0),
                                UniqueId = reader.GetString(1),
                                Name = reader.GetString(2),
                                Grade = reader.GetInt32(3),
                                CuratorId = reader.GetInt32(4),
                            };
                        }
                    }
                }
            }
            return classModel;
        }

        public int GetId(string uniqueId)
        {
            int classId = 0;
            using (SqlConnection conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "select Id from [Class] where [UniqueId] = @uniqueId";
                    command.Parameters.Add("@uniqueId", System.Data.SqlDbType.NVarChar).Value = uniqueId;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            classId = reader.GetInt32(0);
                        }
                    }
                }
            }
            return classId;
        }

        public void Remove(ClassModel classModel)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<ClassModel> GetClassesByTeacherId(int teacherId)
        {
            ObservableCollection<ClassModel> classes = new ObservableCollection<ClassModel>();
            using (SqlConnection conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "select * from [Class] where [CuratorId] = @curatorId";
                    command.Parameters.Add("@curatorId", System.Data.SqlDbType.NVarChar).Value = teacherId;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int classId = reader.GetInt32(0);
                            int curId = reader.GetInt32(4);
                            classes.Add( new ClassModel()
                            {
                                Id = classId,
                                UniqueId = reader.GetString(1),
                                Name = reader.GetString(2),
                                Grade = reader.GetInt32(3),
                                CuratorId = curId,
                                NumOfStudents = CountStudentsInClass(classId),
                                ClassTeacher = GetTeacherByClassCuratorId(curId)
                            });
                        }
                    }
                }
            }
            return classes;
        }

        public ObservableCollection<ClassModel> GetClassesByStudentId(int studentId)
        {
            ObservableCollection<ClassModel> classes = new ObservableCollection<ClassModel>();
            using (SqlConnection conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "select * from [Class] where [Id] in (select ClassId from [StudentClass] where StudentId = @studentId)";
                    command.Parameters.Add("@studentId", System.Data.SqlDbType.Int).Value = studentId;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int classId = reader.GetInt32(0);
                            int curId = reader.GetInt32(4);
                            classes.Add(new ClassModel()
                            {
                                Id = classId,
                                UniqueId = reader.GetString(1),
                                Name = reader.GetString(2),
                                Grade = reader.GetInt32(3),
                                CuratorId = curId,
                                NumOfStudents = CountStudentsInClass(classId),
                                ClassTeacher = GetTeacherByClassCuratorId(curId)
                            });
                        }
                    }
                }
            }
            return classes;
        }

        private int CountStudentsInClass(int Id)
        {
            int count = 0;
            using (SqlConnection conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "select COUNT(*) from [StudentClass] where [ClassId] = @id group by [ClassId]";
                    command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = Id;
                    using(var reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            count = reader.GetInt32(0);
                        }
                    }
                }
            }
            return count;
        }

        private TeacherAccountModel GetTeacherByClassCuratorId(int curatorId)
        {
            TeacherAccountModel teacherAccountModel = null;
            using (SqlConnection conn = GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "select * from [Teacher] where [Id] in (select id from [Class] where CuratorId = @curatorId)";
                    command.Parameters.Add("@curatorId", System.Data.SqlDbType.Int).Value = curatorId;
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

    }
}

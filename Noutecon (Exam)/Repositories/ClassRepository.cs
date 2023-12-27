using Noutecon__Exam_.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noutecon__Exam_.Repositories
{
    public class ClassRepository : RepositoryBase, IClassRepository
    {
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

        public void Edit(ClassModel classModel)
        {
            throw new NotImplementedException();
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

        public void Remove(ClassModel classModel)
        {
            throw new NotImplementedException();
        }
    }
}

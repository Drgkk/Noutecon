using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Noutecon__Exam_.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;
        public RepositoryBase()
        {
            _connectionString = "Data Source=DESKTOP-86BBP7I\\SQLEXPRESS;InitialCatalog=NouteconDB;IntegratedSecurity=True;ConnectTimeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=True";
        }
        protected SqlConnection GetConnection()
        {
            return new SqlConnection( _connectionString );
        }
    }
}

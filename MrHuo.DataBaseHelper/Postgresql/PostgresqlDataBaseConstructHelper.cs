using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MrHuo.DataBaseHelper.Objects;
using System.Data.Common;

namespace MrHuo.DataBaseHelper.Postgresql
{
    public class PostgresqlDataBaseConstructHelper : Interfaces.IDataBaseConstructHelper
    {
        private static object lockObject = new object();
        private DbConnection CreateDbConnection()
        {
            var locker = lockObject;
            lock (locker)
            {
                var _dbConnection = DbConnectionFactory.CreateDbConnection(DataBaseType.Postgresql);
                _dbConnection.ConnectionString = this.ConnectionString;
                _dbConnection.Open();
                return _dbConnection;
            }
        }

        public string ConnectionString { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString"></param>
        public PostgresqlDataBaseConstructHelper(string connectionString)
        {
            this.ConnectionString = connectionString;
            if (string.IsNullOrWhiteSpace(this.ConnectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
        }

        public Task<IEnumerable<Column>> GetColumns(string tableName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ForeignKey>> GetForeignKeys()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Function>> GetFunctions()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetFunctionScript(string functionName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProcedureColumn>> GetProcedureColumns(string procedureName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Procedure>> GetProcedures()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetProcedureScript(string procedureName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Table>> GetTables()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ViewColumn>> GetViewColumns(string viewName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<View>> GetViews()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetViewScript(string viewName)
        {
            throw new NotImplementedException();
        }
    }
}

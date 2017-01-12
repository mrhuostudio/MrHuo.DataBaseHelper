using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MrHuo.DataBaseHelper.Objects;
using System.Data.Common;

namespace MrHuo.DataBaseHelper.SQLite
{
    /// <summary>
    /// SQLite 数据库结构帮助类
    /// </summary>
    public class SQLiteDataBaseConstructHelper : Interfaces.IDataBaseConstructHelper
    {
        private static object lockObject = new object();
        private DbConnection CreateDbConnection()
        {
            var locker = lockObject;
            lock (locker)
            {
                var _dbConnection = DbConnectionFactory.CreateDbConnection(DataBaseType.SQLite);
                _dbConnection.ConnectionString = this.ConnectionString;
                _dbConnection.Open();
                return _dbConnection;
            }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString"></param>
        public SQLiteDataBaseConstructHelper(string connectionString)
        {
            this.ConnectionString = connectionString;
            if (string.IsNullOrWhiteSpace(this.ConnectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
        }

        /// <summary>
        /// 获取表中的所有列
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Task<IEnumerable<Column>> GetColumns(string tableName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取数据库中所有外键关系
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<ForeignKey>> GetForeignKeys()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取数据库中所有函数
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Function>> GetFunctions()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取函数的脚本
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public Task<string> GetFunctionScript(string functionName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取存储过程参数列表
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        public Task<IEnumerable<ProcedureColumn>> GetProcedureColumns(string procedureName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取数据库中所有存储过程
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Procedure>> GetProcedures()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取存储过程的脚本
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        public Task<string> GetProcedureScript(string procedureName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取数据库中所有表
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Table>> GetTables()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取视图结果参数列表
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public Task<IEnumerable<ViewColumn>> GetViewColumns(string viewName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取数据库中所有视图
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<View>> GetViews()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取视图的脚本
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public Task<string> GetViewScript(string viewName)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MrHuo.DataBaseHelper.Objects;
using System.Data.Common;
using System.Data;

namespace MrHuo.DataBaseHelper.MySql
{
    /// <summary>
    /// MySql 数据库结构帮助类
    /// </summary>
    public class MySqlDataBaseConstructHelper : Interfaces.IDataBaseConstructHelper
    {
        private static object lockObject = new object();
        private DbConnection CreateDbConnection()
        {
            var locker = lockObject;
            lock (locker)
            {
                var _dbConnection = DbConnectionFactory.CreateDbConnection(DataBaseType.MySql);
                _dbConnection.ConnectionString = this.ConnectionString;
                _dbConnection.Open();
                return _dbConnection;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString"></param>
        public MySqlDataBaseConstructHelper(string connectionString)
        {
            this.ConnectionString = connectionString;
            if (string.IsNullOrWhiteSpace(this.ConnectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
        }
        
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 获取表中的所有列
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Task<IEnumerable<Column>> GetColumns(string tableName)
        {
            return Task.Factory.StartNew((_tableName) =>
            {
                using (var dbConnection = CreateDbConnection())
                {
                    var sql = MySqlSQL.MySqlQueryTableColumns(dbConnection.Database, (string)_tableName);
                    return dbConnection.ExecuteModels<Column>(sql);
                }
            }, tableName);
        }

        /// <summary>
        /// 获取数据库中所有函数
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Function>> GetFunctions()
        {
            return Task.Factory.StartNew(() =>
            {
                using (var dbConnection = CreateDbConnection())
                {
                    var sql = MySqlSQL.MySqlQueryFunctions(dbConnection.Database);
                    return dbConnection.ExecuteModels<Function>(sql);
                }
            });
        }

        /// <summary>
        /// 获取数据库中所有存储过程
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Procedure>> GetProcedures()
        {
            return Task.Factory.StartNew(() =>
            {
                using (var dbConnection = CreateDbConnection())
                {
                    var sql = MySqlSQL.MySqlQueryProcedures(dbConnection.Database);
                    return dbConnection.ExecuteModels<Procedure>(sql);
                }
            });
        }

        /// <summary>
        /// 获取数据库中所有表
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Table>> GetTables()
        {
            return Task.Factory.StartNew(() =>
            {
                using (var dbConnection = CreateDbConnection())
                {
                    var sql = MySqlSQL.MySqlQueryTables(dbConnection.Database);
                    return dbConnection.ExecuteModels<Table>(sql);
                }
            });
        }

        /// <summary>
        /// 获取数据库中所有视图
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<View>> GetViews()
        {
            return Task.Factory.StartNew(() =>
            {
                using (var dbConnection = CreateDbConnection())
                {
                    var sql = MySqlSQL.MySqlQueryViews(dbConnection.Database);
                    return dbConnection.ExecuteModels<View>(sql);
                }
            });
        }

        /// <summary>
        /// 获取数据库中所有外键关系
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<ForeignKey>> GetForeignKeys()
        {
            return Task.Factory.StartNew(() =>
            {
                using (var dbConnection = CreateDbConnection())
                {
                    var sql = MySqlSQL.MySqlQueryForeignKeys(dbConnection.Database);
                    return dbConnection.ExecuteModels<ForeignKey>(sql);
                }
            });
        }

        /// <summary>
        /// 获取视图的脚本
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public Task<string> GetViewScript(string viewName)
        {
            return Task.Factory.StartNew((_viewName) =>
            {
                var name = (string)_viewName;
                using (var dbConnection = CreateDbConnection())
                {
                    var sql = MySqlSQL.MySqlQueryViewScript(name);
                    var ret = dbConnection.ExecuteScalar(sql);
                    if (ret == null)
                    {
                        return string.Empty;
                    }
                    return (string)ret;
                }
            }, viewName);
        }

        /// <summary>
        /// 获取函数的脚本
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public Task<string> GetFunctionScript(string functionName)
        {
            return Task.Factory.StartNew((_functionName) =>
            {
                var name = (string)_functionName;
                using (var dbConnection = CreateDbConnection())
                {
                    var sql = MySqlSQL.MySqlQueryFunctionScript(name);
                    var dataTable = dbConnection.ExecuteDataTable(sql);
                    if (dataTable.Rows.Count == 0)
                    {
                        throw new Exception($"No procedure named '{functionName}'");
                    }
                    var script = dataTable.Rows[0]["Create Function"];
                    return (string)script;
                }
            }, functionName);
        }

        /// <summary>
        /// 获取存储过程的脚本
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        public Task<string> GetProcedureScript(string procedureName)
        {
            return Task.Factory.StartNew((_procedureName) =>
            {
                var name = (string)_procedureName;
                using (var dbConnection = CreateDbConnection())
                {
                    var sql = MySqlSQL.MySqlQueryProcedureScript(name);
                    var dataTable = dbConnection.ExecuteDataTable(sql);
                    if (dataTable.Rows.Count==0)
                    {
                        throw new Exception($"No procedure named '{procedureName}'");
                    }
                    var script = dataTable.Rows[0]["Create Procedure"];
                    return (string)script;
                }
            }, procedureName);
        }

        /// <summary>
        /// 获取视图结果参数列表
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public Task<IEnumerable<ViewColumn>> GetViewColumns(string viewName)
        {
            return Task.Factory.StartNew((_viewName) =>
            {
                var name = (string)_viewName;
                using (var dbConnection = CreateDbConnection())
                {
                    var sql = $"DESCRIBE {name};";
                    var dataTable = dbConnection.ExecuteDataTable(sql);

                    var columns = new List<ViewColumn>();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var type = row["Type"]?.ToString();
                        var columnType = "";
                        var columnLenth = "0";
                        if (type.Contains("("))
                        {
                            columnType = type.Split('(')[0];
                            columnLenth = type.Split('(')[1].Replace("(", "").Replace(")", "");
                        }
                        else
                        {
                            columnType = type;
                        }
                        var column = new ViewColumn()
                        {
                            ColumnName = row["Field"]?.ToString(),
                            ColumnLength = columnLenth,
                            ColumnType = columnType
                        };
                        columns.Add(column);
                    }
                    return columns.AsEnumerable();
                }
            }, viewName);
        }

        /// <summary>
        /// 获取存储过程参数列表
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        public Task<IEnumerable<ProcedureColumn>> GetProcedureColumns(string procedureName)
        {
            return Task.Factory.StartNew((_procedureName) =>
            {
                var name = (string)_procedureName;

                using (var dbConnection = CreateDbConnection())
                {
                    var sql = MySqlSQL.MySqlQueryProcedureColumns(name);
                    return dbConnection.ExecuteModels<ProcedureColumn>(sql);
                }
            }, procedureName);
        }
    }
}

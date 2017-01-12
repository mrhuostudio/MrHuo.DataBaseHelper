using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MrHuo.DataBaseHelper.Objects;
using System.Data.Common;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace MrHuo.DataBaseHelper.SqlServer
{
    /// <summary>
    /// SqlServer 数据库结构帮助类
    /// </summary>
    public class SqlServerDataBaseConstructHelper : Interfaces.IDataBaseConstructHelper
    {
        private static object lockObject = new object();
        private DbConnection CreateDbConnection()
        {
            var locker = lockObject;
            lock (locker)
            {
                var _dbConnection = DbConnectionFactory.CreateDbConnection(DataBaseType.SqlServer);
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
        public SqlServerDataBaseConstructHelper(string connectionString)
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
            return Task.Factory.StartNew((_tableName) =>
            {
                using (var dbConnection = CreateDbConnection())
                {
                    var sql = SqlServerSQL.SqlServerQueryTableByName((string)_tableName);
                    return dbConnection.ExecuteModels<Column>(sql);
                }
            }, tableName);
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
                    return dbConnection.ExecuteModels<Table>(SqlServerSQL.SqlServerQueryTables());
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
                    return dbConnection.ExecuteModels<View>(SqlServerSQL.SqlServerQueryViews());
                }
            });
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
                    return dbConnection.ExecuteModels<Function>(SqlServerSQL.SqlServerQueryFunctions());
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
                    var list = new List<Procedure>();
                    var result = dbConnection.ExecuteModels<Procedure>(SqlServerSQL.SqlServerQueryProcedures());

                    list.AddRange(result);
                    list.RemoveAll((item) => { return item.ProcedureDescription == "1"; });
                    return list.AsEnumerable();
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
                    return dbConnection.ExecuteModels<ForeignKey>(SqlServerSQL.SqlServerQueryForeignKeys());
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
            return GetScript(viewName);
        }

        /// <summary>
        /// 获取函数的脚本
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public Task<string> GetFunctionScript(string functionName)
        {
            return GetScript(functionName);
        }

        /// <summary>
        /// 获取存储过程的脚本
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        public Task<string> GetProcedureScript(string procedureName)
        {
            return GetScript(procedureName);
        }

        /// <summary>
        /// 获取对象脚本
        /// </summary>
        /// <returns></returns>
        private Task<string> GetScript(string objectName)
        {
            return Task.Factory.StartNew((_objectName) =>
            {
                var name = (string)_objectName;
                using (var dbConnection = CreateDbConnection())
                {
                    var sql = "sp_helptext";
                    var cmd = dbConnection
                        .GetCommand(sql, CommandType.StoredProcedure)
                        .AddParameter("@objname", name.Trim(), DbType.String);

                    var da = new SqlDataAdapter((SqlCommand)cmd);
                    var dt = new DataTable();
                    da.Fill(dt);

                    var sb = new StringBuilder();
                    foreach (DataRow item in dt.Rows)
                    {
                        sb.Append(item[0]?.ToString());
                    }
                    return sb.ToString().Trim();
                }
            }, objectName);
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
                    var sql = "sp_mshelpcolumns";
                    var cmd = dbConnection
                        .GetCommand(sql, CommandType.StoredProcedure)
                        .AddParameter("@tablename", name.Trim(), DbType.String);

                    var da = new SqlDataAdapter((SqlCommand)cmd);
                    var dataTable = new DataTable();
                    da.Fill(dataTable);

                    var list = new List<ViewColumn>();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var column = new ViewColumn()
                        {
                            ColumnName = row["col_name"]?.ToString(),
                            ColumnType = row["col_typename"]?.ToString(),
                            ColumnLength = row["col_len"]?.ToString()
                        };
                        list.Add(column);
                    }
                    return list.AsEnumerable();
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
                    var sql = "sp_help";
                    var cmd = dbConnection
                        .GetCommand(sql, CommandType.StoredProcedure)
                        .AddParameter("@objname", name.Trim(), DbType.String);

                    var da = new SqlDataAdapter((SqlCommand)cmd);
                    var dataSet = new DataSet();
                    da.Fill(dataSet);

                    var dataTable = dataSet.Tables[1];

                    var list = new List<ProcedureColumn>();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var column = new ProcedureColumn()
                        {
                            ColumnName = row["Parameter_name"]?.ToString().TrimStart('@'),
                            ColumnType = row["Type"]?.ToString(),
                            ColumnLength = row["Length"]?.ToString()
                        };
                        list.Add(column);
                    }
                    return list.AsEnumerable();
                }
            }, procedureName);
        }
    }
}

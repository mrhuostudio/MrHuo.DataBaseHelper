using MrHuo.DataBaseHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MrHuo.DataBaseHelper
{
    /// <summary>
    /// 数据库连接创建工厂
    /// </summary>
    public class DbConnectionFactory
    {
        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="dataBaseType"></param>
        /// <returns></returns>
        public static DbConnection CreateDbConnection(DataBaseType dataBaseType)
        {
            DbConnection dbConnection = null;
            var dllPath = "";
            var dllName = "";
            var dbConnectionType = "";
            if (dataBaseType == DataBaseType.SqlServer)
            {
                return new SqlConnection();
            }
            switch (dataBaseType)
            {
                case DataBaseType.MySql:
                    dllName = "MySql.Data.dll";
                    dbConnectionType = "MySql.Data.MySqlClient.MySqlConnection";
                    break;
                case DataBaseType.SQLite:
                    dllName = "System.Data.SQLite.DLL";
                    dbConnectionType = "System.Data.SQLite.SQLiteConnection";
                    break;
                case DataBaseType.Oracle:
                    dllName = "Oracle.DataAccess.dll";
                    dbConnectionType = "Oracle.DataAccess.Client.OracleConnection";
                    break;
                case DataBaseType.Postgresql:
                    dllName = "Npgsql.dll";
                    dbConnectionType = "Npgsql.NpgsqlConnection";
                    break;
            }
            dllPath = AppDomain.CurrentDomain.BaseDirectory + "bin\\" + dllName;
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "bin\\");
            if (!File.Exists(dllPath))
            {
                switch (dataBaseType)
                {
                    case DataBaseType.MySql:
                        File.WriteAllBytes(dllPath, Properties.Resources.MySql_Data);
                        break;
                    case DataBaseType.Oracle:
                        if (Environment.Is64BitOperatingSystem)
                        {
                            File.WriteAllBytes(dllPath, Properties.Resources.Oracle_DataAccess_64);
                        }
                        else {
                            File.WriteAllBytes(dllPath, Properties.Resources.Oracle_DataAccess_86);
                        }
                        break;
                    case DataBaseType.Postgresql:
                        File.WriteAllBytes(dllPath, Properties.Resources.Npgsql);
                        break;
                    case DataBaseType.SQLite:
                        File.WriteAllBytes(dllPath, Properties.Resources.System_Data_SQLite);
                        break;
                    default:
                        throw new FileNotFoundException(RS.STRING_DB_DLL_NOT_EXISTS(dataBaseType));
                }
            }
            var connectionType = Assembly.LoadFile(dllPath).GetType(dbConnectionType);
            dbConnection = (DbConnection)Activator.CreateInstance(connectionType);
            return dbConnection;
        }

        /// <summary>
        /// 创建指定类型的连接
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataBaseType"></param>
        /// <returns></returns>
        public static T CreateDbConnection<T>(DataBaseType dataBaseType) where T : DbConnection
        {
            return (T)CreateDbConnection(dataBaseType);
        }

        /// <summary>
        /// 测试数据库连接是否可以成功打开
        /// </summary>
        /// <param name="dataBaseType"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static TestConnectionResult TestConnection(DataBaseType dataBaseType, string connectionString)
        {
            var task = new Task<TestConnectionResult>((dynamicParam) =>
            {
                try
                {
                    DataBaseType type = ((dynamic)dynamicParam).dataBaseType;
                    string connString = ((dynamic)dynamicParam).connectionString;

                    using (var connection = CreateDbConnection(dataBaseType))
                    {
                        connection.ConnectionString = connString;
                        connection.Open();

                        return new TestConnectionResult()
                        {
                            Success = true,
                            ErrorMessage = RS.STRING_TEST_CONNECTION_SUCCESS()
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new TestConnectionResult()
                    {
                        Success = false,
                        ErrorMessage = ex.ToString()
                    };
                }
            }, new
            {
                dataBaseType = dataBaseType,
                connectionString = connectionString
            });
            task.Start();

            return task.Result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Linq;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MrHuo.DataBaseHelper
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 扩展方法，用于判断一个Object对象是否是Null或Empty
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Boolean IsNullOrEmpty(this Object obj)
        {
            try
            {
                return obj == null || String.IsNullOrEmpty(obj.ToString().Trim());
            }
            catch { return true; }
        }

        /// <summary>
        /// 去掉以 "_" 分割的表名，并去掉后缀 s
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static string Fix(this string tableName, char spliter = '_')
        {
            var table = tableName;
            var splits = tableName.Split(spliter);
            if (splits.Length > 1)
            {
                table = tableName.Split(spliter)[1];
                if (table.EndsWith("s"))
                {
                    table = table.Substring(0, table.Length - 1);
                }
            }
            return table;
        }
        
        /// <summary>
        /// 根据DataTable内容获取指定属性类的属性List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dtSource"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToList<T>(this DataTable dtSource)
        {
            foreach (DataRow dataRow in dtSource.Rows)
            {
                yield return ToModel<T>(dataRow);
            }
        }

        /// <summary>
        /// 将一个 DataRow 转化为指定类型的 Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public static T ToModel<T>(this DataRow dataRow)
        {
            if (dataRow == null)
            {
                return default(T);
            }
            Type type = typeof(T);
            Object entity = Activator.CreateInstance(type);
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance))
            {
                try
                {
                    if (!string.IsNullOrEmpty(dataRow[property.Name].ToString()))
                    {
                        property.SetValue(entity, Convert.ChangeType(dataRow[property.Name], property.PropertyType), null);
                    }
                }
                catch { }
            }
            return (T)entity;
        }

        /// <summary>
        /// 将数据库类型转化为 C# 类型的数据类型
        /// </summary>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        public static string ToCSharpType(this string sqlType)
        {
            var ret = sqlType.Split('(')[0].ToLower();
            switch (ret) {
                case "uniqueidentifier":
                    ret = "Guid";
                    break;
                case "smallint":
                case "short":                //mysql
                case "short?":               //mysql
                    ret = "Int16";
                    break;
                case "ushort":
                case "ushort?":
                    ret = "UInt16";
                    break;
                case "int":
                case "int?":                //mysql
                    ret = "Int32";
                    break;
                case "uint":                //mysql
                case "uint?":               //mysql
                    ret = "UInt32";
                    break;
                case "bigint":
                case "long":
                case "long?":
                    ret = "Int64";
                    break;
                case "ulong":
                case "ulong?":
                    ret = "UInt64";
                    break;
                case "bit":
                case "bool":                //mysql
                case "bool?":               //mysql
                    ret = "Boolean";
                    break;
                case "binary":
                case "image":
                case "varbinary":
                case "timestamp":
                    ret = "Byte[]";
                    break;
                case "char":
                case "char?":
                case "varchar":
                case "nchar":
                case "nvarchar":
                case "text":
                case "ntext":
                case "xml":
                    ret = "String";
                    break;
                case "tinyint":
                case "byte":                //mysql
                case "byte?":               //mysql
                    ret = "Byte";
                    break;
                case "real":
                    ret = "Single";
                    break;
                case "money":
                case "numeric":
                case "smallmoney":
                case "decimal":
                case "decimal?":               //mysql
                    ret = "Decimal";
                    break;
                //TODO: mysql 下的 float/float? 类型对应 Single
                case "float":
                case "float?":                //mysql
                case "double":                //mysql
                case "double?":               //mysql
                    ret = "Double";
                    break;
                case "sql_variant":
                    ret = "Object";
                    break;
                case "date":
                case "smalldatetime":
                case "datetime":
                case "datetime?":
                case "datetime2":
                    ret = "DateTime";
                    break;
                case "time":
                    ret = "TimeSpan";
                    break;
                case "datetimeoffset":
                    ret = "DateTimeOffset";
                    break;
                case "sbyte":                //mysql
                case "sbyte?":               //mysql
                    ret = "SByte";
                    break;
                default:
                    ret = "Object";
                    break;
            }
            return ret;
        }

        /// <summary>
        /// 从 dbConnection 对象中创建 IDbCommand
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IDbCommand GetCommand(this IDbConnection dbConnection, string sql, CommandType commandType = CommandType.Text)
        {
            if (dbConnection == null)
            {
                throw new ArgumentNullException("dbConnection");
            }
            var cmd = dbConnection.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = commandType;

            return cmd;
        }

        /// <summary>
        /// 为 IDbCommand 添加参数
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IDbCommand AddParameter(this IDbCommand dbCommand, string name, object value, DbType type, ParameterDirection direction = ParameterDirection.Input)
        {
            var param = dbCommand.CreateParameter();
            param.ParameterName = name;
            param.Value = value;
            param.Direction = direction;
            param.DbType = type;

            dbCommand.Parameters.Add(param);
            return dbCommand;
        }

        /// <summary>
        /// 使用 dbConnection 执行 sql，返回 IDataReader
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(this IDbConnection dbConnection, string sql, CommandType commandType = CommandType.Text)
        {
            using (var cmd = GetCommand(dbConnection, sql, commandType))
            {
                return cmd.ExecuteReader();
            }
        }

        /// <summary>
        /// 使用 dbConnection 执行 sql，返回记录影响行数
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(this IDbConnection dbConnection, string sql, CommandType commandType = CommandType.Text)
        {
            using (var cmd = GetCommand(dbConnection, sql, commandType))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 使用 dbConnection 执行 sql，返回第一行第一列数据
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static object ExecuteScalar(this IDbConnection dbConnection, string sql, CommandType commandType = CommandType.Text)
        {
            using (var cmd = GetCommand(dbConnection, sql, commandType))
            {
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// 使用 dbConnection 执行 sql，返回 DataTable
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(this IDbConnection dbConnection, string sql, CommandType commandType = CommandType.Text)
        {
            using (var dataReader = ExecuteReader(dbConnection, sql, commandType))
            {
                var dataTable = new DataTable();
                dataTable.Load(dataReader);
                return dataTable;
            }
        }

        /// <summary>
        /// 使用 dbConnection 执行 sql，返回 IEnumerable&lt;T&gt; （将 DataTable 转化为 IEnumerable&lt;T&gt;）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbConnection"></param>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IEnumerable<T> ExecuteModels<T>(this IDbConnection dbConnection, string sql, CommandType commandType = CommandType.Text)
        {
            var dataTable = ExecuteDataTable(dbConnection, sql, commandType);
            return dataTable.ToList<T>();
        }

        /// <summary>
        /// 使用 dbConnection 获取表 table 数据总行数
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static long Count(this IDbConnection dbConnection, string table)
        {
            var sql = $"SELECT COUNT(0) FROM {table}";
            var countObject = dbConnection.ExecuteScalar(sql);
            if (countObject == null)
            {
                return -1;
            }
            return long.Parse(countObject.ToString());
        }

        /// <summary>
        /// 根据 dbConnection 和 connectionString 创建 DataContext 实例
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DataContext CreateDataContext(this IDbConnection dbConnection, string connectionString)
        {
            dbConnection.ConnectionString = connectionString;
            return new DataContext(dbConnection);
        }
    }
}

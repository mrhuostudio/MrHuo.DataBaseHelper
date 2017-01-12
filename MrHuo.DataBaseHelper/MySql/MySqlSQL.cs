using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrHuo.DataBaseHelper.MySql
{
    /// <summary>
    /// MySql 获取数据库结构所使用的 SQL 语句
    /// </summary>
    public class MySqlSQL
    {
        /// <summary>
        /// 根据数据库名，获取数据库中所有表的SQL语句
        /// </summary>
        /// <param name="dbName">数据库名称</param>
        /// <returns></returns>
        public static string MySqlQueryTables(string dbName)
        {
            return $"SELECT TABLE_NAME AS TableName,CREATE_TIME AS CreateDate,TABLE_COMMENT AS TableDescription FROM information_schema.TABLES WHERE TABLE_SCHEMA='{dbName}' AND TABLE_TYPE='BASE TABLE'";
        }

        /// <summary>
        /// 根据数据库名和表名获取表结构的SQL语句
        /// </summary>
        /// <param name="dbName">数据库名称</param>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public static string MySqlQueryTableColumns(string dbName, string tableName)
        {
            return $"SELECT TABLE_NAME AS ColumnName,DATA_TYPE AS ColumnType,CHARACTER_MAXIMUM_LENGTH AS ColumnLength,COLUMN_COMMENT AS ColumnDescription,COLUMN_DEFAULT AS DefaultValue,(CASE EXTRA WHEN 'auto_increment' THEN 1 ELSE 0 END) AS IsIdentity,(CASE COLUMN_KEY WHEN 'PRI' THEN 1 ELSE 0 END) AS IsPrimaryKey,(CASE IS_NULLABLE WHEN 'YES' THEN 1 ELSE 0 END) AS IsNullable FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='{dbName}' AND TABLE_NAME='{tableName}'";
        }

        /// <summary>
        /// 根据数据库名，获取数据库中所有视图的SQL语句
        /// </summary>
        /// <param name="dbName">数据库名称</param>
        /// <returns></returns>
        public static string MySqlQueryViews(string dbName)
        {
            return $"SELECT TABLE_NAME AS ViewName,CREATE_TIME AS CreateDate,TABLE_COMMENT AS TableDescription FROM information_schema.TABLES WHERE TABLE_SCHEMA='{dbName}' AND TABLE_TYPE='VIEW'";
        }

        /// <summary>
        /// 根据视图名称查询视图脚本的SQL语句
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public static string MySqlQueryViewScript(string viewName)
        {
            return $"SELECT VIEW_DEFINITION FROM information_schema.VIEWS WHERE TABLE_NAME='{viewName}'";
        }

        /// <summary>
        /// 根据数据库名，获取数据库中所有存储过程的SQL语句
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static string MySqlQueryProcedures(string dbName)
        {
            return $"SELECT NAME AS ProcedureName,CREATED AS CreateDate,COMMENT AS ProcedureDescription FROM MYSQL.PROC WHERE DB='{dbName}' AND TYPE= 'PROCEDURE'";
        }

        /// <summary>
        /// 根据存储过程名称，获取存储过程参数的SQL语句
        /// </summary>
        /// <param name="produreName"></param>
        /// <returns></returns>
        public static string MySqlQueryProcedureColumns(string produreName)
        {
            return $"SELECT PARAMETER_NAME AS ColumnName,DATA_TYPE AS ColumnType,CHARACTER_MAXIMUM_LENGTH AS ColumnLength FROM information_schema.PARAMETERS WHERE SPECIFIC_NAME='{produreName}'";
        }

        /// <summary>
        /// 根据存储过程名称，获取存储过程创建脚本的SQL语句
        /// </summary>
        /// <param name="produreName"></param>
        /// <returns></returns>
        public static string MySqlQueryProcedureScript(string produreName)
        {
            return $"SHOW CREATE PROCEDURE {produreName}";
        }

        /// <summary>
        /// 根据数据库名称，获取数据库中所有函数名称的SQL语句
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static string MySqlQueryFunctions(string dbName)
        {
            return $"SELECT NAME AS FunctionName,CREATED AS CreateDate,COMMENT AS FunctionDescription FROM MYSQL.PROC WHERE DB='{dbName}' AND TYPE= 'FUNCTION'";
        }

        /// <summary>
        /// 根据函数名称，获取函数创建脚本的SQL语句
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public static string MySqlQueryFunctionScript(string functionName)
        {
            return $"SHOW CREATE FUNCTION {functionName}";
        }

        /// <summary>
        /// 根据数据库名，查询数据库中所有外键关系的SQL语句
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static string MySqlQueryForeignKeys(string dbName)
        {
            return $"select CONSTRAINT_NAME as ForeignKeyName, TABLE_NAME as SourceTable,COLUMN_NAME as SourceColumn, REFERENCED_TABLE_NAME as ReferencedTable,REFERENCED_COLUMN_NAME as ReferencedColumn from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where REFERENCED_TABLE_NAME is not null and CONSTRAINT_SCHEMA = '{dbName}'";
        }
    }
}

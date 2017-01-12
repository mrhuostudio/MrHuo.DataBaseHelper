using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrHuo.DataBaseHelper.SqlServer
{
    /// <summary>
    /// SqlServer 获取数据库结构所使用的 SQL 语句
    /// </summary>
    public class SqlServerSQL
    {
        /// <summary>
        /// 获取当前数据库中所有表的SQL语句
        /// </summary>
        /// <returns></returns>
        public static string SqlServerQueryTables()
        {
            return "select 'TableName'=a.name, 'TableDescription'=ISNULL(b.value,''), 'CreateDate'=c.create_date from  sysobjects AS A  left join sys.extended_properties AS B on A.id=B.major_id and B.minor_id=0  inner join sys.tables AS C on a.name=c.name where  A.xtype='U' and  A.name<>'dtproperties' AND A.name<>'sysdiagrams' order by a.id asc";
        }
        /// <summary>
        /// 获取当前数据库中所有存储过程的SQL语句
        /// </summary>
        /// <returns></returns>
        public static string SqlServerQueryProcedures()
        {
            return "select  'ProcedureName'=a.name, 'ProcedureDescription'=ISNULL(b.value,''), 'CreateDate'=c.create_date from  sysobjects AS A  left join sys.extended_properties AS B on A.id=B.major_id and B.minor_id=0  inner join sys.procedures AS C on a.name=c.name where  A.type='P' order by a.name asc"; 
        }
        /// <summary>
        /// 获取当前数据库中所有视图的SQL语句
        /// </summary>
        /// <returns></returns>
        public static string SqlServerQueryViews()
        {
            return "select  'ViewName'=a.name, 'CreateDate'=c.create_date from  sysobjects AS A  inner join sys.views AS C on a.name=c.name where  A.type='V' and a.name<>'Properties' order by a.id asc";
        }
        /// <summary>
        /// 获取当前数据库中所有函数的存储过程的SQL语句
        /// </summary>
        /// <returns></returns>
        public static string SqlServerQueryFunctions()
        {
            return "select 'FunctionName'=a.name, 'FunctionDescription'=ISNULL(b.value,''), 'FunctionType'=case when a.type='IF' then '表值函数' else '标量值函数' end, 'CreateDate'=c.create_date from sysobjects AS A left join sys.extended_properties AS B on A.id=B.major_id and B.minor_id=0 inner join sys.objects AS C on a.name=c.name where A.type='FN' OR A.type='IF' order by a.name asc";
        }
        /// <summary>
        /// 根据数据库表名查询数据库结构的SQL语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string SqlServerQueryTableByName(string tableName)
        {
            return string.Format("select  'ColumnName'=a.name, 'ColumnType'=(CASE WHEN A.length=-1 THEN 'varchar(MAX)' ELSE c.name END), 'ColumnLength'=a.length, 'ColumnDescription'=isnull(E.[value],''),  'DefaultValue'=isnull(D.text,''), 'IsIdentity'= (case when COLUMNPROPERTY(a.id,a.name,'IsIdentity')=1 then 'True' else 'False' end), 'IsPrimaryKey'= (case when exists ( SELECT 1 FROM sysobjects where xtype='PK' and parent_obj=a.id and name in  ( SELECT name FROM sysindexes WHERE indid in (  SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid  ) ) )  then 'True' else 'False' end ), 'IsNullable'=case when a.isnullable=1 then 'True'else 'False' end FROM  syscolumns AS A  inner join sysobjects AS B ON A.id=B.id left join systypes C on a.xusertype=C.xusertype left join syscomments AS D ON A.cdefault=D.id left join sys.extended_properties E on a.id=E.major_id and a.colid=E.minor_id  WHERE b.name='{0}'   ORDER BY A.id ASC", tableName);
        }
        /// <summary>
        /// 查询当前数据库中所有外键关系
        /// </summary>
        /// <returns></returns>
        public static string SqlServerQueryForeignKeys()
        {
            return ";WITH  CTE AS (SELECT  OBJECT_NAME(constraint_object_id) Constraint_Name , OBJECT_NAME(parent_object_id) Table_Name , C.name Column_Name   FROM    sys.foreign_key_columns FK INNER JOIN sys.columns C   ON FK.parent_object_id = C.object_id  AND FK.parent_column_id = C.column_id  ) SELECT  C.Constraint_Name as ForeignKeyName , C.Table_Name as SourceTable , C.Column_Name as SourceColumn , OBJECT_NAME(FK.referenced_object_id) ReferencedTable , SC.name ReferencedColumn FROM CTE C INNER JOIN sys.foreign_key_columns FK ON C.Constraint_Name = OBJECT_NAME(FK.constraint_object_id) INNER JOIN sys.columns SC ON FK.referenced_object_id = SC.object_id    AND FK.referenced_column_id = SC.column_id"; 
        }
    }
}

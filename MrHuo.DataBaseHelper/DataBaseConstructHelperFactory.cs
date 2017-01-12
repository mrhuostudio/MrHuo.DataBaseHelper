using MrHuo.DataBaseHelper.SqlServer;
using MrHuo.DataBaseHelper.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MrHuo.DataBaseHelper.SQLite;
using MrHuo.DataBaseHelper.Postgresql;

namespace MrHuo.DataBaseHelper
{
    /// <summary>
    /// 数据库结构帮助类工厂
    /// </summary>
    public class DataBaseConstructHelperFactory
    {
        /// <summary>
        /// 创建数据库结构帮助类
        /// </summary>
        /// <param name="dataBaseType"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static Interfaces.IDataBaseConstructHelper CreateDataBaseConstructHelper(DataBaseType dataBaseType, string connectionString)
        {
            switch (dataBaseType)
            {
                case DataBaseType.SqlServer:
                    return new SqlServerDataBaseConstructHelper(connectionString);
                case DataBaseType.MySql:
                    return new MySqlDataBaseConstructHelper(connectionString);
                case DataBaseType.SQLite:
                    return new SQLiteDataBaseConstructHelper(connectionString);
                case DataBaseType.Postgresql:
                    return new PostgresqlDataBaseConstructHelper(connectionString);
                case DataBaseType.Oracle:
                    break;
            }
            throw new Exception(RS.STRING_NOT_SUPPORT_DATABASE_TYPE(dataBaseType));
        }
    }
}

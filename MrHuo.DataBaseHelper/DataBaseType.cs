using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrHuo.DataBaseHelper
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DataBaseType
    {
        /// <summary>
        /// SqlServer 数据库
        /// </summary>
        SqlServer,
        /// <summary>
        /// MySql 数据库
        /// </summary>
        MySql,
        /// <summary>
        /// Oracle 数据库
        /// </summary>
        Oracle,
        /// <summary>
        /// SQLite 数据库
        /// </summary>
        SQLite,
        /// <summary>
        /// Postgresql 数据库
        /// </summary>
        Postgresql
    }
}

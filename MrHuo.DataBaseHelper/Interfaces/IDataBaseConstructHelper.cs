using MrHuo.DataBaseHelper.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MrHuo.DataBaseHelper.Interfaces
{
    /// <summary>
    /// 数据库结构获取接口
    /// </summary>
    public interface IDataBaseConstructHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        string ConnectionString { get; set; }
        /// <summary>
        /// 获取表中的所有列
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<IEnumerable<Column>> GetColumns(string tableName);
        /// <summary>
        /// 获取数据库中所有表
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Table>> GetTables();
        /// <summary>
        /// 获取数据库中所有视图
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<View>> GetViews();
        /// <summary>
        /// 获取视图的脚本
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        Task<string> GetViewScript(string viewName);
        /// <summary>
        /// 获取数据库中所有函数
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Function>> GetFunctions();
        /// <summary>
        /// 获取函数的脚本
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        Task<string> GetFunctionScript(string functionName);
        /// <summary>
        /// 获取数据库中所有存储过程
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Procedure>> GetProcedures();
        /// <summary>
        /// 获取存储过程的脚本
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        Task<string> GetProcedureScript(string procedureName);
        /// <summary>
        /// 获取数据库中所有外键关系
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ForeignKey>> GetForeignKeys();
        /// <summary>
        /// 获取视图列
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        Task<IEnumerable<ViewColumn>> GetViewColumns(string viewName);
        /// <summary>
        /// 获取存储过程参数列表
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcedureColumn>> GetProcedureColumns(string procedureName);
    }
}

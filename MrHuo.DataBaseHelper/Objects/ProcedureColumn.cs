using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrHuo.DataBaseHelper.Objects
{
    /// <summary>
    /// 存储过程参数列名
    /// </summary>
    public class ProcedureColumn
    {
        /// <summary>
        /// 存储过程参数名称
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 存储过程参数类型
        /// </summary>
        public string ColumnType { get; set; }
        /// <summary>
        /// 存储过程参数长度
        /// </summary>
        public string ColumnLength { get; set; }
        /// <summary>
        /// 获取 C# 类型
        /// </summary>
        public virtual string CSharpType
        {
            get
            {
                return this.ColumnType.ToCSharpType();
            }
        }
    }
}

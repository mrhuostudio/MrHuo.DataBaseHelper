using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrHuo.DataBaseHelper.Objects
{
    /// <summary>
    /// 视图列
    /// </summary>
    public class ViewColumn
    {
        /// <summary>
        /// 视图列名称
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 视图列类型
        /// </summary>
        public string ColumnType { get; set; }
        /// <summary>
        /// 视图列长度
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

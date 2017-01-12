using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrHuo.DataBaseHelper.Objects
{
    /// <summary>
    /// 表示数据表结构的属性类
    /// </summary>
    [Serializable]
    public class Column
    {
        /// <summary>
        /// 获取或者设置一个值，该值表示表列名
        /// </summary>
        public String ColumnName { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示列类型
        /// </summary>
        public String ColumnType { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示列长度
        /// </summary>
        public String ColumnLength { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示列描述
        /// </summary>
        public String ColumnDescription { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示列默认值
        /// </summary>
        public String DefaultValue { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示列是否标示列
        /// </summary>
        public Boolean IsIdentity { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示列是否为主键
        /// </summary>
        public Boolean IsPrimaryKey { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示列是否可以为空
        /// </summary>
        public Boolean IsNullable { get; set; }
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

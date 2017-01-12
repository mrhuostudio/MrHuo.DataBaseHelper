using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrHuo.DataBaseHelper.Objects
{
    /// <summary>
    /// 表示数据库外间关系的属性类
    /// </summary>
    [Serializable]
    public class ForeignKey
    {
        /// <summary>
        /// 获取或者设置一个值，该值表示主键名称
        /// </summary>
        public String ForeignKeyName { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示主键所在表
        /// </summary>
        public String SourceTable { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示主键列名称
        /// </summary>
        public String SourceColumn { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示引用的表名
        /// </summary>
        public String ReferencedTable { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示引用的列名
        /// </summary>
        public String ReferencedColumn { get; set; }
    }
}

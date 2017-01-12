using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrHuo.DataBaseHelper.Objects
{
    /// <summary>
    /// 表示数据库中表的属性类
    /// </summary>
    [Serializable]
    public class Table
    {
        /// <summary>
        /// 获取或者设置一个值，该值表示表名
        /// </summary>
        public String TableName { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示表的描述
        /// </summary>
        public String TableDescription { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示表的创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}

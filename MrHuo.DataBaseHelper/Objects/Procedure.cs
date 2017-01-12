using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrHuo.DataBaseHelper.Objects
{
    /// <summary>
    /// 表示数据库中存储过程的属性类
    /// </summary>
    [Serializable]
    public class Procedure
    {
        /// <summary>
        /// 获取或者设置一个值，该值表示存储过程名称
        /// </summary>
        public String ProcedureName { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示存储过程描述
        /// </summary>
        public String ProcedureDescription { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示存储过程创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}

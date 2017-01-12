using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrHuo.DataBaseHelper.Objects
{
    /// <summary>
    /// 标示数据库中函数的属性类
    /// </summary>
    [Serializable]
    public class Function
    {
        /// <summary>
        /// 获取或者设置一个值，该值表示函数名称
        /// </summary>
        public String FunctionName { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示函数描述
        /// </summary>
        public String FunctionDescription { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示函数类型
        /// </summary>
        public String FunctionType { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示函数创建日期
        /// </summary>
        public String CreateDate { get; set; }
    }
}

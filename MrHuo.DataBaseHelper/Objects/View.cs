using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrHuo.DataBaseHelper.Objects
{
    /// <summary>
    /// 表示数据库中视图的属性类
    /// </summary>
    [Serializable]
    public class View
    {
        /// <summary>
        /// 获取或者设置一个值，该值表示视图名称
        /// </summary>
        public String ViewName { get; set; }
        /// <summary>
        /// 获取或者设置一个值，该值表示视图创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}

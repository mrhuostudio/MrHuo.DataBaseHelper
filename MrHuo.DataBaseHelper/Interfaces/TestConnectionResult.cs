using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrHuo.DataBaseHelper.Interfaces
{
    /// <summary>
    /// 测试连接结果
    /// </summary>
    public class TestConnectionResult
    {
        /// <summary>
        /// 测试是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 测试结果
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}

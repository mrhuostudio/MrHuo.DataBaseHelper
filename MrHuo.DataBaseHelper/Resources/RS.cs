using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;

namespace MrHuo.DataBaseHelper
{
    /// <summary>
    /// 内部资源管理器
    /// </summary>
    internal class RS
    {
        /// <summary>
        /// 资源字符串，DLL不存在
        /// </summary>
        /// <param name="dataBaseType"></param>
        /// <returns></returns>
        public static string STRING_DB_DLL_NOT_EXISTS(DataBaseType dataBaseType)
        {
            return string.Format("DLL {0} does not exist!",dataBaseType);
        }

        /// <summary>
        /// 资源字符串，测试连接成功
        /// </summary>
        /// <returns></returns>
        public static string STRING_TEST_CONNECTION_SUCCESS()
        {
            return "Test connect successful!";
        }

        /// <summary>
        /// 资源字符串，不支持的数据库类型
        /// </summary>
        /// <returns></returns>
        public static string STRING_NOT_SUPPORT_DATABASE_TYPE(DataBaseType dataBaseType)
        {
            return string.Format("Not Support DataBaseType.{0}!", dataBaseType);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 函数式的判断语句
    /// </summary>
    public static class Assert
    {
        /// <summary>
        /// 如果参数 obj 为 null，则执行 callback
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="callback"></param>
        public static void IfNull(object obj, Action callback)
        {
            If(obj == null, callback);
        }

        /// <summary>
        /// 如果条件 b 为 true，则执行 callback，如果 b 为 false，falseCallback 不等于 null， 则执行 falseCallback
        /// </summary>
        /// <param name="b"></param>
        /// <param name="trueCallback"></param>
        /// <param name="falseCallback"></param>
        public static void If(bool b, Action trueCallback, Action falseCallback = null)
        {
            if (b)
            {
                trueCallback?.Invoke();
            }
            else
            {
                falseCallback?.Invoke();
            }
        }
  
        /// <summary>
        /// try...catch
        /// </summary>
        /// <param name="try"></param>
        /// <param name="catch"></param>
        /// <param name="finally"></param>
        public static void TryCatch(Action @try, Action<Exception> @catch = null, Action @finally = null)
        {
            try
            {
                @try?.Invoke();
            }
            catch (Exception ex)
            {
                @catch?.Invoke(ex);
            }
            finally
            {
                @finally?.Invoke();
            }
        }
    }
}

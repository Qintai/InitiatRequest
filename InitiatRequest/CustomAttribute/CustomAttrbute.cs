using System;
using System.Reflection;

namespace InitiatRequest
{
    // 3个类：Error-Executing-Executed

    /// <summary>
    /// 方法中发生错误调用
    /// </summary>
    public class Error : Attribute
    {
        public Error()
        {}

        /// <summary>
        /// 方法过程中发生错误
        /// </summary>
        /// <param name="e"></param>
        public void Execute(Exception e)
        {
            // filterContext.ExceptionHandled = true;//表示异常已经被我处理了，MVC中这样用。
            log4helper<Error>.Debug("Filter="+e.Message);
        }
    }

    /// <summary>
    /// 方法执行前
    /// </summary>
    public class Executing : Attribute//方法执行前 
    {
        public Executing()
        { }

        /// <summary>
        /// 方法执行前
        /// </summary>
        /// <param name="par"></param>
        public void Execute(MethodInfo methodInfo)
        {
            string a = "";
            //log4helper<Executing>.Info($"准备进入{methodInfo.Name}方法1");
            //log4helper<Executing>.Info($"准备进入{methodInfo.Name}方法2");
            //log4helper<MethodInfo>.Info($"准备进入{methodInfo.Name}方法3");
        }
    }

    /// <summary>
    /// 方法执行后
    /// </summary>
    public class Executed : Attribute
    {
        /// <summary>
        /// 方法执行后
        /// </summary>
        /// <param name="par"></param>
        public void Execute(MethodInfo methodInfo)
        {
            string a = "";
        }
    }



}
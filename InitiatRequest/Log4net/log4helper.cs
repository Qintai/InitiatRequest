using System;
using System.Collections.Generic;

namespace InitiatRequest
{
    public  class log4helper<T>
    {
        static log4net.ILog log;

        static Dictionary<string, Type> keyValues = new Dictionary<string, Type>();

        /// <summary>
        /// 静态缓存。保存这个类型第一次被反射的结果，只会执行一次。除非T类型有更换
        /// </summary>
        static log4helper()
        {
            Type t = typeof(T);
             log = log4net.LogManager.GetLogger("NETRepository", t);
            keyValues.Add(t.Name,t);
        }

        public static void Info(string str)
        {
            log.Info(str);
        }
        public static void Errror(Exception e)
        {
            log.Error(e);
        }

        public static void Debug(string s)
        {
            log.Debug(s);
        }
    }
}

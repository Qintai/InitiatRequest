using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace InitiatRequest
{
    /// <summary>
    ///调用这个方法，不要放在调用者 的构造函数中，否则会出现死循环
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TypeTes
    {
        static Dictionary<string, Type> valuePairs = new Dictionary<string, Type>();

        static Type type;

        /// <summary>
        /// 反射执行方法。会缓存Type，只是第一次反射后，第二次就不需要反射了
        /// </summary>
        /// <param name="type_name">实例名称</param>
        /// <param name="mode">实例</param>
        /// <param name="memname">你要执行的方法名</param>
        public static void exetype(string type_name,object mode,string memname)
        {
            if (!valuePairs.Keys.Contains(type_name))
            {
                type = mode.GetType();
                valuePairs.Add(type_name, type);
            }
            type = valuePairs[type_name];
            if (type == null)
                valuePairs[type_name]= type = mode.GetType();

            MethodInfo methodInfo = type.GetMethod(memname);
            if (methodInfo == null)
                throw new Exception($"没找到方法{memname}");

            IEnumerable<Attribute> obj = methodInfo.GetCustomAttributes(); //这一句，它里面的实现，会把这个方法的特性，实例化一下，如果在逻辑写在特性的构造方法中，那么这一句就足够
            foreach (Attribute item in obj)
            {
                if (item is Executing executing1)
                {
                    executing1.Execute(methodInfo);
                    methodInfo.Invoke(mode, null);
                    break;
                }
                if (item is Executed executing2)
                {
                    methodInfo.Invoke(mode, null);
                    executing2.Execute(methodInfo);
                    break;
                }
                if (item is Error executing3)
                {
                    var pssr = methodInfo.GetParameters();
                    try
                    {
                         methodInfo.Invoke(mode, null);
                    }
                    catch (Exception e)
                    {
                         executing3.Execute(e);
                        MessageBox.Show("出现异常！请查看日志");
                        break;
                    }
                }
            }
        }

        //有返回值
        public static object rexetype(string type_name, object mode, string memname)
        {
            type = valuePairs[type_name];
            if (type == null)
                type = mode.GetType();

            object oobj=null;
            MethodInfo methodInfo = type.GetMethod(memname); // 只能找出 public 的方法，私有的请加限制
            if (methodInfo == null)
                throw new Exception($"没找到方法{memname}");

            IEnumerable<Attribute> obj = methodInfo.GetCustomAttributes();
            foreach (Attribute item in obj)
            {
                if (item is Executing executing1)
                {
                    executing1.Execute(methodInfo);
                    oobj= methodInfo.Invoke(mode, null);
                    break;
                }
                if (item is Executed executing2)
                {
                    oobj = methodInfo.Invoke(mode, null);
                    executing2.Execute(methodInfo);
                    break;
                }
                if (item is Error executing3)
                {
                    try
                    {
                        oobj = methodInfo.Invoke(mode, null);
                    }
                    catch (Exception e)
                    {
                        executing3.Execute(e);
                    }
                    break;
                }
            }
            return oobj;
        }
    }
}


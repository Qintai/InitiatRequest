using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
using System.Threading.Tasks;

namespace InitiatRequest
{
    public class DataSend
    {

        static ISqlSugarClient sclient = new SqlSugarClient(new ConnectionConfig()
        {
            //这个地方还需要换成，当前目录！！！
            ConnectionString = @"DataSource=F:\VSGitHub\InitiatRequest\InitiatRequest\bin\dat.db",
            DbType = (SqlSugar.DbType)DbType.Sqlite,//设置数据库类型
            IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
            InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
        });

        public void send()
        {
            sclient.DbMaintenance.CreateDatabase(); //创建数据库
            sclient.CodeFirst.InitTables(typeof(ConfigTable));
            SimpleClient simpleClient = new SimpleClient(sclient);
            List<ConfigTable> userlist = new List<ConfigTable>()
            {
                   new ConfigTable(){  Name="接口1",AddTime=DateTime.Now,Url="4324"},
                   new ConfigTable(){  Name="接口2",AddTime=DateTime.Now,Url="432432"},
            };
            simpleClient.InsertRange(userlist);
        }

        public Dictionary<string, string> readsqll()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            List<ConfigTable> list = sclient.Queryable<ConfigTable>().ToList();
            foreach (ConfigTable item in list)
            {
                try
                {
                    keyValuePairs.Add(item.Name,item.Url);
                }
                catch (Exception)
                {
                    break;
                }
            }
            return keyValuePairs;
        }
    }

   public class ConfigTable
    {
        [SugarColumn]
        public string Name { get;  set; }

        [SugarColumn]
        public string Url { get;  set; }

        [SugarColumn]
        public DateTime AddTime { get;  set; }

    }
}

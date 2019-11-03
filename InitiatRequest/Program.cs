using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace InitiatRequest
{
    static class Program
    {

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            #region 启动日志
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Config\\Log4net.config";
            if (!File.Exists(filePath))
                throw new Exception($"没找到配置文件");
            FileInfo fileInfo = new FileInfo(filePath);
            log4net.Repository.ILoggerRepository logger = log4net.LogManager.CreateRepository("NETRepository");//创建日志仓库，名字自定义
            log4net.Config.XmlConfigurator.Configure(logger, fileInfo);//配置
            #endregion

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                zmain zmodel = new zmain();
                zmodel.model = zmodel;
                zmodel.Start();
  //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);//处理未捕获的异常

                //处理UI线程异常
                Application.ThreadException += new ThreadExceptionEventHandler(
                    (sender, e)=> 
                    {
                        //log4helper<Application>.Errror(e.Exception);
                        MessageBox.Show(/*"出现异常！请查看日志"+*/ e.Exception.Message);
                    });

                //处理非UI线程异常
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler((sender,e)=>  {});
                Application.Run(zmodel);
            }
            catch (Exception e)
            {
                log4helper<Application>.Errror(e);
                MessageBox.Show("出现异常！请查看日志");
                return;
            }
        }





    }
}





using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InitiatRequest
{

    public partial class zmain : CCWin.Skin_Mac
    {
        //public static event Action initialization;
        public static Dictionary<string, string> dor = new Dictionary<string, string>();

        /// <summary>
        ///  post/get
        /// </summary>
        public string selectres = "POST";

        /// <summary>
        /// 这个变量存本身
        /// </summary>
        public zmain model;

        public zmain()
        {
            InitializeComponent();
            //initialization += InitData;
            //initialization();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        //初始化基本信息
        //[Executing, Error]  //同时这样写，只能响应前面一个特性
        [Error]//单独写捕获异常
        public void InitData()
        {
            xiangyin.Text = "代码敲不好，浑身都不好";
            formd.ReadOnly = true;
            action.TabIndex = 0;
            action.Focus();

            cBox.SelectedValueChanged += (a, b)=>
            {
                SkinComboBox box=(SkinComboBox)a;
                action.Text=dor[box.Text];
            };

            readE.Click+= (a, e) => 
            {
                // 读取Excel文件内容，就可以赋值给控件，控件下拉展示出来
                dor = new fetchExcel().readexcel(AppDomain.CurrentDomain.BaseDirectory + "DataSouse.xlsx");
                foreach (var item in dor)
                {
                    if (!cBox.Items.Contains(item.Key))
                       cBox.Items.Add(item.Key);
                }
                if (dor.Count == 0)
                {
                    //默认值
                    action.Text = "http://bgtest.vzan.com/nlive/GetTopicanalysis?zid=75999&tid=140789540";  //
                    txt_cook.Text = @"UserCookieNew,52359ffe-a9ec-4ac9-b887-b5e30af0273f;";
                }
            };

            skinButtonlog.Click += (a, b) =>
            {
                // 打开存放日志的文件夹
                string v_OpenFolderPath = AppDomain.CurrentDomain.BaseDirectory + "Logs";
                System.Diagnostics.Process.Start("explorer.exe", v_OpenFolderPath);
            };

            webBrowser1.Navigate("http://www.ofmonkey.com/");
            //webBrowser1.ScriptErrorsSuppressed = true;
            bpost.Checked = true; //默认Post的
            accept.Text = "*/*";
            contentType.Text = "application/x-www-form-urlencoded";
            //      but.Click += GetRequest(); //原始调用方法
            but.Click += (EventHandler)TypeTes.rexetype("zmain", model, "GetRequest");//反射调用方法

            action.TextChanged += (a, e) => GetPars();
            //action.MouseLeave += (a, e) => GetPars();

            bpost.CheckedChanged += (a, e) =>
            {
                var mide = a as RadioButton;
                selectres = mide.Checked ? "POST" : "GET";
            };
            host.TextChanged += (a, e) =>
            {
                var box = a as TextBox;
                if (!string.IsNullOrEmpty(box.Text))
                    txt_cook.Enabled = true;
                else
                    txt_cook.Enabled = false;
            };
        }

        //及时，赋予参数
        public void GetPars()
        {
            var acti = action.Text.Split('?');
            try
            {
                formd.Text = acti[1];
            }
            catch (Exception)
            {
                formd.Text = "";
                return;
            }
        }


        /// <summary>
        /// 初始化基本信息，给控件赋初值
        /// </summary>
        internal void Start()
        {
            //InitData();//原始调用方法
            TypeTes.exetype("zmain", model, "InitData");   //反射调用方法
        }

        /// <summary>
        /// zid:75999;tid:140789540;
        /// 把这种格式的换成 zid=75999&tid=140789540
        /// </summary>
        /// <returns></returns>
        public string sert()
        {
            var text = formd.Text.Replace(';', '&');
            string[] foda = text.Split('&');
            for (int i = 0; i < foda.Length; i++)
                foda[i] = foda[i].Replace(':', '=');
            text = string.Join("&", foda);
            return text;
        }


        [Error]
        public EventHandler GetRequest()
        {
           xiangyin.Text = "响应中^^^^^^^^^^^^^^^^^^";
            string ma = Thread.CurrentThread.ManagedThreadId.ToString();
            return (sender, e) =>
            {
                // 检测链接中是否有http或者https
                bool a = action.Text.Contains("http");
                bool b = action.Text.Contains("https");
                if (!a && !b)
                {
                    MessageBox.Show("接口前面的地址没有http或者https");
                    return;
                }
                string mr = Thread.CurrentThread.ManagedThreadId.ToString();

                //！！！如果是Get请求，最好需要加入  x-requested-with: XMLHttpRequest


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(action.Text);
                request.Method = selectres;
                request.Accept = accept.Text;
                request.ContentType = contentType.Text;

                if (!string.IsNullOrEmpty(textBo_heads.Text))
                {
                    string[] dataSouse = textBo_heads.Text.Split(';');
                    foreach (var item in dataSouse)
                        request.Headers.Add(item);
                    request.Host = host.Text;
                }

                #region Cookies
                if (!string.IsNullOrEmpty(request.Host))
                {
                    Dictionary<string, string> dir = new Dictionary<string, string>();
                    string[] cks = txt_cook.Text.Split(';');
                    foreach (string item in cks)
                    {
                        string[] mm = item.Split(',');
                        if (mm.Length == 2)
                            dir.Add(mm[0], mm[1]);
                    }
                    CookieContainer cookcon = new CookieContainer();
                    if (!request.Host.Contains("localhost"))
                    {
                        foreach (var item in dir)
                            cookcon.Add(new Cookie(item.Key, item.Value, "/", request.Host)); //domain不能为空，domain不能等于localhost，localhost不是域名
                        request.CookieContainer = cookcon;
                    }

                }
                #endregion

                var htmlelement = webBrowser1.Document.GetElementById("json_text");
                Encoding encoding = Encoding.UTF8;
                if (!string.IsNullOrEmpty(formd.Text) && "POST" == request.Method) //post请求时，使用的，否则get请求时，
                {
                    // 如果是以流的方式提交表单数据的时候不能使用get方法，必须用post方法，
                    byte[] buffer = encoding.GetBytes(formd.Text);
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);  //Get请求-报错
                }

                // 异步请求，避免卡主线程
                BenginAsyncCallback(request, htmlelement);

                // 异步请求，避免卡主线程
                //BenginEndInvoke(request, htmlelement);

                //新进程去执行
                //BenginTask(request, htmlelement);

                #region request.BeginGetResponse，拿不到异步执行之后的结果
                //{
                //    var coll = webBrowser1.Document.GetElementsByTagName("button"); //再点击按钮格式化。这一句要放到外面，在异步的里面，报错
                //     AsyncCallback asyncCallback = okm => {
                //         HttpWebResponse response = asyncCallback.EndInvoke(okm);;
                //
                //         using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                //         {
                //             htmlelement.InnerHtml = reader.ReadToEnd(); //OuterText属性赋值后，没有效果
                //             if (string.IsNullOrEmpty(htmlelement.InnerHtml))
                //             {
                //                 htmlelement.InnerHtml = "没有内容";
                //                 return;
                //             }
                //             foreach (HtmlElement item in coll)
                //             {
                //                 if (item.InnerText == "格式化")//if (item.GetAttribute("class") == "btn-primary") //没有找到
                //                 {
                //                     item.InvokeMember("click");
                //                     xiangyin.Text = "响应OK";
                //                     return;
                //                 }
                //             }
                //         }
                //     };
                //     request.BeginGetResponse(asyncCallback, null);
                //}
                #endregion

                #region 同步方法，某个接口的请求时间长，卡主线程
                /*  HttpWebResponse response = (HttpWebResponse)request.GetResponse(); // 耗时间  的，卡UI线程
                  using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                  {
                      htmlelement.InnerHtml = reader.ReadToEnd(); //OuterText属性赋值后，没有效果
                      if (string.IsNullOrEmpty(htmlelement.InnerHtml))
                      {
                          htmlelement.InnerHtml = "没有内容";
                          return;
                      }
                      var coll = webBrowser1.Document.GetElementsByTagName("button"); //再点击按钮格式化
                      foreach (HtmlElement item in coll)
                      {
                          if (item.InnerText == "格式化")//if (item.GetAttribute("class") == "btn-primary") //没有找到
                          {
                              item.InvokeMember("click");
                              xiangyin.Text = "响应OK";
                              return;
                          }
                      }
                  }*/
                #endregion

            };
        }

        private void BenginEndInvoke(HttpWebRequest request, HtmlElement htmlelement)
        {
            var coll = webBrowser1.Document.GetElementsByTagName("button"); //再点击按钮格式化。这一句要放到外面，在异步的里面，报错
            Func<HttpWebResponse> func = () => (HttpWebResponse)request.GetResponse();
            //HttpWebResponse response = func.EndInvoke(null);             //写null的话，会导致error“ 异步结果对象为空或属于意外类型。”
            IAsyncResult asyncResult = func.BeginInvoke(back => { }, null);
            asyncResult.AsyncWaitHandle.WaitOne(10000);//最多等待10秒钟
            HttpWebResponse response = func.EndInvoke(asyncResult);
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                htmlelement.InnerHtml = reader.ReadToEnd(); //OuterText属性赋值后，没有效果
                if (string.IsNullOrEmpty(htmlelement.InnerHtml))
                {
                    htmlelement.InnerHtml = "没有内容";
                    return;
                }
                foreach (HtmlElement item in coll)
                {
                    if (item.InnerText == "格式化")//if (item.GetAttribute("class") == "btn-primary") //没有找到
                    {
                        item.InvokeMember("click");
                        xiangyin.Text = "响应OK";
                        return;
                    }
                }
            }
        }

        public void BenginAsyncCallback(HttpWebRequest request, HtmlElement htmlelement)
        {
            var coll = webBrowser1.Document.GetElementsByTagName("button"); //再点击按钮格式化。这一句要放到外面，在异步的里面，报错
            Func<HttpWebResponse> func = () =>
              {
                  try
                  {
                      return (HttpWebResponse)request.GetResponse();
                  }
                  catch (Exception ex)
                  {
                      htmlelement.InnerHtml = ex.Message;
                      MessageBox.Show(ex.Message);
                      Thread.CurrentThread.Abort(); //避免异常时，窗体退出，整个线程包括主线程退出。退出当前子线程
                      throw ex;
                  }
              };
            AsyncCallback asyncCallback = okm =>
            {
                HttpWebResponse response = func.EndInvoke(okm);
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    htmlelement.InnerHtml = reader.ReadToEnd(); //OuterText属性赋值后，没有效果
                    if (string.IsNullOrEmpty(htmlelement.InnerHtml))
                    {
                        htmlelement.InnerHtml = "没有内容";
                        return;
                    }
                    foreach (HtmlElement item in coll)
                    {
                        if (item.InnerText == "格式化")//if (item.GetAttribute("class") == "btn-primary") //没有找到
                        {
                            item.InvokeMember("click");
                            xiangyin.Text = "响应OK"; //这一句话，可以放到委托中的回调里面
                            return;
                        }
                    }
                }
            };
            try
            {
                IAsyncResult rrest = func.BeginInvoke(asyncCallback, null);
            }
            catch (Exception)
            {
                return;
            }

            // rrest.AsyncWaitHandle.WaitOne(10000);//最多等待10秒钟
        }

        public void BenginTask(HttpWebRequest request, HtmlElement htmlelement)
        {
            var coll = webBrowser1.Document.GetElementsByTagName("button"); //再点击按钮格式化
            Task task = Task.Run(() =>
             {
                 HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                 using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                 {
                     htmlelement.InnerHtml = reader.ReadToEnd(); //OuterText属性赋值后，没有效果
                     if (string.IsNullOrEmpty(htmlelement.InnerHtml))
                     {
                         htmlelement.InnerHtml = "没有内容";
                         return;
                     }
                     foreach (HtmlElement item in coll)
                     {
                         if (item.InnerText == "格式化")//if (item.GetAttribute("class") == "btn-primary") //没有找到
                         {
                             item.InvokeMember("click");
                             xiangyin.Text = "响应OK";
                             return;
                         }
                     }
                 }
             });
        }

    }
}

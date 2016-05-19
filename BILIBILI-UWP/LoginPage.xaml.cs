using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Newtonsoft.Json;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace BILIBILI_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public delegate void DeUpdate();
        public event DeUpdate UpdateUserInfo;

        public LoginPage()
        {
            this.InitializeComponent();
            login_Loading.Visibility = Visibility.Collapsed;
            login_Content.Visibility = Visibility.Visible;
            
        }
        private HttpClient hc;
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (hc != null)
            {
                hc.Dispose();
                hc = null;
            }
        }
        #region 测试时用的方法
        //public async void Login()
        //{
        //    try
        //    {
        //        login_Loading.Visibility = Visibility.Visible;
        //        login_Content.Visibility = Visibility.Collapsed;
        //        hc = new HttpClient();
        //        HttpResponseMessage hr = await hc.GetAsync(new Uri("https://api.bilibili.com/login?appkey=422fd9d7289a1dd9&platform=wp&pwd=" + Login_Pass.Password + "&type=json&userid=" + Login_User.Text.Trim()));
        //        hr.EnsureSuccessStatusCode();
        //        string results = await hr.Content.ReadAsStringAsync();
        //        string acc_key = "";
        //        try
        //        {
        //            JObject josn = JObject.Parse(results);
        //            DateTime dtStart = new DateTime(1970, 1, 1);
        //            //long lTime = long.Parse((long)josn["expires"] + "0000000");
        //            //long lTime = long.Parse(textBox1.Text);
        //            //TimeSpan toNow = new TimeSpan(lTime);
        //            //string i = dtStart.Add(toNow).ToString();
        //            //Login_Results.Text = "用户ID=" + josn["mid"].ToString() + "\r\n密钥=" + josn["access_key"].ToString() + "\r\n到期时间：" + i; ;
        //            acc_key = josn["access_key"].ToString();
        //            HttpResponseMessage hr2 = await hc.GetAsync(new Uri("http://api.bilibili.com/login/sso?&access_key=" + acc_key + "&appkey=422fd9d7289a1dd9&platform=wp"));
        //            hr2.EnsureSuccessStatusCode();
        //            //Login_Results.Text = await hr2.Content.ReadAsStringAsync();
        //            hr2.Dispose();
        //        }
        //        catch (Exception)
        //        {
        //            MessageDialog md = new MessageDialog("登录失败！请检查用户名或密码是否正确！");
        //            await md.ShowAsync();
        //            login_Loading.Visibility = Visibility.Collapsed;
        //            login_Content.Visibility = Visibility.Visible;
        //        }
        //        try
        //        {
        //            if (JObject.Parse(Login_Results.Text).HasValues == true)
        //            {
        //                MessageDialog md = new MessageDialog("登录失败！其它问题！");
        //                await md.ShowAsync();
        //                login_Loading.Visibility = Visibility.Collapsed;
        //                login_Content.Visibility = Visibility.Visible;
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            MessageDialog md = new MessageDialog("登录成功！");
        //            await md.ShowAsync();
        //            hr.Dispose();
        //            this.Frame.GoBack();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Login_Results.Text = ex.Message;
        //    }


        //}

        //登录方法
        #endregion
       /// <summary>
       /// 登录方法
       /// </summary>
        public async void loginBilibili()
        {
            try
            {
                //加载
                login_Loading.Visibility = Visibility.Visible;
                login_Content.Visibility = Visibility.Collapsed;
                //发送第一次请求，得到access_key
                hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("https://api.bilibili.com/login?appkey=422fd9d7289a1dd9&platform=wp&pwd=" + Login_Pass.Password + "&type=json&userid=" + Login_User.Text.Trim()));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                //Json解析及数据判断
                LoginModel model = new LoginModel();
                model = JsonConvert.DeserializeObject<LoginModel>(results);
                if (model.code == -627)
                {
                    MessageDialog md = new MessageDialog("登录失败，密码错误！");
                    await md.ShowAsync();
                    login_Loading.Visibility = Visibility.Collapsed;
                    login_Content.Visibility = Visibility.Visible;
                    return;
                }
                if (model.code == -626)
                {
                    MessageDialog md = new MessageDialog("登录失败，账号不存在！");
                    await md.ShowAsync();
                    login_Loading.Visibility = Visibility.Collapsed;
                    login_Content.Visibility = Visibility.Visible;
                    return;
                }
                if (model.code == -1)
                {
                    MessageDialog md = new MessageDialog("登录失败，程序注册失败！请联系作者！");
                    await md.ShowAsync();
                    login_Loading.Visibility = Visibility.Collapsed;
                    login_Content.Visibility = Visibility.Visible;
                    return;
                }
                if (model.code == 0)
                {
                    HttpResponseMessage hr2 = await hc.GetAsync(new Uri("http://api.bilibili.com/login/sso?&access_key=" + model.access_key + "&appkey=422fd9d7289a1dd9&platform=wp"));
                    hr2.EnsureSuccessStatusCode();
                }
                //看看存不存在Cookie
                HttpBaseProtocolFilter hb = new HttpBaseProtocolFilter();
                HttpCookieCollection cookieCollection = hb.CookieManager.GetCookies(new Uri("http://bilibili.com/"));
                List<string> ls = new List<string>();
                foreach (HttpCookie item in cookieCollection)
                {
                    ls.Add(item.Name);
                }
                if (ls.Contains("DedeUserID"))
                {
                    
                    this.Frame.GoBack();
                }
                else
                {
                    MessageDialog md = new MessageDialog("登录失败！");
                    await md.ShowAsync();
                    login_Loading.Visibility = Visibility.Collapsed;
                    login_Content.Visibility = Visibility.Visible;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog("登录发生错误！\r\n"+ex.Message);
                await md.ShowAsync();
                login_Loading.Visibility = Visibility.Collapsed;
                login_Content.Visibility = Visibility.Visible;
                return;
            }
            UpdateUserInfo();
        }
        /// <summary>
        /// 点击登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            if (Login_User.Text.Length == 0)
            {
                MessageDialog md = new MessageDialog("账号或密码不能为空！");
                await md.ShowAsync();
            }
            else
            {
                loginBilibili();
            }
            
        }
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //读取Cookie，看看登录没有
            /*B站Cookie的域名如下
                bilibili.com
                bilibibi.cn
                biligame.com
                bilibiliyoo.com
                im9.com
                本应用只使用了bilibili.com，所以只操作bilibili.com的Cookie
            */
            HttpBaseProtocolFilter hb = new HttpBaseProtocolFilter();
            HttpCookieCollection cookieCollection = hb.CookieManager.GetCookies(new Uri("http://bilibili.com/"));
            List<string> ls = new List<string>();
            foreach (HttpCookie item in cookieCollection)
            {
                ls.Add(item.Name);
            }
            //存在证明登录了，禁止访问
            if (ls.Contains("DedeUserID"))
            {
                this.Frame.GoBack();
            }
        }

        private void Login_Pass_LostFocus(object sender, RoutedEventArgs e)
        {
            KanZheNi.Visibility = Visibility.Visible;
            BuKanZheNi.Visibility = Visibility.Collapsed;
        }

        private void Login_Pass_GotFocus(object sender, RoutedEventArgs e)
        {
            KanZheNi.Visibility = Visibility.Collapsed;
            BuKanZheNi.Visibility = Visibility.Visible;
        }
    }

    //这个Model用来保存登录请求的access_key
    public class LoginModel
    {
        private string _access_key;
        public string access_key
        {
            get { return _access_key; }
            set { _access_key = value; }
        }
        public string mid { get; set; }
        public int code { get; set; }
        public string expires
        {
            get;set;
        }
    }
}

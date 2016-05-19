using BILIBILI_UWP.PartPage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;


// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace BILIBILI_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        
        public HomePage()
        {
            this.InitializeComponent();
            SetHomeInfo();
            NavigationCacheMode = NavigationCacheMode.Required;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            (rootFrame.Content as MainPage).heh += HomePage_heh;
        }

        
        public  void SetListView(string results, GridView ls,bool isBanner)
        {
            try
            {
                if (isBanner)
                {
                    BannerModel model = new BannerModel();
                    model = JsonConvert.DeserializeObject<BannerModel>(results);
                    List<BannerModel> ban = JsonConvert.DeserializeObject<List<BannerModel>>(model.list.ToString());
                    home_flipView.ItemsSource = ban;
                }
                else
                {
                    InfoModel model = new InfoModel();
                    model = JsonConvert.DeserializeObject<InfoModel>(results);
                    List<InfoModel> ban = JsonConvert.DeserializeObject<List<InfoModel>>(model.list.ToString());
                    ls.ItemsSource = ban;
                }
            }
            catch (Exception)
            {
            }
        }
        WebClientClass wc = new WebClientClass();
        public async void SetHomeInfo()
        {
            //WebClientClass.
            
            string banner = await wc.GetResults(new Uri("http://www.bilibili.com/index/slideshow.json"));
            SetListView(banner, null,true);

            string dh = await wc.GetResults(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=1&page=1&pagesize=20&order=hot&ver=2"));
            SetListView(dh, home_GridView_DH, false);
            string fj = await wc.GetResults(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=13&page=1&pagesize=20&order=hot&ver=2"));
            SetListView(fj, home_GridView_FJ, false);
            string yy = await wc.GetResults(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=3&page=1&pagesize=20&order=hot&ver=2"));
            SetListView(yy, home_GridView_YYWD, false);
            string wd = await wc.GetResults(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=20&page=1&pagesize=20&order=hot&ver=2"));
            SetListView(wd, home_GridView_WD, false);
            string yx = await wc.GetResults(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=4&page=1&pagesize=20&order=hot&ver=2"));
            SetListView(yx, home_GridView_YX, false);
            string kj = await wc.GetResults(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=36&page=1&pagesize=20&order=hot&ver=2"));
            SetListView(kj, home_GridView_KJ, false);
            string YL = await wc.GetResults(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=5&page=1&pagesize=20&order=hot&ver=2"));
            SetListView(YL, home_GridView_YL, false);
            string GC = await wc.GetResults(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=119&page=1&pagesize=20&order=hot&ver=2"));
            SetListView(GC, home_GridView_GC, false);
            string DY = await wc.GetResults(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=23&page=1&pagesize=20&order=hot&ver=2"));
            SetListView(DY, home_GridView_DY, false);
            string DSJ = await wc.GetResults(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=11&page=1&pagesize=20&order=hot&ver=2"));
            SetListView(DSJ, home_GridView_DSJ, false);
            GetZBInfo();
        }

        private void HomePage_heh()
        {
            SetHomeInfo();
        }

        ApplicationDataContainer container = ApplicationData.Current.LocalSettings;
        #region 废弃原代码
        //public async void GetBanner()
        //{
        //    try
        //    {
        //        HttpClient hc = new HttpClient();
        //        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://www.bilibili.com/index/slideshow.json"));
        //        hr.EnsureSuccessStatusCode();
        //        string results = await hr.Content.ReadAsStringAsync();
        //        BannerModel model = new BannerModel();
        //        model= JsonConvert.DeserializeObject<BannerModel>(results);
        //        List<BannerModel> ban = JsonConvert.DeserializeObject<List<BannerModel>>(model.list.ToString());
        //        foreach (BannerModel item in ban)
        //        {
        //            home_flipView.Items.Add(item);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageDialog md = new MessageDialog("Banner Error\r\n"+ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
        //        await md.ShowAsync();
        //    }
        //}
        //public async void GetDHInfo(int pageSize)
        //{
        //    try
        //    {
        //        HttpClient hc = new HttpClient();
        //        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=1&page=1&pagesize="+pageSize+"&order=hot&ver=2"));
        //        hr.EnsureSuccessStatusCode();
        //        string results = await hr.Content.ReadAsStringAsync();
        //        InfoModel model = new InfoModel();
        //        model = JsonConvert.DeserializeObject<InfoModel>(results);
        //        List<InfoModel> ban = JsonConvert.DeserializeObject<List<InfoModel>>(model.list.ToString());
        //        foreach (InfoModel item in ban)
        //        {
        //            home_GridView_DH.Items.Add(item);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        //MessageDialog md = new MessageDialog("Home DH Error\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
        //        //await md.ShowAsync();
        //    }
        //}
        //public async void GetFJInfo(int pageSize)
        //{
        //    try
        //    {
        //        HttpClient hc = new HttpClient();
        //        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=13&page=1&pagesize="+pageSize+"&order=hot&ver=2"));
        //        hr.EnsureSuccessStatusCode();
        //        string results = await hr.Content.ReadAsStringAsync();
        //        InfoModel model = new InfoModel();
        //        model = JsonConvert.DeserializeObject<InfoModel>(results);
        //        List<InfoModel> ban = JsonConvert.DeserializeObject<List<InfoModel>>(model.list.ToString());
        //        foreach (InfoModel item in ban)
        //        {
        //            home_GridView_FJ.Items.Add(item);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        //MessageDialog md = new MessageDialog("Home FJ Error\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
        //        //await md.ShowAsync();
        //    }
        //}
        //public async void GetYYInfo(int pageSize)
        //{
        //    try
        //    {
        //        HttpClient hc = new HttpClient();
        //        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=3&page=1&pagesize="+pageSize+"&order=hot&ver=2"));
        //        hr.EnsureSuccessStatusCode();
        //        string results = await hr.Content.ReadAsStringAsync();
        //        InfoModel model = new InfoModel();
        //        model = JsonConvert.DeserializeObject<InfoModel>(results);
        //        List<InfoModel> ban = JsonConvert.DeserializeObject<List<InfoModel>>(model.list.ToString());
        //        foreach (InfoModel item in ban)
        //        {
        //            home_GridView_YYWD.Items.Add(item);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        //MessageDialog md = new MessageDialog("Home YX Error\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
        //        //await md.ShowAsync();
        //    }
        //}
        //public async void GetWDInfo(int pageSize)
        //{
        //    try
        //    {
        //        HttpClient hc = new HttpClient();
        //        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=20&page=1&pagesize="+pageSize+"&order=hot&ver=2"));
        //        hr.EnsureSuccessStatusCode();
        //        string results = await hr.Content.ReadAsStringAsync();
        //        InfoModel model = new InfoModel();
        //        model = JsonConvert.DeserializeObject<InfoModel>(results);
        //        List<InfoModel> ban = JsonConvert.DeserializeObject<List<InfoModel>>(model.list.ToString());
        //        foreach (InfoModel item in ban)
        //        {
        //            home_GridView_WD.Items.Add(item);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        //MessageDialog md = new MessageDialog("Home WD Error\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
        //        //await md.ShowAsync();
        //    }
        //}
        //public async void GetYXInfo(int pageSize)
        //{
        //    try
        //    {
        //        HttpClient hc = new HttpClient();
        //        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=4&page=1&pagesize="+pageSize+"&order=hot&ver=2"));
        //        hr.EnsureSuccessStatusCode();
        //        string results = await hr.Content.ReadAsStringAsync();
        //        InfoModel model = new InfoModel();
        //        model = JsonConvert.DeserializeObject<InfoModel>(results);
        //        List<InfoModel> ban = JsonConvert.DeserializeObject<List<InfoModel>>(model.list.ToString());
        //        foreach (InfoModel item in ban)
        //        {
        //            home_GridView_YX.Items.Add(item);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //       // MessageDialog md = new MessageDialog("Home YX Error\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
        //       // await md.ShowAsync();
        //    }
        //}
        //public async void GetKJInfo(int pageSize)
        //{
        //    try
        //    {
        //        HttpClient hc = new HttpClient();
        //        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=36&page=1&pagesize="+pageSize+"&order=hot&ver=2"));
        //        hr.EnsureSuccessStatusCode();
        //        string results = await hr.Content.ReadAsStringAsync();
        //        InfoModel model = new InfoModel();
        //        model = JsonConvert.DeserializeObject<InfoModel>(results);
        //        List<InfoModel> ban = JsonConvert.DeserializeObject<List<InfoModel>>(model.list.ToString());
        //        foreach (InfoModel item in ban)
        //        {
        //            home_GridView_KJ.Items.Add(item);
        //        }
        //    }
        //    catch (Exception )
        //    {
        //        //MessageDialog md = new MessageDialog("Home KJ Error\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
        //        //await md.ShowAsync();
        //    }
        //}
        //public async void GetYLInfo(int pageSize)
        //{
        //    try
        //    {
        //        HttpClient hc = new HttpClient();
        //        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=5&page=1&pagesize="+pageSize+"&order=hot&ver=2"));
        //        hr.EnsureSuccessStatusCode();
        //        string results = await hr.Content.ReadAsStringAsync();
        //        InfoModel model = new InfoModel();
        //        model = JsonConvert.DeserializeObject<InfoModel>(results);
        //        List<InfoModel> ban = JsonConvert.DeserializeObject<List<InfoModel>>(model.list.ToString());
        //        foreach (InfoModel item in ban)
        //        {
        //            home_GridView_YL.Items.Add(item);
        //        }
        //    }
        //    catch (Exception )
        //    {
        //        //MessageDialog md = new MessageDialog("Home YL Error\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
        //        //await md.ShowAsync();
        //    }
        //}
        //public async void GetGCInfo(int pageSize)
        //{
        //    try
        //    {
        //        HttpClient hc = new HttpClient();
        //        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=119&page=1&pagesize="+pageSize+"&order=hot&ver=2"));
        //        hr.EnsureSuccessStatusCode();
        //        string results = await hr.Content.ReadAsStringAsync();
        //        InfoModel model = new InfoModel();
        //        model = JsonConvert.DeserializeObject<InfoModel>(results);
        //        List<InfoModel> ban = JsonConvert.DeserializeObject<List<InfoModel>>(model.list.ToString());
        //        foreach (InfoModel item in ban)
        //        {
        //            home_GridView_GC.Items.Add(item);
        //        }
        //    }
        //    catch (Exception )
        //    {
        //        //MessageDialog md = new MessageDialog("Home GC Error\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
        //        //await md.ShowAsync();
        //    }
        //}
        //public async void GetDYInfo(int pageSize)
        //{
        //    try
        //    {
        //        HttpClient hc = new HttpClient();
        //        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=23&page=1&pagesize="+pageSize+"&order=hot&ver=2"));
        //        hr.EnsureSuccessStatusCode();
        //        string results = await hr.Content.ReadAsStringAsync();
        //        InfoModel model = new InfoModel();
        //        model = JsonConvert.DeserializeObject<InfoModel>(results);
        //        List<InfoModel> ban = JsonConvert.DeserializeObject<List<InfoModel>>(model.list.ToString());
        //        foreach (InfoModel item in ban)
        //        {
        //            home_GridView_DY.Items.Add(item);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        //MessageDialog md = new MessageDialog("Home DY Error\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
        //        //await md.ShowAsync();
        //    }
        //}
        //public async void GetDSJInfo(int pageSize)
        //{
        //    try
        //    {
        //        HttpClient hc = new HttpClient();
        //        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=11&page=1&pagesize="+pageSize+"&order=hot&ver=2"));
        //        hr.EnsureSuccessStatusCode();
        //        string results = await hr.Content.ReadAsStringAsync();
        //        InfoModel model = new InfoModel();
        //        model = JsonConvert.DeserializeObject<InfoModel>(results);
        //        List<InfoModel> ban = JsonConvert.DeserializeObject<List<InfoModel>>(model.list.ToString());
        //        foreach (InfoModel item in ban)
        //        {
        //            home_GridView_DSJ.Items.Add(item);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        //MessageDialog md = new MessageDialog("Home DSJ Error\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
        //       // await md.ShowAsync();
        //    }
        //}
        #endregion
        public async void GetZBInfo()
        {
            try
            {
                string results = await wc.GetResults(new Uri("http://api.bilibili.com/live/room_list?pagesize=20&status=LIVE"));
                InfoModel model = new InfoModel();
                model = JsonConvert.DeserializeObject<InfoModel>(results);
                JObject json = JObject.Parse(model.list.ToString());
                List<InfoModel> ReList = new List<InfoModel>();
                for (int i = 0; i < 20; i++)
                {
                    home_GridView_ZB.Items.Add(new InfoModel
                    {
                        room_id = (string)json[i.ToString()]["room_id"],
                        title = (string)json[i.ToString()]["title"],
                        cover = (string)json[i.ToString()]["cover"],
                        uname = (string)json[i.ToString()]["uname"],
                        online = (string)json[i.ToString()]["online"],
                        face = (string)json[i.ToString()]["face"],

                    });
                }
            }
            catch (Exception)
            {
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Regex.IsMatch(((BannerModel)home_flipView.SelectedItem).link , "/video/av(.*)?[/|+](.*)?"))
            {
                string a = Regex.Match(((BannerModel)home_flipView.SelectedItem).link , "/video/av(.*)?[/|+](.*)?").Groups[1].Value;
                this.Frame.Navigate(typeof(VideoPage), a);
            }
            else
            {
                this.Frame.Navigate(typeof(WebViewPage), ((BannerModel)home_flipView.SelectedItem).link);
            }
           
            //TestNade(((BannerModel)home_flipView.SelectedItem).link);
        }


        private void home_GridView_FJ_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(VideoPage), ((InfoModel)e.ClickedItem).aid);
        }

        private void Btn_FJ_More_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FJPage));
        }

        private void home_flipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DHPage));
        }

        private void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(YYPage));
        }

        private void HyperlinkButton_Click_3(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WDPage));
        }

        private void HyperlinkButton_Click_4(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(YXPage));
        }

        private void HyperlinkButton_Click_5(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(KJPage));
        }

        private void HyperlinkButton_Click_6(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(YLPage));
        }

        private void HyperlinkButton_Click_7(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GCPage));
        }

        private void HyperlinkButton_Click_8(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DYPage));
        }

        private void HyperlinkButton_Click_9(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DSJPage));
        }

        private void home_GridView_ZB_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(LivePlayerPage), ((InfoModel)e.ClickedItem).room_id);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth<=500)
            {
                ViewBox_num.Width = ActualWidth / 2 - 20;
                ViewBox2_num.Width = ActualWidth / 2 - 20;
                home_GridView_ZB.Height = ((ViewBox_num.Width+12)/1.15) * 2;
                double d = ((ViewBox2_num.Width + 12) / 1.08) * 2;
                home_GridView_FJ.Height = d;
                home_GridView_DH.Height = d;
                home_GridView_YYWD.Height = d;
                home_GridView_WD.Height = d;
                home_GridView_YX.Height = d;
                home_GridView_KJ.Height = d;
                home_GridView_GC.Height = d;
                home_GridView_YL.Height = d;
                home_GridView_DY.Height = d;
                home_GridView_DSJ.Height = d;
            }
            else
            {
                ViewBox_num.Width = 200;
                ViewBox2_num.Width = 200;
                //home_GridView_ZB.Height = ViewBox_num.Width * 2;
                //home_GridView_ZB.Height = 200;
            }
        }

        private  void scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            
        }

        private void pr_RefreshInvoked(DependencyObject sender, object args)
        {
            SetHomeInfo();
        }
    }
    
    
    public class BannerModel
    {
        public int results { get; set; }
        public object list { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string img { get; set; }
        public string simg { get; set; }
    }

    public class InfoModel
    {
        public object list { get; set; }
        public string pic { get; set; }
        public string title { get; set; }
        public string play { get; set; }
        public string author { get; set; }
        public string video_review { get; set; }
        public string description { get; set; }
        public string mid { get; set; }
        public string aid { get; set; }
        public int num { get; set; }
        //用于直播
        public string room_id { get; set; }
        public string online { get; set; }
        public string uname { get; set; }
        public string cover { get; set; }
        public string face { get; set; }
    }
}

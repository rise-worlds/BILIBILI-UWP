using Newtonsoft.Json;
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

// B了狗啦，页面什么的都搞好了，TMD新版番剧专题API用不了，只能用旧版API了

namespace BILIBILI_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BanSeasonPage : Page
    {
        public BanSeasonPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int results = 0;
            if (int.TryParse(e.Parameter.ToString(),out results))
            {
                GetBanSeasonInfo(e.Parameter.ToString());
                GetBanSeasonVideo(e.Parameter.ToString());
            }
            else
            {
                GetBanSeasonVideoTitle(e.Parameter.ToString());
            }
           
        }


        //貌似根据标题获取信息效果更好一点- -
        public async void GetBanSeasonInfo(string sid)
        {
            BanSeason_Content.Visibility = Visibility.Collapsed;
            BanSeason_Load.Visibility = Visibility.Visible;
            try
            {
                HttpClient hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/sp?spid="+sid));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                BanSeasonModel model = new BanSeasonModel();
                model = JsonConvert.DeserializeObject<BanSeasonModel>(results);
                BanSeason_Grid.DataContext = model;
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog("Banner Error\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
                await md.ShowAsync();
            }
            BanSeason_Content.Visibility = Visibility.Visible;
            BanSeason_Load.Visibility = Visibility.Collapsed;
        }
        public async void GetBanSeasonVideo(string sid)
        {
            BanSeason_Content.Visibility = Visibility.Collapsed;
            BanSeason_Load.Visibility = Visibility.Visible;
            try
            {
                HttpClient hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/spview?type=json&spid=" + sid+ "&bangumi=1"));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                BanSeasonModel model = new BanSeasonModel();
                model = JsonConvert.DeserializeObject<BanSeasonModel>(results);
                List<BanSeasonModel> lsModel = JsonConvert.DeserializeObject<List<BanSeasonModel>>(model.list.ToString());
                BanSea_Video.Items.Clear();
                if (lsModel.Count == 0)
                {
                    BanSea_Video.Items.Add(new BanSeasonModel() { episode = "获取失败" });
                }
                else
                {
                    BanSea_Video.ItemsSource = lsModel;
                }
               
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog("Banner Error\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
                await md.ShowAsync();
            }
            BanSeason_Content.Visibility = Visibility.Visible;
            BanSeason_Load.Visibility = Visibility.Collapsed;
        }
        public async void GetBanSeasonVideoTitle(string title)
        {
            BanSeason_Content.Visibility = Visibility.Collapsed;
            BanSeason_Load.Visibility = Visibility.Visible;
            try
            {
                HttpClient hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/sp?title=" + title));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                BanSeasonModel model = new BanSeasonModel();
                model = JsonConvert.DeserializeObject<BanSeasonModel>(results);
                BanSeason_Grid.DataContext = model;
                GetBanSeasonVideo(model.spid);
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog("Banner Error\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
                await md.ShowAsync();
            }
            BanSeason_Content.Visibility = Visibility.Visible;
            BanSeason_Load.Visibility = Visibility.Collapsed;
        }

        private void BanSea_Video_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (((BanSeasonModel)e.ClickedItem).aid!="")
            {
                this.Frame.Navigate(typeof(VideoPage), ((BanSeasonModel)e.ClickedItem).aid);
            }
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog md = new MessageDialog("挖坑，待填...估计得重写整个专题页才能实现...");
            await md.ShowAsync();
        }

        private void btn_Play_Click(object sender, RoutedEventArgs e)
        {
            if (((BanSeasonModel)BanSea_Video.Items[0]).aid != null)
            {
                this.Frame.Navigate(typeof(VideoPage), ((BanSeasonModel)BanSea_Video.Items[0]).aid);
            }
        }
    }

    public class BanSeasonModel
    {
        public string title { get; set; }
        public string cover { get; set; }
        public string spid { get; set; }
        public string lastupdate_at { get; set; }
        public string description { get; set; }
        public string video_view { get; set; }
        public string attention { get; set; }
        public int count { get; set; }

        public object list { get; set; }
        public string aid { get; set; }
        public string cid { get; set; }//直接播放ID
        public string episode { get; set; }
    }

}

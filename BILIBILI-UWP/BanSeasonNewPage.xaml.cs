using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace BILIBILI_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BanSeasonNewPage : Page
    {
        public BanSeasonNewPage()
        {
            this.InitializeComponent();
            
        }
        private string SeasonID = "";
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            BanSeason_Content.Visibility = Visibility.Collapsed;
            BanSeason_Load.Visibility = Visibility.Visible;
            SeasonID = e.Parameter as string;
            await GetBanSeasonInfo(e.Parameter as string);
            BanSeason_Content.Visibility = Visibility.Visible;
            BanSeason_Load.Visibility = Visibility.Collapsed;
        }
        private async Task GetBanSeasonInfo(string sid)
        {
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://www.bilibili.com/bangumi/i/" + sid + "/"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    NewBanSeasonModel model = new NewBanSeasonModel();
                    model.title = Regex.Match(results, @"<h1 class=""info-title"" data-seasonid="".*?"">(.*?)</h1>").Groups[1].Value;
                    model.desc = Regex.Match(results, @"<div class=""info-desc"">(.*?)</div>").Groups[1].Value;
                    model.image = Regex.Match(results, @"<div class=""bangumi-preview""><img src=""(.*?)"".*?></div>").Groups[1].Value;
                    model.region = Regex.Match(results, @"<span class=""info-detail-item"">地区：<em>(.*?)</em></span>").Groups[1].Value;
                    model.updateTime = Regex.Match(results, @"<span class=""info-detail-item info-detail-item-date"">开播日期：<em>(.*?)</em></span>").Groups[1].Value;
                    string regex = @"<a href=""/video/av(.*?)/.*?>.*?<div class=""t"">.*?<span>(.*?)</span>.*?</div>";
                    MatchCollection mc = Regex.Matches(results, regex, RegexOptions.Singleline);
                    List<NewBanSeasonModel> VideoLs = new List<NewBanSeasonModel>();
                    if (mc != null)
                    {
                        foreach (Match item in mc)
                        {
                            VideoLs.Add(new NewBanSeasonModel() { aid = item.Groups[1].Value, video = item.Groups[2].Value });
                        }
                    }

                    MatchCollection mc2 = Regex.Matches(results, @"<span class=""info-style-item"">(.*?)</span>");
                    if (mc2 != null)
                    {
                        foreach (Match item in mc2)
                        {
                            model.tag += item.Groups[1].Value + ",";
                        }
                    }
                    MatchCollection mc3 = Regex.Matches(results, @"<span class=""info-cv-item""><span class=""separator"">.*?</span>(.*?)</span>");
                    if (mc3 != null)
                    {
                        foreach (Match item in mc3)
                        {
                            model.cv += item.Groups[1].Value + "/";
                        }
                    }
                    if (VideoLs.Count != 0)
                    {
                        VideoLs.RemoveAt(VideoLs.Count - 1);
                    }
                    if (await GetIsConcern(sid))
                    {
                        btn_concern.Label = "取消订阅";
                    }
                    else
                    {
                        btn_concern.Label = "订阅";
                    }
                    BanSea_Video.ItemsSource = VideoLs;
                    BanSeason_Grid.DataContext = model;
                }
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog("获取番剧信息失败！\r\n" + ex.Message);
                await md.ShowAsync();
            }
        }

        private async Task<bool> GetIsConcern(string sid)
        {
            try
            {
                using (HttpClient hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://www.bilibili.com/api_proxy?app=bangumi&action=/user_season_status&season_id=" + sid));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(results);
                    if ((int)json["result"]["attention"] == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_LookMore_Click(object sender, RoutedEventArgs e)
        {
            if (btn_LookMore.Content.ToString()=="展开")
            {
                txt_desc.MaxLines = 0;
                btn_LookMore.Content = "收缩";
            }
            else
            {
                txt_desc.MaxLines = 3;
                btn_LookMore.Content = "展开";
            }
        }

        private void BanSea_Video_ItemClick(object sender, ItemClickEventArgs e)
        {
            if ((e.ClickedItem as NewBanSeasonModel).aid!=null)
            {
                this.Frame.Navigate(typeof(VideoPage), (e.ClickedItem as NewBanSeasonModel).aid);
            }
           
        }

        private async void btn_concern_Click(object sender, RoutedEventArgs e)
        {
            GetLoginInfoClass getLogin = new GetLoginInfoClass();
            if (getLogin.IsLogin())
            {
                try
                {
                    if (btn_concern.Label == "订阅")
                    {
                        //http://www.bilibili.com/api_proxy?app=bangumi&action=/concern_season&season_id=779
                        using (HttpClient hc = new HttpClient())
                        {

                            HttpResponseMessage hr = await hc.GetAsync(new Uri("http://www.bilibili.com/api_proxy?app=bangumi&action=/concern_season&season_id=" + SeasonID));
                            hr.EnsureSuccessStatusCode();
                            string results = await hr.Content.ReadAsStringAsync();
                            JObject json = JObject.Parse(results);
                            if ((int)json["code"] == 0)
                            {
                                MessageDialog md = new MessageDialog("订阅成功！");
                                await md.ShowAsync();
                                btn_concern.Label = "取消订阅";
                            }
                            else
                            {
                                MessageDialog md = new MessageDialog("订阅失败！");
                                await md.ShowAsync();
                            }
                        }
                    }
                    else
                    {
                        //http://www.bilibili.com/api_proxy?app=bangumi&action=/concern_season&season_id=779
                        using (HttpClient hc = new HttpClient())
                        {
                            HttpResponseMessage hr = await hc.GetAsync(new Uri("http://www.bilibili.com/api_proxy?app=bangumi&action=/unconcern_season&season_id=" + SeasonID));
                            hr.EnsureSuccessStatusCode();
                            string results = await hr.Content.ReadAsStringAsync();
                            JObject json = JObject.Parse(results);
                            if ((int)json["code"] == 0)
                            {
                                MessageDialog md = new MessageDialog("取消订阅成功！");
                                await md.ShowAsync();
                                btn_concern.Label = "订阅";
                            }
                            else
                            {
                                MessageDialog md = new MessageDialog("取消订阅失败！");
                                await md.ShowAsync();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    MessageDialog md = new MessageDialog("订阅操作失败！");
                    
                    await md.ShowAsync();
                }
            }
            else
            {
                MessageDialog md = new MessageDialog("先登录好伐", "(´・ω・`) ");
                await md.ShowAsync();
            }
            
        }
    }

    public class NewBanSeasonModel
    {
        public string sid { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string desc { get; set; }
        public string region { get; set; }
        public string updateTime { get; set; }
        public string cv { get; set; }

        public string tag { get; set; }

        public string video { get; set; }
        public string aid { get; set; }
    }


}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class BanSeasonByTag : Page
    {
        public BanSeasonByTag()
        {
            this.InitializeComponent();
          
        }
        private string getTid = "";
        private int getPage = 1;
        private int getType = 1;
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            getTid = e.Parameter as string;
            Grid_Content.Visibility = Visibility.Collapsed;
            Grid_Loading.Visibility = Visibility.Visible;
            await GetTagInfo(getPage, getTid, 0);
            Grid_Content.Visibility = Visibility.Visible;
            Grid_Loading.Visibility = Visibility.Collapsed;
        }

        private async Task GetTagInfo(int page,string tid,int type)
        {
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri(String.Format("http://bangumi.bilibili.com/api/get_season_by_tag?indexType={0}&page={1}&pagesize=20&tag_id={2}", type, page, tid)));
                    string results = await hr.Content.ReadAsStringAsync();
                    BanSeasonTagModel model = JsonConvert.DeserializeObject<BanSeasonTagModel>(results);
                    List<BanSeasonTagModel> ls = JsonConvert.DeserializeObject<List<BanSeasonTagModel>>(model.result.ToString());
                    foreach (BanSeasonTagModel item in ls)
                    {
                        ls_Tag.Items.Add(item);
                    }
                    getPage++;
                    if (model.pages < getPage)
                    {
                        btn_LoadMore.IsEnabled = false;
                        btn_LoadMore.Content = "加载完了...";
                    }
                }
            }
            catch (Exception)
            {
            }


        }

        private void ls_Tag_ItemClick(object sender, ItemClickEventArgs e)
        {
           
            this.Frame.Navigate(typeof(BanSeasonNewPage),(e.ClickedItem as BanSeasonTagModel).season_id);
        }

        private async void btn_type1_Click(object sender, RoutedEventArgs e)
        {
            ls_Tag.Items.Clear();
            btn_LoadMore.IsEnabled = true;
            btn_LoadMore.Content = "加载更多";
            getPage = 1;
            getType = 0;
            Grid_Content.Visibility = Visibility.Collapsed;
            Grid_Loading.Visibility = Visibility.Visible;
            await GetTagInfo(getPage, getTid, getType);
            Grid_Content.Visibility = Visibility.Visible;
            Grid_Loading.Visibility = Visibility.Collapsed;
           
        }

        private async void btn_type2_Click(object sender, RoutedEventArgs e)
        {
            ls_Tag.Items.Clear();
            btn_LoadMore.IsEnabled = true;
            btn_LoadMore.Content = "加载更多";
            getPage = 1;
            getType = 1;
            Grid_Content.Visibility = Visibility.Collapsed;
            Grid_Loading.Visibility = Visibility.Visible;
            await GetTagInfo(getPage, getTid, getType);
            Grid_Content.Visibility = Visibility.Visible;
            Grid_Loading.Visibility = Visibility.Collapsed;
        }

        private async void btn_type3_Click(object sender, RoutedEventArgs e)
        {
            ls_Tag.Items.Clear();
            btn_LoadMore.IsEnabled = true;
            btn_LoadMore.Content = "加载更多";
            getPage = 1;
            getType = 2;
            Grid_Content.Visibility = Visibility.Collapsed;
            Grid_Loading.Visibility = Visibility.Visible;
            await GetTagInfo(getPage, getTid, getType);
            Grid_Content.Visibility = Visibility.Visible;
            Grid_Loading.Visibility = Visibility.Collapsed;
        }

        private async void btn_type4_Click(object sender, RoutedEventArgs e)
        {
            ls_Tag.Items.Clear();
            btn_LoadMore.IsEnabled = true;
            btn_LoadMore.Content = "加载更多";
            getPage = 1;
            getType = 3;
            Grid_Content.Visibility = Visibility.Collapsed;
            Grid_Loading.Visibility = Visibility.Visible;
            await GetTagInfo(getPage, getTid, getType);
            Grid_Content.Visibility = Visibility.Visible;
            Grid_Loading.Visibility = Visibility.Collapsed;
        }

        private  void btn_LoadMore_Click(object sender, RoutedEventArgs e)
        {
           
            
        }
        bool More = true;
        private async void sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (sv.VerticalOffset == sv.ScrollableHeight)
            {
                if (More)
                {
                    More = false;
                    btn_LoadMore.IsEnabled = false;
                    btn_LoadMore.Content = "正在加载";
                    await GetTagInfo(getPage, getTid, getType);
                    if (btn_LoadMore.Content.ToString() != "加载完了...")
                    {
                        btn_LoadMore.IsEnabled = true;
                        btn_LoadMore.Content = "加载更多";
                    }
                    More = true;
                }
            }
        }
    }

    public class BanSeasonTagModel
    {
        public int count { get; set; }
        public int pages { get; set; }
        public object result { get; set; }

        public string bangumi_title { get; set; }//标题
        public string brief { get; set; }//简介
        public string pub_time { get; set; }//时间
        public string Time
        {
            get
            {
                try
                {
                    return String.Format("{0}年{1}月", DateTime.Parse(pub_time).Year, DateTime.Parse(pub_time).Month);
                }
                catch (Exception)
                {
                    return "";
                }
                    //string a = pub_time.Remove(11, pub_time.Length - 1);
            }
        }
        public string squareCover { get; set; }//封面
        public string season_id { get; set; }//SID
        public int is_finish { get; set; }//是否完结
        public string newest_ep_index { get; set; }//最新话
        public string Is_finish
        {
            get
            {
                if (is_finish==1)
                {
                    return String.Format("已完结,共{0}话",newest_ep_index);
                }
                else
                {
                    return String.Format("更新至{0}话", newest_ep_index);
                }
            }
        }
        public int favorites { get; set; }//订阅数
        public string Favorites
        {
            get
            {
                    return String.Format("{0}万人订阅",(double)favorites / 10000);
            }
        }
    }
}

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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace BILIBILI_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        public SearchPage()
        {
            this.InitializeComponent();
        }
        private int pageNum=1;
        private int maxPageNum = 0;
        private string keyword = "";
        private int pageNum_Up = 1;
        private int maxPageNum_Up = 0;
        private int pageNum_Ban= 1;
        private int maxPageNum_Ban = 0;
        private int pageNum_Sp = 1;
        private int maxPageNum_Sp = 0;
        public async void GetSeachInfo()
        {
            try
            {
                HttpClient hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://www.bilibili.com/search?action=autolist&pagesize=20&keyword="+keyword+"&page="+ pageNum + "&type=video"));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                SeachVideoModel model = JsonConvert.DeserializeObject<SeachVideoModel>(results);
                SeachVideoModel model2 = JsonConvert.DeserializeObject<SeachVideoModel>(model.res.ToString());
                maxPageNum = model2.numPages;
                //Video_Grid_Info.DataContext = model;
                List<SeachVideoModel> ban = JsonConvert.DeserializeObject<List<SeachVideoModel>>(model2.result.ToString());
                foreach (SeachVideoModel item in ban)
                {
                    Seach_listview_Video.Items.Add(item);
                }
                pageNum++;
                if (pageNum > maxPageNum)
                {
                    User_load_more.IsEnabled = false;
                    User_load_more.Content = "没有更多了...";
                }
                else
                {
                    User_load_more.IsEnabled = true;
                    User_load_more.Content = "加载更多";
                }
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog("Error for GetVideoComment\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
                await md.ShowAsync();
                //throw;
            }
        }

        public async void GetSeachUpInfo()
        {
            try
            {
                HttpClient hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://www.bilibili.com/search?action=autolist&pagesize=20&keyword=" + keyword + "&page=" + pageNum_Up + "&type=upuser"));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                SeachUpModel model = JsonConvert.DeserializeObject<SeachUpModel>(results);
                SeachUpModel model2 = JsonConvert.DeserializeObject<SeachUpModel>(model.up_res.ToString());
                maxPageNum_Up = model2.numPages;
                //Video_Grid_Info.DataContext = model;
                List<SeachUpModel> ban = JsonConvert.DeserializeObject<List<SeachUpModel>>(model2.result.ToString());
                foreach (SeachUpModel item in ban)
                {
                    Seach_listview_Up.Items.Add(item);
                }
                pageNum_Up++;
                if (pageNum_Up > maxPageNum_Up)
                {
                    Up_load_more.IsEnabled = false;
                    Up_load_more.Content = "没有更多了...";
                }
                else
                {
                    Up_load_more.IsEnabled = true;
                    Up_load_more.Content = "加载更多";
                }
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog("Error for GetVideoComment\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
                await md.ShowAsync();
                //throw;
            }
        }

        public async void GetSeachBanInfo()
        {
            try
            {
                HttpClient hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://www.bilibili.com/search?action=autolist&pagesize=20&keyword=" + keyword + "&page=" + pageNum_Ban + "&type=series"));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                SeachBanModel model = JsonConvert.DeserializeObject<SeachBanModel>(results);
                SeachBanModel model2 = JsonConvert.DeserializeObject<SeachBanModel>(model.tp_res.ToString());
                maxPageNum_Up = model2.numPages;
                //Video_Grid_Info.DataContext = model;
                List<SeachBanModel> ban = JsonConvert.DeserializeObject<List<SeachBanModel>>(model2.result.ToString());
                foreach (SeachBanModel item in ban)
                {
                    Seach_listview_Ban.Items.Add(item);
                }
                pageNum_Ban++;
                if (pageNum_Ban > maxPageNum_Ban)
                {
                    Ban_load_more.IsEnabled = false;
                    Ban_load_more.Content = "没有更多了...";
                }
                else
                {
                    Ban_load_more.IsEnabled = true;
                    Ban_load_more.Content = "加载更多";
                }
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog("Error for GetVideoComment\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
                await md.ShowAsync();
                //throw;
            }
        }

        public async void GetSeachSpInfo()
        {
            try
            {
                HttpClient hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://www.bilibili.com/search?action=autolist&pagesize=20&keyword=" + keyword + "&page=" + pageNum_Sp + "&type=special"));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                SeachBanModel model = JsonConvert.DeserializeObject<SeachBanModel>(results);
                SeachBanModel model2 = JsonConvert.DeserializeObject<SeachBanModel>(model.sp_res.ToString());
                maxPageNum_Sp = model2.numPages;
                //Video_Grid_Info.DataContext = model;
                List<SeachBanModel> ban = JsonConvert.DeserializeObject<List<SeachBanModel>>(model2.result.ToString());
                foreach (SeachBanModel item in ban)
                {
                    Seach_listview_Sp.Items.Add(item);
                }
                pageNum_Sp++;
                if (pageNum_Sp > maxPageNum_Sp)
                {
                    Sp_load_more.IsEnabled = false;
                    Sp_load_more.Content = "没有更多了...";
                }
                else
                {
                    Sp_load_more.IsEnabled = true;
                    Sp_load_more.Content = "加载更多";
                }
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog("Error for GetVideoComment\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
                await md.ShowAsync();
                //throw;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            keyword = (string)e.Parameter;
            text_Title.Text = keyword + " 的搜索结果";
            Seach_listview_Video.Items.Clear();
            GetSeachInfo();
            GetSeachUpInfo();
            GetSeachBanInfo();
            GetSeachSpInfo();
        }

        private void User_load_more_Click(object sender, RoutedEventArgs e)
        {
                GetSeachInfo();
            
        }

        private void Seach_listview_Video_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(VideoPage),((SeachVideoModel)e.ClickedItem).aid);
        }

        private void Seach_listview_Up_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(UserInfoPage), ((SeachUpModel)e.ClickedItem).mid);
        }

        private void Up_load_more_Click(object sender, RoutedEventArgs e)
        {
            GetSeachUpInfo();
        }

        private void Ban_load_more_Click(object sender, RoutedEventArgs e)
        {
            GetSeachBanInfo();
        }

        private void Seach_listview_Ban_ItemClick(object sender, ItemClickEventArgs e)
        {

                this.Frame.Navigate(typeof(BanSeasonNewPage), ((SeachBanModel)e.ClickedItem).season_id);

        }

        private void Seach_listview_Sp_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(BanSeasonPage), ((SeachBanModel)e.ClickedItem).spid);
        }

        private void Sp_load_more_Click(object sender, RoutedEventArgs e)
        {
            GetSeachSpInfo();
        }
    }

    public class SeachVideoModel
    {
        //第一层
        public object res { get; set; }
        //第二层
        public int numResults { get; set; }//结果数量
        public int numPages { get; set; }//页数
        public object result { get; set; }//结果
        //第三层
        public string aid { get; set; }//视频AID
        public string mid { get; set; }//用户Mid
        public string author { get; set; }//作者
        public string title { get; set; }//标题
        public string pic { get; set; }//封面
        public string play { get; set; }//播放
        public string video_review { get; set; }//弹幕
        public string duration { get; set; }//时长
    }
    public class SeachUpModel
    {
        //第一层
        public object up_res { get; set; }
        //第二层
        public int numResults { get; set; }//结果数量
        public int numPages { get; set; }//页数
        public object result { get; set; }//结果
        //第三层
        public string mid { get; set; }//用户Mid
        public string uname { get; set; }//作者
        public string usign { get; set; }//标题
        public string upic { get; set; }//封面
    }
    public class SeachBanModel
    {
        //第一层
        public object tp_res { get; set; }
        public object sp_res { get; set; }
        //第二层
        public int numResults { get; set; }//结果数量
        public int numPages { get; set; }//页数
        public object result { get; set; }//结果
        //第三层
        public string id { get; set; }//id
        public string spid{get;set;}
        public string title { get; set; }//作者
        public string Title
        {
            get
            {
                string s = title.Replace("<em class=\"keyword\">", "");
                return s.Replace("</em>", "");
            }
        }
        public string description { get; set; }//标题
        public string Description
        {
            get
            {
                string s = description.Replace("<em class=\"keyword\">", "");
                return s.Replace("</em>", "");
            }
        }
        public string pic { get; set; }//封面
        public int is_bangumi { get; set; }//是否番剧
        public string season_id { get; set; }
    }

    public class SeachSpModel
    {
        //第一层
        public object sp_res { get; set; }
        //第二层
        public int numResults { get; set; }//结果数量
        public int numPages { get; set; }//页数
        public object result { get; set; }//结果
        //第三层
        public string id { get; set; }//id
        public string spid { get; set; }
        public string title { get; set; }//作者
        public string Title
        {
            get
            {
                string s = title.Replace("<em class=\"keyword\">", "");
                return s.Replace("</em>", "");
            }
        }
        public string description { get; set; }//标题
        public string Description
        {
            get
            {
                string s = description.Replace("<em class=\"keyword\">", "");
                return s.Replace("</em>", "");
            }
        }
        public string pic { get; set; }//封面
    }
}

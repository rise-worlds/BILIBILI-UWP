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
    public sealed partial class BangumiPage : Page
    {
        public BangumiPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            Get();
            GetBangumiCategories();
            GetLastedUpdate();
            GetBangumiType();
            GetBangumiInfo();
            GetBangumiTimeLine();
        }
        public async void Get()
        {
            await GetResults();
            
        }

        public async void GetBangumiInfo()
        {
            try
            {
                HttpClient hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/promo/android3/2620/bangumi.android3.xhdpi.json"));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                BangumiModel model = new BangumiModel();
                model = JsonConvert.DeserializeObject<BangumiModel>(results);
                List<BangumiModel> ban = JsonConvert.DeserializeObject<List<BangumiModel>>(model.list.ToString());
                Ban_GridView_Info.Items.Clear();
                foreach (BangumiModel item in ban)
                {
                    Ban_GridView_Info.Items.Add(item);
                }
             }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog("Error for Bangumi Info Loading\r\n"+ex.Message);
                await md.ShowAsync();
            }
        }

        public async void GetBangumiTimeLine()
        {
            try
            {
                HttpClient hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://www.bilibili.com/api_proxy?app=bangumi&action=timeline_v2"));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                BangumiTimeLineModel model = new BangumiTimeLineModel();
                model = JsonConvert.DeserializeObject<BangumiTimeLineModel>(results);
                List<BangumiTimeLineModel> ban = JsonConvert.DeserializeObject<List<BangumiTimeLineModel>>(model.list.ToString());
                weekday0.Items.Clear();
                weekday1.Items.Clear();
                weekday2.Items.Clear();
                weekday3.Items.Clear();
                weekday4.Items.Clear();
                weekday5.Items.Clear();
                weekday6.Items.Clear();
                weekday7.Items.Clear();
                foreach (BangumiTimeLineModel item in ban)
                {
                    switch (item.weekday)
                    {
                        case -1:
                            weekday7.Items.Add(item);
                            break;
                        case 0:
                            weekday0.Items.Add(item);
                            break;
                        case 1:
                            weekday1.Items.Add(item);
                            break;
                        case 2:
                            weekday2.Items.Add(item);
                            break;
                        case 3:
                            weekday3.Items.Add(item);
                            break;
                        case 4:
                            weekday4.Items.Add(item);
                            break;
                        case 5:
                            weekday5.Items.Add(item);
                            break;
                        case 6:
                            weekday6.Items.Add(item);
                            break;
                        default:
                            break;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog("Error for GetBangumi TimeLine  Loading\r\n" + ex.Message);
                await md.ShowAsync();
            }
        }

        private string results="";
        public async Task GetResults()
        {
            try
            {
                HttpClient hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://bangumi.bilibili.com/api/app_index_page"));
                hr.EnsureSuccessStatusCode();
                results = await hr.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog("Error for get Bangumi app_index_page\r\n"+ex.Message,"发生错误了，WTF!!");
                throw;
            }
           

        }
        public async void GetLastedUpdate()
        {
            try
            {
                if (results=="")
                {
                   await  GetResults();
                }
                BannumiIndexModel model = new BannumiIndexModel();
                model = JsonConvert.DeserializeObject<BannumiIndexModel>(results);
                JObject json = JObject.Parse(model.result.ToString());
                List<BannumiIndexModel> ban = JsonConvert.DeserializeObject<List<BannumiIndexModel>>(json["latestUpdate"]["list"].ToString());
                GridView_Bangumi_NewUpdate.Items.Clear();
                for (int i = 0; i < 6; i++)
                {
                    GridView_Bangumi_NewUpdate.Items.Add(ban[i] as BannumiIndexModel);
                }
                //foreach (BannumiIndexModel item in ban)
                //{
                //    GridView_Bangumi_NewUpdate.Items.Add(item);
                //}

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog("Error for GetLastedUpdate()\r\n" + ex.Message);
                await md.ShowAsync();
            }
        }
        public async void GetBangumiType()
        {
            try
            {
                if (results  == "")
                {
                    await GetResults();
                }
                BannumiIndexModel model = new BannumiIndexModel();
                model = JsonConvert.DeserializeObject<BannumiIndexModel>(results);
                JObject json = JObject.Parse(model.result.ToString());
                List<BannumiIndexModel> ban = JsonConvert.DeserializeObject<List<BannumiIndexModel>>(json["recommendCategory"].ToString());
                GridView_Bangumi_Type.Items.Clear();
                foreach (BannumiIndexModel item in ban)
                {
                    GridView_Bangumi_Type.Items.Add(item);
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog("Error for GetBangumiType() \r\n" + ex.Message);
                await md.ShowAsync();
            }
        }

        public async void GetBangumiCategories()
        {
            try
            {
                if (results == "")
                {
                    await GetResults();
                }
            BannumiIndexModel model = new BannumiIndexModel();
            model = JsonConvert.DeserializeObject<BannumiIndexModel>(results);
            JObject json = JObject.Parse(model.result.ToString());
            List<BannumiIndexModel> ban = JsonConvert.DeserializeObject<List<BannumiIndexModel>>(json["categories"].ToString());
            //GridView_Bangumi_Type.Items.Clear();
            for (int i = 0; i < 10; i++)
            {
                BannumiIndexModel categoryModel = JsonConvert.DeserializeObject<BannumiIndexModel>(ban[i].category.ToString());
                BannumiIndexModel categoryTitleModel = new BannumiIndexModel()
                {
                    cover = categoryModel.cover??"",
                    tag_id = categoryModel.tag_id??"",
                    tag_name = categoryModel.tag_name??""
                };
                //JObject json2 = JObject.Parse(ban[i].list.ToString());
                BannumiIndexModel listModel = JsonConvert.DeserializeObject<BannumiIndexModel>(ban[i].list.ToString());
                List<BannumiIndexModel> banList = JsonConvert.DeserializeObject<List<BannumiIndexModel>>(listModel.list.ToString());
                switch (i)
                {
                    case 0:
                        Bangumi_TypeTitle1.DataContext = categoryTitleModel;
                        foreach (BannumiIndexModel item in banList)
                        {
                            GridView_Bangumi_Type1.Items.Add(item);
                        }
                        break;
                    case 1:
                        Bangumi_TypeTitle2.DataContext = categoryTitleModel;
                        foreach (BannumiIndexModel item in banList)
                        {
                            GridView_Bangumi_Type2.Items.Add(item);
                        }
                        break;
                    case 2:
                        Bangumi_TypeTitle3.DataContext = categoryTitleModel;
                        foreach (BannumiIndexModel item in banList)
                        {
                            GridView_Bangumi_Type3.Items.Add(item);
                        }
                        break;
                    case 3:
                        Bangumi_TypeTitle4.DataContext = categoryTitleModel;
                        foreach (BannumiIndexModel item in banList)
                        {
                            GridView_Bangumi_Type4.Items.Add(item);
                        }
                        break;
                    case 4:
                        Bangumi_TypeTitle5.DataContext = categoryTitleModel;
                        foreach (BannumiIndexModel item in banList)
                        {
                            GridView_Bangumi_Type5.Items.Add(item);
                        }
                        break;
                    case 5:
                        Bangumi_TypeTitle6.DataContext = categoryTitleModel;
                        foreach (BannumiIndexModel item in banList)
                        {
                            GridView_Bangumi_Type6.Items.Add(item);
                        }
                        break;
                    case 6:
                        Bangumi_TypeTitle7.DataContext = categoryTitleModel;
                        foreach (BannumiIndexModel item in banList)
                        {
                            GridView_Bangumi_Type7.Items.Add(item);
                        }
                        break;
                    //case 7:
                    //    Bangumi_TypeTitle8.DataContext = categoryTitleModel;
                    //    foreach (BannumiIndexModel item in banList)
                    //    {
                    //        GridView_Bangumi_Type8.Items.Add(item);
                    //    }
                    //    break;
                    //case 8:
                    //    Bangumi_TypeTitle9.DataContext = categoryTitleModel;
                    //    foreach (BannumiIndexModel item in banList)
                    //    {
                    //        GridView_Bangumi_Type9.Items.Add(item);
                    //    }
                    //    break;
                    //case 9:
                    //    Bangumi_TypeTitle10.DataContext = categoryTitleModel;
                    //    foreach (BannumiIndexModel item in banList)
                    //    {
                    //        GridView_Bangumi_Type10.Items.Add(item);
                    //    }
                    //    break;
                    default:
                        break;
                }
            }
            }
            catch (Exception )
            {

            }
        }

        private  void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void GridView_Bangumi_NewUpdate_ItemClick(object sender, ItemClickEventArgs e)
        {
            BannumiIndexModel mode = ((BannumiIndexModel)e.ClickedItem);
            this.Frame.Navigate(typeof(BanSeasonNewPage),((BannumiIndexModel)e.ClickedItem).season_id);
        }

        private void Ban_GridView_Info_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            this.Frame.Navigate(typeof(BanSeasonPage), ((BangumiModel)e.ClickedItem).spid);
        }

        private void weekday0_ItemClick(object sender, ItemClickEventArgs e)
        {

            this.Frame.Navigate(typeof(BanSeasonNewPage), ((BangumiTimeLineModel)e.ClickedItem).season_id);
        }

        private void GridView_Bangumi_Type_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(BanSeasonByTag),(e.ClickedItem as BannumiIndexModel).tag_id);
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            string tid = (((sender as HyperlinkButton).Content as StackPanel).DataContext as BannumiIndexModel).tag_id;
            if (tid!=null)
            {
                this.Frame.Navigate(typeof(BanSeasonByTag), tid);
            }
            
        }
    }

    public class BangumiModel
    {
        public object list { get; set; }
        public string imageurl { get; set; }
        public string title { get; set; }
        public string spid { get; set; }
    }

    public class BangumiTimeLineModel
    {
        public object list { get; set; }
        public string bgmcount { get; set; }
        public string cover { get; set; }
        public string lastupdate_at { get; set; }
        public string title { get; set; }
        public string square_cover { get; set; }
        public int weekday { get; set; }
        public string spid { get; set; }
        public string season_id { get; set; }
    }

    public class BannumiIndexModel
    {
        public object result { get; set; }
        //最近更新
        public object latestUpdate { get; set; }
        public object list { get; set; }
        public string title { get; set; }
        public int watchingCount { get; set; }
        public string  newest_ep_index { get; set; }
        public string cover { get; set; }
        public string  newest_ep_id { get; set; }
        public string season_id { get; set; }
        //分类
        public object categories { get; set; }
        public object category { get; set; }
        public string bangumi_title { get; set; }
        public int spid { get; set; }
        public string bangumi_id { get; set; }
        public string total_count { get; set; }

        //分类推荐信息
        public object recommendCategory { get; set; }
        public string   tag_name { get; set; }
        public string tag_id { get; set; }
    }


}

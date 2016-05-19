using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
using Windows.Web.Http.Headers;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace BILIBILI_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DHPage : Page
    {
        public DHPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            GetDHHome();
            GetDHDT();
            GetZHHot();
            GetMADHot();
            GetMMDHot();
            GetDPHot();
        }

        private HttpClient hc;
        /// <summary>
        /// Banner，推荐，最新
        /// </summary>
        private async void GetDHHome()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/api/region2/1.json"));
                    hr.EnsureSuccessStatusCode();

                    var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = Encoding.UTF8.GetString(encodeResults.ToArray(), 0, encodeResults.ToArray().Length); 
                    DHModel model  = JsonConvert.DeserializeObject<DHModel>(results);
                    DHModel model2 = JsonConvert.DeserializeObject<DHModel>(model.result.ToString());

                    List<DHModel> BannerModel = JsonConvert.DeserializeObject< List<DHModel>>(model2.banners.ToString());
                    List<DHModel> RecommendsModel = JsonConvert.DeserializeObject<List<DHModel>>(model2.recommends.ToString());
                    List<DHModel> NewsModel = JsonConvert.DeserializeObject<List<DHModel>>(model2.news.ToString());
                    home_flipView.Items.Clear();
                    GridView_TJ.Items.Clear();
                    GridView_New.Items.Clear();
                    foreach (DHModel item in BannerModel)
                    {
                        if (item.aid!=null)
                        {
                            home_flipView.Items.Add(item);
                        }
                    }
                    GridView_TJ.ItemsSource = RecommendsModel;
                    GridView_New.ItemsSource = NewsModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        /// <summary>
        /// 动态
        /// </summary>
        private async void GetDHDT()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://www.bilibili.com/index/ding/1.json?page=1&pagesize=50"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> DTModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    GridView_DT.Items.Clear();

                    GridView_DT.ItemsSource = DTModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }

        private async void GetZHHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=27"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    ZH_HotList.Items.Clear();

                    ZH_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        private async Task GetZHNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=27&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    ZH_NewList.Items.Clear();
                    for (int i = 0; i < 50; i++)
                    {
                        ReList.Add(new DHModel
                        {
                            aid = (string)json[i.ToString()]["aid"],
                            title = (string)json[i.ToString()]["title"],
                            pic = (string)json[i.ToString()]["pic"],
                            author = (string)json[i.ToString()]["author"],
                            play = (string)json[i.ToString()]["play"],
                            video_review = (string)json[i.ToString()]["video_review"],
                        });
                    }
                    ZH_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task GetMADNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=24&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    MAD_NewList.Items.Clear();
                    for (int i = 0; i < 50; i++)
                    {
                        ReList.Add(new DHModel
                        {
                            aid = (string)json[i.ToString()]["aid"],
                            title = (string)json[i.ToString()]["title"],
                            pic = (string)json[i.ToString()]["pic"],
                            author = (string)json[i.ToString()]["author"],
                            play = (string)json[i.ToString()]["play"],
                            video_review = (string)json[i.ToString()]["video_review"],
                        });
                    }
                    MAD_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception )
            {
                throw;
            }
        }

        private async void GetMADHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=24"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    MAD_HotList.Items.Clear();

                    MAD_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }

        private async Task GetMMDNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=25&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    MMD_NewList.Items.Clear();
                    for (int i = 0; i < 50; i++)
                    {
                        ReList.Add(new DHModel
                        {
                            aid = (string)json[i.ToString()]["aid"],
                            title = (string)json[i.ToString()]["title"],
                            pic = (string)json[i.ToString()]["pic"],
                            author = (string)json[i.ToString()]["author"],
                            play = (string)json[i.ToString()]["play"],
                            video_review = (string)json[i.ToString()]["video_review"],
                        });
                    }
                    MMD_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void GetMMDHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=25"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    MMD_HotList.Items.Clear();

                    MMD_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }

        private async Task GetDPNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=47&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    DP_NewList.Items.Clear();
                    for (int i = 0; i < 50; i++)
                    {
                        ReList.Add(new DHModel
                        {
                            aid = (string)json[i.ToString()]["aid"],
                            title = (string)json[i.ToString()]["title"],
                            pic = (string)json[i.ToString()]["pic"],
                            author = (string)json[i.ToString()]["author"],
                            play = (string)json[i.ToString()]["play"],
                            video_review = (string)json[i.ToString()]["video_review"],
                        });
                    }
                    DP_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void GetDPHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=47"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    DP_HotList.Items.Clear();

                    DP_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(VideoPage), ((DHModel)home_flipView.SelectedItem).aid);
        }

        private void ZH_HotList_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(VideoPage), ((DHModel)e.ClickedItem).aid);
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //防止B站返回超速错误
            try
            {
                await GetZHNew();
                await GetMADNew();
                await GetMMDNew();
                await GetDPNew();
            }
            catch (Exception)
            {
            }
           
        }
    }

    public class DHModel
    {
        public object result { get; set; }
        public object list { get; set; }

        public object recommends { get; set; }
        public string aid { get; set; }
        public string title { get; set; }
        public string play { get; set; }
        public string video_review{ get; set; }
        public string mid { get; set; }
        public string pic { get; set; }
        public string author { get; set; }

        public object banners { get; set; }
        public string img { get; set; }

        public object news { get; set; }

    }
}

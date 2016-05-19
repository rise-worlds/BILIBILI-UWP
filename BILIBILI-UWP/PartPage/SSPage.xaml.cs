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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace BILIBILI_UWP.PartPage
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SSPage : Page
    {
        public SSPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            GetDYHome();
            GetDYDT();
            GetMZHot();
            GetFSHot();
            GetZXHot();
        }
        private HttpClient hc;
        /// <summary>
        /// Banner，推荐，最新
        /// </summary>
        private async void GetDYHome()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/api/region2/155.json"));
                    hr.EnsureSuccessStatusCode();

                    var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = Encoding.UTF8.GetString(encodeResults.ToArray(), 0, encodeResults.ToArray().Length);
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    DHModel model2 = JsonConvert.DeserializeObject<DHModel>(model.result.ToString());

                    List<DHModel> BannerModel = JsonConvert.DeserializeObject<List<DHModel>>(model2.banners.ToString());
                    List<DHModel> RecommendsModel = JsonConvert.DeserializeObject<List<DHModel>>(model2.recommends.ToString());
                    List<DHModel> NewsModel = JsonConvert.DeserializeObject<List<DHModel>>(model2.news.ToString());
                    home_flipView.Items.Clear();
                    GridView_TJ.Items.Clear();
                    GridView_New.Items.Clear();
                    foreach (DHModel item in BannerModel)
                    {
                        if (item.aid != null || item.img != null)
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
        private async void GetDYDT()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://www.bilibili.com/index/ding/155.json?page=1&pagesize=50"));
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

        private async void GetMZHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=157"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    MZ_HotList.Items.Clear();

                    MZ_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        private async Task GetMZNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=157&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    MZ_NewList.Items.Clear();
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
                    MZ_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception)
            {

            }
        }

        private async void GetFSHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=158"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    FS_HotList.Items.Clear();

                    FS_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        private async Task GetFSNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=158&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    FS_NewList.Items.Clear();
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
                   FS_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception)
            {

            }
        }

        private async void GetZXHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=159"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    ZX_HotList.Items.Clear();

                    ZX_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        private async Task GetZXNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=159&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    ZX_NewList.Items.Clear();
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
                   ZX_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception)
            {

            }
        }

        private void GridView_TJ_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(VideoPage), ((DHModel)e.ClickedItem).aid);
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(VideoPage), ((DHModel)home_flipView.SelectedItem).aid);
        }

        private void ZH_HotList_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(VideoPage), ((DHModel)e.ClickedItem).aid);
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await GetMZNew();
            await GetFSNew();
            await GetZXNew();
        }
    }
}

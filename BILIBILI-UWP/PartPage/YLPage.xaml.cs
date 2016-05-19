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

namespace BILIBILI_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class YLPage : Page
    {
        public YLPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            GetDHHome();
            GetYYDT();
            GetGXHot();
            GetSHHot();
            GetMSHot();
            GetDWHot();
            GetZYHot();
            GetYLHot();
            GetKOHot();
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

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/api/region2/5.json"));
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
        private async void GetYYDT()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://www.bilibili.com/index/ding/5.json?page=1&pagesize=50"));
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

        private async void GetGXHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=138"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    GX_HotList.Items.Clear();

                   GX_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        private async Task GetGXNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=138&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    GX_NewList.Items.Clear();
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
                    GX_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception)
            {

            }
        }

        private async void GetSHHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=31"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    SH_HotList.Items.Clear();

                    SH_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        private async Task GetSHNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=21&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    SH_NewList.Items.Clear();
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
                    SH_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception)
            {

            }
        }

        private async void GetMSHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=76"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    MS_HotList.Items.Clear();

                    MS_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        private async Task GetMSNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=76&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    MS_NewList.Items.Clear();
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
                    MS_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception)
            {

            }
        }

        private async void GetDWHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=75"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    DW_HotList.Items.Clear();

                    DW_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        private async Task GetDWNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=75&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    DW_NewList.Items.Clear();
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
                    DW_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception)
            {

            }
        }

        private async void GetZYHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=71"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    ZY_HotList.Items.Clear();

                    ZY_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        private async Task GetZYNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=71&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    ZY_NewList.Items.Clear();
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
                    ZY_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception)
            {

            }
        }

        private async void GetYLHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=137"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    YL_HotList.Items.Clear();

                    YL_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        private async Task GetYLNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=137&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    YL_NewList.Items.Clear();
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
                    YL_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception)
            {

            }
        }

        private async void GetKOHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=131"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    KO_HotList.Items.Clear();

                    KO_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        private async Task GetKONew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=131&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    KO_NewList.Items.Clear();
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
                    KO_NewList.ItemsSource = ReList;
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
            await GetGXNew();
            await GetSHNew();
            await GetMSNew();
            await GetDWNew();
            await GetZYNew();
            await GetYLNew();
            await GetKONew();
        }
    }
}

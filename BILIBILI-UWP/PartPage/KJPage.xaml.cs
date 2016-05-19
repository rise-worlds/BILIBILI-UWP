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
    public sealed partial class KJPage : Page
    {
        public KJPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required; 
            GetKJHome();
            GetKJDT();
            GetJLHot();
            GetQWHot();
            GetYSHot();
            GetYJHot();
            GetJSHot();
            GetSMHot();
            GetJXHot();
        }
        private HttpClient hc;
        /// <summary>
        /// Banner，推荐，最新
        /// </summary>
        private async void GetKJHome()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/api/region2/36.json"));
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
        private async void GetKJDT()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://www.bilibili.com/index/ding/36.json?page=1&pagesize=50"));
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

        private async void GetJLHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=37"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    JL_HotList.Items.Clear();

                    JL_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        private async Task GetJLNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=37&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    JL_NewList.Items.Clear();
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
                    JL_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception)
            {

            }
        }

        private async void GetQWHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=124"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    QW_HotList.Items.Clear();

                    QW_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        private async Task GetQWNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=124&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    QW_NewList.Items.Clear();
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
                    QW_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception)
            {

            }
        }

        private async void GetYSHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=122"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    YS_HotList.Items.Clear();

                    YS_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        private async Task GetYSNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=122&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    YS_NewList.Items.Clear();
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
                    YS_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception)
            {

            }
        }

        private async void GetYJHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=39"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    YJ_HotList.Items.Clear();

                    YJ_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        private async Task GetYJNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=39&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    YJ_NewList.Items.Clear();
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
                    YJ_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception)
            {

            }
        }

        private async void GetJSHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=96"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    JS_HotList.Items.Clear();

                    JS_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        private async Task GetJSNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=96&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    JS_NewList.Items.Clear();
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
                    JS_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception)
            {

            }
        }

        private async void GetSMHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=95"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    SM_HotList.Items.Clear();

                    SM_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        private async Task GetSMNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=95&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    SM_NewList.Items.Clear();
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
                    SM_NewList.ItemsSource = ReList;
                }

            }
            catch (Exception)
            {

            }
        }

        private async void GetJXHot()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://app.bilibili.com/bangumi/tid_recommend?appkey=c1b107428d337928&page=1&pagesize=4&tid=98"));
                    hr.EnsureSuccessStatusCode();

                    // var encodeResults = await hr.Content.ReadAsBufferAsync();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    List<DHModel> ZHModel = JsonConvert.DeserializeObject<List<DHModel>>(model.list.ToString());
                    JX_HotList.Items.Clear();

                   JX_HotList.ItemsSource = ZHModel;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        private async Task GetJXNew()
        {
            try
            {
                using (hc = new HttpClient())
                {

                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid=98&page=1&pagesize=50&order=new"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    DHModel model = JsonConvert.DeserializeObject<DHModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    List<DHModel> ReList = new List<DHModel>();
                    JX_NewList.Items.Clear();
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
                    JX_NewList.ItemsSource = ReList;
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
            await GetJLNew();
            await GetQWNew();
            await GetYSNew();
            await GetYJNew();
            await GetJSNew();
            await GetSMNew();
            await GetJXNew();
        }
    }
}

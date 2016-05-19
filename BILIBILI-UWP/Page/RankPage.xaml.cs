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
    public sealed partial class RankPage : Page
    {
        public RankPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            Get();
        }
        HttpClient hc;
        //public async void GetQZRank()
        //{
        //    //try
        //    //{
        //        using (hc = new HttpClient())
        //        {

        //            HttpResponseMessage hr = await hc.GetAsync(new Uri("http://www.bilibili.com/index/catalogy/0-week.json"));
        //            hr.EnsureSuccessStatusCode();
        //            string results = await hr.Content.ReadAsStringAsync();
        //            RankModel model = JsonConvert.DeserializeObject<RankModel>(results);
        //            RankModel model2 = JsonConvert.DeserializeObject<RankModel>(model.hot_original.ToString());
        //           List<RankModel> list = JsonConvert.DeserializeObject<List<RankModel>>(model2.list.ToString());

        //            QQ_Rank_QZ.Items.Clear();
        //            //List<InfoModel> ReList = new List<InfoModel>();
        //            for (int i = 0; i < list.Count; i++)
        //            {
        //                list[i].num = i + 1;
        //            }
        //            QQ_Rank_QZ.ItemsSource = list;
        //        }

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MessageDialog md = new MessageDialog(ex.Message);
        //    //    await md.ShowAsync();
        //    //}
        //}
        public async Task GetQZRank()
        {
            try
            {
                using (hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.cn/list?appkey=84b739484c36d653&order=hot&original=0&page=1&pagesize=20"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    InfoModel model = new InfoModel();
                    model = JsonConvert.DeserializeObject<InfoModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    QQ_Rank_QZ.Items.Clear();
                    List<InfoModel> ReList = new List<InfoModel>();
                    for (int i = 0; i < 20; i++)
                    {
                        ReList.Add(new InfoModel
                        {
                            aid = (string)json[i.ToString()]["aid"],
                            title = (string)json[i.ToString()]["title"],
                            pic = (string)json[i.ToString()]["pic"],
                            author = (string)json[i.ToString()]["author"],
                            play = (string)json[i.ToString()]["play"],
                            video_review = (string)json[i.ToString()]["video_review"],
                            num = i + 1
                        });
                    }
                    QQ_Rank_QZ.ItemsSource = ReList;
                }

            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        public async Task GetYCRank()
        {
            try
            {
                using (hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/list?appkey=422fd9d7289a1dd9&order=hot&original=1&page=1&pagesize=20"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    InfoModel model = new InfoModel();
                    model = JsonConvert.DeserializeObject<InfoModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    QQ_Rank_YC.Items.Clear();
                    List<InfoModel> ReList = new List<InfoModel>();
                    for (int i = 0; i < 20; i++)
                    {
                        ReList.Add(new InfoModel
                        {
                            aid = (string)json[i.ToString()]["aid"],
                            title = (string)json[i.ToString()]["title"],
                            pic = (string)json[i.ToString()]["pic"],
                            author = (string)json[i.ToString()]["author"],
                            play = (string)json[i.ToString()]["play"],
                            video_review = (string)json[i.ToString()]["video_review"],
                            num = i + 1
                        });
                    }
                    QQ_Rank_YC.ItemsSource = ReList;
                }
              
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        public async Task GetFJRank()
        {
            try
            {
                using (hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.cn/list?appkey=422fd9d7289a1dd9&order=hot&original=0&page=1&pagesize=20&tid=13 "));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    InfoModel model = new InfoModel();
                    model = JsonConvert.DeserializeObject<InfoModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    QQ_Rank_FJ.Items.Clear();
                    List<InfoModel> ReList = new List<InfoModel>();
                    for (int i = 0; i < 20; i++)
                    {
                        ReList.Add(new InfoModel
                        {
                            aid = (string)json[i.ToString()]["aid"],
                            title = (string)json[i.ToString()]["title"],
                            pic = (string)json[i.ToString()]["pic"],
                            author = (string)json[i.ToString()]["author"],
                            play = (string)json[i.ToString()]["play"],
                            video_review = (string)json[i.ToString()]["video_review"],
                            num = i + 1
                        });
                    }
                    QQ_Rank_FJ.ItemsSource = ReList;
                } 
                
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }
        public async Task GetDHRank()
        {
            try
            {
                using (hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.cn/list?appkey=422fd9d7289a1dd9&order=hot&original=0&page=1&pagesize=20&tid=1"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    InfoModel model = new InfoModel();
                    model = JsonConvert.DeserializeObject<InfoModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    QQ_Rank_DH.Items.Clear();
                    List<InfoModel> ReList = new List<InfoModel>();
                    for (int i = 0; i < 20; i++)
                    {
                        ReList.Add(new InfoModel
                        {
                            aid = (string)json[i.ToString()]["aid"],
                            title = (string)json[i.ToString()]["title"],
                            pic = (string)json[i.ToString()]["pic"],
                            author = (string)json[i.ToString()]["author"],
                            play = (string)json[i.ToString()]["play"],
                            video_review = (string)json[i.ToString()]["video_review"],
                            num = i + 1
                        });
                    }
                    QQ_Rank_DH.ItemsSource = ReList;
                }
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }

        }

        public async Task GetYYRank()
        {
            try
            {
                using (hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.cn/list?appkey=422fd9d7289a1dd9&order=hot&original=0&page=1&pagesize=20&tid=3"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    InfoModel model = new InfoModel();
                    model = JsonConvert.DeserializeObject<InfoModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    QQ_Rank_YY.Items.Clear();
                    List<InfoModel> ReList = new List<InfoModel>();
                    for (int i = 0; i < 20; i++)
                    {
                        ReList.Add(new InfoModel
                        {
                            aid = (string)json[i.ToString()]["aid"],
                            title = (string)json[i.ToString()]["title"],
                            pic = (string)json[i.ToString()]["pic"],
                            author = (string)json[i.ToString()]["author"],
                            play = (string)json[i.ToString()]["play"],
                            video_review = (string)json[i.ToString()]["video_review"],
                            num = i + 1
                        });
                    }
                    QQ_Rank_YY.ItemsSource = ReList;
                }
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }

        }
        public async Task GetWDRank()
        {
            try
            {
                using (hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.cn/list?appkey=422fd9d7289a1dd9&order=hot&original=0&page=1&pagesize=20&tid=129"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    InfoModel model = new InfoModel();
                    model = JsonConvert.DeserializeObject<InfoModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    QQ_Rank_WD.Items.Clear();
                    List<InfoModel> ReList = new List<InfoModel>();
                    for (int i = 0; i < 20; i++)
                    {
                        ReList.Add(new InfoModel
                        {
                            aid = (string)json[i.ToString()]["aid"],
                            title = (string)json[i.ToString()]["title"],
                            pic = (string)json[i.ToString()]["pic"],
                            author = (string)json[i.ToString()]["author"],
                            play = (string)json[i.ToString()]["play"],
                            video_review = (string)json[i.ToString()]["video_review"],
                            num = i + 1
                        });
                    }
                    QQ_Rank_WD.ItemsSource = ReList;
                } 
               
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }

        }

        public async Task GetYXRank()
        {
            try
            {
                HttpClient hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.cn/list?appkey=422fd9d7289a1dd9&order=hot&original=0&page=1&pagesize=20&tid=4"));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                InfoModel model = new InfoModel();
                model = JsonConvert.DeserializeObject<InfoModel>(results);
                JObject json = JObject.Parse(model.list.ToString());
                QQ_Rank_YX.Items.Clear();
                List<InfoModel> ReList = new List<InfoModel>();
                for (int i = 0; i < 20; i++)
                {
                    ReList.Add(new InfoModel
                    {
                        aid = (string)json[i.ToString()]["aid"],
                        title = (string)json[i.ToString()]["title"],
                        pic = (string)json[i.ToString()]["pic"],
                        author = (string)json[i.ToString()]["author"],
                        play = (string)json[i.ToString()]["play"],
                        video_review = (string)json[i.ToString()]["video_review"],
                        num = i + 1
                    });
                }
                QQ_Rank_YX.ItemsSource = ReList;
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }

        }
        public async Task GetKJRank()
        {
            try
            {
                using (hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.cn/list?appkey=422fd9d7289a1dd9&order=hot&original=0&page=1&pagesize=20&tid=36"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    InfoModel model = new InfoModel();
                    model = JsonConvert.DeserializeObject<InfoModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    QQ_Rank_KJ.Items.Clear();
                    List<InfoModel> ReList = new List<InfoModel>();
                    for (int i = 0; i < 20; i++)
                    {
                        ReList.Add(new InfoModel
                        {
                            aid = (string)json[i.ToString()]["aid"],
                            title = (string)json[i.ToString()]["title"],
                            pic = (string)json[i.ToString()]["pic"],
                            author = (string)json[i.ToString()]["author"],
                            play = (string)json[i.ToString()]["play"],
                            video_review = (string)json[i.ToString()]["video_review"],
                            num = i + 1
                        });
                    }
                    QQ_Rank_KJ.ItemsSource = ReList;
                }
                
              
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }

        }

        public async Task GetYLRank()
        {
            try
            {
                using (hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.cn/list?appkey=422fd9d7289a1dd9&order=hot&original=0&page=1&pagesize=20&tid=5"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    InfoModel model = new InfoModel();
                    model = JsonConvert.DeserializeObject<InfoModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    QQ_Rank_YL.Items.Clear();
                    List<InfoModel> ReList = new List<InfoModel>();
                    for (int i = 0; i < 20; i++)
                    {
                        ReList.Add(new InfoModel
                        {
                            aid = (string)json[i.ToString()]["aid"],
                            title = (string)json[i.ToString()]["title"],
                            pic = (string)json[i.ToString()]["pic"],
                            author = (string)json[i.ToString()]["author"],
                            play = (string)json[i.ToString()]["play"],
                            video_review = (string)json[i.ToString()]["video_review"],
                            num = i + 1
                        });
                    }
                    QQ_Rank_YL.ItemsSource = ReList;
                }
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }

        }
        public async Task GetGCRank()
        {
            try
            {
                HttpClient hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.cn/list?appkey=422fd9d7289a1dd9&order=hot&original=0&page=1&pagesize=20&tid=119"));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                InfoModel model = new InfoModel();
                model = JsonConvert.DeserializeObject<InfoModel>(results);
                JObject json = JObject.Parse(model.list.ToString());
                QQ_Rank_GC.Items.Clear();
                List<InfoModel> ReList = new List<InfoModel>();
                for (int i = 0; i < 20; i++)
                {
                    ReList.Add(new InfoModel
                    {
                        aid = (string)json[i.ToString()]["aid"],
                        title = (string)json[i.ToString()]["title"],
                        pic = (string)json[i.ToString()]["pic"],
                        author = (string)json[i.ToString()]["author"],
                        play = (string)json[i.ToString()]["play"],
                        video_review = (string)json[i.ToString()]["video_review"],
                        num = i + 1
                    });
                }
                QQ_Rank_GC.ItemsSource = ReList;
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }

        }


        public async Task GetDYRank()
        {
            try
            {
                using (hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.cn/list?appkey=422fd9d7289a1dd9&order=hot&original=0&page=1&pagesize=20&tid=23"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    InfoModel model = new InfoModel();
                    model = JsonConvert.DeserializeObject<InfoModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    QQ_Rank_DY.Items.Clear();
                    List<InfoModel> ReList = new List<InfoModel>();
                    for (int i = 0; i < 20; i++)
                    {
                        ReList.Add(new InfoModel
                        {
                            aid = (string)json[i.ToString()]["aid"],
                            title = (string)json[i.ToString()]["title"],
                            pic = (string)json[i.ToString()]["pic"],
                            author = (string)json[i.ToString()]["author"],
                            play = (string)json[i.ToString()]["play"],
                            video_review = (string)json[i.ToString()]["video_review"],
                            num = i + 1
                        });
                    }
                    QQ_Rank_DY.ItemsSource = ReList;
                }
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }

        }
        public async Task GetDSJRank()
        {
            try
            {
                using (hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.cn/list?appkey=84b739484c36d653&order=hot&original=0&page=1&pagesize=20&tid=11"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    InfoModel model = new InfoModel();
                    model = JsonConvert.DeserializeObject<InfoModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    QQ_Rank_DSJ.Items.Clear();
                    List<InfoModel> ReList = new List<InfoModel>();
                    for (int i = 0; i < 20; i++)
                    {
                        ReList.Add(new InfoModel
                        {
                            aid = (string)json[i.ToString()]["aid"],
                            title = (string)json[i.ToString()]["title"],
                            pic = (string)json[i.ToString()]["pic"],
                            author = (string)json[i.ToString()]["author"],
                            play = (string)json[i.ToString()]["play"],
                            video_review = (string)json[i.ToString()]["video_review"],
                            num = i + 1
                        });
                    }
                    QQ_Rank_DSJ.ItemsSource = ReList;
                }
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }

        }
        public async Task GetSSRank()
        {
            try
            {
                using (hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.cn/list?appkey=84b739484c36d653&order=hot&original=0&page=1&pagesize=20&tid=155"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    InfoModel model = new InfoModel();
                    model = JsonConvert.DeserializeObject<InfoModel>(results);
                    JObject json = JObject.Parse(model.list.ToString());
                    QQ_Rank_SS.Items.Clear();
                    List<InfoModel> ReList = new List<InfoModel>();
                    for (int i = 0; i < 20; i++)
                    {
                        ReList.Add(new InfoModel
                        {
                            aid = (string)json[i.ToString()]["aid"],
                            title = (string)json[i.ToString()]["title"],
                            pic = (string)json[i.ToString()]["pic"],
                            author = (string)json[i.ToString()]["author"],
                            play = (string)json[i.ToString()]["play"],
                            video_review = (string)json[i.ToString()]["video_review"],
                            num = i + 1
                        });
                    }
                    QQ_Rank_SS.ItemsSource = ReList;
                } 
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }

        }
        private async void Get()
        {
            await GetQZRank();
            await GetYCRank();
            await GetFJRank();
            await GetDHRank();
            await GetYYRank();
            await GetWDRank();
            await GetYXRank();
            await GetKJRank();
            await GetYLRank();
            await GetGCRank();
            await GetDYRank();
            await GetDSJRank();
            await GetSSRank();
        }
        private  void Page_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void QQ_Rank_YC_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(VideoPage), ((InfoModel)e.ClickedItem).aid);
        }
    }


    public class RankModel
    {
        public object hot_original { get; set; }
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
    }
}

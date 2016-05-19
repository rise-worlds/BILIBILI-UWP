using Newtonsoft.Json;
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
    public sealed partial class SubPage : Page
    {
        public SubPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }
        private string getUid = "";
        private int getPage = 1;
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            getUid = e.Parameter as string;
            Grid_Content.Visibility = Visibility.Collapsed;
            Grid_Loading.Visibility = Visibility.Visible;
            //btn_LoadMore.IsEnabled = false;
            //btn_LoadMore.Content = "正在加载";
            getPage = 1;
            ls_Sub.Items.Clear();
            await GetSubInfo(getPage, getUid);
            Grid_Content.Visibility = Visibility.Visible;
            Grid_Loading.Visibility = Visibility.Collapsed;
        }

        private async Task GetSubInfo(int page, string uid)
        {
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://space.bilibili.com/ajax/member/getSubmitVideos?mid=" + uid + "&pagesize=20"+"&page="+page));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    //一层
                    GetUserSubmit model1 = JsonConvert.DeserializeObject<GetUserSubmit>(results);
                        //二层
                        GetUserSubmit model2 = JsonConvert.DeserializeObject<GetUserSubmit>(model1.data.ToString());
                        //三层
                        List<GetUserSubmit> lsModel = JsonConvert.DeserializeObject<List<GetUserSubmit>>(model2.vlist.ToString());
                        foreach (GetUserSubmit item in lsModel)
                        {
                            ls_Sub.Items.Add(item);
                        }
                        getPage++;
                        if (model2.pages < getPage)
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
            this.Frame.Navigate(typeof(VideoPage), (e.ClickedItem as GetUserSubmit).aid);
        }


        private  void btn_LoadMore_Click(object sender, RoutedEventArgs e)
        {
            //btn_LoadMore.IsEnabled = false;
            //btn_LoadMore.Content = "正在加载";
            //await GetSubInfo(getPage, getUid);
            //if (btn_LoadMore.Content.ToString() != "加载完了...")
            //{
            //    btn_LoadMore.IsEnabled = true;
            //    btn_LoadMore.Content = "加载更多";
            //}

        }

        private async void sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (sv.VerticalOffset == sv.ScrollableHeight)
            {
                btn_LoadMore.IsEnabled = false;
                btn_LoadMore.Content = "正在加载";
                await GetSubInfo(getPage, getUid);
                if (btn_LoadMore.Content.ToString() != "加载完了...")
                {
                    btn_LoadMore.IsEnabled = true;
                    btn_LoadMore.Content = "加载更多";
                }
            }
        }
    }
}

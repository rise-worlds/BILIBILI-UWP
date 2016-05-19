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
using Windows.System.Display;
using Windows.UI.ViewManagement;
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
    public sealed partial class LivePlayerPage : Page
    {
        public LivePlayerPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }
        string room_ID = "";
        private DisplayRequest dispRequest = null;
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            grid_Error.Visibility = Visibility.Collapsed;
            grid_GG.Visibility = Visibility.Visible;
            room_ID = e.Parameter as string;
            string uri = await GetLiveUri(room_ID);
            if (uri!="错误")
            {
                mediaElement.Source = new Uri(uri);
            }
            else
            {
                grid_Error.Visibility = Visibility.Visible;
            }
            if (dispRequest == null)
            {
                // 用户观看视频，需要保持屏幕的点亮状态
                dispRequest = new DisplayRequest();
                dispRequest.RequestActive(); // 激活显示请求
            }
            await Task.Delay(5000);
            grid_GG.Visibility = Visibility.Collapsed;
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ApplicationView.GetForCurrentView().ExitFullScreenMode();
            if (dispRequest != null)
            {
                // 用户暂停了视频，则不需要保持屏幕的点亮状态
                dispRequest.RequestRelease(); // 停用显示请求
                dispRequest = null;
            }
        }

        public async System.Threading.Tasks.Task<string> GetLiveUri(string room_id)
        {
            try
            {
                HttpClient hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://live.bilibili.com/api/playurl?platform=h5&cid=" + room_id));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                InfoModel model = new InfoModel();
                //model = JsonConvert.DeserializeObject<InfoModel>(results);
                JObject json = JObject.Parse(results);
                return (string)json["data"];
            }
            catch (Exception)
            {
                return "错误";
                throw;
            }
            
        }

        private async void btn_re_Click(object sender, RoutedEventArgs e)
        {
            string uri = await GetLiveUri(room_ID);
            if (uri != "错误")
            {
                mediaElement.Source = new Uri(uri);
            }
            else
            {
                grid_Error.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void btn_re_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}

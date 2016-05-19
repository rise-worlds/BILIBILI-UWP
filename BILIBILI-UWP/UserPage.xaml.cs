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
using Windows.Web.Http.Filters;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace BILIBILI_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class UserPage : Page
    {
        public delegate void DeUpdate();
        public event DeUpdate UpdateUserInfo;
        public UserPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            
        }
        

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }
        private void user_Exit_Click(object sender, RoutedEventArgs e)
        {
            //可以使用两个方法完成退出，发送个Get请求，自动删除；手动删除Cookie
            //HttpClient hc = new HttpClient();
            //HttpResponseMessage hr3 = await hc.GetAsync(new Uri("https://account.bilibili.com/login?act=exit"));
            //hr3.EnsureSuccessStatusCode();
            //MessageDialog md = new MessageDialog("已经退出");
            //await md.ShowAsync();
            //hr3.Dispose();
            //HttpCookie cookie = new HttpCookie("sid", ".bilibili.com", "/");
            List<HttpCookie> listCookies = new List<HttpCookie>();
            listCookies.Add(new HttpCookie("sid", ".bilibili.com", "/"));
            listCookies.Add(new HttpCookie("DedeUserID", ".bilibili.com", "/"));
            listCookies.Add(new HttpCookie("DedeUserID__ckMd5", ".bilibili.com", "/"));
            listCookies.Add(new HttpCookie("SESSDATA", ".bilibili.com", "/"));
            listCookies.Add(new HttpCookie("LIVE_LOGIN_DATA", ".bilibili.com", "/"));
            listCookies.Add(new HttpCookie("LIVE_LOGIN_DATA__ckMd5", ".bilibili.com", "/"));
            HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
            foreach (HttpCookie cookie in listCookies)
            {
                filter.CookieManager.DeleteCookie(cookie);
            }
            UpdateUserInfo();
            this.Frame.Navigate(typeof(HomePage));
        }

        private void user_GridView_Submit_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(VideoPage), ((GetUserSubmit)e.ClickedItem).aid);
        }

        private void user_GridView_Bangumi_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(BanSeasonNewPage), ((GetUserBangumi)e.ClickedItem).season_id);
        }

        private void user_GridView_Attention_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(UserInfoPage), ((GetUserAttention)e.ClickedItem).fid);
        }

        private void user_GridView_FovBox_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(FavoritesInfoPage), ((GetUserFovBox)e.ClickedItem).fav_box);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetLoginInfoClass getUser = new GetLoginInfoClass();
            if (!getUser.IsLogin())
            {
                //MessageDialog md = new MessageDialog("请先登录！");
                //await md.ShowAsync();
                this.Frame.Navigate(typeof(LoginPage));
            }
            else
            {
                User_Content.Visibility = Visibility.Collapsed;
                User_Loading.Visibility = Visibility.Visible;
                try
                {
                    GetLoginInfoModel model = await getUser.GetUserInfo();
                    List<GetUserBangumi> lsModel = await getUser.GetUserBangumi();
                    List<GetUserFovBox> lsBoxModel = await getUser.GetUserFovBox();
                    List<GetUserAttention> lsAttentionModel = await getUser.GetUserAttention();
                    List<GetUserSubmit> lsSubmitModel = await getUser.GetUserSubmit();
                    if (lsSubmitModel != null)
                    {
                        user_GridView_Submit.Items.Clear();
                        foreach (GetUserSubmit item in lsSubmitModel)
                        {
                            user_GridView_Submit.Items.Add(item);
                        }
                    }
                    if (model != null || lsModel != null || lsBoxModel != null)
                    {
                        User_Grid_Info.DataContext = model;
                        user_GridView_Bangumi.Items.Clear();
                        user_GridView_FovBox.Items.Clear();
                        foreach (GetUserBangumi item in lsModel)
                        {
                            user_GridView_Bangumi.Items.Add(item);
                        }
                        foreach (GetUserFovBox item in lsBoxModel)
                        {
                            user_GridView_FovBox.Items.Add(item);
                        }
                        foreach (GetUserAttention item in lsAttentionModel)
                        {
                            user_GridView_Attention.Items.Add(item);
                        }

                        User_Content.Visibility = Visibility.Visible;
                        User_Loading.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        MessageDialog md = new MessageDialog("信息读取失败！请重新登录试试！");
                        await md.ShowAsync();
                        this.Frame.Navigate(typeof(LoginPage));
                    }
                }
                catch (Exception)
                {
                    MessageDialog md = new MessageDialog("信息读取了发生错误！请重新试！");
                    await md.ShowAsync();
                    this.Frame.GoBack();
                }
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SubPage), txt_id.Text);
        }
    }
}

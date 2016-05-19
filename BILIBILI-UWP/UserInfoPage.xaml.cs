using Newtonsoft.Json.Linq;
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
    public sealed partial class UserInfoPage : Page
    {
        public UserInfoPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            
            
        }

        private async void Get()
        {
            if (uid != "")
            {
                GetLoginInfoClass getUser = new GetLoginInfoClass();
                User_Content.Visibility = Visibility.Collapsed;
                User_Loading.Visibility = Visibility.Visible;
                GetLoginInfoModel model = await getUser.GetUserInfo(uid);
                List<GetUserBangumi> lsModel = await getUser.GetUserBangumi(uid);
                // List<GetUserFovBox> lsBoxModel = await getUser.GetUserFovBox();
                // List<GetUserAttention> lsAttentionModel = await getUser.GetUserAttention();
                List<GetUserSubmit> lsSubmitModel = await getUser.GetUserSubmit(uid);
                if (lsSubmitModel != null)
                {
                    user_GridView_Submit.Items.Clear();
                    foreach (GetUserSubmit item in lsSubmitModel)
                    {
                        user_GridView_Submit.Items.Add(item);
                    }
                }
                if (model != null || lsModel != null)
                {
                    User_Grid_Info.DataContext = model;
                    user_GridView_Bangumi.Items.Clear();
                    foreach (GetUserBangumi item in lsModel)
                    {
                        user_GridView_Bangumi.Items.Add(item);
                    }
                    await getUser.GetUserAttention();
                    if (GetLoginInfoClass.AttentionList.Contains(uid))
                    {
                        btn_Act.Label= "取消关注";
                    }
                    else
                    {
                        btn_Act.Label = "关注Ta";
                    }
                    User_Content.Visibility = Visibility.Visible;
                    User_Loading.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageDialog md = new MessageDialog("信息读取失败！");
                    await md.ShowAsync();
                }
            }
        }
        private static string uid = "";
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter!=null)
            {
                uid = e.Parameter.ToString();
            }
            Get();
        }
        /// <summary>
        /// 关注、取消关注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void user_Exit_Click(object sender, RoutedEventArgs e)
        {
            GetLoginInfoClass getUser = new GetLoginInfoClass();
            if (getUser.IsLogin())
            {
                try
                {
                    Uri ReUri ;
                    if (btn_Act.Label == "关注Ta")
                    {
                        ReUri = new Uri("http://space.bilibili.com/ajax/friend/AddAttention");
                    }
                    else
                    {
                        ReUri = new Uri("http://space.bilibili.com/ajax/friend/DelAttention");
                    }
                    HttpClient hc = new HttpClient();
                    hc.DefaultRequestHeaders.Referer = new Uri("http://space.bilibili.com/");
                    var response = await hc.PostAsync(ReUri, new HttpStringContent("mid="+uid, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/x-www-form-urlencoded"));
                   response.EnsureSuccessStatusCode();
                  string result = await response.Content.ReadAsStringAsync();
                   JObject json = JObject.Parse(result);
                    if ((bool)json["status"])
                    {
                        if (btn_Act.Label == "关注Ta")
                        {
                            MessageDialog md = new MessageDialog("关注成功！");
                            await md.ShowAsync();
                            btn_Act.Label = "取消关注";
                        }
                        else
                        {
                            MessageDialog md = new MessageDialog("取消关注成功！");
                            await md.ShowAsync();
                            btn_Act.Label = "关注Ta";
                        }
                        await getUser.GetUserAttention();
                    }
                    else
                    {
                        MessageDialog md = new MessageDialog("关注失败！");
                        await md.ShowAsync();
                        btn_Act.Label = "关注Ta";
                    }
                    
                }
                catch (Exception)
                {

                    throw;

                }
            }
            else
            {
                MessageDialog md = new MessageDialog("请先登录！");
                await md.ShowAsync();
            }
        }

        private void user_GridView_Submit_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(VideoPage), ((GetUserSubmit)e.ClickedItem).aid);
        }

        private void user_GridView_Bangumi_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(BanSeasonNewPage), ((GetUserBangumi)e.ClickedItem).season_id);
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SubPage), txt_id.Text);
        }
    }
}

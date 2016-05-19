using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.UI.Popups;
using Windows.Web.Http.Filters;
using Windows.Web.Http.Headers;
using System.Threading.Tasks;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace BILIBILI_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AttentionUpdatePage : Page
    {
        public AttentionUpdatePage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            pageNum = 1;
            if (e.Parameter!=null)
            {
                switch ((int)e.Parameter)
                {
                    case 0:
                        User_Attention_Pivot.SelectedIndex = 0;
                        break;
                    case 1:
                        User_Attention_Pivot.SelectedIndex = 1;
                        break;
                    case 2:
                        User_Attention_Pivot.SelectedIndex = 3;
                        break;
                    default:
                        break;
                }
            }
        }
        private int pageNum = 1;
        //private int pageNum_Fav = 1;
        private int pageNum_His = 1;
        //private int pageNum_Sp = 1;
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetLoginInfoClass getLogin = new GetLoginInfoClass();
            if (getLogin.IsLogin() == false)
            {
                this.Frame.Navigate(typeof(LoginPage));
            }
            else
            {
                User_ListView_Attention.Items.Clear();
                //User_ListView_FavouriteVideo.Items.Clear();
                User_ListView_History.Items.Clear();
                User_Attention_Pivot.Visibility = Visibility.Collapsed;
                User_Loading.Visibility = Visibility.Visible;
                await GetAttentionUpdatesInfo();
                //await GetFavouriteVideoInfo();
                await GetHistoryInfo();
                await GetFavSpInfo();
                User_Attention_Pivot.Visibility = Visibility.Visible;
                User_Loading.Visibility = Visibility.Collapsed;
            }
        }
        private async Task GetFavSpInfo()
        {
            GetLoginInfoClass getLogin = new GetLoginInfoClass();
            List<FavSpModel> lsModel = await getLogin.GetFavouriteSp(pageNum);
            if (lsModel != null)
            {
                foreach (FavSpModel item in lsModel)
                {
                    Sp_ListView_Attention.Items.Add(item);
                }
            }
            else
            {
            }
        }

        private async Task GetAttentionUpdatesInfo()
        {
            GetLoginInfoClass getLogin = new GetLoginInfoClass();
            //User_Attention_Pivot.Visibility = Visibility.Collapsed;
            //User_Loading.Visibility = Visibility.Visible;
            User_load_more.Content = "正在加载";
            User_load_more.IsEnabled = false; 
            List<GetAttentionUpdate> lsModel = await getLogin.GetUserAttentionUpdate(pageNum);
            if (lsModel!=null)
            {
                foreach (GetAttentionUpdate item in lsModel)
                {
                    User_ListView_Attention.Items.Add(item);
                }
                pageNum++;
                if (pageNum > 3)
                {
                    User_load_more.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                MessageDialog md = new MessageDialog("信息读取失败!请重新登陆试试！");
                await md.ShowAsync();
                User_load_more.Visibility = Visibility.Collapsed;
            }
            User_load_more.Content = "";
            User_load_more.IsEnabled = true;
            // User_Attention_Pivot.Visibility = Visibility.Visible;
            //User_Loading.Visibility = Visibility.Collapsed;
        }
        //private async Task GetFavouriteVideoInfo()
        //{
        //    GetLoginInfoClass getLogin = new GetLoginInfoClass();
        //    User_Attention_Pivot.Visibility = Visibility.Collapsed;
        //    User_Loading.Visibility = Visibility.Visible;
        //    List<GetFavouriteVideo> lsModel = await getLogin.GetFavouriteVideo(pageNum_Fav);
        //    if (lsModel != null)
        //    {
        //        foreach (GetFavouriteVideo item in lsModel)
        //        {
        //            User_ListView_FavouriteVideo.Items.Add(item);
        //        }
        //        pageNum_Fav++;
        //    }
        //    else
        //    {
        //        User_load_more_Fav.Visibility = Visibility.Collapsed;
        //    }
        //    User_Attention_Pivot.Visibility = Visibility.Visible;
        //    User_Loading.Visibility = Visibility.Collapsed;
        //}
        private async Task GetHistoryInfo()
        {
            GetLoginInfoClass getLogin = new GetLoginInfoClass();
            //User_Attention_Pivot.Visibility = Visibility.Collapsed;
            //User_Loading.Visibility = Visibility.Visible;
            User_load_more_history.Content = "正在加载";
            User_load_more_history.IsEnabled = false;
            List<GetHistoryModel> lsModel = await getLogin.GetHistory(pageNum_His);
            if (lsModel != null)
            {
                foreach (GetHistoryModel item in lsModel)
                {
                    User_ListView_History.Items.Add(item);
                }
            }
            else
            {
                User_load_more_history.Visibility = Visibility.Collapsed;
            }
            User_load_more_history.Content = "";
            User_load_more_history.IsEnabled = true;
            //User_Attention_Pivot.Visibility = Visibility.Visible;
            //User_Loading.Visibility = Visibility.Collapsed;
        }
        private void User_load_more_Click(object sender, RoutedEventArgs e)
        {
             //GetAttentionUpdatesInfo();
        }

        private void User_ListView_Attention_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(VideoPage), ((GetAttentionUpdate)e.ClickedItem).aid);
        }

        private void User_load_more_Fav_Click(object sender, RoutedEventArgs e)
        {
            
            //GetFavouriteVideoInfo();
        }

        private void User_ListView_FavouriteVideo_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(VideoPage), ((GetFavouriteVideo)e.ClickedItem).aid);
        }

        private void User_ListView_History_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(VideoPage), ((GetHistoryModel)e.ClickedItem).aid);
        }

        private void User_load_more_history_Click(object sender, RoutedEventArgs e)
        {
            pageNum_His++;
            //GetHistoryInfo();
        }

        private void Sp_load_more_Click(object sender, RoutedEventArgs e)
        {
            //GetFavSpInfo();
        }

        private void Sp_ListView_Attention_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(BanSeasonPage), ((FavSpModel)e.ClickedItem).spid);
            
        }
        bool More = true;
        private async void sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (sv.VerticalOffset == sv.ScrollableHeight)
            {
                if (More)
                {
                    More = false;
                    await GetAttentionUpdatesInfo();
                    More = true;
                }
            }
        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

            if ((sender as ScrollViewer).VerticalOffset == (sender as ScrollViewer).ScrollableHeight)
            {
                if (More)
                {
                    More = false;
                    await GetFavSpInfo();
                    More = true;
                }
            }
        }

        private async void sv_Ho_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if ((sender as ScrollViewer).VerticalOffset == (sender as ScrollViewer).ScrollableHeight)
            {
                if (More)
                {
                    More = false;
                    await GetHistoryInfo();
                    More = true;
                }
            }
        }

        private async void pr_RefreshInvoked(DependencyObject sender, object args)
        {
               pageNum = 1;
        //private int pageNum_Fav = 1;
        pageNum_His = 1;
        User_ListView_Attention.Items.Clear();
            //User_ListView_FavouriteVideo.Items.Clear();
            User_ListView_History.Items.Clear();
            User_Attention_Pivot.Visibility = Visibility.Collapsed;
            User_Loading.Visibility = Visibility.Visible;
            await GetAttentionUpdatesInfo();
            //await GetFavouriteVideoInfo();
            await GetHistoryInfo();
            await GetFavSpInfo();
            User_Attention_Pivot.Visibility = Visibility.Visible;
            User_Loading.Visibility = Visibility.Collapsed;
        }
    }


    
}

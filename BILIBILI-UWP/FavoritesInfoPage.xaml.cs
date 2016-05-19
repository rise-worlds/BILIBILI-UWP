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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace BILIBILI_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class FavoritesInfoPage : Page
    {
        public FavoritesInfoPage()
        {
            this.InitializeComponent();
        }
        private GetLoginInfoClass getLogin;
        private int pageNum = 1;
        private int MaxPage = 0;
        private string fid = "";
        protected override  void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                fid = (string)e.Parameter;
                GetFavouriteBoxVideo();
            }
            else
            {
                this.Frame.GoBack();
            }
        }
        private async void GetFavouriteBoxVideo()
        {
            getLogin = new GetLoginInfoClass();
            List<GetFavouriteBoxsVideoModel> lsModel = await getLogin.GetFavouriteBoxVideo(fid, pageNum);
            if (lsModel != null)
            {
                foreach (GetFavouriteBoxsVideoModel item in lsModel)
                {
                    MaxPage = item.pages;
                    User_ListView_FavouriteVideo.Items.Add(item);
                }
                //为下一页做准备
                pageNum++;
                if (pageNum > MaxPage)
                {
                    User_load_more.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                MessageDialog md = new MessageDialog("信息获取失败！");
                await md.ShowAsync();
                User_load_more.Visibility = Visibility.Collapsed;
            }
        }

        private void User_load_more_Click(object sender, RoutedEventArgs e)
        {
               GetFavouriteBoxVideo();
        }

        private void User_ListView_FavouriteVideo_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(VideoPage), ((GetFavouriteBoxsVideoModel)e.ClickedItem).aid);
        }
    }
}

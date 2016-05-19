using BILIBILI_UWP.PartPage;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace BILIBILI_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PartitionPage : Page
    {
        public PartitionPage()
        {
            this.InitializeComponent();
        }

        //0番剧，1动画，2娱乐，3音乐，4舞蹈，5科技，6游戏，7鬼畜，8电视剧，9电影，10时尚
        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            switch (int.Parse(((StackPanel)e.ClickedItem).Tag.ToString()))
            {
                case 0:
                    this.Frame.Navigate(typeof(FJPage));
                    break;
                case 1:
                    this.Frame.Navigate(typeof(DHPage));
                    break;
                case 2:
                    this.Frame.Navigate(typeof(YLPage));
                    break;
                case 3:
                    this.Frame.Navigate(typeof(YYPage));
                    break;
                case 4:
                    this.Frame.Navigate(typeof(WDPage));
                    break;
                case 5:
                    this.Frame.Navigate(typeof(KJPage));
                    break;
                case 6:
                    this.Frame.Navigate(typeof(YXPage));
                    break;
                case 7:
                    this.Frame.Navigate(typeof(GCPage));
                    break;
                case 8:
                    this.Frame.Navigate(typeof(DSJPage));
                    break;
                case 9:
                    this.Frame.Navigate(typeof(DYPage));
                    break;
                case 10:
                    this.Frame.Navigate(typeof(SSPage));
                    break;
                default:
                    break;
            }

        }
    }
}

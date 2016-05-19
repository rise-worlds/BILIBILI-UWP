using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
    public sealed partial class WebViewPage : Page
    {
        public WebViewPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            webview_WebView.Navigate(new Uri((string)e.Parameter));
        }

        private void webview_btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            webview_WebView.Refresh();
        }

        private void webview_btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void webview_WebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            //text .Text= args.Uri.AbsoluteUri;
            webview_progressBar.Visibility = Visibility.Visible;
        }

        private void webview_WebView_FrameDOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            webview_progressBar.Visibility = Visibility.Collapsed;
        }

        private void webview_WebView_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            webview_progressBar.Visibility = Visibility.Collapsed;
        }


        private  void webview_WebView_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            //
            //我的正则真的真的真的不会啊- -
            if (Regex.IsMatch(args.Uri.AbsoluteUri , "/video/av(.*)?[/|+](.*)?"))
            {
                args.Handled = true;
                string a = Regex.Match(args.Uri.AbsoluteUri , "/video/av(.*)?[/|+](.*)?").Groups[1].Value;
                this.Frame.Navigate(typeof(VideoPage), a);
            }
            else
            {
                if (Regex.IsMatch(args.Uri.AbsoluteUri + "+", "/video/av(.*)[/|+]"))
                {
                    args.Handled = true;
                    string a = Regex.Match(args.Uri.AbsoluteUri + "+", "/video/av(.*)[/|+]").Groups[1].Value;
                    this.Frame.Navigate(typeof(VideoPage), a);
                }
                else
                {
                    args.Handled = true;
                    text.Text = "已禁止跳转：" + args.Uri.AbsoluteUri;
                }
            }
            
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.System;
using Windows.System.Display;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace BILIBILI_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public delegate void ReSh();
        public event ReSh heh;
        
        public MainPage()
        {
            this.InitializeComponent();
            GetSetting();
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
            GetUpdate();
            
            //GetAnn();
        }
        ApplicationDataContainer container = ApplicationData.Current.LocalSettings;
        public void GetSetting()
        {
            if (container.Values["Theme"] != null)
            {
                if (container.Values["Theme"].ToString() == "日间模式")
                {
                    ChangeLight();
                }
                else
                {
                    ChangeDrak();
                }
            }
            else
            {
                ChangeLight();
                container.Values["Theme"] = "日间模式";
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            GetLoginInfoClass getLogin = new GetLoginInfoClass();
            if (!getLogin.IsLogin())
            {
                user_Info.DataContext = new GetLoginInfoModel()
                {
                    name = "请登录",
                    face = "http://static.hdslb.com/images/member/noface.gif"
                };
            }
            else
            {
                GetLoginInfoModel model = await getLogin.GetUserInfo();
                if (model != null)
                {
                    user_Info.DataContext = model;
                }
            }
        }

        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (mainFrame.CanGoBack)
            {
                e.Handled = true;
                mainFrame.GoBack();
                //Application.Current.Exit();
            }
        }
        private void top_btn_OpenMenu_Click(object sender, RoutedEventArgs e)
        {
            if (content_SplitView.IsPaneOpen)
            {
                content_SplitView.IsPaneOpen = false;
            }
            else
            {
                content_SplitView.IsPaneOpen = true;
            }
        }

        private void top_btn_find_Click(object sender, RoutedEventArgs e)
        {
            if (top_txt_find.Visibility == Visibility.Collapsed)
            {
                top_txt_find.Visibility = Visibility.Visible;
                top_txt_find.Focus(FocusState.Programmatic);
                top_btn_find.Visibility = Visibility.Collapsed;
            }
        }

        private async void Menu_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (content_SplitView.DisplayMode == SplitViewDisplayMode.Overlay || content_SplitView.DisplayMode == SplitViewDisplayMode.CompactOverlay)
            {
                content_SplitView.IsPaneOpen = false;
            }
            if (Menu_List.SelectedIndex != -1)
            {
                if (Menu_List.SelectedIndex == 0)
                {
                    this.mainFrame.Navigate(typeof(HomePage));
                }
                if (Menu_List.SelectedIndex == 1)
                {
                    this.mainFrame.Navigate(typeof(PartitionPage));
                }
                if (Menu_List.SelectedIndex == 3)
                {
                    mainFrame.Navigate(typeof(RankPage));
                }
                if (Menu_List.SelectedIndex == 2)
                {
                    mainFrame.Navigate(typeof(BangumiPage));
                }
                if (Menu_List.SelectedIndex == 4)
                {
                    mainFrame.Navigate(typeof(AttentionUpdatePage), 0);//带参数，0表示动态
                }

                if (Menu_List.SelectedIndex == 5)
                {
                    MessageDialog md = new MessageDialog("努力开发中...", "(´・ω・`) Wait...");
                    await md.ShowAsync();
                    //mainFrame.Navigate(typeof(DownloadPage));
                    //mainFrame.Navigate(typeof(AttentionUpdatePage),2);//带参数，3表示历史
                }
                if (Menu_List.SelectedIndex == 6)
                {
                    mainFrame.Navigate(typeof(DownloadPage));
                    //mainFrame.Navigate(typeof(AttentionUpdatePage),1);//带参数，1表示收藏
                }
                if (Menu_List.SelectedIndex == 7)
                {
                    //mainFrame.Navigate(typeof(DownloadPage));
                }
            }


        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Menu_List.SelectedIndex = 0;

            downFrame.Navigate(typeof(DownloadPage));
            //mainFrame.Navigate(typeof(HomePage));
        }

        private  void mainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            EdgeUIThemeTransition edge = new EdgeUIThemeTransition();

            if (e.NavigationMode == NavigationMode.New)
            {
                edge.Edge = EdgeTransitionLocation.Bottom;
                TransitionCollection tc = new TransitionCollection();
                tc.Add(edge);
                mainFrame.ContentTransitions = tc;
            }
            else
            {
                edge.Edge = EdgeTransitionLocation.Left;
                TransitionCollection tc = new TransitionCollection();
                tc.Add(edge);
                mainFrame.ContentTransitions = tc;
            }
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
    (mainFrame).BackStack.Any()
    ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            top_btn_Resh.Visibility = Visibility.Collapsed;
            if (mainFrame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }

            if (((Page)e.Content).Tag != null)
            {
                top_txt_Header.Text = ((Page)e.Content).Tag.ToString();
                if (((Page)e.Content).Tag.ToString() == "视频播放")
                {
                    DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;
                    //content_SplitView.DisplayMode = SplitViewDisplayMode.Overlay;
                    content_SplitView.OpenPaneLength = 0;
                    top_Grid.Visibility = Visibility.Collapsed;
                }
                else
                {
                    DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
                    content_SplitView.Pane.Visibility = Visibility.Visible;
                    content_SplitView.OpenPaneLength = 200;
                    top_Grid.Visibility = Visibility.Visible;
                }
                if (((Page)e.Content).Tag.ToString() == "下载管理")
                {
                    downFrame.Content = null;
                }
                else
                {
                    downFrame.Navigate(typeof(DownloadPage));
                }
                string selectListItem = ((Page)e.Content).Tag.ToString();
                switch (selectListItem)
                {
                    case "首页":
                        Menu_List.SelectedIndex = 0;
                        break;
                    case "分区":
                        Menu_List.SelectedIndex = 1;
                        break;
                    case "番剧":
                        Menu_List.SelectedIndex = 2;
                        break;
                    case "排行榜":
                        Menu_List.SelectedIndex = 3;
                        break;
                    case "动态、收藏及历史":
                        Menu_List.SelectedIndex = 4;
                        break;
                    case "下载管理":
                        Menu_List.SelectedIndex = 6;
                        break;
                    case "设置":
                        (mainFrame.Content as SettingPage).ChangeDrak += MainPage_ChangeLight;
                        Menu_List.SelectedIndex = -1;
                        break;
                    case "登录":
                        (mainFrame.Content as LoginPage).UpdateUserInfo += MainPage_UpdateUserInfo;
                        Menu_List.SelectedIndex = -1;
                        break;
                    case "用户中心":
                        (mainFrame.Content as UserPage).UpdateUserInfo += MainPage_UpdateUserInfo;
                        Menu_List.SelectedIndex = -1;
                        break;
                    default:
                        Menu_List.SelectedIndex = -1;
                        break;
                }
            }
        }

        private void MainPage_ChangeLight()
        {
            GetSetting();
        }

        private async void MainPage_UpdateUserInfo()
        {
            GetLoginInfoClass getLogin = new GetLoginInfoClass();
            if (!getLogin.IsLogin())
            {
                user_Info.DataContext = new GetLoginInfoModel()
                {
                    name = "请登录",
                    face = "http://static.hdslb.com/images/member/noface.gif"
                };
            }
            else
            {
                GetLoginInfoModel model = await getLogin.GetUserInfo();
                if (model != null)
                {
                    user_Info.DataContext = model;
                }
            }
        }

        private void top_btn_Back_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.GoBack();
        }

        private void Menu_List_Buttom_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (content_SplitView.DisplayMode == SplitViewDisplayMode.Overlay || content_SplitView.DisplayMode == SplitViewDisplayMode.CompactOverlay)
            {
                content_SplitView.IsPaneOpen = false;
            }
            if (((StackPanel)e.ClickedItem).Tag.ToString() == "User")
            {
                mainFrame.Navigate(typeof(UserPage));
            }
            else
            {
                mainFrame.Navigate(typeof(SettingPage));
            }
        }
        //版本号，重要
        public int VerNum = 1190;

        public async void GetUpdate()
        {
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://7xpria.dl1.z0.glb.clouddn.com/BiVer.json"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    UpdateModel model = JsonConvert.DeserializeObject<UpdateModel>(results);
                    if (model.VersionNum > VerNum)
                    {
                        var dialog = new MessageDialog(model.UpdateText, "发现新版本");
                        dialog.Commands.Add(new UICommand("确定", async cmd =>
                        {
                            await Launcher.LaunchUriAsync(new Uri(model.Uri));
                        }, commandId: 0));
                        dialog.Commands.Add(new UICommand("取消", cmd => { }, commandId: 1));

                        //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
                        dialog.DefaultCommandIndex = 0;
                        dialog.CancelCommandIndex = 1;
                        //获取返回值
                        var result = await dialog.ShowAsync();
                    }
                }
            }
            catch
            {
            }
        }
        public async void GetAnn()
        {
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://7xpria.dl1.z0.glb.clouddn.com/BiAnn.json"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    UpdateModel model = JsonConvert.DeserializeObject<UpdateModel>(results);
                    var dialog = new MessageDialog(model.UpdateText, "公告");
                    dialog.Commands.Add(new UICommand("不再提醒", cmd =>
                    {

                    }, commandId: 0));
                    dialog.Commands.Add(new UICommand("知道了", cmd => { }, commandId: 1));

                    //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
                    dialog.DefaultCommandIndex = 0;
                    dialog.CancelCommandIndex = 1;
                    //获取返回值
                    var result = await dialog.ShowAsync();
                }
            }
            catch
            {
                throw;
            }
        }

        private void top_txt_find_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (sender.Text.Length == 0)
            {
                top_txt_find.Visibility = Visibility.Collapsed;
                top_btn_find.Visibility = Visibility.Visible;
            }
            else
            {
                this.mainFrame.Navigate(typeof(SearchPage), top_txt_find.Text);
            }
        }


        //侧滑来源http://www.cnblogs.com/hebeiDGL/p/4775377.html
        #region  从屏幕左侧边缘滑动屏幕时，打开 SplitView 菜单

        // SplitView 控件模板中，Pane部分的 Grid
        Grid PaneRoot;

        //  引用 SplitView 控件中， 保存从 Pane “关闭” 到“打开”的 VisualTransition
        //  也就是 <VisualTransition From="Closed" To="OpenOverlayLeft"> 这个 
        VisualTransition from_ClosedToOpenOverlayLeft_Transition;

        private void Border_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            e.Handled = true;

            // 仅当 SplitView 处于 Overlay 模式时（窗口宽度最小时）
            if (content_SplitView.DisplayMode == SplitViewDisplayMode.Overlay)
            {
                if (PaneRoot == null)
                {
                    // 找到 SplitView 控件中，模板的父容器
                    Grid grid = FindVisualChild<Grid>(content_SplitView);

                    PaneRoot = grid.FindName("PaneRoot") as Grid;

                    if (from_ClosedToOpenOverlayLeft_Transition == null)
                    {
                        // 获取 SplitView 模板中“视觉状态集合”
                        IList<VisualStateGroup> stateGroup = VisualStateManager.GetVisualStateGroups(grid);

                        //  获取 VisualTransition 对象的集合。
                        IList<VisualTransition> transitions = stateGroup[0].Transitions;

                        // 找到 SplitView.IsPaneOpen 设置为 true 时，播放的 transition
                        from_ClosedToOpenOverlayLeft_Transition = transitions?.Where(train => train.From == "Closed" && train.To == "OpenOverlayLeft").First();

                        //// 遍历所有 transitions，打印到输出窗口
                        //foreach (var tran in transitions)
                        //{
                        //    Debug.WriteLine("From : " + tran.From + "   To : " + tran.To);
                        //}
                    }
                }


                // 默认为 Collapsed，所以先显示它
                PaneRoot.Visibility = Visibility.Visible;

                // 当在 Border 上向右滑动，并且滑动的总距离需要小于 Panel 的默认宽度。否则会脱离左侧窗口，继续向右拖动
                if (e.Cumulative.Translation.X >= 0 && e.Cumulative.Translation.X < content_SplitView.OpenPaneLength)
                {
                    CompositeTransform ct = PaneRoot.RenderTransform as CompositeTransform;
                    ct.TranslateX = (e.Cumulative.Translation.X - content_SplitView.OpenPaneLength);
                }
            }
        }

        private void Border_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            e.Handled = true;

            // 仅当 SplitView 处于 Overlay 模式时（窗口宽度最小时）
            if (content_SplitView.DisplayMode == SplitViewDisplayMode.Overlay && PaneRoot != null)
            {
                // 因为当 IsPaneOpen 为 true 时，会通过 VisualStateManager 把 PaneRoot.Visibility  设置为
                // Visibility.Visible，所以这里把它改为 Visibility.Collapsed，以回到初始状态
                PaneRoot.Visibility = Visibility.Collapsed;

                // 恢复初始状态 
                CompositeTransform ct = PaneRoot.RenderTransform as CompositeTransform;


                // 如果大于 MySplitView.OpenPaneLength 宽度的 1/2 ，则显示，否则隐藏
                if ((content_SplitView.OpenPaneLength + ct.TranslateX) > content_SplitView.OpenPaneLength / 2)
                {
                    content_SplitView.IsPaneOpen = true;

                    // 因为上面设置 IsPaneOpen = true 会再次播放向右滑动的动画，所以这里使用 SkipToFill()
                    // 方法，直接跳到动画结束状态
                    from_ClosedToOpenOverlayLeft_Transition?.Storyboard?.SkipToFill();

                }

                ct.TranslateX = 0;
            }
        }
        #endregion

        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            int count = Windows.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = Windows.UI.Xaml.Media.VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }

            return null;
        }

        private void top_btn_Resh_Click(object sender, RoutedEventArgs e)
        {
            heh();
            //(mainFrame.Content as HomePage)
            // mainFrame.Navigate(typeof(HomePage));
        }

        private void top_btn_Click(object sender, RoutedEventArgs e)
        {
            if (RequestedTheme == ElementTheme.Light|| RequestedTheme == ElementTheme.Default)
            {
                ChangeDrak();
            }
            else
            {
                ChangeLight();
            }
        }
        public void ChangeDrak()
        {
            RequestedTheme = ElementTheme.Dark;
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                // StatusBar.GetForCurrentView().HideAsync();
                StatusBar statusBar = StatusBar.GetForCurrentView();
                statusBar.ForegroundColor = Colors.White;
                statusBar.BackgroundColor = Color.FromArgb(255, 50, 50, 50);
                statusBar.BackgroundOpacity = 100;
            }
        }
        public void ChangeLight()
        {
            RequestedTheme = ElementTheme.Light;
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                // StatusBar.GetForCurrentView().HideAsync();
                StatusBar statusBar = StatusBar.GetForCurrentView();
                statusBar.ForegroundColor = Colors.White;
                statusBar.BackgroundColor = Color.FromArgb(255, 223, 133, 160);
                statusBar.BackgroundOpacity = 100;
            }
        }
    }
    public class UpdateModel
    {
        public string Version { get; set; }
        public bool Beta { get; set; }
        public bool OnStore { get; set; }
        public string UpdateTime { get; set; }
        public string UpdateText { get; set; }
        public string Uri { get; set; }
        public int AnnID { get; set; }
        public int VersionNum { get; set; }
    }
}

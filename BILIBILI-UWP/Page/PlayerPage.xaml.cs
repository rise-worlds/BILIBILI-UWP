using BILIBILI_UWP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System.Display;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Text;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace BILIBILI_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PlayerPage : Page,IDisposable
    {
        public PlayerPage()
        {
            this.InitializeComponent();

           NavigationCacheMode = NavigationCacheMode.Required;
           
    }
        string Cid = "";
        string Aid = "";
        int Quality = 2;
        int fontSize = 5;
        int Speed = 7;
      
        ApplicationDataContainer container = ApplicationData.Current.LocalSettings;
        private DisplayRequest dispRequest = null;
        List<DanMuModel> DanMuPool=null;
        List<string> DanMuYb=new List<string>();
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Cid = "";
            Aid = "";
            Quality = 2;
            fontSize = 3;
            DanMuPool = null;
            dispRequest = null;
            grid_error.Visibility = Visibility.Collapsed;
            grid_end.Visibility = Visibility.Collapsed;
            //mediaElement.GetFocus(FocusState.Keyboard);
            
            slider_V.Value = 1;
            mediaElement.Source = null;
            if (container.Values["AutoFull"] != null)
            {
                if (container.Values["AutoFull"].ToString() == "0")
                {
                    ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
                    btn_Full.Visibility = Visibility.Collapsed;
                    btn_ExitFull.Visibility = Visibility.Visible;
                    //menu_Full.Text = "退出全屏";
                }
                else
                {
                    ApplicationView.GetForCurrentView().ExitFullScreenMode();
                    //menu_Full.Text = "全屏播放";
                    btn_Full.Visibility = Visibility.Visible;
                    btn_ExitFull.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
                //menu_Full.Text = "退出全屏";
                btn_Full.Visibility = Visibility.Collapsed;
                btn_ExitFull.Visibility = Visibility.Visible;
            }
           
            if (dispRequest == null)
            {
                // 用户观看视频，需要保持屏幕的点亮状态
                dispRequest = new DisplayRequest();
                dispRequest.RequestActive(); // 激活显示请求
            }
            VideoInfoModel model = e.Parameter as VideoInfoModel;
            GetSetting();

            //grid_DanMu.Visibility = Visibility.Collapsed;
            loading.Visibility = Visibility.Visible;
            
            //txt_ID.Text = model.cid;
            if (model.path == null)
            {
                txt_title.Text = model.title;
                Cid = model.cid;
                Aid = model.aid;
                txt_Buff.Text = "弹幕填充中...";
                DanMuPool = await GetDM(model.cid,false);
                txt_Buff.Text = "读取视频中...";
                GetPlayInfo(model.cid, Quality);   
            }
            else
            {
                txt_title.Text = model.title;
                StorageFile file = await StorageFile.GetFileFromPathAsync(model.path);
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                mediaElement.SetSource(stream, file.ContentType);
                DanMuPool = await GetDM(model.cid, true);
                text_Comment.PlaceholderText = "Sorry，本地视频暂不支持发送弹幕...";
                text_Comment.IsEnabled = false;
                btn_SendComment.IsEnabled = false;
            }
        }

        private void GetSetting()
        {
            GetLoginInfoClass login = new GetLoginInfoClass();
            if (!login.IsLogin())
            {
                text_Comment.PlaceholderText = "登录后才可以发送弹幕！";
                text_Comment.IsEnabled = false;
                btn_SendComment.IsEnabled = false;
            }
            else
            {
                text_Comment.PlaceholderText = "让弹幕飞一会....";
                text_Comment.IsEnabled = true;
                btn_SendComment.IsEnabled = true;
            }
            if (container.Values["Quality"] != null)
            {
                switch (container.Values["Quality"].ToString())
                {
                    case "1":
                        cb_Quality.SelectedIndex = 0;
                        Quality =1;
                        break;
                    case "2":
                        cb_Quality.SelectedIndex = 1;
                        Quality = 2;
                        break;
                    case "3":
                        cb_Quality.SelectedIndex = 2;
                        Quality = 3;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                cb_Quality.SelectedIndex = 1;
                //Quality = 2;
            }

            if (container.Values["DanMuSize"] != null)
            {
                switch (container.Values["DanMuSize"].ToString())
                {
                    case "0":
                        //cb_Quality.SelectedIndex = 0;
                        fontSize = 3;
                        break;
                    case "1":
                        fontSize = 5;
                        break;
                    case "2":
                        fontSize = 8;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                fontSize = 5;
                //Quality = 2;
            }

            if (container.Values["AutoPlay"] != null)
            {
                if (container.Values["AutoPlay"].ToString() == "0")
                {
                    mediaElement.AutoPlay = true;
                }
                else
                {
                    mediaElement.AutoPlay = false;
                }
            }
            else
            {
                mediaElement.AutoPlay = true;
            }

            if (container.Values["PlaySS"] != null)
            {
                if (container.Values["PlaySS"].ToString() == "0")
                {
                    SS_Volume.Visibility = Visibility.Visible;
                    SS_Post.Visibility = Visibility.Visible;
                }
                else
                {
                    SS_Volume.Visibility = Visibility.Collapsed;
                    SS_Post.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                SS_Volume.Visibility = Visibility.Visible;
                SS_Post.Visibility = Visibility.Visible;
            }
            //弹幕速度
            if (container.Values["DanMuSpeed"] != null)
            {
                Speed = int.Parse( container.Values["DanMuSpeed"].ToString());
            }
            else
            {
                //container.Values["AutoPlay"] = "0";
                Speed = 7;
            }
            //弹幕透明度
            if (container.Values["DanMuTran"] != null)
            {

              double d = (double)container.Values["DanMuTran"];
               DanMuModel.Tran = 255 * (d / 100);

            }
            else
            {
                //container.Values["AutoPlay"] = "0";
                DanMuModel.Tran = 255;
            }
            if (container.Values["DanMuYB"] != null)
            {
                txt_DanMuYB.Text = container.Values["DanMuYB"].ToString();
                string[] yb = container.Values["DanMuYB"].ToString().Split(',');
                if (yb.Length != 0 || yb != null)
                {
                    foreach (string item in yb)
                    {
                        DanMuYb.Add(item);
                    }
                } 
            }
            else
            {
                container.Values["DanMuYB"] = "";
            }
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
            //释放
            Dispose();
            // DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
        }

        public async void GetPlayInfo(string mid,int quality)
        {
            //http://interface.bilibili.com/playurl?platform=android&cid=5883400&quality=2&otype=json&appkey=422fd9d7289a1dd9&type=mp4
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://interface.bilibili.com/playurl?platform=android&cid=" + mid + "&quality="+ quality + "&otype=json&appkey=422fd9d7289a1dd9&type=mp4"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    VideoUriModel model = JsonConvert.DeserializeObject<VideoUriModel>(results);
                    List<VideoUriModel> model1 = JsonConvert.DeserializeObject<List<VideoUriModel>>(model.durl.ToString());
                    mediaElement.Source =new Uri(model1[0].url);
                    if (model1[0].url==null)
                    {
                        grid_error.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        grid_error.Visibility = Visibility.Collapsed;
                    }
                    txt.Text = model1[0].url;
                }
               }
                catch (Exception)
                {
                grid_error.Visibility = Visibility.Visible;
                //MessageDialog md = new MessageDialog("视频地址获取失败！", "错误");
                //await md.ShowAsync();
            }
        }
        private void slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            //mediaElement.Position = TimeSpan.FromSeconds(slider.Value);
            
        }
        private void btn_Play_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Play();
            btn_Pause.Visibility = Visibility.Visible;
            btn_Play.Visibility = Visibility.Collapsed;
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement.CanPause)
            {
                mediaElement.Pause();
                btn_Pause.Visibility = Visibility.Collapsed;
                btn_Play.Visibility = Visibility.Visible;
            }
        }


        private void mediaElement_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (mediaElement.CurrentState == MediaElementState.Playing)
            {
                mediaElement.Pause();
            }
            else
            {
                mediaElement.Play();
            }
        }

        private void mediaElement_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (grid_1.Visibility == Visibility.Visible)
            {
                grid_1.Visibility = Visibility.Collapsed;
                grid_2.Visibility = Visibility.Collapsed;
                grid_DanMu.Visibility = Visibility.Collapsed;
            }
            else
            {
                grid_1.Visibility = Visibility.Visible;
                grid_2.Visibility = Visibility.Visible;
                grid_DanMu.Visibility = Visibility.Visible;
            }

        }

        private void mediaElement_BufferingProgressChanged(object sender, RoutedEventArgs e)
        {
            txt_Buff.Text = "正在缓冲 " + mediaElement.BufferingProgress.ToString("P");
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
            //await Windows.UI.ViewManagement.StatusBar.GetForCurrentView().HideAsync();
            //ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            if (slider_V.Visibility == Visibility.Collapsed)
            {
                slider_V.Visibility = Visibility.Visible;
            }
            else
            {
                slider_V.Visibility = Visibility.Collapsed;
            }
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private async void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            string content = "视频宽度：" + mediaElement.NaturalVideoWidth + "\r\n视频高度：" + mediaElement.NaturalVideoHeight + "\r\n视频长度：" + mediaElement.NaturalDuration.TimeSpan.ToString() + "\r\n缓冲进度：" + mediaElement.DownloadProgress.ToString("P")+ "\r\n弹幕池数量：" + DanMuPool.Count??""; 
            MessageDialog md = new MessageDialog(content, "视频信息");
            await md.ShowAsync();
        }

        private void mediaElement_CurrentStateChanged_1(object sender, RoutedEventArgs e)
        {

            if (mediaElement.CurrentState == MediaElementState.Opening||mediaElement.CurrentState == MediaElementState.Buffering)
            {
                loading.Visibility = Visibility.Visible;
                txt_Buff.Text = "正在缓冲 " + mediaElement.BufferingProgress.ToString("P");

            }
            else
            {
                switch (mediaElement.CurrentState)
                {
                    case MediaElementState.Closed:
                        btn_Pause.Visibility = Visibility.Collapsed;
                        btn_Play.Visibility = Visibility.Visible;
                        break;
                    case MediaElementState.Buffering:
                        btn_Pause.Visibility = Visibility.Visible;
                        btn_Play.Visibility = Visibility.Collapsed;
                        break;
                    case MediaElementState.Playing:
                        btn_Pause.Visibility = Visibility.Visible;
                        btn_Play.Visibility = Visibility.Collapsed;
                        break;
                    case MediaElementState.Paused:
                        btn_Pause.Visibility = Visibility.Collapsed;
                        btn_Play.Visibility = Visibility.Visible;
                        break;
                    case MediaElementState.Stopped:
                        btn_Pause.Visibility = Visibility.Visible;
                        btn_Play.Visibility = Visibility.Collapsed;
                        break;
                    default:
                        break;
                }
                loading.Visibility = Visibility.Collapsed;
            }
        }

        private void mediaElement_DownloadProgressChanged(object sender, RoutedEventArgs e)
        {
            slider1.Value = mediaElement.DownloadProgress * 100;
        }

        private void slider2_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            mediaElement.Position = TimeSpan.FromSeconds(slider2.Value);
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (grid_1.Visibility == Visibility.Visible)
            {
                grid_1.Visibility = Visibility.Collapsed;
                grid_2.Visibility = Visibility.Collapsed;
                grid_DanMu.Visibility = Visibility.Collapsed;
            }
            else
            {
                grid_1.Visibility = Visibility.Visible;
                grid_2.Visibility = Visibility.Visible;
                grid_DanMu.Visibility = Visibility.Visible;
            }
        }

        private void Slider_ValueChanged_1(object sender, RangeBaseValueChangedEventArgs e)
        {
            mediaElement.Position = TimeSpan.FromSeconds(((Slider)sender).Value);
        }

        private void cb_Quality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cid!="")
            {
                switch (cb_Quality.SelectedIndex)
                {
                    case 0:
                        GetPlayInfo(Cid, 1);
                        break;
                    case 1:
                        GetPlayInfo(Cid, 2);
                        break;
                    case 2:
                        GetPlayInfo(Cid, 3);
                        break;
                    default:
                        break;
                }
            }
            
        }

        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key== Windows.System.VirtualKey.Space)
            {
                if (mediaElement.CurrentState == MediaElementState.Playing)
                {
                    mediaElement.Pause();
                }
                else
                {
                    mediaElement.Play();
                }
            }
        }


        public async Task<List<DanMuModel>> GetDM(string cid,bool isLocal)
        {
            List<DanMuModel> ls = new List<DanMuModel>();
            try
            {
                string a = "";
                if (!isLocal)
                {
                    HttpClient hc = new HttpClient();
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://comment.bilibili.com/" + cid + ".xml"));
                    hr.EnsureSuccessStatusCode();
                    a = await hr.Content.ReadAsStringAsync();
                }
                else
                {
                    StorageFolder folder = ApplicationData.Current.LocalFolder;
                    StorageFolder DownFolder = await folder.GetFolderAsync("DownLoad");
                    StorageFile file = await DownFolder.GetFileAsync(cid + ".xml");
                    a = await FileIO.ReadTextAsync(file);
                }
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(a);
                XmlElement el = xdoc.DocumentElement;
                XmlNodeList xml = el.ChildNodes;
                
                foreach (XmlNode item in xml)
                {
                    if (item.Attributes["p"] != null)
                    {
                        string heheda = item.Attributes["p"].Value;
                        string[] haha = heheda.Split(',');
                        ls.Add(new DanMuModel
                        {
                            DanTime = decimal.Parse(haha[0]),
                            DanMode = haha[1],
                            DanSize = haha[2],
                            _DanColor = haha[3],
                            DanSendTime = haha[4],
                            DanPool = haha[5],
                            DanID = haha[6],
                            DanRowID = haha[7],
                            DanText = item.InnerText
                        });
                    }
                }
                return ls;
            }
            catch (Exception)
            {
                return ls;
            }
           
        }
        int row = 0;
        //TextBlock tx;
        //TextBlock tx2;//黑色重叠
        //Grid grid;
        public async void heiehi(DanMuModel model)
        {
            try
            {
                bool IsGuanjianzi = false;
                foreach (string guanjianzi in DanMuYb)
                {
                    if (model.DanText.Contains(guanjianzi))
                    {
                        //IsGuanjianzi = model.DanText.Contains(guanjianzi);
                        IsGuanjianzi = true;
                        break;
                    }
                }
                if (IsGuanjianzi)
                {
                    return;
                }
                TextBlock tx = new TextBlock();
                TextBlock tx2 = new TextBlock();
                Grid grid = new Grid();
                grid.Margin = new Thickness(0, 0, 20, 0);
                
                tx2.Text = model.DanText;//"这是第【"+ num + "】条弹幕";
                tx.Text = model.DanText;//"这是第【"+ num + "】条弹幕";

                //tx2.Opacity = Tran;
                //tx.Margin = new Thickness(0, 0, 20, 0);
                //tx.Margin = new Thickness(0,0,20,0);

                tx2.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                tx.Foreground = new SolidColorBrush(model.DanColor);//new SolidColorBrush(co[rd.Next(0, 7)]);
                double size = double.Parse(model.DanSize);
                if (size==25)
                {
                    tx2.FontSize = (size - fontSize) + 0.1;
                    tx.FontSize = size - fontSize;
                   
                }
                else
                {
                    tx2.FontSize = (size - fontSize + 2) + 0.1;
                    tx.FontSize = size - fontSize + 2;
                }
                grid.Children.Add(tx2);
                grid.Children.Add(tx);
                grid.VerticalAlignment = VerticalAlignment.Top;
                //tx.VerticalAlignment = VerticalAlignment.Top;
                grid.HorizontalAlignment = HorizontalAlignment.Left;
                //tx.HorizontalAlignment = HorizontalAlignment.Left;
                // Create the transform
                TranslateTransform moveTransform = new TranslateTransform();
                moveTransform.X = ActualWidth;

                //tx2.RenderTransform = moveTransform;
                grid.RenderTransform = moveTransform;
             
                //tx2.RenderTransform = moveTransform;
                // Add the rectangle to the tree.
                //thisRow = row;
                test_grid.Children.Add(grid);
                //test_grid.Children.Add(tx);
                Grid.SetRow(grid, row);
                //Grid.SetRow(tx, row+1);
                row++;
                if (row==11)
                {
                    row = 0;
                }
                // Create a duration of 2 seconds.
                Duration duration = new Duration(TimeSpan.FromSeconds(Speed));

                // Create two DoubleAnimations and set their properties.
                DoubleAnimation myDoubleAnimationX = new DoubleAnimation();

                myDoubleAnimationX.Duration = duration;
                //myDoubleAnimationY.Duration = duration;

                Storyboard justintimeStoryboard = new Storyboard();
                justintimeStoryboard.Duration = duration;

                justintimeStoryboard.Children.Add(myDoubleAnimationX);

                Storyboard.SetTarget(myDoubleAnimationX, moveTransform);

                Storyboard.SetTargetProperty(myDoubleAnimationX, "X");
                test_grid.Resources.Remove("justintimeStoryboard");
                myDoubleAnimationX.To = -700;
                // Make the Storyboard a resource.
                test_grid.Resources.Add("justintimeStoryboard", justintimeStoryboard);

                // Begin the animation.
                justintimeStoryboard.Begin();
                await Task.Delay(Speed*1000);
                grid.Children.Remove(tx);
                grid.Children.Remove(tx2);
                test_grid.Children.Remove(grid);
               // test_grid.Children.Remove(tx);        
            }
            catch (Exception)
            {
            }
        }


        public async void TestA(DanMuModel model, bool istop)
        {
            bool IsGuanjianzi = false;
            foreach (string guanjianzi in DanMuYb)
            {
                if (model.DanText.Contains(guanjianzi))
                {
                    //IsGuanjianzi = model.DanText.Contains(guanjianzi);
                    IsGuanjianzi = true;
                    break;
                }
            }
            if (IsGuanjianzi)
            {
                return;
            }
            TextBlock tx = new TextBlock();
            tx.Text = model.DanText;//"这是第【"+ num + "】条弹幕";
            tx.Foreground = new SolidColorBrush(model.DanColor);//new SolidColorBrush(co[rd.Next(0, 7)]);
            double size = double.Parse(model.DanSize);
            if (size == 25)
            {
                tx.FontSize = size - fontSize;
            }
            else
            {
                tx.FontSize = size - fontSize + 2;
            }
           
            // tx.FontSize = Double.Parse(model.DanSize) - fontSize;
            tx.HorizontalAlignment = HorizontalAlignment.Center;
            tx.VerticalAlignment = VerticalAlignment.Top;
            if (istop)
            {
                D_Top.Children.Add(tx);
                await Task.Delay(5000);
                D_Top.Children.Remove(tx);
            }
            else
            {
                D_Bottom.Children.Add(tx);
                await Task.Delay(5000);
                D_Bottom.Children.Remove(tx);
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            #region
            if (DanMuPool != null)
            {
                foreach (var item in DanMuPool)
                {
                    if (Convert.ToInt32(item.DanTime) == Convert.ToInt32(mediaElement.Position.TotalSeconds))
                    {
                        if (item.DanMode == "5")
                        {
                            TestA(item, true);
                        }
                        else
                        {
                            //heiehi(item);
                            if (item.DanMode == "4")
                            {
                                TestA(item, false);
                            }
                            else
                            {
                                heiehi(item);
                               
                            }
                        }
                      
                    }
                    
                }
            }
            #endregion
        }

        private void btn_DM_Click(object sender, RoutedEventArgs e)
        {
            if (test_grid.Visibility == Visibility.Collapsed)
            {
                test_grid.Visibility = Visibility.Visible;
                test_grid2.Visibility = Visibility.Visible;
            }
            else
            {
                test_grid.Visibility =Visibility.Collapsed;
                test_grid2.Visibility = Visibility.Collapsed;
            }
        }

        private void btn_SendDanMu_Click(object sender, RoutedEventArgs e)
        {
            if (grid_DanMu.Visibility == Visibility.Collapsed)
            {
                grid_DanMu.Visibility = Visibility.Visible;
            }
            else
            {
                grid_DanMu.Visibility = Visibility.Collapsed;
            }
        }

        private async void btn_SendComment_Click(object sender, RoutedEventArgs e)
        {
            if (text_Comment.Text.Length==0)
            {
                txt_Status.Text = "弹幕内容不能为空！";
                await Task.Delay(3000);
                txt_Status.Text = "";
                return;
            }
            try
            {
                Uri ReUri = new Uri("http://interface.bilibili.com/dmpost?cid=" + Cid + "&aid=" + Aid + "&pid=1");
                int modeInt = 1;
                if (cb_Mode.SelectedIndex == 2)
                {
                    modeInt = 4;
                }
                if (cb_Mode.SelectedIndex == 1)
                {
                    modeInt = 5;
                }
                string Canshu = "message=" + text_Comment.Text + "&pool=0&playTime=" + mediaElement.Position.TotalSeconds.ToString() + "&cid=" + Cid + "&date=" + DateTime.Now.ToString() + "&fontsize=25&mode=" + modeInt + "&rnd=933253860&color=" + ((ComboBoxItem)cb_Color.SelectedItem).Tag;
                HttpClient hc = new HttpClient();
                hc.DefaultRequestHeaders.Referer = new Uri("http://space.bilibili.com/");
                var response = await hc.PostAsync(ReUri, new HttpStringContent(Canshu, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/x-www-form-urlencoded"));
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                long code = long.Parse(result);

                if (modeInt==1)
                {
                    heiehi(new DanMuModel { DanText = text_Comment.Text, _DanColor = ((ComboBoxItem)cb_Color.SelectedItem).Tag.ToString(), DanSize = "25" });
                }
                if (modeInt == 4)
                {
                    TestA(new DanMuModel { DanText = text_Comment.Text, _DanColor = ((ComboBoxItem)cb_Color.SelectedItem).Tag.ToString(), DanSize = "25" }, false);
                }
                if (modeInt == 5)
                {
                    TestA(new DanMuModel {DanText= text_Comment.Text,_DanColor = ((ComboBoxItem)cb_Color.SelectedItem).Tag.ToString(),DanSize="25"}, true);
                }
                if (code<0)
                {
                    txt_Status.Text = "发送弹幕失败！";
                    await Task.Delay(3000);
                    txt_Status.Text = "";
                }
                else
                {
                    txt_Status.Text = "已发送弹幕";
                    text_Comment.Text = "";
                    await Task.Delay(3000);
                    txt_Status.Text = "";
                }

            }
            catch (Exception ex)
            {
                txt_Status.Text = "发送弹幕发生错误！"+ex.Message;
                await Task.Delay(3000);
                txt_Status.Text = "";
            }
            

        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (chb_Gun.IsChecked==true)
            {
                test_grid.Visibility = Visibility.Collapsed;
            }
            else
            {
                test_grid.Visibility = Visibility.Visible;
            }

            if (chb_Botton.IsChecked == true)
            {
                D_Bottom.Visibility = Visibility.Collapsed;
            }
            else
            {
                D_Bottom.Visibility = Visibility.Visible;
            }

            if (chb_Top.IsChecked == true)
            {
                D_Top.Visibility = Visibility.Collapsed;
            }
            else
            {
                D_Top.Visibility = Visibility.Visible;
            }
            //container.Values["DanMuYB"] = txt_DanMuYB.Text;
            //string[] yb = container.Values["DanMuYB"].ToString().Split(',');
            //if (yb.Length != 0 || yb != null)
            //{
            //    foreach (string item in yb)
            //    {
            //        DanMuYb.Add(item);
            //    }
            //}
            btn_Setting.Flyout.Hide();
        }

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            grid_1.Visibility = Visibility.Visible;
            grid_2.Visibility = Visibility.Visible;
            grid_end.Visibility = Visibility.Visible;
        }

        private void btn_RePlay_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Play();
            grid_end.Visibility = Visibility.Collapsed;
        }

        private void btn_ExitPlay_Click(object sender, RoutedEventArgs e)
        {
            grid_error.Visibility = Visibility.Collapsed;
            grid_end.Visibility = Visibility.Collapsed;
            this.Frame.GoBack();
        }

        private void btn_re_Click(object sender, RoutedEventArgs e)
        {
            grid_error.Visibility = Visibility.Collapsed;
            GetPlayInfo(Cid, Quality);
        }

        private void btn_Full_Click(object sender, RoutedEventArgs e)
        {
                ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            btn_Full.Visibility = Visibility.Collapsed;
                btn_ExitFull.Visibility = Visibility.Visible;
        }

        private void btn_ExitFull_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView.GetForCurrentView(). ExitFullScreenMode();
            btn_Full.Visibility = Visibility.Visible;
            btn_ExitFull.Visibility = Visibility.Collapsed;
        }

        private void SS_Volume_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
           // mediaElement.Volume = (double)SS_Volume.Value;
        }

        private void mediaElement_MarkerReached(object sender, TimelineMarkerRoutedEventArgs e)
        {

        }

        public void Dispose()
        {
            //if (tx!=null)
            //{
            //    tx = null;
            //}
            //if (tx2!=null)
            //{
            //    tx = null;
            //}
            //if (grid!=null)
            //{
            //    grid = null;
            //}
            test_grid.Children.Clear();
          
            //throw new NotImplementedException();
        }

        private void test_grid_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
        public bool Truning = false;
        private void btn_Turn_Click(object sender, RoutedEventArgs e)
        {
            if (!Truning)
            {
                RotateTransform rotateTransform = new RotateTransform();
                rotateTransform.Angle = 180;
                grid_test.RenderTransform = rotateTransform;
                Truning = true;
            }
            else
            {
                RotateTransform rotateTransform = new RotateTransform();
                rotateTransform.Angle = 0;
                grid_test.RenderTransform = rotateTransform;
                Truning = false;
            }
           //  < Grid.RenderTransform >
           //  < CompositeTransform Rotation = "180" />
           // </ Grid.RenderTransform >
        }

        private void mediaElement_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key== Windows.System.VirtualKey.Left)
            {
                slider2.Value -= 5;
            }
            if(e.Key == Windows.System.VirtualKey.Right)
            {
                slider2.Value += 5;
            }
            if (e.Key==Windows.System.VirtualKey.Space)
            {
                if (mediaElement.CurrentState == MediaElementState.Playing)
                {
                    mediaElement.Pause();
                }
                else
                {
                    mediaElement.Play();
                }
            }
        }
    }
    public class VeidoUriModel
    {
        public string accept { get; set; }
        public object backup { get; set; }
        public string size { get; set; }
        public string url { get; set; }
    }

    public class DanMuModel
    {
       public static double Tran = 255;
        //头声明
        public string chatserver { get; set; }//弹幕服务器
        public string chatid { get; set; }//弹幕ID
        public string mission { get; set; }//任务？
        public string maxlimit { get; set; }//弹幕池上限
        public string source { get; set; }//来源？

        //弹幕信息
        //<d p = "1355.2700195312,5,25,16776960,1447587837,0,222d0737,1347973259" > やがて巡り巡る季節に僕らは息をする</d>
        private decimal _DanTime;//弹幕出现时间
        public decimal DanTime
        {
            get { return _DanTime; }
            set { _DanTime = value; }
        }
        public string DanMode { get; set; }//弹幕模式 1..3 滚动弹幕 4底端弹幕 5顶端弹幕 6.逆向弹幕 7精准定位 8高级弹幕
        public string DanSize { get; set; }//弹幕大小 12非常小,16特小,18小,25中,36大,45很大,64特别大
        public string _DanColor { get; set; }//弹幕颜色，十进制
        public Color DanColor
        {
            get
            {
                try
                {
                    _DanColor = Convert.ToInt32(_DanColor).ToString("X2");
                    if (_DanColor.StartsWith("#"))
                        _DanColor = _DanColor.Replace("#", string.Empty);
                    int v = int.Parse(_DanColor, System.Globalization.NumberStyles.HexNumber);
                    return new Color()
                    {
                        A = Convert.ToByte(Tran),
                        R = Convert.ToByte((v >> 16) & 255),
                        G = Convert.ToByte((v >> 8) & 255),
                        B = Convert.ToByte((v >> 0) & 255)
                    };
                }
                catch (Exception)
                {
                    return new Color()
                    {
                        A = 255,
                        R =255,
                        G = 255,
                        B = 255
                    };
                }
               
            }
        }
        public string DanSendTime { get; set; }//弹幕发送时间
        public string DanPool { get; set; }//弹幕池，0普通池 1字幕池 2特殊池 【目前特殊池为高级弹幕专用】
        public string DanID { get; set; }//弹幕发送人ID
        public string DanRowID { get; set; }
        public string DanText { get; set; }//信息
    }
}

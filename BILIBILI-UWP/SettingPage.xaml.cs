using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class SettingPage : Page
    {
        public delegate void DeChangeTheme();
        public event DeChangeTheme ChangeLight;
        public event DeChangeTheme ChangeDrak;
        public SettingPage()
        {
            this.InitializeComponent();
        }
        ApplicationDataContainer container = ApplicationData.Current.LocalSettings;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                Setting_pivot.SelectedIndex = (int)e.Parameter;
            }
            GetSetting();
        }
        public void GetSetting()
        {
            //清晰度
            if (container.Values["Quality"] != null)
            {
                switch (container.Values["Quality"].ToString())
                {
                    case "1":
                        cb_Quality.SelectedIndex = 0;
                        break;
                    case "2":
                        cb_Quality.SelectedIndex = 1;
                        break;
                    case "3":
                        cb_Quality.SelectedIndex = 2;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                container.Values["Quality"] = "2";
                cb_Quality.SelectedIndex = 1;
            }
            //首页展示数
            if (container.Values["SSZS"] != null)
            {
                switch (container.Values["SSZS"].ToString())
                {
                    case "4":
                        cb_SYZS.SelectedIndex = 0;
                        break;
                    case "6":
                        cb_SYZS.SelectedIndex = 1;
                        break;
                    case "12":
                        cb_SYZS.SelectedIndex = 2;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                container.Values["SSZS"] = "6";
                cb_SYZS.SelectedIndex = 1;
            }
            //弹幕大小
            if (container.Values["DanMuSize"] != null)
            {
                switch (container.Values["DanMuSize"].ToString())
                {
                    case "0":
                        cb_DanMuSize.SelectedIndex = 0;
                        break;
                    case "1":
                        cb_DanMuSize.SelectedIndex = 1;
                        break;
                    case "2":
                        cb_DanMuSize.SelectedIndex = 2;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                container.Values["DanMuSize"] = "1";
                cb_DanMuSize.SelectedIndex = 1;
            }
            //自动播放
            if (container.Values["AutoPlay"] != null)
            {
                if (container.Values["AutoPlay"].ToString() == "0")
                {
                    sw_AutoPlay.IsOn = true;
                }
                else
                {
                    sw_AutoPlay.IsOn = false;
                }
            }
            else
            {
                //container.Values["AutoPlay"] = "0";
                sw_AutoPlay.IsOn = true;
            }
            //手势
            if (container.Values["PlaySS"] != null)
            {
                if (container.Values["PlaySS"].ToString() == "0")
                {
                    sw_SS.IsOn = true;
                }
                else
                {
                    sw_SS.IsOn = false;
                }
            }
            else
            {
                //container.Values["AutoPlay"] = "0";
                sw_SS.IsOn = true;
            }
            //全屏
            if (container.Values["AutoFull"] != null)
            {
                if (container.Values["AutoFull"].ToString() == "0")
                {
                    sw_AutoFull.IsOn = true;
                }
                else
                {
                    sw_AutoFull.IsOn = false;
                }
            }
            else
            {
                //container.Values["AutoPlay"] = "0";
                sw_AutoFull.IsOn = true;
            }
            //主题
            if (container.Values["Theme"] != null)
            {
                if (container.Values["Theme"].ToString() == "夜间模式")
                {
                    sw_Dark.IsOn = true;
                }
                else
                {
                    sw_Dark.IsOn = false;
                }
               
            }
            else
            {
                //container.Values["AutoPlay"] = "0";
                sw_Dark.IsOn = false;
            }
            ChangeDrak();
            //弹幕透明度
            if (container.Values["DanMuTran"] != null)
            {
                slider_Tran.Value = double.Parse(container.Values["DanMuTran"].ToString());
            }
            else
            {
                //container.Values["AutoPlay"] = "0";
                slider_Tran.Value = 100;
            }
          
            //弹幕速度
            if (container.Values["DanMuSpeed"] != null)
            {
                slider_Speed.Value = Convert.ToDouble(container.Values["DanMuSpeed"].ToString());
            }
            else
            {
                //container.Values["AutoPlay"] = "0";
                slider_Speed.Value = 7;
            }

            //使用数据
            if (container.Values["UseWifi"] != null)
            {
                if (container.Values["UseWifi"].ToString() == "0")
                {
                    sw_UseWifi.IsOn = true;
                }
                else
                {
                    sw_UseWifi.IsOn = false;
                }
            }
            else
            {
                //container.Values["AutoPlay"] = "0";
                sw_UseWifi.IsOn = false;
            }

            if (container.Values["DanMuYB"] != null)
            {
                txt_danmu.Text = container.Values["DanMuYB"].ToString();
            }
            else
            {
                container.Values["DanMuYB"] = "";
            }
            //if (container.Values["DownPath"] != null)
            //{
            //    if (container.Values["DownPath"].ToString() == "系统默认视频库")
            //    {
            //        txt_Path.Text= "系统默认视频库";
            //    }
            //    else
            //    {
            //        txt_Path.Text = container.Values["DownPath"].ToString();
            //    }
            //}
            //else
            //{
            //    container.Values["DownPath"]= "系统默认视频库";
            //}

            // ApplicationDataContainer container = ApplicationData.Current.LocalSettings;
            //ApplicationDataContainer localSettings = container.CreateContainer("Quality", ApplicationDataCreateDisposition.Always);
            // // 在容器内保存“设置”数据
            // if (container.Containers.ContainsKey("Quality"))
            // {
            //     container.Containers["Quality"].Values["Player"] = "0";
            //     container.Containers["DownOk"].Values["10010"] = @"F:\B.MP4";
            // }
            // txt_Ver.Text = "";
            // foreach (var item in container.Containers["Download"].Values)
            // {
            //     txt_Ver.Text += item.Key+item.Value+"\t";
            // }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                DownloadModel.DownFlie = folder;
                //container.Values["DownPath"] = folder.Path;
                txt_Path.Text = folder.Path;
            }
        }

        private void cb_Quality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cb_Quality.SelectedIndex)
            {
                case 0:
                    container.Values["Quality"] = "1";
                    break;
                case 1:
                    container.Values["Quality"] = "2";
                    break;
                case 2:
                    container.Values["Quality"] = "3";
                    break;
                default:
                    break;
            }
        }

        private void cb_SYZS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cb_SYZS.SelectedIndex)
            {
                case 0:
                    container.Values["SSZS"] = "4";
                    break;
                case 1:
                    container.Values["SSZS"] = "6";
                    break;
                case 2:
                    container.Values["SSZS"] = "12";
                    break;
                default:
                    break;
            }
        }

        private void sw_AutoPlay_Toggled(object sender, RoutedEventArgs e)
        {
            
            if (sw_AutoPlay.IsOn == true)
            {
                container.Values["AutoPlay"] = "0";
            }
            else
            {
                container.Values["AutoPlay"] = "1";
            }
        }

        private void sw_SS_Toggled(object sender, RoutedEventArgs e)
        {
            if (sw_SS.IsOn == true)
            {
                container.Values["PlaySS"] = "0";
            }
            else
            {
                container.Values["PlaySS"] = "1";
            }
        }

        private void sw_UseWifi_Toggled(object sender, RoutedEventArgs e)
        {
            if (sw_UseWifi.IsOn == true)
            {
                container.Values["UseWifi"] = "0";
            }
            else
            {
                container.Values["UseWifi"] = "1";
            }
        }

        private  void btn_ClearCache_Click(object sender, RoutedEventArgs e)
        {
            //StorageFolder _folder = ApplicationData.Current.GetPublisherCacheFolder(@"AC\INetCache");
            //await ApplicationData.Current.ClearPublisherCacheFolderAsync("ms-appx:///AC");
            //StorageFolder _folder = ApplicationData.Current.LocalCacheFolder;
            
            //// await  ApplicationData.Current.ClearAsync();
            //// await ApplicationData.Current.ClearAsync(ApplicationDataLocality.Local);
            //// await _folder.DeleteAsync(StorageDeleteOption.PermanentDelete);
            ////StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(@"ms-appx:///AC");
            //MessageDialog md = new MessageDialog(_folder.Path);
            //await md.ShowAsync();
        }

        private void sw_AutoFull_Toggled(object sender, RoutedEventArgs e)
        {
            if (sw_AutoFull.IsOn == true)
            {
                container.Values["AutoFull"] = "0";
            }
            else
            {
                container.Values["AutoFull"] = "1";
            }
        }

        private void cb_DanMuSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cb_DanMuSize.SelectedIndex)
            {
                case 0:
                    container.Values["DanMuSize"] = "0";
                    break;
                case 1:
                    container.Values["DanMuSize"] = "1";
                    break;
                case 2:
                    container.Values["DanMuSize"] = "2";
                    break;
                default:
                    break;
            }
        }

        private void slider_Tran_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            container.Values["DanMuTran"] = slider_Tran.Value;
        }

        private void slider_Speed_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            container.Values["DanMuSpeed"] = slider_Speed.Value;
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            // NavigateUri = "https://github.com/xiaoyaocz/BILIBILI-UWP"
            MessageDialog md = new MessageDialog("GitHub暂未上传源码，若需要请到交流群530991215内自取，谢谢");
            await md.ShowAsync();
        }

        private void sw_Dark_Toggled(object sender, RoutedEventArgs e)
        {
            if (sw_Dark.IsOn)
            {
                container.Values["Theme"] = "夜间模式";
            }
            else
            {
                container.Values["Theme"] = "日间模式";
            }
            ChangeDrak();
        }

        private void btn_SaveDanMu_Click(object sender, RoutedEventArgs e)
        {
            container.Values["DanMuYB"] = txt_danmu.Text;
            string[] gjz= txt_danmu.Text.Split(',');
            txt_TS.Text = string.Format("保存成功！共{0}个关键词",gjz.Length);
            txt_TS.Visibility = Visibility.Visible;
        }
    }
}

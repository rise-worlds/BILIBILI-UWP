using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace BILIBILI_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DownloadPage : Page
    {
        public DownloadPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
           // GetDownOk();
        }

        // 下载任务的集合
        public static Dictionary<string, string> ListDownEnd = new Dictionary<string, string>();
        // 所有下载任务的关联的 CancellationTokenSource 对象
        //private CancellationTokenSource _cancelToken = new CancellationTokenSource();
        private List<DownloadOperation> ls = new List<DownloadOperation>();
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            list_Downing.Items.Clear();
            GetDownOk();
            ls.Clear();
            IReadOnlyList<DownloadOperation> downloads = null;
            // 获取所有后台下载任务
            downloads = await BackgroundDownloader.GetCurrentDownloadsForTransferGroupAsync(DownloadModel.group);
            foreach (var item in downloads)
            {
                DownloadModel model = new DownloadModel() {
                   //DownCid= ListDownEnd[item.Guid.ToString()],
                  DownFileName = item.ResultFile.Name,
                    DownMaxNum = item.Progress.TotalBytesToReceive / 1024 / 1024,
                    DownNum = item.Progress.BytesReceived / 1024 / 1024,
                    DownStatus = item.Progress.Status.ToString(),
                    downOp = item,
                    DownFilePath=item.ResultFile.Path
                };
                list_Downing.Items.Add(model);
                ls.Add(model.downOp);
            }
            
           
            if (downloads.Count > 0)
            {
                List<Task> tasks = new List<Task>();
                foreach (DownloadModel model in list_Downing.Items)
                {
                    // 监视指定的后台下载任务
                    tasks.Add(HandleDownloadAsync(model));
                }

               await Task.WhenAll(tasks);
                    
            }
        }
        private async void GetDownOk()
        {
            //ApplicationDataContainer container = ApplicationData.Current.LocalSettings;
            //container.CreateContainer("DownOk", ApplicationDataCreateDisposition.Always);
            //container.CreateContainer("DownDanMu", ApplicationDataCreateDisposition.Always);
            //// 在容器内保存“设置”数据
            List<DownloadOkModel> lsa = new List<DownloadOkModel>();
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFolder DownFolder = await folder.CreateFolderAsync("DownLoad", CreationCollisionOption.OpenIfExists);
            IReadOnlyList<StorageFile> list= await DownFolder.GetFilesAsync();
            foreach (var item in list)
            {
                if (item.FileType ==".txt")
                {
                    string textContent = await FileIO.ReadTextAsync(item);
                    string[] tmdhaofan = textContent.Split(',');
                    if (tmdhaofan[4]=="ok")
                    {
                        lsa.Add(new DownloadOkModel { Cid = tmdhaofan[0], DownUrl = tmdhaofan[1], FileName = tmdhaofan[2], DownName = tmdhaofan[2], ResultFilePath = tmdhaofan[3], FilePath = tmdhaofan[3], Date = tmdhaofan[5] });
                    }
                }
            }
            list_DownOk.ItemsSource = lsa;
        }
        
        /// <summary>
        /// 监视指定的后台下载任务
        /// </summary>
        /// <param name="download">后台下载任务</param>
        private async Task HandleDownloadAsync(DownloadModel model)
        {
            try
            {
                // 当下载进度发生变化时的回调函数
                Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);
                
                await model.downOp.AttachAsync().AsTask(model.cancelToken.Token, progressCallback); // 监视已存在的后台下载任务
                
                // 下载完成后获取服务端的响应信息
                ResponseInformation response = model.downOp.GetResponseInformation();

                ToastTemplateType toastTemplate = ToastTemplateType.ToastText01;
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
                ((XmlElement)toastNode).SetAttribute("duration", "short");
                toastTextElements[0].AppendChild(toastXml.CreateTextNode(String.Format("任务{0}完成",model.downOp.ResultFile.Name)));
                ToastNotification toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier().Show(toast);

                GetDownOk();
                //list_DownOk.ItemsSource = GetDownOk();
                string fileCid=  DownloadPage.ListDownEnd[model.downOp.Guid.ToString()];
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFolder DownFolder = await folder.GetFolderAsync("DownLoad");

                StorageFile file =await DownFolder.GetFileAsync(fileCid+".txt");
                string textContent = await FileIO.ReadTextAsync(file);
                
                //string[] tmdhaofan = textContent.Split(',');
                //tmdhaofan[4] = "ok";
               // string info = tmdhaofan[0] + "," + tmdhaofan[1] + "," + tmdhaofan[2] + "," + tmdhaofan[3] +","+ tmdhaofan[4] +","+ tmdhaofan[5];
                await FileIO.WriteTextAsync(file, textContent.Replace("ing", "ok"));

                ls.Remove(model.downOp);
                list_Downing.Items.Remove(model);

            }
            catch (TaskCanceledException)
            {
                ToastTemplateType toastTemplate = ToastTemplateType.ToastText01;
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
                ((XmlElement)toastNode).SetAttribute("duration", "short");
                toastTextElements[0].AppendChild(toastXml.CreateTextNode(String.Format("取消任务{0}", model.downOp.ResultFile.Name)));
                ToastNotification toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier().Show(toast);
                if (DownloadModel.DownFlie==null)
                {

                    StorageFile file = await StorageFile.GetFileFromPathAsync(model.downOp.ResultFile.Path);
                    await file.DeleteAsync();
                }
            }
            catch (Exception ex)
            {
                WebErrorStatus error = BackgroundTransferError.GetStatus(ex.HResult);
                //throw;
            }
            finally
            {
                list_Downing.Items.Remove(model);
                ls.Remove(model.downOp);
            }
        }

        //进度发生变化时，更新 TransferModel 的 Progress，以便通知
        private void DownloadProgress(DownloadOperation downOp)
        {
            ((DownloadModel)list_Downing.Items[ls.IndexOf(downOp)]).DownMaxNum = downOp.Progress.TotalBytesToReceive / 1024 / 1024;
            ((DownloadModel)list_Downing.Items[ls.IndexOf(downOp)]).DownNum = downOp.Progress.BytesReceived / 1024 / 1024;
            ((DownloadModel)list_Downing.Items[ls.IndexOf(downOp)]).DownStatus = downOp.Progress.Status.ToString();
        }
 
        private async void GetDownInfo()
        {
            list_Downing.Items.Clear();
            IReadOnlyList<DownloadOperation> downloads = null;
            // 获取所有后台下载任务
            downloads = await BackgroundDownloader.GetCurrentDownloadsForTransferGroupAsync(DownloadModel.group);
            foreach (var item in downloads)
            {
                DownloadModel model = new DownloadModel()
                {
                    DownFileName = item.ResultFile.Name,
                    DownFilePath=item.ResultFile.Path,
                    DownMaxNum = item.Progress.TotalBytesToReceive / 1024 / 1024,
                    DownNum = item.Progress.BytesReceived / 1024 / 1024,
                    DownStatus = item.Progress.Status.ToString(),
                    downOp = item
                };
                list_Downing.Items.Add(model);
            }

        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
           // list_Downing.SelectionMode = ListViewSelectionMode.Multiple;
            if (list_Downing.SelectedItems.Count!=0)
            {
                foreach (DownloadModel item in list_Downing.SelectedItems)
                {
                    if (item.downOp.Progress.Status==BackgroundTransferStatus.PausedByApplication)
                    {
                        item.downOp.Resume();
                        //GetDownInfo();
                    }
                }
            }
        }

        private void btn_Pause_Click(object sender, RoutedEventArgs e)
        {
            if (list_Downing.SelectedItems.Count != 0)
            {
                foreach (DownloadModel item in list_Downing.SelectedItems)
                {
                    if (item.downOp.Progress.Status == BackgroundTransferStatus.Running)
                    {
                        item.downOp.Pause();
                    }
                }
            }
        }

        private async void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (list_Downing.SelectedItems.Count != 0)
            {
                foreach (DownloadModel item in list_Downing.SelectedItems)
                {
                    item.cancelToken.Cancel(false);
                    item.cancelToken.Dispose();
                    try
                    {
                        StorageFolder folder = ApplicationData.Current.LocalFolder;
                        StorageFolder DownFolder = await folder.GetFolderAsync("DownLoad");
                        StorageFile fileText = await DownFolder.GetFileAsync(item.DownCid + ".txt");
                        StorageFile fileXml = await DownFolder.GetFileAsync(item.DownCid + ".xml");
                        //StorageFile file =await StorageFile.GetFileFromPathAsync(item.DownFilePath);
                        await fileText.DeleteAsync();
                        await fileXml.DeleteAsync();
                       // await file.DeleteAsync();
                    }
                    catch (Exception)
                    {
                        //throw;
                    }
                }
            }
            //   _cancelToken.Cancel(false);
            //    _cancelToken.Dispose();

        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingPage), 1);
        }
        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as Pivot).SelectedIndex==0)
            {
                bar_down.Visibility = Visibility.Visible;
                bar_down_OK.Visibility = Visibility.Collapsed;
            }
            else
            {
                bar_down.Visibility = Visibility.Collapsed;
                bar_down_OK.Visibility = Visibility.Visible;
            }
        }

        private async void btn_Play_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromPathAsync(((DownloadOkModel)list_DownOk.SelectedItem).FilePath);
                this.Frame.Navigate(typeof(PlayerPage), new VideoInfoModel { path = ((DownloadOkModel)list_DownOk.SelectedItem).FilePath, title = ((DownloadOkModel)list_DownOk.SelectedItem).FileName,cid = ((DownloadOkModel)list_DownOk.SelectedItem).Cid });
            }
            catch (Exception)
            {
                MessageDialog md = new MessageDialog("无权限访问，可能是视频存在于非系统默认视频库中！");
               await md.ShowAsync();
            }
            
        }

        private async void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //ApplicationDataContainer container = ApplicationData.Current.LocalSettings;
                // container.CreateContainer("DownOk", ApplicationDataCreateDisposition.Always);
                //container.CreateContainer("DownStatus", ApplicationDataCreateDisposition.Always);
                // 在容器内保存“设置”数据
                //List<DownloadOkModel> ls = new List<DownloadOkModel>();
                //container.Containers["DownOk"].Values.Remove(((DownloadOkModel)list_DownOk.SelectedItem).FileName);
                //container.Containers["DownDanMu"].Values.Remove(((DownloadOkModel)list_DownOk.SelectedItem).FileName);
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFolder DownFolder = await folder.GetFolderAsync("DownLoad");
                StorageFile fileText = await DownFolder.GetFileAsync(((DownloadOkModel)list_DownOk.SelectedItem).Cid + ".txt");
                StorageFile fileXml = await DownFolder.GetFileAsync(((DownloadOkModel)list_DownOk.SelectedItem).Cid + ".xml");
                StorageFile file = await StorageFile.GetFileFromPathAsync(((DownloadOkModel)list_DownOk.SelectedItem).FilePath);
                await fileText.DeleteAsync();
                await fileXml.DeleteAsync();
                await file.DeleteAsync();
                GetDownOk();
                //list_DownOk.ItemsSource = GetDownOk();
            }
            catch (Exception)
            {
                MessageDialog md = new MessageDialog("无权限访问，已从列表删除，请到文件所在位置自行删除文件!");
                await md.ShowAsync();
                GetDownOk();
               // list_DownOk.ItemsSource = GetDownOk();
            }
        }
    }

    public class DownloadModel : INotifyPropertyChanged
    {
        
        public CancellationTokenSource cancelToken = new CancellationTokenSource();
        public string DownCid { get; set; }//下载的视频编号
        public string DownName { get; set; }//下载的视频名称
        private string _downStatus;
        public string DownStatus {
            get {
                switch (_downStatus)
                {
                    case "Idle":
                        return "空闲中";
                case "Error":
                        return "错误";
                case "PausedByApplication":
                        return "暂停";
                case "PausedCostedNetwork":
                        return "因网络成本暂停";
                case "PausedNoNetwork":
                        return "因网络问题暂停";
                case "PausedSystemPolicy":
                        return "因系统问题暂停";
                 case "Running":
                        return "下载中";
                default:
                        return "未知状态";
                }

            }
            set { _downStatus = value;
                thisPropertyChanged("DownStatus");
            }
             }//下载的状态

        private ulong _downMaxNum;
        public ulong DownMaxNum {
            get { return _downMaxNum; }
            set { _downMaxNum = value;
                thisPropertyChanged("DownMaxNum");
            }
        }//下载文件大小总数

        private ulong _downNum;
        public ulong DownNum {
            get { return _downNum; }
           set {
                _downNum = value;
                thisPropertyChanged("DownNum");
            }
        }//已经下载的文件大小

        public string DownUrl { get; set; }//下载地址
        public string DownFileName{ get; set; }//下载存放文件名
        public DownloadOperation downOp { get; set; }
        
        public string DownFilePath { get; set; }//下载存放文件名
        public static StorageFolder DownFlie = null;//下载文件夹
        public static BackgroundTransferGroup group = BackgroundTransferGroup.CreateGroup("BILIBILI-UWP");//下载组，方便管理

        public event PropertyChangedEventHandler PropertyChanged;
        protected void thisPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

    }

    public class DownloadOkModel
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Cid { get; set; }
        public string DownUrl { get; set; }
        public string DownName { get; set; }
        public string ResultFilePath { get; set; }
        public string Date { get; set; }
        //public string FileSize { get; set; }
        //public string FileSize
        //{
        //    get
        //    {
        //        //FileInfo file = File.
        //        return "";
        //    }
        //}
        //public DateTime FileCreTime { get; set; }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;


// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace BILIBILI_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class VideoPage : Page
    {
        public VideoPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }
        WebClientClass wc = new WebClientClass();
        string aid = "";
        string rootsid = "";
        int Pagenum = 1;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Tag
            Video_ListView_Favbox.IsEnabled = true;
            Video_Error.Visibility = Visibility.Collapsed;
            aid = (string)e.Parameter;
            GetVideoInfo((string)e.Parameter);
            GetVideoComment((string)e.Parameter);
            GetFavBox();
        }



        /// <summary>
        /// 读取视频信息
        /// </summary>
        /// <param name="aid"></param>
        private async void GetVideoInfo(string aid)
        {
            try
            {
                Video_Content.Visibility = Visibility.Collapsed;
                Video_Load.Visibility = Visibility.Visible;
                string results = await wc.GetResults(new Uri("http://api.bilibili.com/view?type=json&appkey=422fd9d7289a1dd9&id=" + aid + "&batch=1"));

                VideoInfoModel model = new VideoInfoModel();
                model = JsonConvert.DeserializeObject<VideoInfoModel>(results);
                Video_Grid_Info.DataContext = model;
                List<VideoInfoModel> ban = JsonConvert.DeserializeObject<List<VideoInfoModel>>(model.list.ToString());
                foreach (VideoInfoModel item in ban)
                {
                    item.title = model.title;
                    item.aid = aid;
                }
                Video_List.ItemsSource = ban;
                Video_Content.Visibility = Visibility.Visible;
                Video_Load.Visibility = Visibility.Collapsed;
            }
            catch (Exception)
            {
                //throw;
                Video_Error.Visibility = Visibility.Visible;
                await Task.Delay(3000);
                if (this.Tag.ToString() == "视频信息")
                {
                    if (this.Frame.CanGoBack)
                    {
                        this.Frame.GoBack();
                    }
                }
            }
        }
        /// <summary>
        /// 取热门评论
        /// </summary>
        /// <param name="aid"></param>
        private async void GetVideoComment(string aid)
        {
            try
            {
                string results = await wc.GetResults(new Uri("http://api.bilibili.com/x/reply?jsonp=jsonp&type=1&sort=2&oid=" + aid + "&pn=1&nohot=1&ps=5"));
                CommentModel model = new CommentModel();
                model = JsonConvert.DeserializeObject<CommentModel>(results);
                CommentModel model3 = new CommentModel();
                model3 = JsonConvert.DeserializeObject<CommentModel>(model.data.ToString());
                List<CommentModel> ban = JsonConvert.DeserializeObject<List<CommentModel>>(model3.replies.ToString());
                List<CommentModel> ha = new List<CommentModel>();
                foreach (CommentModel item in ban)
                {
                    CommentModel model1 = new CommentModel();
                    model1 = JsonConvert.DeserializeObject<CommentModel>(item.member.ToString());
                    CommentModel model2 = new CommentModel();
                    model2 = JsonConvert.DeserializeObject<CommentModel>(item.content.ToString());
                    CommentModel resultsModel = new CommentModel()
                    {
                        avatar = model1.avatar,
                        message = model2.message,
                        floor = item.floor,
                        uname = model1.uname,
                        mid = model1.mid,
                        ctime = item.ctime,
                        like = item.like,
                        rcount = item.rcount,
                        rpid = item.rpid
                    };
                    ha.Add(resultsModel);
                }
                ListView_Comment.ItemsSource = ha;
            }
            catch (Exception ex)
            {
                grid_GG.Visibility = Visibility.Visible;
                txt_GG.Text = "读取热门评论失败!" + ex.Message;
                await Task.Delay(2000);
                grid_GG.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// 取热门评论下的回复
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="rootid"></param>
        private async void GetComments(string aid, string rootid)
        {
            try
            {
                Comment_loading.Visibility = Visibility.Visible;
                CanLoad = false;
                string results = await wc.GetResults(new Uri("http://api.bilibili.com/x/reply/reply?oid=" + aid + "&pn=1&ps=20&root=" + rootid + "&type=1"));
                CommentModel model = new CommentModel();
                model = JsonConvert.DeserializeObject<CommentModel>(results);
                CommentModel model3 = new CommentModel();
                model3 = JsonConvert.DeserializeObject<CommentModel>(model.data.ToString());
             
                List<CommentModel> ls = new List<CommentModel>();
                List<CommentModel> ban = JsonConvert.DeserializeObject<List<CommentModel>>(model3.replies.ToString());
                foreach (CommentModel item in ban)
                {
                    CommentModel model1 = new CommentModel();
                    model1 = JsonConvert.DeserializeObject<CommentModel>(item.member.ToString());
                    CommentModel model2 = new CommentModel();
                    model2 = JsonConvert.DeserializeObject<CommentModel>(item.content.ToString());
                    CommentModel resultsModel = new CommentModel()
                    {
                        avatar = model1.avatar,
                        message = model2.message,
                        floor = item.floor,
                        uname = model1.uname,
                        mid = model1.mid,
                        ctime = item.ctime,
                        like = item.like,
                        rcount = item.rcount,
                        rpid = item.rpid
                    };
                    ListView_Flyout.Items.Add(resultsModel);
                }
                Comment_loading.Visibility = Visibility.Collapsed;
                CanLoad = true;
            }
            catch (Exception ex)
            {
                grid_GG.Visibility = Visibility.Visible;
                txt_GG.Text = "Error for GetComments\r\n" + ex.Message;
                await Task.Delay(2000);
                grid_GG.Visibility = Visibility.Collapsed;
                //MessageDialog md = new MessageDialog("Error for GetComments\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
                //  await md.ShowAsync();
                Comment_loading.Visibility = Visibility.Collapsed;
            }

        }
        /// <summary>
        /// 取热门评论下的回复？？？？应该是
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="rootid"></param>
        private async void GetComments(string aid, string rootid, int num)
        {
            try
            {
                btn_Load_More.Content = "加载中....";
                btn_Load_More.IsEnabled = false;
                Comment_loading.Visibility = Visibility.Visible;
                string results = await wc.GetResults(new Uri("http://api.bilibili.com/x/reply/reply?oid=" + aid + "&pn=" + num + "&ps=20&root=" + rootid + "&type=1"));
                CommentModel model = new CommentModel();
                model = JsonConvert.DeserializeObject<CommentModel>(results);
                CommentModel model3 = new CommentModel();
                model3 = JsonConvert.DeserializeObject<CommentModel>(model.data.ToString());
                List<CommentModel> ban = JsonConvert.DeserializeObject<List<CommentModel>>(model3.replies.ToString());
                if (ban.Count == 0)
                {
                    btn_Load_More.Content = "加载完了...";
                    btn_Load_More.IsEnabled = false;
                }
                else
                {
                    btn_Load_More.Content = "加载更多";
                    btn_Load_More.IsEnabled = true;
                }
                foreach (CommentModel item in ban)
                {
                    CommentModel model1 = new CommentModel();
                    model1 = JsonConvert.DeserializeObject<CommentModel>(item.member.ToString());
                    CommentModel model2 = new CommentModel();
                    model2 = JsonConvert.DeserializeObject<CommentModel>(item.content.ToString());
                    CommentModel resultsModel = new CommentModel()
                    {
                        avatar = model1.avatar,
                        message = model2.message,
                        floor = item.floor,
                        uname = model1.uname,
                        mid = model1.mid,
                        ctime = item.ctime,
                        like = item.like,
                        rcount = item.rcount,
                        rpid = item.rpid
                    };
                    ListView_Flyout.Items.Add(resultsModel);
                }
                Comment_loading.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                grid_GG.Visibility = Visibility.Visible;
                txt_GG.Text = "读取评论信息失败!" + ex.Message;
                await Task.Delay(2000);
                grid_GG.Visibility = Visibility.Collapsed;
                /// MessageDialog md = new MessageDialog("Error for GetComments" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
                // await md.ShowAsync();
                Comment_loading.Visibility = Visibility.Collapsed;
            }

        }
        /// <summary>
        /// 跳转播放页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Video_List_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Frame rootFrame = Window.Current.Content as Frame;
            //rootFrame.Navigate((typeof(PlayerPage)), (VideoInfoModel)e.ClickedItem);

            this.Frame.Navigate(typeof(PlayerPage), (VideoInfoModel)e.ClickedItem);
        }
        /// <summary>
        /// 弹出评论回复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_Comment_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplitView_Comment.IsPaneOpen = true;
            //btn_Load_More.Content = "加载更多";
            // btn_Load_More.IsEnabled = true;
            ListView_Flyout.Items.Clear();
            rootsid = ((CommentModel)e.ClickedItem).rpid;
            Pagenum = 1;
            GetComments(aid, ((CommentModel)e.ClickedItem).rpid);
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Video_Refresh_Click(object sender, RoutedEventArgs e)
        {
            GetVideoInfo(aid);
            GetVideoComment(aid);
        }
        /// <summary>
        /// 加载更多
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Load_More_Click(object sender, RoutedEventArgs e)
        {
            Pagenum++;
            GetComments(aid, rootsid, Pagenum);
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UserInfoPage), ((VideoInfoModel)Video_Grid_Info.DataContext).mid);
        }

        private void btn_Comm_Click(object sender, RoutedEventArgs e)
        {
            string[] comment = new string[2];
            comment[0] = aid;
            comment[1] = Video_Title.Text;
            this.Frame.Navigate(typeof(CommentPage), comment);
        }
        /// <summary>
        /// 读取收藏夹信息，用于收藏视频
        /// </summary>
        private async void GetFavBox()
        {
            GetLoginInfoClass getLogin = new GetLoginInfoClass();
            if (getLogin.IsLogin())
            {
                try
                {
                    string results = await wc.GetResults(new Uri("http://api.bilibili.com/x/favourite/folder?jsonp=jsonp"));
                    FavboxModel model = JsonConvert.DeserializeObject<FavboxModel>(results);
                    List<FavboxModel> ban = JsonConvert.DeserializeObject<List<FavboxModel>>(model.data.ToString());
                    Video_ListView_Favbox.ItemsSource = ban;
                }
                catch (Exception ex)
                {
                    FavBox_Header.Text = "获取失败！" + ex.Message;
                }
            }
            else
            {
                FavBox_Header.Text = "请先登录！";
                Video_ListView_Favbox.IsEnabled = false;
            }
        }
        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Video_ListView_Favbox_ItemClick(object sender, ItemClickEventArgs e)
        {
            GetLoginInfoClass getLogin = new GetLoginInfoClass();
            if (getLogin.IsLogin())
            {
                try
                {
                    Uri ReUri = new Uri("http://api.bilibili.com/x/favourite/video/add");
                    //HttpClient hc = new HttpClient();
                   // hc.DefaultRequestHeaders.Referer = new Uri("http://www.bilibili.com/");
                    string QuStr = "jsonp=jsonp&fid=" + ((FavboxModel)e.ClickedItem).fid + "&aid=" + aid;
                   // var response = await hc.PostAsync(ReUri, new HttpStringContent(QuStr, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/x-www-form-urlencoded"));
                    //response.EnsureSuccessStatusCode();
                    string result = await wc.PostResults(ReUri,QuStr);
                    JObject json = JObject.Parse(result);
                    if ((int)json["code"] == 0)
                    {
                        grid_GG.Visibility = Visibility.Visible;
                        txt_GG.Text = "收藏成功！";
                        await Task.Delay(2000);
                        grid_GG.Visibility = Visibility.Collapsed;
                        //MessageDialog md = new MessageDialog("收藏成功！");
                        //wait md.ShowAsync();
                        GetFavBox();
                    }
                    else
                    {
                        if ((int)json["code"] == 11007)
                        {
                            grid_GG.Visibility = Visibility.Visible;
                            txt_GG.Text = "视频已经收藏!";
                            await Task.Delay(2000);
                            grid_GG.Visibility = Visibility.Collapsed;
                            //MessageDialog md = new MessageDialog("视频已经收藏！");
                            //await md.ShowAsync();
                        }
                        else
                        {
                            grid_GG.Visibility = Visibility.Visible;
                            txt_GG.Text = "收藏失败！";
                            await Task.Delay(2000);
                            grid_GG.Visibility = Visibility.Collapsed;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageDialog md = new MessageDialog(ex.Message, "收藏时发生错误！");
                    await md.ShowAsync();
                    throw;
                }
            }
            else
            {
                grid_GG.Visibility = Visibility.Visible;
                txt_GG.Text = "没有登录!";
                await Task.Delay(2000);
                grid_GG.Visibility = Visibility.Collapsed;
            }
        }

        private void btn_No_Click(object sender, RoutedEventArgs e)
        {
            grid_Tb.Hide();
        }
        /// <summary>
        /// 投币
        /// </summary>
        /// <param name="num">数量</param>
        public async void TouBi(int num)
        {
            GetLoginInfoClass getLogin = new GetLoginInfoClass();
            if (getLogin.IsLogin())
            {
                try
                {
                    Uri ReUri = new Uri("http://www.bilibili.com/plus/comment.php");
                   //HttpClient hc = new HttpClient();
                   // hc.DefaultRequestHeaders.Referer = new Uri("http://www.bilibili.com/");
                    string QuStr = "aid=" + aid + "&rating=100&player=1&multiply=" + num;
                    //var response = await hc.PostAsync(ReUri, new HttpStringContent(QuStr, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/x-www-form-urlencoded"));
                    //response.EnsureSuccessStatusCode();
                    string result = await wc.PostResults(ReUri,QuStr);
                    if (result == "OK")
                    {
                        grid_GG.Visibility = Visibility.Visible;
                        txt_GG.Text = "投币成功！";
                        await Task.Delay(2000);
                        grid_GG.Visibility = Visibility.Collapsed;
                        //MessageDialog md = new MessageDialog("投币成功！");
                        // await md.ShowAsync();
                    }
                    else
                    {
                        grid_GG.Visibility = Visibility.Visible;
                        txt_GG.Text = "投币失败！";
                        await Task.Delay(2000);
                        grid_GG.Visibility = Visibility.Collapsed;
                        //MessageDialog md = new MessageDialog("投币失败！");
                        // await md.ShowAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageDialog md = new MessageDialog(ex.Message, "投币时发生错误！");
                    await md.ShowAsync();
                    throw;
                }
            }
            else
            {
                grid_GG.Visibility = Visibility.Visible;
                txt_GG.Text = "没有登录！";
                await Task.Delay(2000);
                grid_GG.Visibility = Visibility.Collapsed;
                //MessageDialog md = new MessageDialog("没有登录！");
                //await md.ShowAsync();
            }
        }

        private void btn_TB_1_Click(object sender, RoutedEventArgs e)
        {
            TouBi(1);
        }

        private void btn_TB_2_Click(object sender, RoutedEventArgs e)
        {
            TouBi(2);
        }

        private void btn_Download_Click(object sender, RoutedEventArgs e)
        {
            Video_List.Focus(FocusState.Programmatic);
            Video_List.SelectionMode = ListViewSelectionMode.Multiple;
            Video_List.IsItemClickEnabled = false;
            Down_ComBar.Visibility = Visibility.Visible;
            Video_ComBar.Visibility = Visibility.Collapsed;
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Video_List.SelectionMode = ListViewSelectionMode.None;
            Video_List.IsItemClickEnabled = true;
            Down_ComBar.Visibility = Visibility.Collapsed;
            Video_ComBar.Visibility = Visibility.Visible;
        }

        private async void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            if (Video_List.SelectedItems.Count != 0)
            {
                foreach (VideoInfoModel item in Video_List.SelectedItems)
                {
                    int quality = cb_Qu.SelectedIndex + 1;
                    string url = await GetVideoUri(item.cid, quality);
                    if (url != "")
                    {
                        DownloadModel model = new DownloadModel()
                        {

                            DownCid = item.cid,
                            DownName = item.title + "_P" + item.page,
                            DownUrl = url
                        };
                        StartDownload(model);
                        DownDanMu(model);
                    }
                    else
                    {
                        MessageDialog md = new MessageDialog(item.title + "\t视频地址获取失败");
                        await md.ShowAsync();
                    }
                }
                Video_List.SelectionMode = ListViewSelectionMode.None;
                Video_List.IsItemClickEnabled = true;
                Down_ComBar.Visibility = Visibility.Collapsed;
                Video_ComBar.Visibility = Visibility.Visible;

                MessageDialog md1 = new MessageDialog("任务已经加入下载队列！\r\n请尽量不要将程序完全退出,否则会使下载完成无法通知");
                await md1.ShowAsync();
            }
            else
            {
                Video_List.SelectionMode = ListViewSelectionMode.None;
                Video_List.IsItemClickEnabled = true;
                Down_ComBar.Visibility = Visibility.Collapsed;
                Video_ComBar.Visibility = Visibility.Visible;
            }
        }


        private async System.Threading.Tasks.Task<string> GetVideoUri(string cid, int quality)
        {
            //http://interface.bilibili.com/playurl?platform=android&cid=5883400&quality=2&otype=json&appkey=422fd9d7289a1dd9&type=mp4
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://interface.bilibili.com/playurl?platform=android&cid=" + cid + "&quality=" + quality + "&otype=json&appkey=422fd9d7289a1dd9&type=mp4"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    VideoUriModel model = JsonConvert.DeserializeObject<VideoUriModel>(results);
                    List<VideoUriModel> model1 = JsonConvert.DeserializeObject<List<VideoUriModel>>(model.durl.ToString());
                    return model1[0].url;
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        ApplicationDataContainer container = ApplicationData.Current.LocalSettings;
        private async void StartDownload(DownloadModel downModel)
        {

            try
            {
                BackgroundDownloader downloader = new BackgroundDownloader();
                downloader.TransferGroup = DownloadModel.group;
                if (container.Values["UseWifi"] != null)
                {
                    if (container.Values["UseWifi"].ToString() == "0")
                    {
                        downloader.CostPolicy = BackgroundTransferCostPolicy.Always;
                    }
                    else
                    {
                        downloader.CostPolicy = BackgroundTransferCostPolicy.UnrestrictedOnly;
                    }
                }
                else
                {
                    downloader.CostPolicy = BackgroundTransferCostPolicy.UnrestrictedOnly;
                }
                StorageFile file = null;
                //string path = container.Values["DownPath"].ToString();
                //if (path!= "系统默认视频库")
                //{
                //    DownloadModel.DownFlie = await StorageFolder.GetFolderFromPathAsync(path);
                //}

                if (DownloadModel.DownFlie != null)
                {
                    file = await DownloadModel.DownFlie.CreateFileAsync(downModel.DownName + ".mp4", CreationCollisionOption.GenerateUniqueName);
                }
                else
                {
                    file = await KnownFolders.VideosLibrary.CreateFileAsync(downModel.DownName + ".mp4", CreationCollisionOption.GenerateUniqueName);
                }
                DownloadOperation downloadOp = downloader.CreateDownload(new Uri(downModel.DownUrl), file);
                downloadOp.CostPolicy = BackgroundTransferCostPolicy.UnrestrictedOnly;
                //downloadOp.ResultFile.Name;
                BackgroundTransferStatus downloadStatus = downloadOp.Progress.Status;

                //container.CreateContainer("DownFilePath", ApplicationDataCreateDisposition.Always);
                //container.CreateContainer("DownDanMu", ApplicationDataCreateDisposition.Always);
                //// 在容器内保存“设置”数据
                //if (container.Containers.ContainsKey("DownDanMu"))
                //{
                //    container.Containers["DownDanMu"].Values[downModel.DownCid] = downloadOp.ResultFile.Path;
                //}
                //if (container.Containers.ContainsKey("DownStatus"))
                //{
                //    container.Containers["DownStatus"].Values[downModel.DownCid] = "Start";
                //}
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                var test = await folder.TryGetItemAsync("DownLoad");
                if (test == null)
                {
                    await folder.CreateFolderAsync("DownLoad");
                }
                StorageFolder DowFolder = await folder.GetFolderAsync("DownLoad");

                StorageFile fileWrite = await DowFolder.CreateFileAsync(downModel.DownCid + ".txt", CreationCollisionOption.ReplaceExisting);

                string info = downModel.DownCid + "," + downModel.DownUrl + "," + downModel.DownName + "," + downloadOp.ResultFile.Path + ",ing," + DateTime.Now.ToString();
                await FileIO.WriteTextAsync(fileWrite, info);
                DownloadPage.ListDownEnd.Add(downloadOp.Guid.ToString(), downModel.DownCid);
                await downloadOp.StartAsync();
            }
            catch (Exception)
            {
                //MessageDialog md = new MessageDialog("创建下载任务失败!\r\n"+ex.Message);
                //await md.ShowAsync();
            }
        }

        private async void DownDanMu(DownloadModel downModel)
        {
            HttpClient hc = new HttpClient();
            HttpResponseMessage hr = await hc.GetAsync(new Uri("http://comment.bilibili.com/" + downModel.DownCid + ".xml"));
            hr.EnsureSuccessStatusCode();
            string results = await hr.Content.ReadAsStringAsync();
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            var test = await folder.TryGetItemAsync("DownLoad");
            if (test == null)
            {
                await folder.CreateFolderAsync("DownLoad");
            }
            StorageFolder DowFolder = await folder.GetFolderAsync("DownLoad");
            StorageFile fileWrite = await DowFolder.CreateFileAsync(downModel.DownCid + ".xml", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(fileWrite, results);
        }

        private void btn_playP1_Click(object sender, RoutedEventArgs e)
        {
            if (Video_List.Items.Count != 0)
            {
                this.Frame.Navigate(typeof(PlayerPage), (VideoInfoModel)Video_List.Items[0]);
            }
        }

        private async void btn_Zan_Click(object sender, RoutedEventArgs e)
        {
            string rpid = ((sender as HyperlinkButton).DataContext as CommentModel).rpid;
            GetLoginInfoClass getUser = new GetLoginInfoClass();
            if (getUser.IsLogin())
            {
                try
                {
                    Uri ReUri = new Uri("http://api.bilibili.com/x/reply/action");

                    HttpClient hc = new HttpClient();
                    hc.DefaultRequestHeaders.Referer = new Uri("http://www.bilibili.com/");
                    string sendString = "";
                    if (((sender as HyperlinkButton).Content as TextBlock).Text == "赞同")
                    {
                        sendString = "jsonp=jsonp&oid=" + aid + "&type=1&rpid=" + rpid + "&action=1";
                    }
                    else
                    {
                        sendString = "jsonp=jsonp&oid=" + aid + "&type=1&rpid=" + rpid + "&action=0";
                    }
                    var response = await hc.PostAsync(ReUri, new HttpStringContent(sendString, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/x-www-form-urlencoded"));
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(result);
                    if ((int)json["code"] == 0)
                    {
                        if (((sender as HyperlinkButton).Content as TextBlock).Text == "赞同")
                        {
                            ((sender as HyperlinkButton).Content as TextBlock).Text = "取消赞";
                        }
                        else
                        {
                            ((sender as HyperlinkButton).Content as TextBlock).Text = "赞同";
                        }
                    }
                    else
                    {
                        grid_GG.Visibility = Visibility.Visible;
                        txt_GG.Text = "点赞失败！";
                        await Task.Delay(2000);
                        grid_GG.Visibility = Visibility.Collapsed;
                    }

                }
                catch (Exception)
                {
                }
            }
            else
            {
                grid_GG.Visibility = Visibility.Visible;
                txt_GG.Text = "请先登录！";
                await Task.Delay(2000);
                grid_GG.Visibility = Visibility.Collapsed;
            }
        }

        private void btn_TouX_Click(object sender, RoutedEventArgs e)
        {
            CommentModel model = (sender as HyperlinkButton).DataContext as CommentModel;
            this.Frame.Navigate(typeof(UserInfoPage), model.mid);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            text_Comment1.Text += ((Button)sender).Content.ToString();
        }

        private void text_Comment1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (text_Comment1.Text == "")
            {
                root = "";
            }
        }

        private async void btn_SendComment1_Click(object sender, RoutedEventArgs e)
        {
            if (text_Comment1.Text.Length == 0)
            {
                grid_GG.Visibility = Visibility.Visible;
                txt_GG.Text = "内容不能为空！";
                await Task.Delay(2000);
                grid_GG.Visibility = Visibility.Collapsed;
                return;
            }
            GetLoginInfoClass getUser = new GetLoginInfoClass();
            if (getUser.IsLogin())
            {
                try
                {
                    Uri ReUri = new Uri("http://api.bilibili.com/x/reply/add");
                    //HttpClient hc = new HttpClient();
                    //hc.DefaultRequestHeaders.Referer = new Uri("http://www.bilibili.com/");
                    if (root == "")
                    {
                        root = rootsid;
                    }
                    string QuStr = "jsonp=jsonp&message=" + text_Comment1.Text + "&parent=" + rootsid + "&root=" + root + "&type=1&plat=1&oid=" + aid;
                    //var response = await hc.PostAsync(ReUri, new HttpStringContent(QuStr, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/x-www-form-urlencoded"));
                   // response.EnsureSuccessStatusCode();
                    string result = await wc.PostResults(ReUri,QuStr);
                    JObject json = JObject.Parse(result);
                    if ((int)json["code"] == 0)
                    {
                        grid_GG.Visibility = Visibility.Visible;
                        txt_GG.Text = "评论成功！";
                        await Task.Delay(2000);
                        grid_GG.Visibility = Visibility.Collapsed;
                        text_Comment1.Text = "";
                        //MessageDialog md = new MessageDialog("评论成功！");
                        //await md.ShowAsync();
                    }
                    else
                    {
                        grid_GG.Visibility = Visibility.Visible;
                        txt_GG.Text = "评论失败！";
                        await Task.Delay(2000);
                        grid_GG.Visibility = Visibility.Collapsed;
                        // MessageDialog md = new MessageDialog("评论失败！");
                        //await md.ShowAsync();
                    }

                }
                catch (Exception ex)
                {
                    MessageDialog md = new MessageDialog(ex.Message, "评论时发生错误");
                    await md.ShowAsync();
                }
            }
            else
            {
                grid_GG.Visibility = Visibility.Visible;
                txt_GG.Text = "你造吗，你没有登录(～￣▽￣)，先登录好伐~";
                await Task.Delay(2000);
                grid_GG.Visibility = Visibility.Collapsed;
                //MessageDialog md = new MessageDialog("你造吗，你没有登录 (～￣▽￣)，先登录好伐~");
                //await md.ShowAsync();
            }
        }

        string root = "";
        private void ListView_Flyout_ItemClick(object sender, ItemClickEventArgs e)
        {
            root = (e.ClickedItem as CommentModel).rpid;
            text_Comment1.Text = "回复 @" + (e.ClickedItem as CommentModel).uname + "：";
        }
        bool CanLoad = true;
        private void sv1_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (sv1.VerticalOffset == sv1.ScrollableHeight)
            {
                if (CanLoad)
                {
                    Pagenum++;
                    GetComments(aid, rootsid, Pagenum);
                }
            }
        }
    }

    public class VideoInfoModel
    {
        public object list { get; set; }
        public string pic { get; set; }
        public string title { get; set; }
        public string play { get; set; }
        public string author { get; set; }
        public string video_review { get; set; }
        public string description { get; set; }
        public string mid { get; set; }
        public string aid { get; set; }
        public string created_at { get; set; }
        public string favorites { get; set; }
        public string face { get; set; }
        public string coins { get; set; }
        public string page { get; set; }
        public string part { get; set; }
        public string cid { get; set; }
        public string tag { get; set; }
        //用于判断是否本地视频
        public string path { get; set; }
    }
    public class CommentModel
    {
        public object data { get; set; }
        public object replies { get; set; }
        public object member { get; set; }
        public object content { get; set; }
        public string avatar { get; set; }
        public string uname { get; set; }
        public string floor { get; set; }
        public string rpid { get; set; }
        public long ctime { set; get; }
        public string mid { get; set; }
        public string time
        {
            get
            {
                DateTime dtStart = new DateTime(1970, 1, 1);
                long lTime = long.Parse(ctime + "0000000");
                //long lTime = long.Parse(textBox1.Text);
                TimeSpan toNow = new TimeSpan(lTime);
                return dtStart.Add(toNow).ToString();
            }
        }
        public string rcount { get; set; }
        public string like { get; set; }
        public string message { get; set; }

    }

    public class FavboxModel
    {
        public object data { get; set; }

        public int code { get; set; }

        public string fid { get; set; }
        public string mid { get; set; }
        public string name { get; set; }
        public int max_count { get; set; }//总数
        public int cur_count { get; set; }//现存

        public string Count
        {
            get
            {
                return cur_count + "/" + max_count;
            }
        }
    }

    public class VideoUriModel
    {
        public string format { get; set; }//视频类型

        public object durl { get; set; }//视频信息

        public string url { get; set; }//视频地址

        public object backup_url { get; set; }//视频备份地址
    }

}

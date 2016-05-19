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
    public sealed partial class CommentPage : Page
    {
        public CommentPage()
        {
            this.InitializeComponent();
        }
        private int pageNum_New = 1;
        private int Pagenum = 1;
        private string aid = "";
        string rootsid = "";
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
           if (e.Parameter!=null)
            {
                string[] par = (string[])e.Parameter;
                text_Title.Text = par[1]+" 的全部评论";
                aid = par[0];
                ListView_Comment.Items.Clear();
                GetVideoComment(aid);
                ListView_Comment_New.Items.Clear(); 
                GetVideoComment_New(aid);

            }
            
        }
        private async void GetVideoComment(string aid)
        {
            
            try
            {
                grid_Load.Visibility = Visibility.Visible;
                grid_Conent.Visibility = Visibility.Collapsed;
                HttpClient hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/x/reply?jsonp=jsonp&type=1&sort=2&oid=" + aid + "&pn=1&nohot=1&ps=3"));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                CommentModel model = new CommentModel();
                model = JsonConvert.DeserializeObject<CommentModel>(results);
                CommentModel model3 = new CommentModel();
                model3 = JsonConvert.DeserializeObject<CommentModel>(model.data.ToString());
                //Video_Grid_Info.DataContext = model;
                List<CommentModel> ban = JsonConvert.DeserializeObject<List<CommentModel>>(model3.replies.ToString());
                ListView_Comment.Items.Clear();
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
                        ctime = item.ctime,
                        mid=model1.mid,
                        like = item.like,
                        rcount = item.rcount,
                        rpid = item.rpid
                    };
                    ListView_Comment.Items.Add(resultsModel);
                }
                grid_Load.Visibility = Visibility.Collapsed;
                grid_Conent.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog("Error for GetVideoComment\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
                await md.ShowAsync();
                //throw;
            }
        }
        private async void GetVideoComment_New(string aid)
        {
            try
            {
                //grid_Load.Visibility = Visibility.Visible;
                //grid_Conent.Visibility = Visibility.Collapsed;
                btn_Load_More_New.IsEnabled = false;
                btn_Load_More_New.Content = "正在加载...";
                HttpClient hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/x/reply?jsonp=jsonp&type=1&sort=0&oid=" + aid + "&pn="+ pageNum_New + "&nohot=1&ps=20"));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                CommentModel model = new CommentModel();
                model = JsonConvert.DeserializeObject<CommentModel>(results);
                CommentModel model3 = new CommentModel();
                model3 = JsonConvert.DeserializeObject<CommentModel>(model.data.ToString());
                //Video_Grid_Info.DataContext = model;
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
                    ListView_Comment_New.Items.Add(resultsModel);
                }
                pageNum_New++;
                if (ban.Count==0)
                {
                    btn_Load_More_New.IsEnabled = false;
                    btn_Load_More_New.Content = "没有更多了...";
                }
                if (btn_Load_More.Content.ToString() != "没有更多了...")
                {
                    btn_Load_More_New.IsEnabled = true;
                    btn_Load_More_New.Content = "加载更多";
                }
                //grid_Load.Visibility = Visibility.Collapsed;
                //grid_Conent.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog("Error for GetVideoComment\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
                await md.ShowAsync();
                //throw;
            }
        }

        private async Task GetComments(string aid, string rootid)
        {
            try
            {
                HttpClient hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/x/reply/reply?oid=" + aid + "&pn=1&ps=20&root=" + rootid + "&type=1"));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                CommentModel model = new CommentModel();
                model = JsonConvert.DeserializeObject<CommentModel>(results);
                CommentModel model3 = new CommentModel();
                model3 = JsonConvert.DeserializeObject<CommentModel>(model.data.ToString());
                //Video_Grid_Info.DataContext = model;
                List<CommentModel> ban = JsonConvert.DeserializeObject<List<CommentModel>>(model3.replies.ToString());
                ListView_Flyout.Items.Clear();
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
            }
            catch (Exception)
            {
                //MessageDialog md = new MessageDialog("Error for GetComments\r\n" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
                //await md.ShowAsync();
            }

        }

        private async Task GetComments(string aid, string rootid, int num)
        {
            try
            {
                btn_Load_More.Content = "加载中....";
                btn_Load_More.IsEnabled = false;

                HttpClient hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/x/reply/reply?oid=" + aid + "&pn=" + num + "&ps=20&root=" + rootid + "&type=1"));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                CommentModel model = new CommentModel();
                model = JsonConvert.DeserializeObject<CommentModel>(results);
                CommentModel model3 = new CommentModel();
                model3 = JsonConvert.DeserializeObject<CommentModel>(model.data.ToString());
                //Video_Grid_Info.DataContext = model;
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

            }
            catch (Exception)
            {
                //throw;
                //MessageDialog md = new MessageDialog("Error for GetComments" + ex.Message, "啊！发生错误了！可恶‘(*>﹏<*)′");
                //await md.ShowAsync();
            }

        }

        private async void ListView_Comment_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplitView_Comment.IsPaneOpen = true;
            
            ListView_Flyout.Items.Clear();
            Pagenum = 1;
            if (((CommentModel)e.ClickedItem).rcount != "0")
            {
                Comment_loading.Visibility = Visibility.Visible;
                rootsid = ((CommentModel)e.ClickedItem).rpid;
                await GetComments(aid, ((CommentModel)e.ClickedItem).rpid);
                Comment_loading.Visibility = Visibility.Collapsed;
            }

        }

        private void btn_Load_More_Click(object sender, RoutedEventArgs e)
        {
           // Pagenum++;
            //GetComments(aid, rootsid, Pagenum);
        }


        private void btn_Load_More_New_Click(object sender, RoutedEventArgs e)
        {
            GetVideoComment_New(aid);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            text_Comment.Text += ((Button)sender).Content.ToString();
        }

        private async void btn_SendComment_Click(object sender, RoutedEventArgs e)
        {
            GetLoginInfoClass getUser = new GetLoginInfoClass();
            if (getUser.IsLogin())
            {
                try
                {
                    Uri ReUri = new Uri("http://api.bilibili.com/x/reply/add");
                    HttpClient hc = new HttpClient();
                    hc.DefaultRequestHeaders.Referer = new Uri("http://www.bilibili.com/");
                    string QuStr = "jsonp=jsonp&message="+text_Comment.Text+"&type=1&oid="+aid;
                    var response = await hc.PostAsync(ReUri, new HttpStringContent(QuStr, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/x-www-form-urlencoded"));
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(result);
                    if ((int)json["code"]==0)
                    {
                        grid_GG.Visibility = Visibility.Visible;
                        txt_GG.Text = "评论成功！";
                        await Task.Delay(2000);
                        grid_GG.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        grid_GG.Visibility = Visibility.Visible;
                        txt_GG.Text = "评论失败！";
                        await Task.Delay(2000);
                        grid_GG.Visibility = Visibility.Collapsed;
                    }

                }
                catch (Exception ex)
                {
                    MessageDialog md = new MessageDialog(ex.Message,"评论时发生错误");
                    await md.ShowAsync();
                }
            }
            else
            {
                MessageDialog md = new MessageDialog("你造吗，你没有登录 (～￣▽￣)，先登录好伐~");
                await md.ShowAsync();
            }
        }
        bool More = true;
        private void sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (sv.VerticalOffset == sv.ScrollableHeight)
            {
                if (More)
                {
                    More = false;
                    GetVideoComment_New(aid);
                    More = true;
                }
            }
            //GetVideoComment_New(aid);
        }

        private  void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            //(((sender as HyperlinkButton).Parent as Grid).Parent as ListViewItem)as CommentModel
            CommentModel model=(sender as HyperlinkButton).DataContext as CommentModel;
            this.Frame.Navigate(typeof(UserInfoPage), model.mid);
            
        }

        private async void btn_Zan_Click(object sender, RoutedEventArgs e)
        {
            string rpid = ((sender as HyperlinkButton).DataContext as CommentModel).rpid;
            GetLoginInfoClass getUser = new GetLoginInfoClass();
            if (getUser.IsLogin())
            {
                try
                {
                    Uri ReUri= new Uri("http://api.bilibili.com/x/reply/action");
                    
                    HttpClient hc = new HttpClient();
                    hc.DefaultRequestHeaders.Referer = new Uri("http://www.bilibili.com/");
                    string sendString = "";
                    if (((sender as HyperlinkButton).Content as TextBlock).Text == "赞同")
                    {
                        sendString = "jsonp=jsonp&oid="+ aid + "&type=1&rpid="+ rpid + "&action=1";
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

        bool CanLoad = true;
        private async  void sv1_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (sv1.VerticalOffset == sv1.ScrollableHeight)
            {
                if (CanLoad)
                {
                    CanLoad = false;
                    Pagenum++;
                    await GetComments(aid, rootsid, Pagenum);
                    CanLoad = true;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            text_Comment1.Text += ((Button)sender).Content.ToString();
        }


        string root = "";
      
        private async void btn_SendComment1_Click(object sender, RoutedEventArgs e)
        {
            if (text_Comment1.Text.Length==0)
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
                    HttpClient hc = new HttpClient();
                    hc.DefaultRequestHeaders.Referer = new Uri("http://www.bilibili.com/");
                    if (root=="")
                    {
                        root = rootsid;
                    }
                    string QuStr = "jsonp=jsonp&message=" + text_Comment1.Text + "&parent="+ rootsid + "&root="+root+"&type=1&plat=1&oid=" + aid;
                    var response = await hc.PostAsync(ReUri, new HttpStringContent(QuStr, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/x-www-form-urlencoded"));
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();
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

        private void ListView_Flyout_ItemClick(object sender, ItemClickEventArgs e)
        {
            root = (e.ClickedItem as CommentModel).rpid;
            text_Comment1.Text = "回复 @" + (e.ClickedItem as CommentModel).uname+"：";
        }

        private void text_Comment1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (text_Comment1.Text=="")
            {
                root = "";
            }
        }
    }
}

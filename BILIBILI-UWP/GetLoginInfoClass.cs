using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace BILIBILI_UWP
{
    class GetLoginInfoClass
    {
        public static string Uid = "";
        public static List<string> AttentionList = new List<string>();
        HttpClient hc;
        /// <summary>
        /// 用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<GetLoginInfoModel> GetUserInfo()
        {
            if (IsLogin())
            {
                try
                {
                    using (hc = new HttpClient())
                    {
                        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/userinfo?mid=" + Uid));
                        hr.EnsureSuccessStatusCode();
                        string results = await hr.Content.ReadAsStringAsync();
                        GetLoginInfoModel model = new GetLoginInfoModel();
                        model = JsonConvert.DeserializeObject<GetLoginInfoModel>(results);
                        JObject json = JObject.Parse(model.level_info.ToString());
                        model.current_level = "LV" + json["current_level"].ToString();
                        return model;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public async Task<GetLoginInfoModel> GetUserInfo(string uid)
        {
            try
            {
                using (hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/userinfo?mid=" + uid));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    GetLoginInfoModel model = new GetLoginInfoModel();
                    model = JsonConvert.DeserializeObject<GetLoginInfoModel>(results);
                    JObject json = JObject.Parse(model.level_info.ToString());
                    model.current_level = "LV" + json["current_level"].ToString();
                    return model;
                }


            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 追番
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetUserBangumi>> GetUserBangumi()
        {
            if (IsLogin())
            {
                try
                {
                    using (hc = new HttpClient())
                    {
                        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://space.bilibili.com/ajax/Bangumi/getList?mid=" + Uid + "&pagesize=8"));
                        hr.EnsureSuccessStatusCode();
                        string results = await hr.Content.ReadAsStringAsync();
                        //一层
                        GetUserBangumi model1 = JsonConvert.DeserializeObject<GetUserBangumi>(results);
                        if (model1.status)
                        {
                            //二层
                            GetUserBangumi model2 = JsonConvert.DeserializeObject<GetUserBangumi>(model1.data.ToString());
                            //三层
                            List<GetUserBangumi> lsModel = JsonConvert.DeserializeObject<List<GetUserBangumi>>(model2.result.ToString());
                            return lsModel;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public async Task<List<GetUserBangumi>> GetUserBangumi(string uid)
        {
            try
            {
                hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://space.bilibili.com/ajax/Bangumi/getList?mid=" + uid + "&pagesize=9999"));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                //一层
                GetUserBangumi model1 = JsonConvert.DeserializeObject<GetUserBangumi>(results);
                if (model1.status)
                {
                    //二层
                    GetUserBangumi model2 = JsonConvert.DeserializeObject<GetUserBangumi>(model1.data.ToString());
                    //三层
                    List<GetUserBangumi> lsModel = JsonConvert.DeserializeObject<List<GetUserBangumi>>(model2.result.ToString());
                    return lsModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 收藏夹
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetUserFovBox>> GetUserFovBox()
        {
            if (IsLogin())
            {
                try
                {
                    using (hc = new HttpClient())
                    {
                        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://space.bilibili.com/ajax/fav/getBoxList?mid=" + Uid));
                        hr.EnsureSuccessStatusCode();
                        string results = await hr.Content.ReadAsStringAsync();
                        //一层
                        GetUserFovBox model1 = JsonConvert.DeserializeObject<GetUserFovBox>(results);
                        if (model1.status)
                        {
                            //二层
                            GetUserFovBox model2 = JsonConvert.DeserializeObject<GetUserFovBox>(model1.data.ToString());
                            //三层
                            List<GetUserFovBox> lsModel = JsonConvert.DeserializeObject<List<GetUserFovBox>>(model2.list.ToString());
                            return lsModel;
                        }
                        else
                        {
                            return null;
                        }
                    }

                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 收藏夹下视频
        /// </summary>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public async Task<List<GetFavouriteBoxsVideoModel>> GetFavouriteBoxVideo(string fid, int PageNum)
        {
            //啊啊啊啊，没心情啊，下面代码都是乱写的，啊啊啊啊啊 啊啊啊啊啊啊
            if (IsLogin())
            {
                try
                {
                    using (hc = new HttpClient())
                    {
                        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://space.bilibili.com/ajax/fav/getList?mid=" + Uid + "&pagesize=20&fid=" + fid + "&pid=" + PageNum));
                        hr.EnsureSuccessStatusCode();
                        string results = await hr.Content.ReadAsStringAsync();
                        //一层
                        GetFavouriteBoxsVideoModel model = JsonConvert.DeserializeObject<GetFavouriteBoxsVideoModel>(results);
                        //二层
                        if (model.status)
                        {
                            GetFavouriteBoxsVideoModel model2 = JsonConvert.DeserializeObject<GetFavouriteBoxsVideoModel>(model.data.ToString());
                            //三层
                            List<GetFavouriteBoxsVideoModel> lsModel = JsonConvert.DeserializeObject<List<GetFavouriteBoxsVideoModel>>(model2.vlist.ToString());
                            List<GetFavouriteBoxsVideoModel> RelsModel = new List<GetFavouriteBoxsVideoModel>();
                            foreach (GetFavouriteBoxsVideoModel item in lsModel)
                            {
                                item.pages = model2.pages;
                                RelsModel.Add(item);
                            }
                            return RelsModel;
                        }
                        else
                        {
                            return null;
                        }
                    }
                
            }
                catch (Exception)
            {
                return null;
            }
        }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 收藏专题
        /// </summary>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public async Task<List<FavSpModel>> GetFavouriteSp(int PageNum)
        {
            if (IsLogin())
            {
                try
                {
                    using (hc = new HttpClient())
                    {
                        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/sp/list?page="+PageNum+"&pagesize=20"));
                        hr.EnsureSuccessStatusCode();
                        string results = await hr.Content.ReadAsStringAsync();
                        //一层
                        FavSpModel model = JsonConvert.DeserializeObject<FavSpModel>(results);
                        //二层
                            List<FavSpModel> model2 = JsonConvert.DeserializeObject< List<FavSpModel>>(model.list.ToString());
                            return model2;
                    }

                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 关注的人
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetUserAttention>> GetUserAttention()
    {
        if (IsLogin())
        {
            try
            {
                    using (hc = new HttpClient())
                    {
                        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://space.bilibili.com/ajax/friend/GetAttentionList?mid=" + Uid + "&pagesize=100"));
                        hr.EnsureSuccessStatusCode();
                        string results = await hr.Content.ReadAsStringAsync();
                        //一层
                        GetUserFovBox model1 = JsonConvert.DeserializeObject<GetUserFovBox>(results);
                        if (model1.status)
                        {
                            //二层
                            GetUserAttention model2 = JsonConvert.DeserializeObject<GetUserAttention>(model1.data.ToString());
                            //三层
                            List<GetUserAttention> lsModel = JsonConvert.DeserializeObject<List<GetUserAttention>>(model2.list.ToString());
                            AttentionList.Clear();
                            foreach (GetUserAttention item in lsModel)
                            {
                                AttentionList.Add(item.fid);
                            }
                            return lsModel;
                        }
                        else
                        {
                            return null;
                        }
                    }
            }
            catch (Exception)
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 投稿
    /// </summary>
    /// <returns></returns>
    public async Task<List<GetUserSubmit>> GetUserSubmit()
    {
        if (IsLogin())
        {
            try
            {
                    using (hc = new HttpClient())
                    {
                        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://space.bilibili.com/ajax/member/getSubmitVideos?mid=" + Uid + "&pagesize=6"));
                        hr.EnsureSuccessStatusCode();
                        string results = await hr.Content.ReadAsStringAsync();
                        //一层
                        GetUserSubmit model1 = JsonConvert.DeserializeObject<GetUserSubmit>(results);
                        if (model1.status)
                        {
                            //二层
                            GetUserSubmit model2 = JsonConvert.DeserializeObject<GetUserSubmit>(model1.data.ToString());
                            //三层
                            List<GetUserSubmit> lsModel = JsonConvert.DeserializeObject<List<GetUserSubmit>>(model2.vlist.ToString());
                            return lsModel;
                        }
                        else
                        {
                            return null;
                        }
                    }
                
            }
            catch (Exception)
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
    public async Task<List<GetUserSubmit>> GetUserSubmit(string uid)
    {
        try
        {
                using (hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(new Uri("http://space.bilibili.com/ajax/member/getSubmitVideos?mid=" + uid + "&pagesize=6"));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    //一层
                    GetUserSubmit model1 = JsonConvert.DeserializeObject<GetUserSubmit>(results);
                    if (model1.status)
                    {
                        //二层
                        GetUserSubmit model2 = JsonConvert.DeserializeObject<GetUserSubmit>(model1.data.ToString());
                        //三层
                        List<GetUserSubmit> lsModel = JsonConvert.DeserializeObject<List<GetUserSubmit>>(model2.vlist.ToString());
                        return lsModel;
                    }
                    else
                    {
                        return null;
                    }
                }
        }
        catch (Exception)
        {
            return null;
        }
    }
    /// <summary>
    /// 关注动态
    /// </summary>
    /// <returns></returns>
    public async Task<List<GetAttentionUpdate>> GetUserAttentionUpdate(int PageNum)
    {
        if (IsLogin())
        {
            try
            {
                    using (hc = new HttpClient())
                    {
                        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/x/feed/pull?jsonp=jsonp&ps=20&type=1&pn=" + PageNum));
                        hr.EnsureSuccessStatusCode();
                        string results = await hr.Content.ReadAsStringAsync();
                        //一层
                        GetAttentionUpdate model1 = JsonConvert.DeserializeObject<GetAttentionUpdate>(results);
                        if (model1.code == 0)
                        {
                            //二层
                            GetAttentionUpdate model2 = JsonConvert.DeserializeObject<GetAttentionUpdate>(model1.data.ToString());
                            //三层
                            List<GetAttentionUpdate> ls = JsonConvert.DeserializeObject<List<GetAttentionUpdate>>(model2.feeds.ToString());
                            //四层
                            List<GetAttentionUpdate> lsModel = new List<GetAttentionUpdate>();
                            foreach (GetAttentionUpdate item in ls)
                            {
                                GetAttentionUpdate m = JsonConvert.DeserializeObject<GetAttentionUpdate>(item.addition.ToString());
                                m.page = model2.page;
                                lsModel.Add(m);
                            }
                            return lsModel;
                        }
                        else
                        {
                            return null;
                        }
                    }
               
            }
            catch (Exception)
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 收藏的视频
    /// </summary>
    /// <returns></returns>
    public async Task<List<GetFavouriteVideo>> GetFavouriteVideo(int PageNum)
    {
        if (IsLogin())
        {
            try
            {
                    using (hc = new HttpClient())
                    {
                        HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/favourite?type=json&pagesize=20&page=" + PageNum));
                        hr.EnsureSuccessStatusCode();
                        string results = await hr.Content.ReadAsStringAsync();
                        //一层
                        JObject json = JObject.Parse(results);
                        List<GetFavouriteVideo> lsModel = new List<GetFavouriteVideo>();
                        for (int i = 0; i < json.Count - 2; i++)
                        {
                            string tmd = i.ToString();
                            lsModel.Add(JsonConvert.DeserializeObject<GetFavouriteVideo>(json[tmd].ToString()));
                        }
                        return lsModel;
                    }
              
            }
            catch (Exception)
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 观看历史
    /// </summary>
    /// <returns></returns>
    public async Task<List<GetHistoryModel>> GetHistory(int PageNum)
    {
        if (IsLogin())
        {
            try
            {
                hc = new HttpClient();
                HttpResponseMessage hr = await hc.GetAsync(new Uri("http://api.bilibili.com/x/history?jsonp=jsonp&ps=20&pn=" + PageNum));
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                //一层
                GetHistoryModel model = JsonConvert.DeserializeObject<GetHistoryModel>(results);
                if (model.data == null)
                {
                    return null;
                }
                else
                {
                    List<GetHistoryModel> lsModel = JsonConvert.DeserializeObject<List<GetHistoryModel>>(model.data.ToString());
                    return lsModel;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
    //是否登录
    public bool IsLogin()
    {
        HttpBaseProtocolFilter hb = new HttpBaseProtocolFilter();
        HttpCookieCollection cookieCollection = hb.CookieManager.GetCookies(new Uri("http://bilibili.com/"));
        List<string> ls = new List<string>();
        foreach (HttpCookie item in cookieCollection)
        {
            ls.Add(item.Name);
        }
        if (!ls.Contains("DedeUserID") || !ls.Contains("DedeUserID__ckMd5"))
        {
            return false;
        }
        else
        {
            hb.CookieManager.GetCookies(new Uri("http://bilibili.com/"));
            foreach (HttpCookie item in cookieCollection)
            {
                if (item.Name == "DedeUserID")
                {
                    Uid = item.Value;
                }
            }
            return true;

        }
    }
}



class GetLoginInfoModel
{
    public string mid { get; set; }//ID
    public string name { get; set; }//昵称
    public string sex { get; set; }//性别
    public string coins { get; set; }//硬币
    public string face { get; set; }//头像
    public string birthday { get; set; }//生日
    public long regtime { get; set; }//注册时间
    public string Regtime
    {
        get
        {
            DateTime dtStart = new DateTime(1970, 1, 1);
            long lTime = long.Parse(regtime + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow).ToString();
        }
    }//转换后注册时间
    public string sign { get; set; }//个性签名
    public int fans { get; set; }//粉丝
    public string attention { get; set; }//关注
    public object level_info { get; set; }//等级信息
    public string current_level { get; set; }//等级
    public string place { get; set; }//地址
}
class GetUserBangumi
{
    //Josn：http://space.bilibili.com/ajax/Bangumi/getList?mid=XXX&pagesize=9999
    //第一层
    public bool status { get; set; }//状态
    public object data { get; set; }//数据，包含第二层
                                    //第二层
    public int count { get; set; }//总数量
    public object result { get; set; }//结果，包含第三层
                                      //第三层
    public string season_id { get; set; }//专题ID，重要！！！
    public string title { get; set; }//标题
    public int is_finish { get; set; }//是否完结，0为连载，1为完结
    public string favorites { get; set; }//有多少人关注
    public int newest_ep_index { get; set; }//最新话
    public int total_count { get; set; }//一共多少话
    public string NewOver
    {
        get
        {
            if (is_finish == 0)
            {
                return "更新至第" + newest_ep_index + "话";
            }
            else
            {
                return total_count + "话全";
            }
        }
    }
    public string cover { get; set; }//封面
    public string brief { get; set; }//简介
}
class GetUserFovBox
{
    //Josn：http://space.bilibili.com/ajax/fav/getBoxList?mid=XXXXX
    //第一层
    public bool status { get; set; }//状态
    public object data { get; set; }//数据，包含第二层
                                    //第二层
    public object list { get; set; }//结果，包含第三层
                                    //第三层
    public string fav_box { get; set; }//收藏夹ID，重要！！！
    public int count { get; set; }//数量
    public string Count
    {
        get
        {
            return count + "个视频";
        }
    }
    public string name { get; set; }//标题
    public long ctime { get; set; }//未转换创建时间
    public int max_count { get; set; }//最大数量

}
class GetUserAttention
{
    //Josn：http://space.bilibili.com/ajax/friend/GetAttentionList?mid=XXXX&pagesize=999
    //第一层
    public bool status { get; set; }//状态
    public object data { get; set; }//数据，包含第二层
                                    //第二层
    public object list { get; set; }//结果，包含第三层
                                    //第三层
    public string record_id { get; set; }//记录ID，重要！！！
    public string uname { get; set; }//昵称
    public string face { get; set; }//头像
    public string fid { get; set; }//FID
    public long addtime { get; set; }//记录时间

}
    class GetUserSubmit
    {
        //Josn：http://space.bilibili.com/ajax/friend/GetAttentionList?mid=XXXX&pagesize=999
        //第一层
        public bool status { get; set; }//状态
        public object data { get; set; }//数据，包含第二层
                                        //第二层
        public object vlist { get; set; }//结果，包含第三层
                                         //第三层
        public string aid { get; set; }//视频ID
        public string title { get; set; }//标题
        public string pic { get; set; }//图片
        public string video_review { get; set; }//弹幕
        public string play { get; set; }//播放
        public string created { get; set; }//上传时间
        public string length { get; set; }//长度
        public string description{get;set ;}
    public int count { get; set; }
    public int pages { get; set; }
}
class GetAttentionUpdate
{
    //必须有登录Cookie
    //Josn：http://api.bilibili.com/x/feed/pull?jsonp=jsonp&ps=20&type=1&pn=1
    //第一层
    public int code { get; set; }//状态，0为正常
    public object data { get; set; }//数据，包含第二层
                                    //第二层
    public object feeds { get; set; }//结果，包含第三层
    public object page { get; set; }//结果数量，包含第三层
                                    //第三层
    public string add_id { get; set; }//视频ID
    public object source { get; set; }//作者信息，包含第四层
    public object addition { get; set; }//视频信息，包含第四层
                                        //第四层
    public string author { get; set; }//上传人员
    public string mid { get; set; }//上传人员ID
    public string aid { get; set; }//视频ID
    public string title { get; set; }//标题
    public string play { get; set; }//播放数
    public string video_review { get; set; }//弹幕数
    public string create { get; set; }//上传时间
    public string pic { get; set; }//封面
}
class GetFavouriteVideo
{
    //必须有登录Cookie
    //Josn：http://api.bilibili.com/favourite?type=json&pagesize=20&page=1
    //第一层
    public string title { get; set; }//标题
    public string type { get; set; }//类型
    public string author { get; set; }//上传人员
    public string cover { get; set; }//封面
    public string aid { get; set; }//视频ID
    public object results { get; set; }//结果，没有记录时才会出现
    public object code { get; set; }//出错才会出现
    public string totalResult { get; set; }
}
class GetFavouriteBoxsVideoModel
{
    //Josn：http://space.bilibili.com/ajax/fav/getList?mid=用户ID&pagesize=30&fid=收藏夹编号
    //第一层
    public bool status { get; set; }//标题
    public object data { get; set; }//包含第二层
                                    //第二层
    public int pages { get; set; }//页数
    public int count { get; set; }//数量
    public object vlist { get; set; }//包含第三层
                                     //第三层
    public string aid { get; set; }//AID
    public string typename { get; set; }//类型
    public string title { get; set; }//标题
    public string author { get; set; }//作者
    public string pic { get; set; }//封面
    public string fav_create_at { get; set; }
}
class GetHistoryModel
{
    //必须有登录Cookie
    //Josn：http://api.bilibili.com/x/history?jsonp=jsonp&ps=20&pn=1
    public int code { get; set; }
    public object data { get; set; }
    public string aid { get; set; }
    public string cover { get; set; }
    public string pic { get; set; }
    public string title { get; set; }
    public string view_at { get; set; }
    public string typename { get; set; }
}
class FavSpModel
    {
        public int pages { get; set; }
        public object list { get; set; }
        public string title { get; set; }
        public string cover { get; set; }
        public string create_at { get; set; }
        public string spid { get; set; }
    }
}

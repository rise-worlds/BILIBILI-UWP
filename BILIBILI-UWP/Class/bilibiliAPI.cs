using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILIBILI_UWP
{
    class bilibiliAPI
    {
        /*
            所有Get方法的API均返回JOSN；
            视频AV号前不加AV；
            未加{}说明参数即为未知参数，默认即可
       */
        /// <summary>
        /// BannerAPI
        /// </summary>
        public static string HomeBanner = "http://www.bilibili.com/index/slideshow.json";
        /// <summary>
        /// 各分区推荐信息，{0}填充分区编号，{1}填充显示数量，{2}填充排序，可选hot,new,default
        /// </summary>
        public static string HomePartitionInfo = "http://api.bilibili.com/list?type=json&appkey=422fd9d7289a1dd9&tid={0}&page=1&pagesize={1}&order={2}&ver=2";
        /// <summary>
        /// 视频信息，{0}填充视频Aid
        /// </summary>
        public static string VideoInfo = "http://api.bilibili.com/view?type=json&appkey=422fd9d7289a1dd9&id={0}&batch=1";
        /// <summary>
        /// 视频评论，{0}按什么顺序排序0是默认，1是按赞同，2是按回复；{1}填充AV号；{2}填充页数；{3}填充每页显示数量
        /// </summary>
        public static string VideoComment = "http://api.bilibili.com/x/reply?jsonp=jsonp&type=1&sort={0}&oid={1}&pn={2}&nohot=1&ps={3}";
        /// <summary>
        /// 评论下回复，{0}视频AV号，{1}页数，{2}每页显示数量，{3}视频评论ID，RootID
        /// </summary>
        public static string Comments = "http://api.bilibili.com/x/reply/reply?oid={0}&pn={1}&ps={2}&root={3}&type=1";
        /// <summary>
        /// 收藏夹信息，需要登录
        /// </summary>
        public static string FavouriteBox= "http://api.bilibili.com/x/favourite/folder?jsonp=jsonp";
        /// <summary>
        /// 收藏POST
        /// 参数jsonp=jsonp&fid={收藏夹ID}&aid={视频AV号}
        /// </summary>
        public static string AddFav = "http://api.bilibili.com/x/favourite/video/add";
        /// <summary>
        /// 投币POST
        /// 参数aid={视频AV号}&rating=100&player=1&multiply={投币数量，1-2}
        /// </summary>
        public static string Toubi = "http://www.bilibili.com/plus/comment.php";
        /// <summary>
        /// 视频地址，{0}视频CID，可从视频信息中获取；{1}清晰度，1-3，数字越高越清晰，{2}视频类型，可选flv、mp4
        /// </summary>
        public static string VideoUrl = "http://interface.bilibili.com/playurl?cid={0}&quality={1}&otype=json&appkey=422fd9d7289a1dd9&type={2}";
        /// <summary>
        /// 排行榜，{0}0为非原创，1为原创，{2}分区ID，0为全站
        /// </summary>
        public static string Ranking = "http://api.bilibili.cn/list?appkey=422fd9d7289a1dd9&order=hot&original={0}&page=1&pagesize=20&tid={1}";
        /// <summary>
        /// 番剧推荐，返回很多....
        /// </summary>
        public static string BangumiInfo = "http://app.bilibili.com/promo/android3/2620/bangumi.android3.xhdpi.json";
        /// <summary>
        /// 番剧更新时间表
        /// </summary>
        public static string BangumiTimeLine = "http://www.bilibili.com/api_proxy?app=bangumi&action=timeline_v2";
        /// <summary>
        /// 番剧最近更新，推荐番剧分类
        /// </summary>
        public static string BangumiRecommend = "http://bangumi.bilibili.com/api/app_index_page";
        /// <summary>
        /// 用户信息,{0}用户编号
        /// </summary>
        public static string UserInfo = "http://api.bilibili.com/userinfo?mid={0}";
        /// <summary>
        /// 用户追番，{0}用户编号
        /// </summary>
        public static string UserBangumi = "http://space.bilibili.com/ajax/Bangumi/getList?mid={0}&pagesize=9999";
        /// <summary>
        /// 用户收藏，{0}用户编号
        /// </summary>
        public static string UserFavourite= "http://space.bilibili.com/ajax/fav/getBoxList?mid={0}";
        /// <summary>
        /// 用户收藏夹下视频,{0}用户编号,{1}收藏夹编号，{2}页数
        /// </summary>
        public static string UserFavouritesVideo = "http://space.bilibili.com/ajax/fav/getList?mid={0}&pagesize=20&fid={1}&pid={2}";
        /// <summary>
        /// 用户专题收藏，{0}页数
        /// </summary>
        public static string UserSeason = "http://api.bilibili.com/sp/list?page={0}&pagesize=20";
        /// <summary>
        /// 关注的人，{0}用户编号，{1}页数，{2}每页显示数量
        /// </summary>
        public static string UserAttention = "http://space.bilibili.com/ajax/friend/GetAttentionList?mid={0}&pagenum={1}&pagesize={2}";
        /// <summary>
        /// 投稿，{0}用户编号，{1}页数，{2}每页显示数量
        /// </summary>
        public static string UserSubmit = "http://space.bilibili.com/ajax/member/getSubmitVideos?mid={0}&pagenum={1}&pagesize={2}";
        /// <summary>
        /// 关注的动态，{0}页数，需要登录
        /// </summary>
        public static string UserAttentionUpdate = "http://api.bilibili.com/x/feed/pull?jsonp=jsonp&ps=20&type=1&pn={0}";
        /// <summary>
        /// 收藏的视频，{0}页数，需要登录
        /// </summary>
        public static string UserFavouriteVideo = "http://api.bilibili.com/favourite?type=json&pagesize=20&page={0}";
        /// <summary>
        /// 观看历史，{0}页数，需要登录
        /// </summary>
        public static string UserHistory = "http://api.bilibili.com/x/history?jsonp=jsonp&ps=20&pn={0}";
        /// <summary>
        /// 搜索，{0}关键字，{1}页数，{2}类型，可选video，upuser，series，special
        /// </summary>
        public static string SearchVideo = "http://www.bilibili.com/search?action=autolist&pagesize=20&keyword={0}&page={1}&type={2}";
        /// <summary>
        /// 登录读取ACCESSKEY，{0}密码，{1}用户名
        /// </summary>
        public static string LoginGetAccessKey="https://api.bilibili.com/login?appkey=422fd9d7289a1dd9&platform=wp&pwd={0}&type=json&userid={1}";
        /// <summary>
        /// 登录，用GET方法 - -，不是POST，{0}ACCESSKEY
        /// </summary>
        public static string Login= "http://api.bilibili.com/login/sso?&access_key={0}&appkey=422fd9d7289a1dd9&platform=wp";
        
        /// <summary>
        /// 专题信息，{0}填充标题或SPID
        /// </summary>
        public static string SeasonByID = "http://api.bilibili.com/sp?spid={0}";
        public static string SeasonByTitle = "http://api.bilibili.com/sp?title={0}";
    }
}

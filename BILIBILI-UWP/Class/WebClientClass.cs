using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace BILIBILI_UWP
{
    class WebClientClass
    {
        public async Task<string> GetResults(Uri url)
        {
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    HttpResponseMessage hr = await hc.GetAsync(url);
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();
                    return results;
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public async Task<string> PostResults(Uri url, string PostContent)
        {
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    hc.DefaultRequestHeaders.Referer = new Uri("http://www.bilibili.com/");
                    var response = await hc.PostAsync(url, new HttpStringContent(PostContent, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/x-www-form-urlencoded"));
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

    }
}

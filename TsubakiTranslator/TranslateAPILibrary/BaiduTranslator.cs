
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Web;

namespace TsubakiTranslator.TranslateAPILibrary
{
    public class BaiduTranslator : ITranslator
    {
        //语言简写列表 https://api.fanyi.baidu.com/product/113

        private string appId;//百度翻译API 的APP ID
        private string secretKey;//百度翻译API 的密钥

        private readonly string name = "百度";
        public string Name { get =>name; }
        
        
        public string Translate(string sourceText)
        {

            string desLang = "zh";
            string srcLang = "auto";

            string retString;

            string salt = new Random().Next(100000).ToString();

            string sign = CommonFunction.EncryptString(appId + sourceText + salt + secretKey);


            string bodyString = $"q={HttpUtility.UrlEncode(sourceText)}&from={srcLang}&to={desLang}&appid={appId}&salt={salt}&sign={sign}";

            HttpContent content = new StringContent(bodyString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

            //var sb = new StringBuilder("https://api.fanyi.baidu.com/api/trans/vip/translate?")
            //    .Append("q=").Append(HttpUtility.UrlEncode(q))
            //    .Append("&from=").Append(srcLang)
            //    .Append("&to=").Append(desLang)
            //    .Append("&appid=").Append(appId)
            //    .Append("&salt=").Append(salt)
            //    .Append("&sign=").Append(sign);
            //string url = sb.ToString();

            string url = @"https://api.fanyi.baidu.com/api/trans/vip/translate";

            var client = CommonFunction.Client;
            try
            {
                HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();//用来抛异常的
                retString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                //retString = hc.GetStringAsync(url).GetAwaiter().GetResult();
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                return ex.Message;
            }
            catch (System.Threading.Tasks.TaskCanceledException ex)
            {
                return ex.Message;
            }

            BaiduTransOutInfo oinfo = JsonSerializer.Deserialize<BaiduTransOutInfo>(retString);

            if (oinfo.trans_result == null)
            {
                //得到翻译结果
                return oinfo.trans_result[0].dst;
                
            }
            else
            {
                BaiduErrorMessage errorinfo = JsonSerializer.Deserialize<BaiduErrorMessage>(retString);
                return $"{errorinfo.error_code}: {errorinfo.error_msg}";
            }
            
        }

        public void TranslatorInit(string param1, string param2)
        {
            appId = param1;
            secretKey = param2;
        }

#pragma warning disable 0649
#pragma warning disable 0169
        class BaiduTransOutInfo
        {
            string from;
            string to;
            public List<BaiduTransResult> trans_result;
            public string error_code;
        }

        class BaiduTransResult
        {
            string src;
            public string dst;
        }

        class BaiduErrorMessage
        {
            public int error_code;
            public string error_msg;
        }
#pragma warning restore 0649
#pragma warning restore 0169



    }


}

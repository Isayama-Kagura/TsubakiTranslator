
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
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

        public string SourceLanguage { get; set; }

        public string Translate(string sourceText)
        {

            string desLang = "zh";

            string retString;

            string salt = new Random().Next(100000).ToString();

            string sign = CommonFunction.EncryptString(appId + sourceText + salt + secretKey);


            string bodyString = $"q={HttpUtility.UrlEncode(sourceText)}&from={SourceLanguage}&to={desLang}&appid={appId}&salt={salt}&sign={sign}";

            HttpContent content = new StringContent(bodyString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

            //var sb = new StringBuilder("https://api.fanyi.baidu.com/api/trans/vip/translate?")
            //    .Append("q=").Append(HttpUtility.UrlEncode(q))
            //    .Append("&from=").Append(SourceLanguage)
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

            retString = Regex.Unescape(retString);
            Regex reg = new Regex(@""",""dst"":""(.*?)""\}");
            Match match = reg.Match(retString);

            string result = match.Groups[1].Value;
            result = Regex.Unescape(result);
            return result;


           
            
        }

        public void TranslatorInit(string param1, string param2)
        {
            appId = param1;
            secretKey = param2;
        }





    }


}

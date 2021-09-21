
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace TsubakiTranslator.TranslateAPILibrary
{
    //API文档 https://cloud.tencent.com/document/product/551/15614
    public class TencentTranslator : ITranslator
    {

        private string SecretId;//腾讯翻译API SecretId
        private string SecretKey;//腾讯翻译API SecretKey

        private readonly string name = "腾讯";
        public string Name { get => name; }

        public async Task<string> Translate(string sourceText )
        {
            string desLang = "zh";
            string srcLang = "auto";
            string retString;

            string salt = new Random().Next(100000).ToString();

            string url = "https://tmt.tencentcloudapi.com/?";

            var sb = new StringBuilder()
                .Append("Action=TextTranslate")
                .Append("&Nonce=").Append(salt)
                .Append("&ProjectId=0")
                .Append("&SecretId=").Append(SecretId)
                .Append("&Source=").Append(srcLang)
                .Append("&SourceText=").Append(sourceText)
                .Append("&Target=").Append(desLang)
                .Append("&Timestamp=").Append(CommonFunction.GetTimeStamp())
                .Append("&Version=2018-03-21");
            string req = sb.ToString();

            HMACSHA1 hmac = new HMACSHA1()
            {
                Key = Encoding.UTF8.GetBytes(SecretKey)
            };

            byte[] data = Encoding.UTF8.GetBytes("GETtmt.tencentcloudapi.com/?" + req);
            var result = hmac.ComputeHash(data);
            //req = req + "&Signature=" + HttpUtility.UrlEncode(Convert.ToBase64String(result));
            req = req + "&Signature=" + Convert.ToBase64String(result);

            HttpContent content = new StringContent(req);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var client = CommonFunction.Client;


            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);//改成自己的
                response.EnsureSuccessStatusCode();//用来抛异常的
                retString = await response.Content.ReadAsStringAsync();

            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                return ex.Message;
            }
            catch (TaskCanceledException ex)
            {
                return ex.Message;
            }

            TencentOldTransOutInfo oinfo = JsonSerializer.Deserialize<TencentOldTransOutInfo>(retString);

            if (oinfo.Response.Error == null)
                //得到翻译结果
                return oinfo.Response.TargetText;
            else
                return "ErrorID:" + oinfo.Response.Error.Code + " ErrorInfo:" + oinfo.Response.Error.Message;

        }

        public void TranslatorInit(string param1, string param2)
        {
            SecretId = param1;
            SecretKey = param2;
        }

        class TencentOldTransOutInfo
        {
            public TencentOldTransResult Response { get; set; }
        }

        class TencentOldTransResult
        {
            public string RequestId { get; set; }
            public string TargetText { get; set; }
            public string Source { get; set; }
            public string Target { get; set; }
            public TencentOldTransOutError Error { get; set; }
        }

        class TencentOldTransOutError
        {
            public string Code { get; set; }
            public string Message { get; set; }
        }

    }



}

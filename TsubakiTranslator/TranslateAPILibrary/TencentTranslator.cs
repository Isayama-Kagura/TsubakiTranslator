using System;
using System.Collections.Generic;
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

        public string SourceLanguage { get; set; }

        public string Translate(string sourceText )
        {
            string desLang = "zh";

            string retString;

            string salt = new Random().Next(100000).ToString();

            string url = "https://tmt.tencentcloudapi.com/";

            string action = "TextTranslate";
            string version = "2018-03-21";

            // long timestamp = ToTimestamp() / 1000;
            // string requestTimestamp = timestamp.ToString();
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("Nonce", salt);
            param.Add("ProjectId", "0");
            param.Add("SourceText", sourceText);
            param.Add("Region", "ap-guangzhou");
            param.Add("Action", action);
            param.Add("Source", SourceLanguage);
            // param.Add("Nonce", Math.Abs(new Random().Next()).ToString());
            param.Add("Timestamp", CommonFunction.GetTimeStamp());
            param.Add("Version", version);
            param.Add("Target", desLang);
            param.Add("SecretId", SecretId);

            SortedDictionary<string, string> headers = new SortedDictionary<string, string>(param, StringComparer.Ordinal);




            //string sigInParam = MakeSignPlainText(headers, "GET", endpoint, "/");
            string retStr = "POSTtmt.tencentcloudapi.com/?";
            string body = "";
            foreach (string key in headers.Keys)
            {
                body += string.Format("{0}={1}&", key, headers[key]);
            }
            body = body.TrimEnd('&');

            retStr += body;
            string sigInParam = retStr;

            //string sigOutParam = Sign(SecretKey, sigInParam);
            string signRet = string.Empty;
            using (HMACSHA1 mac = new HMACSHA1(Encoding.UTF8.GetBytes(SecretKey)))
            {
                byte[] hash = mac.ComputeHash(Encoding.UTF8.GetBytes(sigInParam));
                signRet = Convert.ToBase64String(hash);
            }
            string sigOutParam = signRet;

            //req = req + "&Signature=" + HttpUtility.UrlEncode(Convert.ToBase64String(result));
            body = body + "&Signature=" + HttpUtility.UrlEncode(sigOutParam);

            HttpContent content = new StringContent(body);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");


            var client = CommonFunction.Client;


            try
            {
                HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();//改成自己的
                response.EnsureSuccessStatusCode();//用来抛异常的
                retString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

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

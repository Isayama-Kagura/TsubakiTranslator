using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace TsubakiTranslator.TranslateAPILibrary
{
    public class ICiBaTranslator : ITranslator
    {
        public string SourceLanguage { get; set; }

        private readonly string name = "爱词霸";
        public string Name { get => name; }

        public string Translate(string sourceText)
        {
            string desLang = "zh";

            string sign;

            using (MD5 md5Hash = MD5.Create())
            {
                // 将输入字符串转换为字节数组并计算哈希数据  
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes("6key_web_fanyiifanyiweb8hc9s98e" + sourceText.Trim()));
                // 返回16进制字符串  
                sign = Convert.ToHexString(data).ToLower();
            }

            sign = sign.Substring(0, 16);

            string param = $"c=trans&m=fy&client=6&auth_user=key_web_fanyi&sign={sign}";

            var bodyString = $"from={SourceLanguage}&to={desLang}&q={sourceText}";

            string url = @"http://ifanyi.iciba.com/index.php" + "?" + param;

            var client = CommonFunction.Client;

            HttpContent content = new StringContent(bodyString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

            try
            {
                HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();//改成自己的
                response.EnsureSuccessStatusCode();//用来抛异常的
                string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                Regex reg = new Regex(@",""out"":""(.*?)"",""reqid""");
                Match match = reg.Match(responseBody);

                string result = match.Groups[1].Value;
                result = Regex.Unescape(result);
                return result;
                //string ret = result.payload.translations[0].translation;

                //return ret;

                //return responseBody;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {

                return ex.Message;
            }
            catch (System.Threading.Tasks.TaskCanceledException ex)
            {

                return ex.Message;
            }

        }

        public void TranslatorInit(string param1, string param2)
        {

        }
    }
}

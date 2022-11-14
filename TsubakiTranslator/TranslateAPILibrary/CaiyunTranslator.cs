using System.Net.Http;
using System.Text.RegularExpressions;

namespace TsubakiTranslator.TranslateAPILibrary
{
    //API查询文档 https://fanyi.caiyunapp.com/#/api
    /// <summary>
    /// 只能中英或者中日互译
    /// </summary>
    public class CaiyunTranslator : ITranslator
    {
        private string caiyunToken;//彩云小译 令牌

        private readonly string name = "彩云";
        public string Name { get => name; }

        public string SourceLanguage { get; set; }

        public string Translate(string sourceText)
        {
            string desLang = "zh";

            string retString;


            string url = "https://api.interpreter.caiyunai.com/v1/translator";
            //json参数
            string jsonParam = "{\"source\": [\"" + sourceText + "\"], \"trans_type\": \"" + $"{SourceLanguage}2{desLang}" + "\", \"request_id\": \"demo\", \"detect\": true}";

            var client = CommonFunction.Client;

            HttpContent content = new StringContent(jsonParam);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            content.Headers.Add("X-Authorization", "token " + caiyunToken);

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
            catch (System.Threading.Tasks.TaskCanceledException ex)
            {
                return ex.Message;
            }

            string result = Regex.Unescape(retString);
            Regex reg = new Regex(@"""target"":\[""(.*?)""\],");
            Match match = reg.Match(result);

            result = match.Groups[1].Value;

            return result;

        }

        public void TranslatorInit(string param1, string param2 = "")
        {
            caiyunToken = param1;
        }


    }


}

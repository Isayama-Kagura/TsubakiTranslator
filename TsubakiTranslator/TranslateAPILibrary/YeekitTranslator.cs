using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;

namespace TsubakiTranslator.TranslateAPILibrary
{
    public class YeekitTranslator : ITranslator
    {
        private readonly string name = "Yeekit";
        public string Name { get => name; }

        public string SourceLanguage { get; set; }

        public string Translate(string sourceText)
        {
            string desLang = "nzh";

            //var body = new
            //{
            //    srcl = SourceLanguage,
            //    tgtl = desLang,
            //    app_source = 9001,
            //    text = sourceText,
            //    domain = "auto"
            //};

            //string bodyString = JsonSerializer.Serialize(body);

            string bodyString = $"content[]={HttpUtility.UrlEncode(sourceText)}&sourceLang={SourceLanguage}&targetLang={desLang}";

            HttpContent content = new StringContent(bodyString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

            string url = @"https://www.yeekit.com/site/dotranslate";

            HttpClient client = CommonFunction.Client;

            try
            {
                HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();//改成自己的
                response.EnsureSuccessStatusCode();//用来抛异常的
                string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                responseBody = responseBody.Replace(@"\n", string.Empty).Replace(" ", string.Empty);

                responseBody = Regex.Unescape(responseBody);

                Regex reg = new Regex(@"""text"":""(.*?)"",""translatetime""");
                Match match = reg.Match(responseBody);

                string result = match.Groups[1].Value;

                return result;
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

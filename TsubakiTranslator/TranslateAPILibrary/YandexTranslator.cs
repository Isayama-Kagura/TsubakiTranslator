using System.Net.Http;
using System.Text.RegularExpressions;

namespace TsubakiTranslator.TranslateAPILibrary
{

    //API文档 https://yandex.com/dev/translate/doc/dg/concepts/api-overview.html
    public class YandexTranslator : ITranslator
    {
        public string ApiKey;

        private readonly string name = "Yandex";
        public string Name { get => name; }

        public string Translate(string sourceText)
        {
            string desLang = "zh";
            string srcLang = "ja";
            var client = CommonFunction.Client;
            string url = @"https://translate.yandex.net/api/v1.5/tr.json/translate";
            string bodyString = $"key={ ApiKey}&lang={srcLang}-{desLang }&text={srcLang}";

            HttpContent content = new StringContent(bodyString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

            try
            {
                HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();//改成自己的
                response.EnsureSuccessStatusCode();//用来抛异常的
                string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                Regex reg = new Regex(@"""text"":\[""(.*?)""\]\}");
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

        public void TranslatorInit(string param1, string param2="")
        {
            ApiKey = param1;
        }

    }
}

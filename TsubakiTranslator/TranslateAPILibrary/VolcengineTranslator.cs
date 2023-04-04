using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace TsubakiTranslator.TranslateAPILibrary
{
    class VolcengineTranslator : ITranslator
    {
        private readonly string[] langList = { "ja", "en" };
        private readonly string name = "火山";
        public string Name { get => name; }

        private string SourceLanguage { get; set; }

        public string Translate(string sourceText)
        {
            string desLang = "zh";

            var body = new
            {
                text = sourceText,
                source = SourceLanguage,
                target = desLang
            };

            string bodyString = JsonSerializer.Serialize(body);

            string url = @"https://www.volcengine.com/api/exp/2/model-ii";

            HttpClient client = CommonFunction.Client;

            HttpContent content = new StringContent(bodyString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            //HttpResponseMessage resp;
            //var hc = CommonFunction.Client;
            //var req = new HttpRequestMessage(HttpMethod.Post, URL);
            //req.Content = new StringContent("{\"text\":[\""+ sourceText.Replace("\"", "") +"\"],\"model_id\":\"" + SourceLanguage + "-" + desLang + "\"}",null,"application/json");
            //req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(ApiKey)));

            try
            {
                HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();//改成自己的
                response.EnsureSuccessStatusCode();//用来抛异常的
                string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                Regex reg = new Regex(@"\[\{""Translation"":""(.*?)"",""DetectedSourceLanguage""");
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

        public void TranslatorInit(int index, string param1, string param2)
        {
            SourceLanguage = langList[index];
        }
    }
}

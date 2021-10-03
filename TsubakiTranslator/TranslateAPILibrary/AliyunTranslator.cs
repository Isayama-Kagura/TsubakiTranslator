using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;

namespace TsubakiTranslator.TranslateAPILibrary
{
    //API文档介绍 https://help.aliyun.com/document_detail/158269.html

    class AliyunTranslator : ITranslator
    {
        private readonly string name = "阿里云";
        public string Name { get => name; }

        public string SourceLanguage { get; set; }

        public string Translate(string sourceText)
        {
            string desLang = "zh";
            

            string bodyString = $"srcLanguage={SourceLanguage}&tgtLanguage={desLang}&srcText={HttpUtility.UrlEncode(sourceText)}&bizType=general&source=aliyun";

            string url = @"https://translate.alibaba.com/trans/TranslateTextAddAlignment.do";

            HttpClient client = CommonFunction.Client;

            HttpContent content = new StringContent(bodyString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");


            try
            {
                HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();//改成自己的
                response.EnsureSuccessStatusCode();//用来抛异常的
                string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                Regex reg = new Regex(@",""listTargetText"":\[""(.*?)""\],");
                Match match = reg.Match(responseBody);

                string result = match.Groups[1].Value;
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

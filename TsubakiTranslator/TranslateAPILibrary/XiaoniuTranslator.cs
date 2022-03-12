using System.Text.Json;
using System;
using System.Net.Http;
using System.Text;

namespace TsubakiTranslator.TranslateAPILibrary
{
    //API文档 https://niutrans.com/documents/contents/trans_text
    public class XiaoniuTranslator : ITranslator
    {
        private readonly string name = "小牛";

        private string ApiKey;
        public string Name { get => name; }

        public string SourceLanguage { get; set; }

        public string Translate(string sourceText)
        {
            string desLang = "zh";


            string retString;

            var body = new
            {
                from = SourceLanguage,
                to = desLang,
                apikey = ApiKey,
                src_text = sourceText,
            };

            string bodyString = JsonSerializer.Serialize(body);

            string url = @"https://api.niutrans.com/NiuTransServer/translation";

            HttpClient client = CommonFunction.Client;

            HttpContent content = new StringContent(bodyString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");


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

            XiaoniuTransOutInfo oinfo = JsonSerializer.Deserialize<XiaoniuTransOutInfo>(retString);

            if (oinfo.error_code == null)
                return oinfo.tgt_text;
            else
                return oinfo.error_msg;



        }

        public void TranslatorInit(string param1, string param2)
        {
            ApiKey = param1;
        }

        class XiaoniuTransOutInfo
        {
            public string from { get; set; }
            public string to { get; set; }
            public string src_text { get; set; }
            public string tgt_text { get; set; }
            public string error_code { get; set; }
            public string error_msg { get; set; }
        }

    }

    
}

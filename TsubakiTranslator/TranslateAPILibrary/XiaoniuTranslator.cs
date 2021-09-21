using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TsubakiTranslator.TranslateAPILibrary
{
    //API文档 https://niutrans.com/documents/contents/trans_text
    public class XiaoniuTranslator : ITranslator
    {
        private string apiKey;//小牛翻译API 的APIKEY

        private readonly string name = "小牛";
        public string Name { get => name; }

        public async Task<string> Translate(string sourceText)
        {
            string desLang = "zh";
            string srcLang = "ja";

            string retString;

            //var sb = new StringBuilder("https://api.niutrans.com/NiuTransServer/translation?")
            //    .Append("&from=").Append(srcLang)
            //    .Append("&to=").Append(desLang)
            //    .Append("&apikey=").Append(apiKey)
            //    .Append("&src_text=").Append(Uri.EscapeDataString(sourceText));

            var body = new
            {
                from = srcLang,
                to = desLang,
                apikey = apiKey,
                src_text = sourceText
            };

            string bodyString = JsonSerializer.Serialize(body);

            string url = @"https://free.niutrans.com/NiuTransServer/translation";

            var client = CommonFunction.Client;

            HttpContent content = new StringContent(bodyString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

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
            catch (System.Threading.Tasks.TaskCanceledException ex)
            {
                return ex.Message;
            }

            XiaoniuTransOutInfo oinfo = JsonSerializer.Deserialize<XiaoniuTransOutInfo>(retString);

            if (oinfo.error_code == null || oinfo.error_code == "52000")
                return oinfo.tgt_text;
            else
                return oinfo.error_msg;

        }

        public void TranslatorInit(string param1, string param2 = "")
        {
            //第二参数无用
            apiKey = param1;
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

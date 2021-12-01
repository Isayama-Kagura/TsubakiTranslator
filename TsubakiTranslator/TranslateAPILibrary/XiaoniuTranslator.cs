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
        public string Name { get => name; }

        public string SourceLanguage { get; set; }

        public string Translate(string sourceText)
        {
            string desLang = "zh";


            string retString;

            var sb = new StringBuilder("https://test.niutrans.com/NiuTransServer/testaligntrans?")
                .Append("&from=").Append(SourceLanguage)
                .Append("&to=").Append(desLang)
                .Append("&src_text=").Append(Uri.EscapeDataString(sourceText))
                .Append("&source=").Append("text")
                .Append("&dictNo=")
                .Append("&memoryNo=")
                .Append("&isUseDict=").Append("0")
                .Append("&isUseMemory=").Append("0")
                .Append("&time=").Append(CommonFunction.GetTimeStamp());

            string url = sb.ToString();

            var client = CommonFunction.Client;


            try
            {
                HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult(); //改成自己的
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

            if (oinfo.error_code == null || oinfo.error_code == "52000")
                return oinfo.tgt_text;
            else
                return oinfo.error_msg;



        }

        public void TranslatorInit(string param1, string param2)
        {

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

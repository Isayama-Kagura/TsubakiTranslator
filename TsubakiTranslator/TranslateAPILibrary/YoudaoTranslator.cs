using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace TsubakiTranslator.TranslateAPILibrary
{
    public class YoudaoTranslator : ITranslator
    {

        private readonly string name = "有道";
        public string Name { get => name; }

        public string SourceLanguage { get; set; }

        public string Translate(string sourceText)
        {
            string desLang = "zh_cn";

            string retString;

            string trans_type = $"{SourceLanguage}2{desLang}";
            trans_type = trans_type.ToUpper();
            string url = $"https://fanyi.youdao.com/translate?&doctype=json&type={trans_type}&i={sourceText}";

            var client = CommonFunction.Client;
            try
            {
                HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult();
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

            YoudaoTransResult oinfo;
            
            oinfo = JsonSerializer.Deserialize<YoudaoTransResult>(retString);

            if (oinfo.errorCode == 0)
                return string.Join("", oinfo.translateResult[0].Select(x => x.tgt));
            else
                return "ErrorID:" + oinfo.errorCode; ;

        }

        public void TranslatorInit(string param1 = "", string param2 = "")
        {
            //不用初始化
        }
    }

    class YoudaoTransResult
    {
        public string type { get; set; }
        public int errorCode { get; set; }
        public int elapsedTime { get; set; }
        public List<List<YoudaoTransData>> translateResult { get; set; }
    }

    class YoudaoTransData {
        public string src { get; set; }
        public string tgt { get; set; }
    }
}

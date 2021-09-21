
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TsubakiTranslator.TranslateAPILibrary
{
    public class YoudaoTranslator : ITranslator
    {

        private readonly string name = "有道";
        public string Name { get => name; }

        public async Task<string> Translate(string sourceText)
        {
            string desLang = "zh_cn";
            string srcLang = "jp";
            string retString;

            string trans_type = $"{srcLang}2{desLang}";
            trans_type = trans_type.ToUpper();
            string url = $"https://fanyi.youdao.com/translate?&doctype=json&type={trans_type}&i={sourceText}";

            var client = CommonFunction.Client;
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
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


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

namespace TsubakiTranslator.TranslateAPILibrary
{
    //API查询文档 https://fanyi.caiyunapp.com/#/api
    /// <summary>
    /// 只能中英或者中日互译
    /// </summary>
    public class CaiyunTranslator : ITranslator
    {
        private string caiyunToken;//彩云小译 令牌

        private readonly string name = "彩云小译";
        public string Name { get => name; }


        public async Task<string> Translate(string sourceText)
        {
            string desLang = "zh";
            string srcLang = "auto";

            string retString;


            string url = "https://api.interpreter.caiyunai.com/v1/translator";
            //json参数
            string jsonParam = "{\"source\": [\"" + sourceText + "\"], \"trans_type\": \"" + $"{srcLang}2{desLang}" + "\", \"request_id\": \"demo\", \"detect\": true}";

            var client = CommonFunction.Client;

            HttpContent content = new StringContent(jsonParam);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            content.Headers.Add("X-Authorization", "token " + caiyunToken);

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


            CaiyunTransResult oinfo;
            oinfo = JsonSerializer.Deserialize<CaiyunTransResult>(retString);

            return oinfo.target;
            
            
        }

        public void TranslatorInit(string param1, string param2 = "")
        {
            caiyunToken = param1;
        }

        private class CaiyunTransResult
        {
            public string target { get; set; }
        }

    }


}

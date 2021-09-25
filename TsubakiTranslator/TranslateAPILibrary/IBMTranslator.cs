using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace TsubakiTranslator.TranslateAPILibrary
{
    //API文档 https://cloud.ibm.com/docs/language-translator?topic=language-translator-translation-models

    public class IBMTranslator : ITranslator
    {

        private readonly string name = "IBM";
        public string Name { get => name; }

        public string Translate(string sourceText )
        {
            string desLang = "zh";
            string srcLang = "ja";
            var body = new
            {
                text = sourceText,
                source = srcLang,
                target = desLang
            };

            string bodyString = JsonSerializer.Serialize(body);

            string url = @"https://www.ibm.com//demos/live/watson-language-translator/api/translate/text";

            HttpClient client = CommonFunction.Client;

            HttpContent content = new StringContent(bodyString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            //HttpResponseMessage resp;
            //var hc = CommonFunction.Client;
            //var req = new HttpRequestMessage(HttpMethod.Post, URL);
            //req.Content = new StringContent("{\"text\":[\""+ sourceText.Replace("\"", "") +"\"],\"model_id\":\"" + srcLang + "-" + desLang + "\"}",null,"application/json");
            //req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(ApiKey)));

            try
            {
                HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();//改成自己的
                response.EnsureSuccessStatusCode();//用来抛异常的
                string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                Regex reg = new Regex(@"\[\{""translation"":""(.*?)""\}\]");
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

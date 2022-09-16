using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace TsubakiTranslator.TranslateAPILibrary
{
    //API文档 https://www.deepl.com/docs-api/accessing-the-api/error-handling/
    /*
     * DeepL translator integration
     * Author: kjstart
     * API version: v2
     */
    public class DeepLTranslator : ITranslator
    {
        // DeepL免费版和收费版使用不同url
        // https://api-free.deepl.com/v2/translate
        // https://api.deepl.com/v2/translate
        private string apiUrl;
        private string secretKey; //DeepL翻译API的秘钥

        private readonly string name = "DeepL";
        public string Name { get => name; }

        public string SourceLanguage { get; set; }

        public string Translate(string sourceText)
        {
            string desLang = "ZH";

            string resultStr;

            string payload = $"text={ sourceText }&auth_key={ secretKey}&source_lang={ SourceLanguage}&target_lang={ desLang}";

            HttpContent body = new StringContent(payload, null, "application/x-www-form-urlencoded");

            var client = CommonFunction.Client;

            string url = apiUrl;

            try
            {
                HttpResponseMessage response = client.PostAsync(url, body).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();//用来抛异常的
                resultStr = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            }
            catch (System.Net.Http.HttpRequestException ex)
            {

                return ex.Message;
            }
            catch (System.Threading.Tasks.TaskCanceledException ex)
            {

                return ex.Message;
            }

            DeepLTranslateResult translateResult = JsonSerializer.Deserialize<DeepLTranslateResult>(resultStr);
            if (translateResult != null && translateResult.translations != null && translateResult.translations.Count > 0)
            {
                return translateResult.translations[0].text;
            }
            else
            {
                return "Cannot get translation from: " + resultStr;
            }
        }

        public void TranslatorInit(string param1, string param2)
        {
            apiUrl = param1;
            secretKey = param2;
        }

        class DeepLTranslateResult
        {
            public List<DeepLTranslations> translations { get; set; }
        }

        class DeepLTranslations
        {
            public string detected_source_language { get; set; }
            public string text { get; set; }
        }

    }


}

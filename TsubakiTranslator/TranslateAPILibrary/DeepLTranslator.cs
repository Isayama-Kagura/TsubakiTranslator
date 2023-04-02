using RestSharp;
using System.Collections.Generic;
using System.Text.Json;

namespace TsubakiTranslator.TranslateAPILibrary
{
    //API文档 https://www.deepl.com/docs-api
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

            var client = new RestClient(CommonFunction.Client);
            var request = new RestRequest(apiUrl, Method.Post);
            request.AddHeader("Authorization", $"DeepL-Auth-Key {secretKey}");

            request.AddQueryParameter("text", sourceText).AddQueryParameter("source_lang", SourceLanguage).AddQueryParameter("target_lang", desLang);

            var response = client.Execute(request);

            resultStr = response.Content;


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
            secretKey = param1;
            if (param2 == null)
                apiUrl = @"https://api-free.deepl.com/v2/translate";
            else
                apiUrl = @"https://api.deepl.com/v2/translate";
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

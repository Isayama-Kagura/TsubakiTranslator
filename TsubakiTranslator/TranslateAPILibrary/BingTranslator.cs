using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TsubakiTranslator.TranslateAPILibrary
{
    public class BingTranslator : ITranslator
    {
        public string SourceLanguage { get; set; }

        private readonly string name = "必应";
        public string Name { get => name; }
        private string Key { get; set; }
        private string Token { get; set; }
        private string Iid { get; set; }
        private string Ig { get; set; }
        private int TranslateTimes { get; set; }

        private RestClient Client { get; }

        public BingTranslator()
        {
            Client = new RestClient(CommonFunction.Client);
            Task.Run(async()=> await InitializeAsync());
        }

        public string Translate(string sourceText)
        {
            string desLang = "zh-Hans";
            

            string url = "https://cn.bing.com/ttranslatev3"+ $"?isVertical=1&&IG={Ig}&IID={Iid}"+ TranslateTimes++;
            var request = new RestRequest(url, Method.Post);
            string bodyString = $"fromLang={SourceLanguage}&text={sourceText}&to={desLang}&token={Token}&key={Key}";
            request.AddStringBody(bodyString, "application/x-www-form-urlencoded");


            var response = Client.Execute(request);

            string result = "";

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string responseBody = response.Content;

                var regex = new Regex(@"""translations"":\[{""text"":""(.*?)""");
                var match = regex.Match(responseBody);
                result = match.Groups[1].Value;
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Task.Run(async () => await InitializeAsync());
                result = Translate(sourceText);
            }
            else
            {
                result = response.ErrorMessage;
            }


            return result;
        }

        private async Task InitializeAsync()
        {
            string url = "https://cn.bing.com/translator";
            var request = new RestRequest(url, Method.Post);
            var response = await Client.ExecuteAsync(request);
            string result = response.Content;
            if (result == null)
            {
                await Task.Delay(5000);
                await InitializeAsync();
                return;
            }
             
            Regex regex = new Regex("params_RichTranslateHelper = \\[(.+?),\"(.+?)\",.+?");
            var match = regex.Match(result);
            Token = match.Groups[2].Value;
            Key = match.Groups[1].Value;

            regex = new Regex(@"""rich_tta"" data-iid=""(.+?)""");
            match = regex.Match(result);
            Iid = match.Groups[1].Value+".";

            regex = new Regex(@""",IG:""(.+?)"",EventID:");
            match = regex.Match(result);
            Ig = match.Groups[1].Value;

            TranslateTimes = 0;
        }

        public void TranslatorInit(string param1, string param2)
        {

        }
    }
}

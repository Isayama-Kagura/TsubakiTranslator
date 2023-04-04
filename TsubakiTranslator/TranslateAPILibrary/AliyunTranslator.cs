using RestSharp;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace TsubakiTranslator.TranslateAPILibrary
{
    //API文档介绍 https://help.aliyun.com/document_detail/158269.html

    class AliyunTranslator : ITranslator
    {
        private readonly string name = "阿里";
        private readonly string[] langList = { "ja", "en" };

        public string Name { get => name; }

        private string SourceLanguage { get; set; }

        private string SecretId { get; set; }
        private string SecretKey { get; set; }

        public string Translate(string sourceText)
        {
            string desLang = "zh";

            string method = "POST";
            string accept = "application/json";
            string contentType = "application/json; charset=utf-8";
            string date = DateTime.UtcNow.ToString("r");
            string host = "mt.cn-hangzhou.aliyuncs.com";
            string path = "/api/translate/web/general";


            var body = new
            {
                FormatType = "text",
                Scene = "general",
                SourceLanguage = SourceLanguage,
                SourceText = sourceText,
                TargetLanguage = desLang,
            };

            string bodyString = JsonSerializer.Serialize(body);
            string bodyMd5 = string.Empty;
            using (MD5 md5Hash = MD5.Create())
            {
                // 将输入字符串转换为字节数组并计算哈希数据  
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(bodyString));
                // 返回BASE64字符串  
                bodyMd5 = Convert.ToBase64String(data);
            }

            string uuid = Guid.NewGuid().ToString();

            string stringToSign = method + "\n" + accept + "\n" + bodyMd5 + "\n" + contentType + "\n" + date + "\n"
                    + "x-acs-signature-method:HMAC-SHA1\n"
                    + "x-acs-signature-nonce:" + uuid + "\n"
                    + path;

            string signature = string.Empty;
            using (HMACSHA1 mac = new HMACSHA1(Encoding.UTF8.GetBytes(SecretKey)))
            {
                byte[] hash = mac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign));
                signature = Convert.ToBase64String(hash);
            }

            string authHeader = "acs " + SecretId + ":" + signature;

            var client = new RestClient(CommonFunction.Client);
            var request = new RestRequest($"https://{host}{path}", Method.Post);
            request.AddHeader("Authorization", authHeader);
            request.AddHeader("Accept", accept);
            request.AddHeader("Content-MD5", bodyMd5);
            //request.AddHeader("Content-Type", host);
            request.AddHeader("Date", date);
            request.AddHeader("x-acs-signature-method", "HMAC-SHA1");
            request.AddHeader("x-acs-signature-nonce", uuid);

            request.AddStringBody(bodyString, DataFormat.Json);


            var response = client.Execute(request);
            string result = response.Content;
            if (response.IsSuccessful)
            {
                Regex codeReg = new Regex("},\"Code\":\"200\"");
                if (codeReg.IsMatch(result))
                {
                    Regex reg = new Regex(@",""Translated"":""(.*?)""},");
                    Match match = reg.Match(response.Content);
                    result = match.Groups[1].Value;
                }
                return result;
            }

            else
            {
                return result;
            }


        }

        public void TranslatorInit(int index, string param1, string param2)
        {
            SourceLanguage = langList[index];
            SecretId = param1;
            SecretKey = param2;
        }
    }
}

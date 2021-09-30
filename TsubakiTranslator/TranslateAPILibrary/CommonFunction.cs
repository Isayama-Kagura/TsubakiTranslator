using System;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace TsubakiTranslator.TranslateAPILibrary
{
    public static class CommonFunction
    {
        

        static CommonFunction()
        {
            client = new HttpClient(new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip
            });
            //{ Timeout = TimeSpan.FromSeconds(6) };

            HttpRequestHeaders headers = client.DefaultRequestHeaders;
            headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36");
            headers.AcceptEncoding.ParseAdd("gzip");
            headers.Connection.ParseAdd("keep-alive");
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }

        static readonly private HttpClient client;
        /// <summary>
        /// 获得HttpClinet单例，第一次调用自动初始化
        /// </summary>
        public static HttpClient Client
        {
            get => client;
        }

        /// <summary>
        /// 计算MD5值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }

        /// <summary>
        /// 计算时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }



    }
}

using RestSharp;
using System;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Timers;
using TsubakiTranslator.TranslateAPILibrary;

namespace TsubakiTranslator.BasicLibrary
{
    public class TTSHandler
    {
        private string Region { get; }
        private string Key { get; }

        private string Language { get; }

        private string VoiceName { get; }

        private Timer RequestTokenTimer { get; }
        private string Token { get; set; }

        private RestClient Client { get; }

        private string errorMessage;
        public string ErrorMessage { get => errorMessage; }

        public TTSHandler(string region, string key, string language, string voiceName)
        {
            Region = region;
            Key = key;
            Language = language;
            VoiceName = voiceName;

            Client = new RestClient(CommonFunction.Client);

            //每9分钟请求一次
            int interval = 9 * 60 * 1000;
            RequestTokenTimer = new Timer(interval);
            RequestTokenTimer.AutoReset = true;
            RequestTokenTimer.Elapsed += OnTimedEvent;

            OnTimedEvent(null, null);
            RequestTokenTimer.Start();

        }

        public async Task<bool> SpeakTextAsync(string text)
        {
            var request = new RestRequest($"https://{Region}.tts.speech.microsoft.com/cognitiveservices/v1", Method.Post);
            request.AddHeader("Authorization", "Bearer " + Token);
            request.AddHeader("X-Microsoft-OutputFormat", "riff-24khz-16bit-mono-pcm");
            //request.AddHeader("content-type", "application/ssml+xml");

            string bodyString = @$"<speak version='1.0' xml:lang='{Language}'><voice xml:lang='{Language}' xml:gender='Female' name = '{VoiceName}' >{text}</voice ></speak > ";

            //request.AddParameter("application/ssml+xml", bodyString, ParameterType.RequestBody);
            //request.AddXmlBody(bodyString);
            request.AddStringBody(bodyString, "application/ssml+xml");

            RestResponse response = await Client.ExecuteAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                byte[] result = response.RawBytes;
                using (MemoryStream ms = new MemoryStream(result))
                {
                    // Construct the sound player
                    SoundPlayer player = new SoundPlayer(ms);
                    player.Play();
                }
                return true;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                OnTimedEvent(null, null);
                errorMessage = response.ErrorException.Message;
                return false;
            }
            else
            {
                errorMessage = response.ErrorException.Message;
                return false;
            }

        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            var request = new RestRequest($"https://{Region}.api.cognitive.microsoft.com/sts/v1.0/issuetoken", Method.Post);
            request.AddHeader("Ocp-Apim-Subscription-Key", Key);

            var response = Client.Execute(request);

            Token = response.Content;

        }

        public void Dispose()
        {
            RequestTokenTimer.Stop();
            RequestTokenTimer.Dispose();
        }
    }
}

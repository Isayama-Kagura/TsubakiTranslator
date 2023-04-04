using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TsubakiTranslator.TranslateAPILibrary
{
    internal class Message
    {
        [JsonPropertyName("role")] public string Role { get; set; }
        [JsonPropertyName("content")] public string Content { get; set; }
    }

    internal class RequestBody
    {
        [JsonPropertyName("model")] public string Model { get; set; }
        [JsonPropertyName("messages")] public Message[] Messages { get; set; }
        [JsonPropertyName("temperature")] public int Temperature { get; set; }
        [JsonPropertyName("max_tokens")] public int MaxTokens { get; set; }
    }

    internal class Response
    {
        public class Usage_
        {
            [JsonPropertyName("prompt_tokens")] public int PromptTokens { get; set; }
            [JsonPropertyName("completion_tokens")] public int CompletionTokens { get; set; }
            [JsonPropertyName("total_tokens")] public int TotalTokens { get; set; }
        }
        public class Choice
        {
            [JsonPropertyName("message")] public Message Message { get; set; }
            [JsonPropertyName("finish_reason")] public string FinishReason { get; set; }
            [JsonPropertyName("index")] public int Index { get; set; }
        }

        [JsonPropertyName("id")] public string Id { get; set; }
        [JsonPropertyName("model")] public string Model { get; set; }
        [JsonPropertyName("usage")] public Usage_ Usage { get; set; }
        [JsonPropertyName("choices")] public Choice[] Choices { get; set; }
    }

    public class ChatGptTranslator : ITranslator
    {
        private string token;
        private HttpClient client;
        private const string Url = @"https://api.openai.com/v1/chat/completions";

        private RequestBody NewRequest(string content)
        {
            return new RequestBody
            {
                Messages = new Message[] {
                    new() {
                        Role = "system",
                        Content = "You are a translation engine that can only translate text and cannot interpret it.",
                    },
                    new() {
                        Role = "user",
                        Content = $"translate from {SourceLanguage} to Simplified Chinese",
                    },
                    new() {
                        Role = "user",
                        Content = $"\"{content}\"",
                    },
                },
                Model = "gpt-3.5-turbo",
                Temperature = 0,
                MaxTokens = 250,
            };
        }

        private readonly string[] langList = { "Japanese", "English" };
        private readonly string name = "ChatGPT";
        public string Name { get => name; }

        private string SourceLanguage { get; set; }


        public string Translate(string sourceText)
        {
            var bodyString = JsonSerializer.Serialize(NewRequest(sourceText));

            HttpContent content = new StringContent(bodyString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            string respString;
            try
            {
                var response = client.PostAsync(Url, content).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                respString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            catch (HttpRequestException ex)
            {
                return ex.Message;
            }
            catch (System.Threading.Tasks.TaskCanceledException ex)
            {
                return ex.Message;
            }

            var resp = JsonSerializer.Deserialize<Response>(respString);
            return resp?.Choices.Length != 0 ? resp?.Choices[0].Message.Content.Trim('"') : "";
        }

        public void TranslatorInit(int index, string param1, string _)
        {
            SourceLanguage = langList[index];
            token = param1;
            client = CommonFunction.NewClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsubakiTranslator.BasicLibrary
{
    class APISourceLangDict
    {
        public Dictionary<string, string> Japanese { get; }
        public Dictionary<string, string> English { get;  }

        public APISourceLangDict()
        {
            Japanese = new Dictionary<string, string>();
            Japanese.Add("阿里云", "ja");
            Japanese.Add("百度", "auto");
            Japanese.Add("彩云", "auto");
            Japanese.Add("DeepL", "JA");
            Japanese.Add("谷歌", "auto");
            Japanese.Add("IBM", "ja");
            Japanese.Add("腾讯", "auto");
            Japanese.Add("小牛", "ja");
            Japanese.Add("Yandex", "ja");
            Japanese.Add("Yeekit", "ja");
            Japanese.Add("有道", "jp");

            English = new Dictionary<string, string>();
            English.Add("阿里云", "en");
            English.Add("百度", "auto");
            English.Add("彩云", "auto");
            English.Add("DeepL", "EN");
            English.Add("谷歌", "auto");
            English.Add("IBM", "en");
            English.Add("腾讯", "auto");
            English.Add("小牛", "en");
            English.Add("Yandex", "en");
            English.Add("Yeekit", "nen");
            English.Add("有道", "en");
        }

    }
}

using System;
using System.Collections.Generic;
using TsubakiTranslator.TranslateAPILibrary;

namespace TsubakiTranslator.BasicLibrary
{
    class TranslateHandler
    {
        //对翻译API进行初始化。
        public static LinkedList<ITranslator> GetSelectedTranslators(TranslateAPIConfig translateAPIConfig)
        {
            LinkedList<ITranslator> translators = new LinkedList<ITranslator>();

            if (translateAPIConfig.AliIsEnabled)
            {
                ITranslator aliyun = new AliyunTranslator();
                aliyun.TranslatorInit(translateAPIConfig.AliSecretId, translateAPIConfig.AliSecretKey);
                translators.AddLast(aliyun);
            }

            if (translateAPIConfig.BaiduIsEnabled)
            {
                ITranslator baidu = new BaiduTranslator();
                baidu.TranslatorInit(translateAPIConfig.BaiduAppID, translateAPIConfig.BaiduSecretKey);
                translators.AddLast(baidu);
            }

            if (translateAPIConfig.BingIsEnabled)
            {
                ITranslator bing = new BingTranslator();
                translators.AddLast(bing);
            }

            if (translateAPIConfig.CaiyunIsEnabled)
            {
                ITranslator caiyun = new CaiyunTranslator();
                caiyun.TranslatorInit(translateAPIConfig.CaiyunToken, "");
                translators.AddLast(caiyun);
            }

            if (translateAPIConfig.DeeplIsEnabled)
            {
                ITranslator deepl = new DeepLTranslator(translateAPIConfig.DeeplIsFreeApi);
                deepl.TranslatorInit(translateAPIConfig.DeeplSecretKey, "");
                translators.AddLast(deepl);
            }


            if (translateAPIConfig.IbmIsEnabled)
            {
                ITranslator ibm = new IBMTranslator();
                translators.AddLast(ibm);

            }

            if (translateAPIConfig.ICiBaIsEnabled)
            {
                ITranslator iCiBa = new ICiBaTranslator();
                translators.AddLast(iCiBa);

            }

            if (translateAPIConfig.TencentIsEnabled)
            {
                ITranslator tencent = new TencentTranslator();
                tencent.TranslatorInit(translateAPIConfig.TencentSecretID, translateAPIConfig.TencentSecretKey);
                translators.AddLast(tencent);
            }

            if (translateAPIConfig.XiaoniuIsEnabled)
            {
                ITranslator xiaoniu = new XiaoniuTranslator();
                xiaoniu.TranslatorInit(translateAPIConfig.XiaoniuApiKey, "");
                translators.AddLast(xiaoniu);
            }

            if (translateAPIConfig.VolcengineIsEnabled)
            {
                ITranslator volcengine = new VolcengineTranslator();
                translators.AddLast(volcengine);
            }

            if (translateAPIConfig.YeekitIsEnabled)
            {
                ITranslator yeekit = new YeekitTranslator();
                translators.AddLast(yeekit);
            }


            Dictionary<string, string> langDict = GetAPISourceLangDict(App.OtherConfig.SourceLanguage);

            foreach (var t in translators)
                t.SourceLanguage = langDict[t.Name];

            return translators;
        }

        public static Dictionary<string, string> GetAPISourceLangDict(string srcLang)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            if (srcLang.Equals("Japanese"))
            {
                dict.Add("阿里", "ja");
                dict.Add("百度", "auto");
                dict.Add("必应", "ja");
                dict.Add("彩云", "ja");
                dict.Add("DeepL", "JA");
                dict.Add("IBM", "ja");
                dict.Add("爱词霸", "ja");
                dict.Add("腾讯", "auto");
                dict.Add("小牛", "ja");
                dict.Add("火山", "ja");
                dict.Add("Yeekit", "nja");
            }
            else if (srcLang.Equals("English"))
            {
                dict.Add("阿里", "en");
                dict.Add("百度", "auto");
                dict.Add("必应", "en");
                dict.Add("彩云", "en");
                dict.Add("DeepL", "EN");
                dict.Add("IBM", "en");
                dict.Add("爱词霸", "en");
                dict.Add("腾讯", "auto");
                dict.Add("小牛", "en");
                dict.Add("火山", "en");
                dict.Add("Yeekit", "nen");
            }
            return dict;

        }



        /// <summary>
        /// 根据翻译类名创建对象实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullName">命名空间.类型名</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string fullName)
        {
            //string path = fullName + "," + assemblyName;//命名空间.类型名,程序集
            Type o = Type.GetType(fullName);//加载类型
            object obj = Activator.CreateInstance(o, true);//根据类型创建实例
            return (T)obj;//类型转换并返回
        }
    }
}

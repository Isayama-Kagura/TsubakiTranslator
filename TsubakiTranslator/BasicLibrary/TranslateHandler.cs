using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsubakiTranslator.TranslateAPILibrary;

namespace TsubakiTranslator.BasicLibrary
{
    class TranslateHandler
    {
        public static LinkedList<ITranslator> GetSelectedTranslators(TranslateAPIConfig translateAPIConfig)
        {
            LinkedList<ITranslator> translators = new LinkedList<ITranslator>();

            if (translateAPIConfig.BaiduIsEnabled)
            {
                ITranslator baidu = new BaiduTranslator();
                baidu.TranslatorInit(translateAPIConfig.BaiduAppID, translateAPIConfig.BaiduSecretKey);
                translators.AddLast(baidu);
            }

            if (translateAPIConfig.CaiyunIsEnabled)
            {
                ITranslator caiyun = new CaiyunTranslator();
                caiyun.TranslatorInit(translateAPIConfig.CaiyunToken, "");
                translators.AddLast(caiyun);
            }

            if (translateAPIConfig.DeeplIsEnabled)
            {
                ITranslator deepl = new DeepLTranslator();
                deepl.TranslatorInit(translateAPIConfig.DeeplSecretKey, "");
                translators.AddLast(deepl);
            }

            if (translateAPIConfig.GoogleIsEnabled)
            {
                ITranslator google = new GoogleTranslator();
                translators.AddLast(google);
            }

            if (translateAPIConfig.IbmIsEnabled)
            {
                ITranslator ibm = new IBMTranslator();
                translators.AddLast(ibm);

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
                xiaoniu.TranslatorInit(translateAPIConfig.XiaoniuAPIKey, "");
                translators.AddLast(xiaoniu);
            }

            if (translateAPIConfig.YandexIsEnabled)
            {
                ITranslator yandex = new XiaoniuTranslator();
                yandex.TranslatorInit(translateAPIConfig.YandexAPIKey, "");
                translators.AddLast(yandex);
            }

            if (translateAPIConfig.YeekitIsEnabled)
            {
                ITranslator yeekit = new YeekitTranslator();
                translators.AddLast(yeekit);
            }

            if (translateAPIConfig.YoudaoIsEnabled)
            {
                ITranslator youdao = new YoudaoTranslator();
                translators.AddLast(youdao);
            }

            return translators;
        }

        public static string RemoveDuplicatedChar(string text, int times)
        {
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < text.Length; i += times)
                sb.Append(text[i]);

            return sb.ToString();
            
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

using System;
using System.Collections.Generic;
using TsubakiTranslator.TranslateAPILibrary;

namespace TsubakiTranslator.BasicLibrary
{
    class TranslateHandler
    {
        //对翻译API进行初始化。
        public static LinkedList<ITranslator> GetSelectedTranslators(TranslateAPIConfig translateAPIConfig, int srcLangIndex)
        {
            LinkedList<ITranslator> translators = new LinkedList<ITranslator>();

            if (translateAPIConfig.AliIsEnabled)
            {
                ITranslator aliyun = new AliyunTranslator();
                aliyun.TranslatorInit(srcLangIndex, translateAPIConfig.AliSecretId, translateAPIConfig.AliSecretKey);
                translators.AddLast(aliyun);
            }

            if (translateAPIConfig.BaiduIsEnabled)
            {
                ITranslator baidu = new BaiduTranslator();
                baidu.TranslatorInit(srcLangIndex, translateAPIConfig.BaiduAppID, translateAPIConfig.BaiduSecretKey);
                translators.AddLast(baidu);
            }

            if (translateAPIConfig.BingIsEnabled)
            {
                ITranslator bing = new BingTranslator();
                bing.TranslatorInit(srcLangIndex, null, null);
                translators.AddLast(bing);
            }

            if (translateAPIConfig.CaiyunIsEnabled)
            {
                ITranslator caiyun = new CaiyunTranslator();
                caiyun.TranslatorInit(srcLangIndex, translateAPIConfig.CaiyunToken, "");
                translators.AddLast(caiyun);
            }

            if (translateAPIConfig.ChatGptIsEnabled)
            {
                ITranslator chatgpt = new ChatGptTranslator();
                chatgpt.TranslatorInit(srcLangIndex, translateAPIConfig.ChatGptToken, "");
                translators.AddLast(chatgpt);
            }

            if (translateAPIConfig.DeeplIsEnabled)
            {
                ITranslator deepl = new DeepLTranslator();
                deepl.TranslatorInit(srcLangIndex, translateAPIConfig.DeeplSecretKey, translateAPIConfig.DeeplIsFreeApi ? null : "");
                translators.AddLast(deepl);
            }


            if (translateAPIConfig.IbmIsEnabled)
            {
                ITranslator ibm = new IBMTranslator();
                ibm.TranslatorInit(srcLangIndex, null, null);
                translators.AddLast(ibm);

            }

            if (translateAPIConfig.ICiBaIsEnabled)
            {
                ITranslator iCiBa = new ICiBaTranslator();
                iCiBa.TranslatorInit(srcLangIndex, null, null);
                translators.AddLast(iCiBa);

            }

            if (translateAPIConfig.TencentIsEnabled)
            {
                ITranslator tencent = new TencentTranslator();
                tencent.TranslatorInit(srcLangIndex, translateAPIConfig.TencentSecretID, translateAPIConfig.TencentSecretKey);
                translators.AddLast(tencent);
            }

            if (translateAPIConfig.XiaoniuIsEnabled)
            {
                ITranslator xiaoniu = new XiaoniuTranslator();
                xiaoniu.TranslatorInit(srcLangIndex, translateAPIConfig.XiaoniuApiKey, "");
                translators.AddLast(xiaoniu);
            }

            if (translateAPIConfig.VolcengineIsEnabled)
            {
                ITranslator volcengine = new VolcengineTranslator();
                volcengine.TranslatorInit(srcLangIndex, null, null);
                translators.AddLast(volcengine);
            }

            if (translateAPIConfig.YeekitIsEnabled)
            {
                ITranslator yeekit = new YeekitTranslator();
                yeekit.TranslatorInit(srcLangIndex, null, null);
                translators.AddLast(yeekit);
            }

            return translators;
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

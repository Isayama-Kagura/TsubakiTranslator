﻿namespace TsubakiTranslator.TranslateAPILibrary
{
    public interface ITranslator
    {
        /// <summary>
        /// 翻译API初始化
        /// </summary>
        /// <param name="param1">参数一 一般是appID或者路径（为路径时参数二无效）</param>
        /// <param name="param2">参数二 一般是密钥</param>
        void TranslatorInit(string param1,string param2);

        /// <summary>
        /// 翻译一条语句
        /// </summary>
        /// <param name="sourceText">源文本</param>
        /// <param name="desLang">目标语言</param>
        /// <param name="srcLang">源语言</param>
        /// <returns>翻译后的语句,如果翻译有错误会返回空，可以通过GetLastError来获取错误</returns>
        string Translate(string sourceText);

        string Name { get; }

    }
}

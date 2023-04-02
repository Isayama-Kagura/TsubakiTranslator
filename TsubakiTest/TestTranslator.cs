using System;
using NUnit.Framework;
using TsubakiTranslator.TranslateAPILibrary;

namespace TsubakiTest {
    public class Tests {
        string translateWithTranslator(ITranslator translator, string param1, string param2, string content) {
            translator.TranslatorInit(param1, param2);
            return translator.Translate(content);
        }

        [Test]
        public void TestChatGpt() {
            var content = @"宇宙に始まりはあるが、終わりはない。---無限
星にもまた始まりはあるが、自らの力をもって滅び逝く。---有限";
            var translator = new ChatGptTranslator {
                SourceLanguage = "Japanese"
            };
            var result = translateWithTranslator(translator,
                ApiKeys.ChatGptKey,
                "",
                content);

            /*
             * a possible result is:
             * 宇宙有开端，但没有终点。---无限
             * 星辰也有起源，但注定会因自身力量的消耗而灭亡。---有限
             */
            Console.WriteLine(result);
        }
    }
}
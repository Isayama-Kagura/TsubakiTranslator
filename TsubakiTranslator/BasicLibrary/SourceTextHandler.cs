using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TsubakiTranslator.BasicLibrary
{
    public class SourceTextHandler
    {
        public int DuplicateTimes { get; }
        public LinkedList<RegexRuleData> RegexRules { get; }

        public SourceTextHandler(int duplicateTimes, LinkedList<RegexRuleData> regexRules)
        {
            DuplicateTimes = duplicateTimes;
            RegexRules = regexRules;
        }

        public string HandleText(string text)
        {
            string result = text;

            if (DuplicateTimes >= 2)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < text.Length; i += DuplicateTimes)
                    sb.Append(text[i]);
                result = sb.ToString();
            }

            if (RegexRules.Count != 0)
            {
                foreach (RegexRuleData rule in RegexRules)
                    if (rule.SourceRegex.Trim().Length != 0)
                        result = Regex.Replace(result, rule.SourceRegex, rule.DestinationRegex);
            }

            return result;
        }


    }
}

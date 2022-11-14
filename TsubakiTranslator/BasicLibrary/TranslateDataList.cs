using System.Collections.Generic;
using System.Linq;

namespace TsubakiTranslator
{
    //存储单次翻译的结果数据
    public class TranslateData
    {
        public string SourceText { get; }
        public Dictionary<string, string> ResultText { get; }

        public TranslateData(string sourceText, Dictionary<string, string> resultText)
        {
            SourceText = sourceText;
            ResultText = resultText;
        }

    }

    public class TranslateDataList
    {

        public int MaxLength { get; }
        private LinkedList<TranslateData> list;

        private LinkedListNode<TranslateData> currentData;
        private LinkedListNode<TranslateData> CurrentData { get => currentData; }

        public TranslateDataList(int maxLength)
        {
            MaxLength = maxLength;
            list = new LinkedList<TranslateData>();
        }

        /// <summary>
        /// 输入string数组，0项是源文本，从1开始是翻译结果
        /// </summary>
        /// <param name="sourceText"></param>
        /// <param name="resultText"></param>
        public void AddTranslateData(TranslateData translateData)
        {
            if (list.Count >= MaxLength)
                list.RemoveFirst();
            list.AddLast(translateData);
            currentData = list.Last;
        }

        //获得最新的翻译结果数据
        public TranslateData GetCurrentData()
        {
            return list.Last.Value;
        }

        public TranslateData GetNextData()
        {
            if (CurrentData != list.Last)
                currentData = CurrentData.Next;
            return CurrentData.Value;
        }

        public TranslateData GetPreviousData()
        {
            if (CurrentData != list.First)
                currentData = CurrentData.Previous;
            return CurrentData.Value;
        }

        public TranslateData GetFirstData()
        {
            currentData = list.First;
            return CurrentData.Value;
        }

        public TranslateData GetLastData()
        {
            currentData = list.Last;
            return CurrentData.Value;
        }

        public int Count()
        {
            return list.Count();
        }
    }
}

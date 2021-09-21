using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsubakiTranslator
{
    //public class TranslateData
    //{
    //    public string SourceText { get; }
    //    public List<string> ResultText { get; }

    //    public TranslateData(string sourceText, List<string> resultText)
    //    {
    //        SourceText = sourceText;
    //        ResultText = resultText;
    //    }

    //}

    public class TranslateDataList
    {
        
        public int MaxLength { get; }
        private LinkedList<string[]> list;

        private LinkedListNode<string[]> currentData;
        private LinkedListNode<string[]> CurrentData { get => currentData;  }

        public TranslateDataList(int maxLength)
        {
            MaxLength = maxLength;
            list = new LinkedList<string[]>();
        }

        /// <summary>
        /// 输入string数组，0项是源文本，从1开始是翻译结果
        /// </summary>
        /// <param name="sourceText"></param>
        /// <param name="resultText"></param>
        public void AddTranslateData(string[] text)
        {
            if (list.Count >= MaxLength)
                list.RemoveFirst();
            list.AddLast(text);
            currentData = list.Last;
        }

        //获得最新的翻译结果数据
        public string[] GetCurrentData()
        {
            return list.Last.Value;
        }

        public string[] GetNextData()
        {
            if (CurrentData != list.Last)
                currentData = CurrentData.Next;
            return CurrentData.Value;
        }

        public string[] GetPreviousData()
        {
            if (CurrentData != list.First)
                currentData = CurrentData.Previous;
            return CurrentData.Value;
        }

        public int Count()
        {
            return list.Count();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TsubakiTranslator.BasicLibrary;

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

        private int MaxLength { get; }
        private LinkedList<TranslateData> list;

        private LinkedListNode<TranslateData> currentData;
        private LinkedListNode<TranslateData> CurrentData { get => currentData; }

        private string DataLogFilePath { get; }

        public TranslateDataList(int maxLength)
        {
            MaxLength = maxLength;
            list = new LinkedList<TranslateData>();
        }
        public TranslateDataList(int maxLength, string logPath)
        {
            MaxLength = maxLength;
            list = new LinkedList<TranslateData>();

            DateTime dt = DateTime.Now;
            DataLogFilePath = logPath + "\\translated_" + string.Format("{0:yyMMddHHmmss}", dt) + ".log";
            FileHandler.CreateFile(DataLogFilePath);
            
        }

        /// <summary>
        /// 输入TranslateData，插入队尾
        /// </summary>
        /// <param name="sourceText"></param>
        /// <param name="resultText"></param>
        public void AddTranslateData(TranslateData translateData)
        {
            if (list.Count >= MaxLength)
            {
                if (DataLogFilePath != null)
                    SaveDataToFile(list.First.Value);
                list.RemoveFirst();
            }
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

        public void SaveAllDataToFile()
        {
            foreach (TranslateData data in list)
            {
                SaveDataToFile(data);
            }
        }
        private void SaveDataToFile(TranslateData data)
        {
            var sb = new StringBuilder();
            sb.AppendLine(data.SourceText);
            foreach (string key in data.ResultText.Keys)
                sb.Append(key).Append(": ").Append(data.ResultText[key]).AppendLine();

            string result = sb.ToString();

            FileHandler.AppendTextToFile(result, DataLogFilePath);
        }
    }
}

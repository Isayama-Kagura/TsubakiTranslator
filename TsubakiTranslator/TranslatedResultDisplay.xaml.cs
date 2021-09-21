using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using TsubakiTranslator.BasicLibrary;
using TsubakiTranslator.TranslateAPILibrary;

namespace TsubakiTranslator
{
    /// <summary>
    /// TranslatedResultDisplay.xaml 的交互逻辑
    /// </summary>
    public partial class TranslatedResultDisplay : UserControl
    {
        SourceTextContent sourceTextContent;
        Dictionary<string, TranslatedData> resultTextContent;

        TextHookHandler textHookHandler;
        LinkedList<ITranslator> translators;
        TranslateDataList results;

        public TranslatedResultDisplay(TextHookHandler textHookHandler)
        {
            InitializeComponent();

            translators = TranslateHandler.GetSelectedTranslators(UserConfigPage.TranslateAPIConfig);

            resultTextContent = new Dictionary<string, TranslatedData>();
            foreach (ITranslator t in translators)
            {
                TranslatedResultItem resultItem = new TranslatedResultItem(t.Name, "");
                TranslateResultPanel.Children.Add(resultItem);
                resultTextContent.Add(t.Name, resultItem.TranslatedData);
            }
            

            sourceTextContent = new SourceTextContent();
            this.DataContext = sourceTextContent;
            

            //最多保留30条历史记录
            results = new TranslateDataList(30);

            this.textHookHandler = textHookHandler;

            textHookHandler.ProcessTextractor.OutputDataReceived += DisplayTranslateResult;

        }

        public async void DisplayTranslateResult(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (textHookHandler.SelectedHookCode == null )
                return;
            if (results.Count()!=0 && results.GetCurrentData()[0].Equals(TranslateHandler.RemoveDuplicatedChar(textHookHandler.HookDict[textHookHandler.SelectedHookCode], textHookHandler.DuplicateTimes)))
                return;

            //string result = await google.Translate(textHookHandler.HookDict[textHookHandler.SelectedHookCode]);

            string[] currentResult = new string[translators.Count + 1];

            results.AddTranslateData(currentResult);

            string sourceText= TranslateHandler.RemoveDuplicatedChar(textHookHandler.HookDict[textHookHandler.SelectedHookCode], textHookHandler.DuplicateTimes);
            currentResult[0] = sourceText;

            sourceTextContent.BindingText = currentResult[0];

            foreach(var key in resultTextContent.Keys)
                resultTextContent[key].TranslatedResult = "";

            int i = 1;
            foreach(ITranslator t in translators)
            {
                currentResult[i] = await t.Translate(currentResult[0]);
                resultTextContent[t.Name].TranslatedResult = currentResult[i];
                i++;
            }

            

            //App.Current.Dispatcher.Invoke((Action)(() =>
            //{
            //    TranslateResultPanel.Children.Clear();
            //    /// start 你的逻辑代码

            //    TranslatedResultItem resultItem = new TranslatedResultItem("IBM", result);
            //    TranslateResultPanel.Children.Add(resultItem);
            //    ///  end
            //}));

        }


        class SourceTextContent : ObservableObject
        {

            private string text;

            public string BindingText
            {
                get => text;
                set => SetProperty(ref text, value);
            }
        }

        private void PreviousResult_Button_Click(object sender, RoutedEventArgs e)
        {
            string[] result = results.GetPreviousData();
            sourceTextContent.BindingText = result[0];
            int i = 1;
            foreach (ITranslator t in translators)
            {
                resultTextContent[t.Name].TranslatedResult = result[i];
                i++;
            }
        }

        private void NextResult_Button_Click(object sender, RoutedEventArgs e)
        {
            string[] result = results.GetNextData();
            sourceTextContent.BindingText = result[0];
            int i = 1;
            foreach (ITranslator t in translators)
            {
                resultTextContent[t.Name].TranslatedResult = result[i];
                i++;
            }
        }
    }
}

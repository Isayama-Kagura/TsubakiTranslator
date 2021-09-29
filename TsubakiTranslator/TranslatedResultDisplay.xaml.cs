using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        Dictionary<string, TranslatedData> displayTextContent;

        TextHookHandler textHookHandler;
        LinkedList<ITranslator> translators;
        TranslateDataList results;

        public TranslatedResultDisplay(TextHookHandler textHookHandler)
        {
            InitializeComponent();

            translators = TranslateHandler.GetSelectedTranslators(UserConfigPage.TranslateAPIConfig);

            displayTextContent = new Dictionary<string, TranslatedData>();
            foreach (ITranslator t in translators)
            {
                TranslatedResultItem resultItem = new TranslatedResultItem(t.Name, "");
                TranslateResultPanel.Children.Add(resultItem);
                displayTextContent.Add(t.Name, resultItem.TranslatedData);
            }
            

            sourceTextContent = new SourceTextContent();
            this.DataContext = sourceTextContent;
            

            //最多保留30条历史记录
            results = new TranslateDataList(30);

            this.textHookHandler = textHookHandler;

            textHookHandler.ProcessTextractor.OutputDataReceived += DisplayTranslateResult;

        }

        public void DisplayTranslateResult(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (textHookHandler.SelectedHookCode == null )
                return;

            string sourceText = TranslateHandler.RemoveDuplicatedChar(textHookHandler.HookDict[textHookHandler.SelectedHookCode], textHookHandler.DuplicateTimes);

            //hook跳转（一种可能），或者响应输出事件（设置了HookCode后的两种可能）
            if (results.Count() > 0 && results.GetCurrentData().SourceText.Equals(sourceText))
                return;

            //string result = await google.Translate(textHookHandler.HookDict[textHookHandler.SelectedHookCode]);

            TranslateData currentResult = new TranslateData(sourceText, new Dictionary<string, string>());
            results.AddTranslateData(currentResult);

            sourceTextContent.BindingText = currentResult.SourceText;

            foreach(var key in displayTextContent.Keys)
            {
                displayTextContent[key].TranslatedResult = "";
                currentResult.ResultText.Add(key,"");
            }

            Parallel.ForEach(translators,
                t => {  
                    string result = t.Translate(currentResult.SourceText);
                    currentResult.ResultText[t.Name] = result;
                    displayTextContent[t.Name].TranslatedResult = result;
                });

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

        //class StateObject
        //{
        //    public string  TranslatorName{ get; set; }
        //    public TranslateData TranslateData { get; set; }
        //    public Func<string, string> TranslateDelegate { get; set; }
        //}
        
        //private void TranslateCallback(IAsyncResult asyncResult)
        //{
        //    StateObject state = (StateObject)asyncResult.AsyncState;
        //    Func<string, string> translateDelegate = state.TranslateDelegate;
        //    string translatorName = state.TranslatorName;
        //    TranslateData translateData = state.TranslateData;

        //    string result = translateDelegate.EndInvoke(asyncResult);
        //    translateData.ResultText[translatorName] = result;
        //    ShowTranslateResult(translateData);

        //}

        private void ArrowLeft_Button_Click(object sender, RoutedEventArgs e)
        {
            TranslateData result = results.GetPreviousData();
            ShowTranslateResult(result);
        }

        private void ArrowRight_Button_Click(object sender, RoutedEventArgs e)
        {
            TranslateData result = results.GetNextData();
            ShowTranslateResult(result);
        }

        private void ChevronTripleLeft_Button_Click(object sender, RoutedEventArgs e)
        {
            TranslateData result = results.GetFirstData();
            ShowTranslateResult(result);
            
        }

        private void ChevronTripleRight_Button_Click(object sender, RoutedEventArgs e)
        {
            TranslateData result = results.GetLastData();
            ShowTranslateResult(result);
        }

        private void ShowTranslateResult(TranslateData data)
        {
            sourceTextContent.BindingText = data.SourceText;

            foreach (ITranslator t in translators)
                displayTextContent[t.Name].TranslatedResult = data.ResultText[t.Name];

        }

        private void SourceText_TextBlock_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Clipboard.SetDataObject(SourceText.Text);

            if (ResultDisplaySnackbar.MessageQueue is { } messageQueue)
                Task.Run(() => messageQueue.Enqueue("源文本已复制。", "好", () => { }));
        }
    }
}

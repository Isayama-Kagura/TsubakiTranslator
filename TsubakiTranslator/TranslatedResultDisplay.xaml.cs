using CommunityToolkit.Mvvm.ComponentModel;
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

        SourceTextHandler sourceTextHandler;

        public TranslatedResultDisplay(TextHookHandler textHookHandler, SourceTextHandler sourceTextHandler)
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

            this.sourceTextHandler = sourceTextHandler;

            textHookHandler.ProcessTextractor.OutputDataReceived += TranslateHookText;

        }

        public void TranslateHookText(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (textHookHandler.SelectedHookCode == null )
                return;

            Regex reg = new Regex(@"\[(.*?)\]");
            Match match = reg.Match(outLine.Data);

            if (match.Value.Length == 0)
                return;

            string hookcode = match.Groups[1].Value;

            if (!hookcode.Equals(textHookHandler.SelectedHookCode))
                return;

            string content = outLine.Data.Replace(match.Value, "").Trim();//实际获取到的内容


            string sourceText = sourceTextHandler.HandleText(content);


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

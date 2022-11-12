using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

        ClipboardHookHandler clipboardHookHandler;

        public bool TranslatorEnabled { get; set; } = true;

        private void Init()
        {
            SourceText.Foreground = new SolidColorBrush(App.WindowConfig.SourceTextColor);
            SourceText.FontFamily = new FontFamily(App.WindowConfig.SourceTextFontFamily);

            translators = TranslateHandler.GetSelectedTranslators(App.TranslateAPIConfig);

            displayTextContent = new Dictionary<string, TranslatedData>();
            foreach (ITranslator t in translators)
            {
                TranslatedResultItem resultItem = new TranslatedResultItem(t.Name, "");

                if(!App.WindowConfig.TranslatorNameVisibility)
                    resultItem.APINameTextBlock.Visibility = Visibility.Collapsed;

                resultItem.ResultTextBlock.Foreground = new SolidColorBrush(App.WindowConfig.TranslatedTextColor);
                resultItem.ResultTextBlock.FontFamily = new FontFamily(App.WindowConfig.TranslatedTextFontFamily);
                TranslateResultPanel.Children.Add(resultItem);
                displayTextContent.Add(t.Name, resultItem.TranslatedData);
            }


            sourceTextContent = new SourceTextContent();
            this.DataContext = sourceTextContent;

            //最多保留40条历史记录
            results = new TranslateDataList(40);
        }

        //对应注入模式
        public TranslatedResultDisplay(TextHookHandler textHookHandler, SourceTextHandler sourceTextHandler)
        {
            InitializeComponent();

            Init();

            this.textHookHandler = textHookHandler;

            this.sourceTextHandler = sourceTextHandler;

            textHookHandler.ProcessTextractor.OutputDataReceived += TranslateHookText;

        }

        //对应剪切板翻译模式
        public TranslatedResultDisplay(ClipboardHookHandler clipboardHookHandler, SourceTextHandler sourceTextHandler)
        {
            InitializeComponent();

            Init();

            this.clipboardHookHandler = clipboardHookHandler;

            this.sourceTextHandler = sourceTextHandler;

            this.clipboardHookHandler.ClipboardUpdated += TranslteClipboardText;
        }

        //对应OCR翻译模式
        public TranslatedResultDisplay()
        {
            InitializeComponent();

            Init();

        }

        public void TranslateHookText(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!TranslatorEnabled || outLine.Data == null)
                return;

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

            if (Regex.Replace(sourceText, @"\s", "").Equals(""))
                return;


            Task.Run(() => TranslateAndDisplay(sourceText));

        }

        public void TranslteClipboardText(object sender, EventArgs e)
        {
            if (!TranslatorEnabled)
                return;

            IDataObject iData = Clipboard.GetDataObject();
           
            if (!iData.GetDataPresent(DataFormats.Text))
                return;

            string sourceText = Clipboard.GetText();
            sourceText = Regex.Replace(sourceText, @"[\r\n\t\f]", "");
            sourceText = sourceTextHandler.HandleText(sourceText);
            Task.Run(()=> TranslateAndDisplay(sourceText));
        }

        

        public void TranslateAndDisplay(string sourceText)
        {
            TranslateData currentResult = new TranslateData(sourceText, new Dictionary<string, string>());
            results.AddTranslateData(currentResult);

            sourceTextContent.BindingText = currentResult.SourceText;

            foreach (var key in displayTextContent.Keys)
            {
                displayTextContent[key].TranslatedResult = "";
                currentResult.ResultText.Add(key, "");
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
            if (results.Count() == 0)
                return;
            TranslateData result = results.GetPreviousData();
            ShowTranslateResult(result);
        }

        private void ArrowRight_Button_Click(object sender, RoutedEventArgs e)
        {
            if (results.Count() == 0)
                return;
            TranslateData result = results.GetNextData();
            ShowTranslateResult(result);
        }

        private void ChevronTripleLeft_Button_Click(object sender, RoutedEventArgs e)
        {
            if (results.Count() == 0)
                return;
            TranslateData result = results.GetFirstData();
            ShowTranslateResult(result);
            
        }

        private void ChevronTripleRight_Button_Click(object sender, RoutedEventArgs e)
        {
            if (results.Count() == 0)
                return;
            TranslateData result = results.GetLastData();
            ShowTranslateResult(result);
        }

        private void ShowTranslateResult(TranslateData data)
        {
            sourceTextContent.BindingText = data.SourceText;

            foreach (ITranslator t in translators)
                displayTextContent[t.Name].TranslatedResult = data.ResultText[t.Name];

        }

    }
}

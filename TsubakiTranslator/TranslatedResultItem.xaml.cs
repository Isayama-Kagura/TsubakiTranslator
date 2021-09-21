using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TsubakiTranslator
{
    /// <summary>
    /// TranslatedResultDisplay.xaml 的交互逻辑
    /// </summary>
    public partial class TranslatedResultItem : UserControl
    {
        public TranslatedData TranslatedData { get; }

        public TranslatedResultItem(string apiName, string translatedResult)
        {
            InitializeComponent();

            TranslatedData = new TranslatedData();
            TranslatedData.TranslatorName = apiName;
            TranslatedData.TranslatedResult = translatedResult;

            this.DataContext = TranslatedData;
        }

        
    }

    public class TranslatedData : ObservableObject
    {

        private string translatorName;
        private string translatedResult;

        public string TranslatorName
        {
            get => translatorName;
            set => SetProperty(ref translatorName, value);
        }
        public string TranslatedResult
        {
            get => translatedResult;
            set => SetProperty(ref translatedResult, value);
        }

    }
}

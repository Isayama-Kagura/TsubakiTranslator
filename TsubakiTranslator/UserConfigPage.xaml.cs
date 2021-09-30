using System.Windows.Controls;
using TsubakiTranslator.BasicLibrary;

namespace TsubakiTranslator
{
    /// <summary>
    /// UserConfig.xaml 的交互逻辑
    /// </summary>
    public partial class UserConfigPage : UserControl
    {

        public static TranslateAPIConfig TranslateAPIConfig
        {
            get;
            set;
        }

        public UserConfigPage()
        {
            InitializeComponent();

            UserConfigPage.TranslateAPIConfig = FileHandler.DeserializeObject<TranslateAPIConfig>(System.AppDomain.CurrentDomain.BaseDirectory + @"config/APIConfig.json", new TranslateAPIConfig());
            this.DataContext = TranslateAPIConfig;
        }

        private void SelectPath_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string path = FileHandler.SelectPath();
            TranslateAPIConfig.LEPath = path;
        }
    }


}

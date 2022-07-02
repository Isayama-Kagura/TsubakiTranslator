using System.Windows.Controls;
using TsubakiTranslator.BasicLibrary;

namespace TsubakiTranslator
{
    /// <summary>
    /// UserConfig.xaml 的交互逻辑
    /// </summary>
    public partial class UserConfigPage : UserControl
    {

        public UserConfigPage()
        {
            InitializeComponent();
            this.DataContext = App.TranslateAPIConfig;
        }

        //private void SelectPath_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    string path = FileHandler.SelectPath();
        //    TranslateAPIConfig.LEPath = path;
        //}
    }


}

using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace TsubakiTranslator
{
    /// <summary>
    /// AboutMePage.xaml 的交互逻辑
    /// </summary>
    public partial class AboutMePage : UserControl
    {
        public AboutMePage()
        {
            InitializeComponent();
        }

        private void GitHubButton_OnClick(object sender, RoutedEventArgs e)
        {

            // 激活的是当前默认的浏览器
            Process.Start("explorer.exe", "https://github.com/Isayama-Kagura/TsubakiTranslator");
        }
    }
}

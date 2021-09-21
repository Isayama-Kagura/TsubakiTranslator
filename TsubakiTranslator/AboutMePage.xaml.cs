using System;
using System.Collections.Generic;
using System.Diagnostics;
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

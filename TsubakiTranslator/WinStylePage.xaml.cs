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
    /// WinStylePage.xaml 的交互逻辑
    /// </summary>
    public partial class WinStylePage : UserControl
    {
        private class ColorItem
        {
            public ColorItem(string content, Color value)
            {
                Content = content;
                Value = value;
            }

            public string Content { get;}
            public Color Value { get;}
        }

        public WinStylePage()
        {
            InitializeComponent();

            List<ColorItem> list = new List<ColorItem>();

            list.Add(new ColorItem("烟白", Colors.WhiteSmoke));
            list.Add(new ColorItem("原木", Colors.BurlyWood));
            list.Add(new ColorItem("天蓝", Colors.SkyBlue));
            list.Add(new ColorItem("金菊黄", Colors.Goldenrod));
            list.Add(new ColorItem("石灰绿", Colors.LimeGreen));
            list.Add(new ColorItem("苍紫罗兰", Colors.PaleVioletRed));
            

            SourceTextColorComboBox.ItemsSource = list;
            TranslatedTextColorComboBox.ItemsSource = list;
        }

        private void DefaultTransparency_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.WindowConfig.TranslateWindowTransparency = 165;
        }
    }
}

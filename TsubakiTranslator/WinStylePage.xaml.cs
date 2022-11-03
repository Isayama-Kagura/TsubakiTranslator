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
using System.Windows.Markup;
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
        
        public WinStylePage()
        {
            InitializeComponent();

            SetTextColorList();

            SetTextFontFamilyList();
        }

        private void SetTextColorList()
        {
            List<Object> list = new List<Object>();

            list.Add(new { Content = "烟白", Value = Colors.WhiteSmoke });
            list.Add(new { Content = "原木", Value = Colors.BurlyWood });
            list.Add(new { Content = "天蓝", Value = Colors.SkyBlue });
            list.Add(new { Content = "金菊黄", Value = Colors.Goldenrod });
            list.Add(new { Content = "石灰绿", Value = Colors.LimeGreen });
            list.Add(new { Content = "苍紫罗兰", Value = Colors.PaleVioletRed });


            SourceTextColorComboBox.ItemsSource = list;
            TranslatedTextColorComboBox.ItemsSource = list;
        }

        private void SetTextFontFamilyList()
        {
            List<string> fontFamilies = new List<string>();

            foreach (FontFamily ff in Fonts.SystemFontFamilies)
            {
                LanguageSpecificStringDictionary fontDic = ff.FamilyNames;

                string fontName = null;
                if (fontDic.TryGetValue(XmlLanguage.GetLanguage("zh-cn"), out fontName))
                    fontFamilies.Add(fontName);
            }

            SourceTextFontFamilyComboBox.ItemsSource = fontFamilies;
            TranslatedTextFontFamilyComboBox.ItemsSource = fontFamilies;

        }

        private void DefaultTransparency_Button_Click(object sender, RoutedEventArgs e)
        {
            App.WindowConfig.TranslateWindowTransparency = 165;
            App.WindowConfig.SourceTextColor = Colors.BurlyWood;
            App.WindowConfig.TranslatedTextColor = Colors.WhiteSmoke;
            App.WindowConfig.TranslatorNameVisibility = true;
            App.WindowConfig.SourceTextFontFamily = "Microsoft YaHei UI";
            App.WindowConfig.TranslatedTextFontFamily = "Microsoft YaHei UI";
        }
    }
}

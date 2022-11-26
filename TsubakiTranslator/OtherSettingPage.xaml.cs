using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using TsubakiTranslator.BasicLibrary;

namespace TsubakiTranslator
{
    /// <summary>
    /// OcrSettingPage.xaml 的交互逻辑
    /// </summary>
    public partial class OtherSettingPage : UserControl
    {
        private byte HotkeysModifiers { get; set; }
        private int HotkeysKey { get; set; }
        private string HotkeysText { get; set; } = "";

        private OtherConfig OtherConfig { get; }

        public OtherSettingPage()
        {
            InitializeComponent();

            OtherConfig = App.OtherConfig;
            this.DataContext = App.OtherConfig;

        }

        //快捷键复用
        private void HotkeyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            HotkeysModifiers = 0;
            HotkeysKey = 0;
            HotkeysText = "";
            Key key = (e.Key == Key.System ? e.SystemKey : e.Key);
            if (key == Key.LeftShift || key == Key.RightShift
                || key == Key.LeftCtrl || key == Key.RightCtrl
                || key == Key.LeftAlt || key == Key.RightAlt
                || key == Key.LWin || key == Key.RWin)
            {
                return;
            }
            StringBuilder shortcutText = new StringBuilder();
            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                HotkeysModifiers += 2;
                shortcutText.Append("Ctrl + ");
            }
            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                HotkeysModifiers += 4;
                shortcutText.Append("Shift + ");
            }
            if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0)
            {
                HotkeysModifiers += 1;
                shortcutText.Append("Alt + ");
            }
            if (HotkeysModifiers == 0 && (key < Key.F1 || key > Key.F12))
            {
                HotkeysKey = 0;
                shortcutText.Clear();
                ((TextBox)sender).Text = HotkeysText = "";
                return;
            }
            HotkeysKey = KeyInterop.VirtualKeyFromKey(key);
            shortcutText.Append(key.ToString());
            ((TextBox)sender).Text = HotkeysText = shortcutText.ToString();
        }

        private void ScreenshotHotkeyTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Key key = (e.Key == Key.System ? e.SystemKey : e.Key);
            if (key == Key.LeftShift || key == Key.RightShift
                || key == Key.LeftCtrl || key == Key.RightCtrl
                || key == Key.LeftAlt || key == Key.RightAlt
                || key == Key.LWin || key == Key.RWin)
            {
                return;
            }
            App.OtherConfig.ScreenshotHotkey.Modifiers = HotkeysModifiers;
            App.OtherConfig.ScreenshotHotkey.Key = HotkeysKey;
            App.OtherConfig.ScreenshotHotkey.Text = HotkeysText.ToString();
            //HotKeysUtil.ReRegisterHotKey();
            //HotKeyConflictCheck();
        }

        //private void ScreenshotHotkeyConflictCheck()
        //{
        //this.OcrHotKeyConflictLabel.Visibility = GlobalConfig.HotKeys.Ocr.Conflict ? Visibility.Visible : Visibility.Hidden;
        //this.GetWordsTranslateHotKeyConflictLabel.Visibility = GlobalConfig.HotKeys.GetWordsTranslate.Conflict ? Visibility.Visible : Visibility.Hidden;
        //this.ScreenshotTranslateHotKeyConflictLabel.Visibility = GlobalConfig.HotKeys.ScreenshotTranslate.Conflict ? Visibility.Visible : Visibility.Hidden;
        //this.TopMostHotKeyConflictLabel.Visibility = GlobalConfig.HotKeys.TopMost.Conflict ? Visibility.Visible : Visibility.Hidden;
        //}

        private void SelectFolder_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string path = FileHandler.SelectFolderPath();

            App.OtherConfig.LogFolderPath = path;
        }
    }
}

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TsubakiTranslator.BasicLibrary;
using System.Windows.Interop;
using MaterialDesignThemes.Wpf;
using System.Runtime.Versioning;
using System.Timers;
using System.Drawing;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TsubakiTranslator
{
    /// <summary>
    /// TranslateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TranslateWindow : Window
    {
        private Window mainWindow { get; set; }
        private TranslatedResultDisplay TranslatedResultDisplay { get; set; }
        private HookResultDisplay HookResultDisplay { get; set; }
        private TextHookHandler textHookHandler;
        public TextHookHandler TextHookHandler { get => textHookHandler; }

        private ClipboardHookHandler clipboardHookHandler;
        private TTSHandler TTSHandler { get; set; }

        private OcrProgram ocrProgram;
        private OcrProgram OcrProgram { get => ocrProgram; }

        private HotkeyHandler hotkeyHandler;
        private HotkeyHandler HotkeyHandler { get=> hotkeyHandler; }

        private Timer autoOcrTimer;
        private Timer AutoOcrTimer { get => autoOcrTimer; }
        private Bitmap LastOcrBitmap { get; set; } = new Bitmap(100,100);
        private string LastOcrResult { get; set; } = "";
        private void Init()
        {
            this.DataContext = App.WindowConfig;
            AutoScreenshotButton.DataContext = App.OcrConfig;

            if (!App.WindowConfig.TranslateWindowTopmost)
            {
                PackIcon packIcon = new PackIcon();
                packIcon.Kind = PackIconKind.PinOff;
                PinButton.Content = packIcon;
            }

            this.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb((byte)App.WindowConfig.TranslateWindowTransparency, 0, 0, 0));

            if (!App.WindowConfig.SourceTextVisibility)
            {
                TranslatedResultDisplay.SourceText.Visibility = Visibility.Collapsed;
                PackIcon packIcon = new PackIcon();
                packIcon.Kind = PackIconKind.BookOff;
                SourceTextButton.Content = packIcon;
            }

            //TTS
            if (App.TranslateAPIConfig.TTSIsEnabled)
            {
                TTSButton.Visibility = Visibility.Visible;


                if (App.TranslateAPIConfig.SourceLanguage.Equals("Japanese"))
                {
                    this.TTSHandler = new TTSHandler(App.TranslateAPIConfig.TTSRegion, App.TranslateAPIConfig.TTSResourceKey, "ja-JP", "ja-JP-NanamiNeural");
                }

                else
                {
                    this.TTSHandler = new TTSHandler(App.TranslateAPIConfig.TTSRegion, App.TranslateAPIConfig.TTSResourceKey, "en-US", "en-US-AmberNeural");
                }

            }
                

        }

        //Hook文本模式
        public TranslateWindow(Window mainWindow, TextHookHandler textHookHandler, SourceTextHandler sourceTextHandler )
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.textHookHandler = textHookHandler;

            //注意顺序，Hook窗口的事件处理先于Translate结果窗口
            HookResultDisplay = new HookResultDisplay(this);
            TranslatedResultDisplay = new TranslatedResultDisplay(textHookHandler, sourceTextHandler);

            Init();

            textHookHandler.ProcessGame.Exited += GameExitHandler;
            textHookHandler.ProcessTextractor.OutputDataReceived += On_TextHook_OutputDataReceived;

            TranslateWindowContent.Content = HookResultDisplay;

            TranslatedResultPanel.Visibility = Visibility.Hidden;
        }

        //监视剪切板模式
        public TranslateWindow(Window mainWindow, SourceTextHandler sourceTextHandler)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;

            clipboardHookHandler = new ClipboardHookHandler(mainWindow);
            TranslatedResultDisplay = new TranslatedResultDisplay(clipboardHookHandler, sourceTextHandler);

            Init();

            //textHookHandler.ProcessGame.Exited += GameExitHandler;

            TranslateWindowContent.Content = TranslatedResultDisplay;

            if (TranslatedResultDisplay.ResultDisplaySnackbar.MessageQueue is { } messageQueue)
                Task.Run(() => messageQueue.Enqueue("剪切板文本发生变化时将自动翻译。", "好", () => { }));
        }

        //OCR模式
        
        public TranslateWindow(Window mainWindow)
        {
            InitializeComponent();

            ScreenshotButton.Visibility = Visibility.Visible;

            AutoScreenshotButton.Visibility = Visibility.Visible;
            

            this.mainWindow = mainWindow;

            TranslatedResultDisplay = new TranslatedResultDisplay();

            Init();

            ocrProgram = new OcrProgram(App.TranslateAPIConfig.SourceLanguage);

            TranslateWindowContent.Content = TranslatedResultDisplay;

        }

        public void GameExitHandler(object sendingProcess, EventArgs outLine)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
                this.Close()
            ));
        }

        public void On_TextHook_OutputDataReceived(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (outLine.Data == null)
                return;

            Regex reg = new Regex(@"\[(.*?)\]");
            Match match = reg.Match(outLine.Data);

            if (match.Value.Length == 0)
                return;

            string content = outLine.Data.Replace(match.Value, "").Trim();//实际获取到的内容
            string hookcode = match.Groups[1].Value;

            HookResultDisplay.UpdateHookResultItem(hookcode,content);

            if (TranslatedResultDisplay.TranslatorEnabled && textHookHandler.SelectedHookCode != null 
                && hookcode.Equals(textHookHandler.SelectedHookCode))
            {
                TranslatedResultDisplay.TranslateHookText(content);
            }

        }

        //供Hook文本选择界面使用
        public void SwitchToTranslateDisplay(string currentText)
        {
            if(!TranslateWindowMenu.IsEnabled)
                TranslateWindowMenu.IsEnabled = true;

            TranslateWindowContent.Content = TranslatedResultDisplay;

            if (TranslatedResultDisplay.ResultDisplaySnackbar.MessageQueue is { } messageQueue)
                Task.Run(() => messageQueue.Enqueue("点击左上角菜单可重新选择源文本。", "好", () => { }));

            //翻译当前选择的文本
            TranslatedResultDisplay.TranslateHookText(currentText);

            TranslatedResultPanel.Visibility = Visibility.Visible;
        }


        private void On_TranslateWindow_Closed(object sender, EventArgs e)
        {
            if (TextHookHandler != null)
            {
                TextHookHandler.ProcessTextractor.OutputDataReceived -= On_TextHook_OutputDataReceived;
                textHookHandler.ProcessGame.Exited -= GameExitHandler;
                TextHookHandler.CloseTextractor();
            }
            if (clipboardHookHandler != null)
            {
                clipboardHookHandler.ClipboardUpdated -= TranslatedResultDisplay.TranslteClipboardText;
                clipboardHookHandler.Dispose();
            }
            if(TTSHandler != null)
            {
                TTSHandler.Dispose();
            }

            mainWindow.Show();

            mainWindow.Topmost = true;
            mainWindow.Topmost = false;
        }

        private void TranslateWindow_CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Tranlate_Display_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            TranslateWindowContent.Content = TranslatedResultDisplay;
            TranslatedResultPanel.Visibility = Visibility.Visible;
        }

        private void Hook_Display_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            TranslateWindowContent.Content = HookResultDisplay;
            TranslatedResultPanel.Visibility = Visibility.Hidden;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            /* 如何在Window.ResizeMode属性为CanResize的时候，阻止窗口拖动到屏幕边缘自动最大化。
               (When the Window.ResizeMode property is CanResize, 
               when the window is dragged to the edge of the screen, 
               it prevents the window from automatically maximizing.)*/
            if (e.ChangedButton == MouseButton.Left)
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    var windowMode = this.ResizeMode;
                    if (this.ResizeMode != ResizeMode.NoResize)
                    {
                        this.ResizeMode = ResizeMode.NoResize;
                    }

                    this.UpdateLayout();


                    /* 当点击拖拽区域的时候，让窗口跟着移动
                    (When clicking the drag area, make the window follow) */
                    DragMove();


                    if (this.ResizeMode != windowMode)
                    {
                        this.ResizeMode = windowMode;
                    }

                    this.UpdateLayout();
                }
            }

        }

        private void Pin_Button_Click(object sender, RoutedEventArgs e)
        {
            PackIcon packIcon = new PackIcon();
            if (this.Topmost)
            {
                packIcon.Kind = PackIconKind.PinOff;
                PinButton.Content = packIcon;
                this.Topmost = false;
                //TopmostTimer.Stop();
            }
            else
            {
                packIcon.Kind = PackIconKind.Pin;
                PinButton.Content = packIcon;
                this.Topmost = true;
                //TopmostTimer.Start();
            }
        }

        private void TranslateWindow_MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void FormatFontSizeDecrease_Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var t in TranslatedResultDisplay.TranslateResultPanel.Children)
                if (t is TextBox)
                    ((TextBox)t).FontSize--;
                else
                    ((TranslatedResultItem)t).DecreaseFontSize();
        }

        private void FormatFontSizeIncrease_Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var t in TranslatedResultDisplay.TranslateResultPanel.Children)
                if (t is TextBox)
                    ((TextBox)t).FontSize++;
                else
                    ((TranslatedResultItem)t).IncreaseFontSize();
        }

        private async void VolumeSource_Button_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            string sourceText = TranslatedResultDisplay.SourceText.Text;

            if(!sourceText.Equals(""))
                flag = await TTSHandler.SpeakTextAsync(sourceText);

            if (!flag && TranslatedResultDisplay.ResultDisplaySnackbar.MessageQueue is { } messageQueue)
                await Task.Run(() => messageQueue.Enqueue($"{TTSHandler.ErrorMessage}", "好", () => { }));

        }


        private void DisplaySourceText_Button_Click(object sender, RoutedEventArgs e)
        {
            PackIcon packIcon = new PackIcon();
            if (App.WindowConfig.SourceTextVisibility)
            {
                App.WindowConfig.SourceTextVisibility = false;
                packIcon.Kind = PackIconKind.BookOff;
                SourceTextButton.Content = packIcon;
                TranslatedResultDisplay.SourceText.Visibility = Visibility.Collapsed;
            }
            else
            {
                App.WindowConfig.SourceTextVisibility = true;
                packIcon.Kind = PackIconKind.Book;
                SourceTextButton.Content = packIcon;
                TranslatedResultDisplay.SourceText.Visibility = Visibility.Visible;
            }
        }

        private void TranslatorEnable_Button_Click(object sender, RoutedEventArgs e)
        {
            PackIcon packIcon = new PackIcon();
            if (TranslatedResultDisplay.TranslatorEnabled)
            {
                TranslatedResultDisplay.TranslatorEnabled = false;
                packIcon.Kind = PackIconKind.Play;
                TranslatorEnableButton.Content = packIcon;
                TranslatorEnableButton.ToolTip = "启动翻译";
            }
            else
            {
                TranslatedResultDisplay.TranslatorEnabled = true;
                packIcon.Kind = PackIconKind.Pause;
                TranslatorEnableButton.Content = packIcon;
                TranslatorEnableButton.ToolTip = "暂停翻译";
            }
        }

        [SupportedOSPlatform("windows10.0.10240")]
        private async void Screenshot_Button_Click(object sender, RoutedEventArgs e)
        {
            //两种OCR模式的不同处理
            if (App.OcrConfig.IsAutoScreenshot)
            {
                AutoOcrTimer.Stop();

                await ScreenshotWindow.Start();

                if (ScreenshotWindow.DrawRegion != default)
                    AutoOcrTimer.Start();
            }
            else if(TranslatedResultDisplay.TranslatorEnabled)
            {
                await ScreenshotWindow.Start();

                string ocrResult = default;

                if (ScreenshotWindow.Bitmap != null)
                    ocrResult = await OcrProgram.RecognizeAsync(ScreenshotWindow.Bitmap);

                if (ocrResult!= null && !ocrResult.Trim().Equals(""))
                    TranslatedResultDisplay.TranslateAndDisplay(ocrResult);
            }

        }

        [SupportedOSPlatform("windows10.0.10240")]
        private void TranslateWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(OcrProgram != null)
            {
                ScreenshotWindow.Current = null;
            }
            if (HotkeyHandler != null)
            {
                //截屏热键处理
                HotkeyHandler.UnRegisterHotKey();
                HwndSource source = HwndSource.FromHwnd(HotkeyHandler.MainFormHandle);
                source.RemoveHook(WndProc);
            }

            if(AutoOcrTimer != null)
            {
                AutoOcrTimer.Stop();
                AutoOcrTimer.Dispose();
            }

        }


        //OCR模式
        [SupportedOSPlatform("windows10.0.10240")]
        private void TranslateWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (OcrProgram == null)
                return;

            //截屏热键处理
            IntPtr handle = new WindowInteropHelper(this).Handle;
            hotkeyHandler = new HotkeyHandler();
            HotkeyHandler.RegisterHotKey(handle, App.OcrConfig.ScreenshotHotkey);

            HwndSource source = HwndSource.FromHwnd(handle);
            source.AddHook(WndProc);

            //设置定时间隔(毫秒为单位)
            int interval = App.OcrConfig.Interval*1000;
            autoOcrTimer = new System.Timers.Timer(interval);
            //设置执行一次（false）还是一直执行(true)
            AutoOcrTimer.AutoReset = true;
            //绑定Elapsed事件
            AutoOcrTimer.Elapsed += new System.Timers.ElapsedEventHandler(async (s,e) =>
            {
                if (!TranslatedResultDisplay.TranslatorEnabled)
                    return;

                Bitmap bitmap = BasicLibrary.ScreenshotHandler.GetCapture(ScreenshotWindow.DrawRegion);

                if(!ScreenshotHandler.ImageBase64Compare(bitmap,LastOcrBitmap))
                {
                    string ocrResult = await OcrProgram.RecognizeAsync(bitmap);

                    if(!LastOcrResult.Equals(ocrResult))
                        if (ocrResult != null && !ocrResult.Trim().Equals(""))
                            TranslatedResultDisplay.TranslateAndDisplay(ocrResult);

                    LastOcrResult = ocrResult;
                }

                LastOcrBitmap = bitmap;

            });

        }

        /// <summary>
        /// 热键的功能
        /// </summary>
        /// <param name="m"></param>
        [SupportedOSPlatform("windows10.0.10240")]
        protected IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handle)
        {
            switch (msg)
            {
                case 0x0312: //这个是window消息定义的 注册的热键消息
                    if (wParam.ToString().Equals(HotkeyHandler.Id + ""))
                    {
                        this.Screenshot_Button_Click(null, null);
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        private void TranslateWindow_MouseEnter(object sender, MouseEventArgs e)
        {
            if (App.WindowConfig.TranslateWindowTopmost)
            {
                this.Topmost = false;
                this.Topmost = true;
            }
        }

        private void AutoScreenshot_ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (App.OcrConfig.ScreenshotHotkey.Conflict)
            {
                if (TranslatedResultDisplay.ResultDisplaySnackbar.MessageQueue is { } messageQueue)
                    Task.Run(() => messageQueue.Enqueue($"自动模式，快捷键（{App.OcrConfig.ScreenshotHotkey.Text}）冲突！按界面按钮选定识别区域。", "好", () => { }));
            }
            else if (TranslatedResultDisplay.ResultDisplaySnackbar.MessageQueue is { } messageQueue)
                Task.Run(() => messageQueue.Enqueue($"自动模式，按界面按钮或快捷键（{App.OcrConfig.ScreenshotHotkey.Text}）选定识别区域。", "好", () => { }));
        }

        private void AutoScreenshot_ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            AutoOcrTimer.Stop();
            if (App.OcrConfig.ScreenshotHotkey.Conflict)
            {
                if (TranslatedResultDisplay.ResultDisplaySnackbar.MessageQueue is { } messageQueue)
                    Task.Run(() => messageQueue.Enqueue($"手动模式，快捷键（{App.OcrConfig.ScreenshotHotkey.Text}）冲突！按界面按钮截图识别。", "好", () => { }));
            }
            else if (TranslatedResultDisplay.ResultDisplaySnackbar.MessageQueue is { } messageQueue)
                Task.Run(() => messageQueue.Enqueue($"手动模式，按界面按钮或快捷键（{App.OcrConfig.ScreenshotHotkey.Text}）截图识别。", "好", () => { }));
        }
    }
}

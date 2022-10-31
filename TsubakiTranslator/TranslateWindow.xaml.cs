using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TsubakiTranslator.BasicLibrary;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using MaterialDesignThemes.Wpf;
using System.Runtime.Versioning;

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

        private SpeechSynthesizer synthesizer;

        private OcrProgram ocrProgram;
        private OcrProgram OcrProgram { get => ocrProgram; }

        private HotkeyHandler hotkeyHandler;
        private HotkeyHandler HotkeyHandler { get=> hotkeyHandler; }
        private void Init()
        {
            this.DataContext = App.WindowConfig;

            if (!App.WindowConfig.TranslateWindowTopmost)
            {
                PackIcon packIcon = new PackIcon();
                packIcon.Kind = PackIconKind.PinOff;
                PinButton.Content = packIcon;
            }

            this.Background = new SolidColorBrush(Color.FromArgb((byte)App.WindowConfig.TranslateWindowTransparency, 0, 0, 0));

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
                var config = SpeechConfig.FromSubscription(App.TranslateAPIConfig.TTSResourceKey, App.TranslateAPIConfig.TTSRegion);
                config.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Riff24Khz16BitMonoPcm);
                // Note: if only language is set, the default voice of that language is chosen.
                if (App.TranslateAPIConfig.SourceLanguage.Equals("Japanese"))
                {
                    // The voice setting will overwrite language setting.
                    // The voice setting will not overwrite the voice element in input SSML.
                    config.SpeechSynthesisLanguage = "ja-JP";
                    config.SpeechSynthesisVoiceName = "ja-JP-NanamiNeural";
                }

                else
                {
                    config.SpeechSynthesisLanguage = "en-US";
                    config.SpeechSynthesisVoiceName = "en-US-AmberNeural";
                }

                synthesizer = new SpeechSynthesizer(config);
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

            this.mainWindow = mainWindow;

            ocrProgram = new OcrProgram(App.TranslateAPIConfig.SourceLanguage);

            TranslatedResultDisplay = new TranslatedResultDisplay();

            Init();

            TranslateWindowContent.Content = TranslatedResultDisplay;

            if (TranslatedResultDisplay.ResultDisplaySnackbar.MessageQueue is { } messageQueue)
                Task.Run(() => messageQueue.Enqueue("点击截取文本按钮或快捷键进行OCR识别翻译。", "好", () => { }));
        }


        

        public void GameExitHandler(object sendingProcess, EventArgs outLine)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
                this.Close()
            ));
        }

        //供Hook文本选择界面使用
        public void SwitchToTranslateDisplay()
        {
            if(!TranslateWindowMenu.IsEnabled)
                TranslateWindowMenu.IsEnabled = true;

            TranslateWindowContent.Content = TranslatedResultDisplay;

            if (TranslatedResultDisplay.ResultDisplaySnackbar.MessageQueue is { } messageQueue)
                Task.Run(() => messageQueue.Enqueue("点击左上角菜单可重新选择源文本。", "好", () => { }));

            //翻译当前选择的文本
            Task.Run(()=>TranslatedResultDisplay.TranslateHookText(new Object(), textHookHandler.HookHandlerDict[textHookHandler.SelectedHookCode]));

            TranslatedResultPanel.Visibility = Visibility.Visible;
        }


        private void On_TranslateWindow_Closed(object sender, EventArgs e)
        {
            if (TextHookHandler != null)
            {
                TextHookHandler.ProcessTextractor.OutputDataReceived -= HookResultDisplay.DisplayHookResult;
                TextHookHandler.ProcessTextractor.OutputDataReceived -= TranslatedResultDisplay.TranslateHookText;
                textHookHandler.ProcessGame.Exited -= GameExitHandler;
                TextHookHandler.CloseTextractor();
            }
            if (clipboardHookHandler != null)
            {
                clipboardHookHandler.ClipboardUpdated -= TranslatedResultDisplay.TranslteClipboardText;
                clipboardHookHandler.Dispose();
            }

           
            //TopmostTimer.Stop();
            //topmostTimer = null;
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

        private void VolumeSource_Button_Click(object sender, RoutedEventArgs e)
        {
            string sourceText = TranslatedResultDisplay.SourceText.Text;
            Parallel.Invoke(async () => { await synthesizer.SpeakTextAsync(sourceText); });
        }


        private void SourceText_Button_Click(object sender, RoutedEventArgs e)
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
            await ScreenshotWindow.Start();

            if(ScreenshotWindow.Bitmap != null)
            {
                string ocrResult = await OcrProgram.RecognizeAsync(ScreenshotWindow.Bitmap);

                if (TranslatedResultDisplay.TranslatorEnabled && !ocrResult.Trim().Equals(""))
                    TranslatedResultDisplay.TranslateAndDisplay(ocrResult);
            }
        }

        [SupportedOSPlatform("windows10.0.10240")]
        private void TranslateWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (HotkeyHandler != null)
            {
                //截屏热键处理
                HotkeyHandler.UnRegisterHotKey();
                HwndSource source = HwndSource.FromHwnd(HotkeyHandler.MainFormHandle);
                source.RemoveHook(WndProc);
            }
        }

        private void TranslateWindow_Deactivated(object sender, EventArgs e)
        {
            if (App.WindowConfig.TranslateWindowTopmost)
            {
                WindowInteropHelper helper = new WindowInteropHelper(this);
                User32.BringWindowToTop(HwndSource.FromHwnd(helper.Handle).Handle);
            }

        }

        [SupportedOSPlatform("windows10.0.10240")]
        private void TranslateWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //截屏热键处理
            IntPtr handle = new WindowInteropHelper(this).Handle;
            hotkeyHandler = new HotkeyHandler();
            HotkeyHandler.RegisterHotKey(handle, App.OcrConfig.ScreenshotHotkey);

            HwndSource source = HwndSource.FromHwnd(handle);
            source.AddHook(WndProc);
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

        
    }
}

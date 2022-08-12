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

        private DispatcherTimer timer;

        private SpeechSynthesizer synthesizer;
        public bool IsHookMode { get;  }

        private void Init()
        {
            this.DataContext = App.WindowConfig;

            //确保翻译窗口永远在前
            WindowInteropHelper helper = new WindowInteropHelper(this);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, e) => BringWindowToTop(HwndSource.FromHwnd(helper.Handle).Handle);

            if (App.WindowConfig.TranslateWindowTopmost)
            {
                timer.Start();
            }
            else
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
                TTSButton.IsEnabled = true;
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
            IsHookMode = true;


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
            IsHookMode = false;


            //注意顺序，Hook窗口的事件处理先于Translate结果窗口
            //HookResultDisplay = new HookResultDisplay(this);
            //TranslatedResultDisplay = new TranslatedResultDisplay(textHookHandler, sourceTextHandler);
            clipboardHookHandler = new ClipboardHookHandler(mainWindow);
            TranslatedResultDisplay = new TranslatedResultDisplay(clipboardHookHandler, sourceTextHandler);

            Init();

            //textHookHandler.ProcessGame.Exited += GameExitHandler;

            TranslateWindowContent.Content = TranslatedResultDisplay;

            if (TranslatedResultDisplay.ResultDisplaySnackbar.MessageQueue is { } messageQueue)
                Task.Run(() => messageQueue.Enqueue("剪切板文本发生变化时将自动翻译。", "好", () => { }));
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


        private void On_TranslateWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IsHookMode)
            {
                TextHookHandler.ProcessTextractor.OutputDataReceived -= HookResultDisplay.DisplayHookResult;
                TextHookHandler.ProcessTextractor.OutputDataReceived -= TranslatedResultDisplay.TranslateHookText;
                textHookHandler.ProcessGame.Exited -= GameExitHandler;
                TextHookHandler.CloseTextractor();
            }
            else
            {
                clipboardHookHandler.ClipboardUpdated -= TranslatedResultDisplay.TranslteClipboardText;
                clipboardHookHandler.Dispose();
            }
            timer.Stop();
            mainWindow.Show();
            
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
                timer.Stop();
            }
            else
            {
                packIcon.Kind = PackIconKind.Pin;
                PinButton.Content = packIcon;
                this.Topmost = true;
                timer.Start();
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

        /// <summary>   
        /// 该函数将指定的窗口设置到Z序的顶部。   
        /// </summary>   
        /// 
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int BringWindowToTop(IntPtr hWnd);


        
    }
}

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TsubakiTranslator.BasicLibrary;

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

        public bool IsHookMode { get;  }

        private void Init()
        {
            this.DataContext = MainWindow.WindowConfig;
            if (MainWindow.WindowConfig.TranslateWindowTopmost)
            {
                PinButton.Visibility = Visibility.Visible;
                PinOffButton.Visibility = Visibility.Collapsed;
            }
            this.Background = new SolidColorBrush(Color.FromArgb((byte)MainWindow.WindowConfig.TranslateWindowTransparency, 0, 0, 0));
        }

        //Hook文本模式
        public TranslateWindow(Window mainWindow, TextHookHandler textHookHandler, SourceTextHandler sourceTextHandler )
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.textHookHandler = textHookHandler;

            Init();
            IsHookMode = true;


            //注意顺序，Hook窗口的事件处理先于Translate结果窗口
            HookResultDisplay = new HookResultDisplay(this);
            TranslatedResultDisplay = new TranslatedResultDisplay(textHookHandler, sourceTextHandler);

            textHookHandler.ProcessGame.Exited += GameExitHandler;

            TranslateWindowContent.Content = HookResultDisplay;
        }

        //监视剪切板模式
        public TranslateWindow(Window mainWindow, SourceTextHandler sourceTextHandler)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;

            Init();
            IsHookMode = false;


            //注意顺序，Hook窗口的事件处理先于Translate结果窗口
            //HookResultDisplay = new HookResultDisplay(this);
            //TranslatedResultDisplay = new TranslatedResultDisplay(textHookHandler, sourceTextHandler);
            clipboardHookHandler = new ClipboardHookHandler(mainWindow);
            TranslatedResultDisplay = new TranslatedResultDisplay(clipboardHookHandler, sourceTextHandler);

            //textHookHandler.ProcessGame.Exited += GameExitHandler;

            TranslateWindowContent.Content = TranslatedResultDisplay;
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
                Task.Run(() => messageQueue.Enqueue("点击左上角按钮可重新选择源文本。", "好", () => { }));

            //翻译当前选择的文本
            Task.Run(()=>TranslatedResultDisplay.TranslateHookText(new Object(), textHookHandler.HookHandlerDict[textHookHandler.SelectedHookCode]));

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

            mainWindow.Show();
            
        }

        private void TranslateWindow_CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Tranlate_Display_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            TranslateWindowContent.Content = TranslatedResultDisplay;
        }

        private void Hook_Display_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            TranslateWindowContent.Content = HookResultDisplay;
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
            PinButton.Visibility = Visibility.Collapsed;
            PinOffButton.Visibility = Visibility.Visible;
            this.Topmost = false;
        }

        private void PinOff_Button_Click(object sender, RoutedEventArgs e)
        {
            PinButton.Visibility = Visibility.Visible;
            PinOffButton.Visibility = Visibility.Collapsed;
            this.Topmost = true;
        }

        private void TranslateWindow_MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void TranslateWindow_MouseEnter(object sender, MouseEventArgs e)
        {
            TranslatedResultDisplay.SourceText.Visibility = Visibility.Visible;
        }

        private void TranslateWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            TranslatedResultDisplay.SourceText.Visibility = Visibility.Collapsed;
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
    }
}

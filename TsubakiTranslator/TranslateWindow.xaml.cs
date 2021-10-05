using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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


        public TranslateWindow(Window mainWindow, TextHookHandler textHookHandler, SourceTextHandler sourceTextHandler )
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.textHookHandler = textHookHandler;

            this.Height = MainWindow.WindowConfig.TranslateWindowHeight;
            this.Width = MainWindow.WindowConfig.TranslateWindowWidth;
            this.Left = MainWindow.WindowConfig.TranslateWindowLeft;
            this.Top = MainWindow.WindowConfig.TranslateWindowTop;
            this.Topmost = MainWindow.WindowConfig.TranslateWindowTopmost;
            if (this.Topmost)
            {
                PinButton.Visibility = Visibility.Visible;
                PinOffButton.Visibility = Visibility.Collapsed;
            }


            //注意顺序，Hook窗口的事件处理先于Translate结果窗口
            HookResultDisplay = new HookResultDisplay(this);
            TranslatedResultDisplay = new TranslatedResultDisplay(textHookHandler, sourceTextHandler);

            textHookHandler.ProcessGame.Exited += GameExitHandler;

            HookDisplayButton.IsChecked = true;

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
            TranslateDisplayButton.IsEnabled = true;
            TranslateDisplayButton.IsChecked = true;

            if (TranslatedResultDisplay.ResultDisplaySnackbar.MessageQueue is { } messageQueue)
                Task.Run(() => messageQueue.Enqueue("右键源文本可复制至剪切板。", "好", () => { }));

            //翻译当前选择的文本
            Task.Run(()=>TranslatedResultDisplay.TranslateHookText(new Object(), textHookHandler.HookHandlerDict[textHookHandler.SelectedHookCode]));

        }


        private void On_TranslateWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.WindowConfig.TranslateWindowHeight = this.Height;
            MainWindow.WindowConfig.TranslateWindowWidth = this.Width;
            MainWindow.WindowConfig.TranslateWindowLeft = this.Left;
            MainWindow.WindowConfig.TranslateWindowTop = this.Top;
            MainWindow.WindowConfig.TranslateWindowTopmost = this.Topmost;

            TextHookHandler.ProcessTextractor.OutputDataReceived -= HookResultDisplay.DisplayHookResult;
            TextHookHandler.ProcessTextractor.OutputDataReceived -= TranslatedResultDisplay.TranslateHookText;
            textHookHandler.ProcessGame.Exited -= GameExitHandler;
            TextHookHandler.CloseTextractor();

            mainWindow.Show();
            
        }

        private void TranslateWindow_CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Tranlate_Display_Button_Checked(object sender, RoutedEventArgs e)
        {
            TranslateWindowContent.Content = TranslatedResultDisplay;
        }

        private void Hook_Display_Button_Checked(object sender, RoutedEventArgs e)
        {
            TranslateWindowContent.Content = HookResultDisplay;
        }

        private void ColorZone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();//实现整个窗口的拖动
        }

        private void Pin_Button_Click(object sender, RoutedEventArgs e)
        {
            PinButton.Visibility =  Visibility.Collapsed;
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
            TranslateWindowMenu.Visibility = Visibility.Visible;
        }

        private void TranslateWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            TranslateWindowMenu.Visibility = Visibility.Collapsed;
        }
    }
}

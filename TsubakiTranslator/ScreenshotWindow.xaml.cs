using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using TsubakiTranslator.BasicLibrary;

namespace TsubakiTranslator
{
    /// <summary>
    /// ScreenshotWindowTest.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenshotWindow : Window
    {
        public ScreenshotWindow()
        {
            InitializeComponent();
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


        public Bitmap TakeScreenshot()
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            User32.RECT rect1;
            User32.GetWindowRect(HwndSource.FromHwnd(helper.Handle).Handle, out rect1);

            // get te hDC of the target window
            //IntPtr hdcSrc = User32.GetWindowDC(hWnd);
            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(HwndSource.FromHwnd(helper.Handle).Handle, out windowRect);

            var rect = new Rect(Math.Round((double)windowRect.Left, 0),
                    Math.Round((double)windowRect.Top, 0),
                    Math.Round((double)windowRect.Width, 0),
                    Math.Round((double)windowRect.Height, 0));

            Bitmap image = ScreenshotHandler.GetCapture(rect);

            //image.Save(@"C:\Users\HP\Desktop\test.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            return image;
        }

    }
}

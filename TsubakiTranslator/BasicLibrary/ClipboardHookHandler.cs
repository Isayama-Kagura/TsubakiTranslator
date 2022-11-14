using System;
using System.Windows;
using System.Windows.Interop;

namespace TsubakiTranslator.BasicLibrary
{
    public class ClipboardHookHandler : IDisposable
    {
        const int WM_CLIPBOARDUPDATE = 0x031D;



        HwndSource _hwndSource;

        public void Dispose()
        {
            _hwndSource.RemoveHook(new HwndSourceHook(OnHooked));
            User32.RemoveClipboardFormatListener(_hwndSource.Handle);
            //_hwndSource?.Dispose();
        }

        public ClipboardHookHandler(Window window)
        {
            WindowInteropHelper helper = new WindowInteropHelper(window);
            _hwndSource = HwndSource.FromHwnd(helper.Handle);
            bool r = User32.AddClipboardFormatListener(_hwndSource.Handle);
            if (r)
            {
                _hwndSource.AddHook(new HwndSourceHook(OnHooked));
            }
        }

        private IntPtr OnHooked(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_CLIPBOARDUPDATE)
            {
                ClipboardUpdated?.Invoke(this, EventArgs.Empty);
                return IntPtr.Zero;
            }
            return IntPtr.Zero;
        }

        public event EventHandler ClipboardUpdated;
    }
}


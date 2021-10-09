using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace TsubakiTranslator.BasicLibrary
{
    public class ClipboardHookHandler : IDisposable
    {
        const int WM_CLIPBOARDUPDATE = 0x031D;

        [DllImport("User32.dll")]
        static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("User32.dll")]
        static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        HwndSource _hwndSource;

        public void Dispose()
        {
            _hwndSource.RemoveHook(new HwndSourceHook(OnHooked));
            RemoveClipboardFormatListener(_hwndSource.Handle);
            //_hwndSource?.Dispose();
        }

        public ClipboardHookHandler(Window window)
        {
            WindowInteropHelper helper = new WindowInteropHelper(window);
            _hwndSource = HwndSource.FromHwnd(helper.Handle);
            bool r = AddClipboardFormatListener(_hwndSource.Handle);
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


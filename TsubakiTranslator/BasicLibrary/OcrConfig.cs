using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsubakiTranslator.BasicLibrary
{
    public class OcrConfig : ObservableObject
    {
        private bool isAutoScreenshot = false;
        private ScreenshotHotkey screenshotHotkey = new ScreenshotHotkey();
        private int interval = 2;

        public bool IsAutoScreenshot
        {
            get => isAutoScreenshot;
            set => SetProperty(ref isAutoScreenshot, value);
        }
        public ScreenshotHotkey ScreenshotHotkey
        {
            get => screenshotHotkey;
            set => SetProperty(ref screenshotHotkey, value);
        }

        public int Interval
        {
            get => interval;
            set => SetProperty(ref interval, value);
        }
    }
    public class ScreenshotHotkey : ObservableObject
    {
        private byte modifiers = 0;
        private int key = 115;
        private string text = "F4";
        private bool conflict = false;

        public byte Modifiers
        {
            get => modifiers;
            set => SetProperty(ref modifiers, value);
        }

        public int Key
        {
            get => key;
            set => SetProperty(ref key, value);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public bool Conflict
        {
            get => conflict;
            set => SetProperty(ref conflict, value);
        }
    }
}

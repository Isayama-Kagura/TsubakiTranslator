using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace TsubakiTranslator.BasicLibrary
{
    public class ConstantValues
    {
        public enum Language { Japanese, English };
    }

    public partial class OtherConfig : ObservableObject
    {
        [ObservableProperty]
        private bool isAutoScreenshot = false;
        [ObservableProperty]
        private ScreenshotHotkey screenshotHotkey = new ScreenshotHotkey();
        [ObservableProperty]
        private int interval = 3;
        [ObservableProperty]
        private int sourceLangIndex = 0;
        [ObservableProperty]
        private bool saveLogEnabled = false;
        [ObservableProperty]
        private string logFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    }
    public partial class ScreenshotHotkey : ObservableObject
    {
        [ObservableProperty]
        private byte modifiers = 0;
        [ObservableProperty]
        private int key = 115;
        [ObservableProperty]
        private string text = "F4";
        [ObservableProperty]
        private bool conflict = false;

    }
}

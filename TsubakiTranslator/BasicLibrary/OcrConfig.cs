using CommunityToolkit.Mvvm.ComponentModel;

namespace TsubakiTranslator.BasicLibrary
{
    public partial class OcrConfig : ObservableObject
    {
        [ObservableProperty]
        private bool isAutoScreenshot = false;
        [ObservableProperty]
        private ScreenshotHotkey screenshotHotkey = new ScreenshotHotkey();
        [ObservableProperty]
        private int interval = 2;

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

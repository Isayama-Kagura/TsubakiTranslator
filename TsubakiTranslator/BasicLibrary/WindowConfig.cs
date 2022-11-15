using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace TsubakiTranslator.BasicLibrary
{
    public partial class WindowConfig : ObservableObject
    {
        [ObservableProperty]
        private double mainWindowWidth = 400;
        [ObservableProperty]
        private double mainWindowHeight = 800;
        [ObservableProperty]
        private double translateWindowHeight = 400;
        [ObservableProperty]
        private double translateWindowWidth = 800;
        [ObservableProperty]
        private double translateWindowLeft = 400;
        [ObservableProperty]
        private double translateWindowTop = 200;
        [ObservableProperty]
        private bool translateWindowTopmost = false;
        [ObservableProperty]
        private int translateWindowTransparency = 165;
        [ObservableProperty]
        private Color sourceTextColor = Colors.BurlyWood;
        [ObservableProperty]
        private Color translatedTextColor = Colors.WhiteSmoke;
        [ObservableProperty]
        private bool sourceTextVisibility = true;
        [ObservableProperty]
        private bool translatorNameVisibility = true;
        [ObservableProperty]
        private string sourceTextFontFamily = "Microsoft YaHei UI";
        [ObservableProperty]
        private string translatedTextFontFamily = "Microsoft YaHei UI";

    }
}

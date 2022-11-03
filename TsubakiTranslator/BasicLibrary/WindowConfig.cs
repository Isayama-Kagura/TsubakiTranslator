using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace TsubakiTranslator.BasicLibrary
{
    public class WindowConfig : ObservableObject
    {
        private double mainWindowWidth = 400;
        private double mainWindowHeight = 800;
        private double translateWindowHeight = 400;
        private double translateWindowWidth = 800;
        private double translateWindowLeft = 400;
        private double translateWindowTop = 200;
        private bool translateWindowTopmost = false;
        private int translateWindowTransparency = 165;
        private Color sourceTextColor = Colors.BurlyWood;
        private Color translatedTextColor = Colors.WhiteSmoke;
        private bool sourceTextVisibility = true;
        private bool translatorNameVisibility = true;
        private string sourceTextFontFamily = "Microsoft YaHei UI";
        private string translatedTextFontFamily = "Microsoft YaHei UI";

        public double MainWindowWidth
        {
            get => mainWindowWidth;
            set => SetProperty(ref mainWindowWidth, value);
        }
        public double MainWindowHeight
        {
            get => mainWindowHeight;
            set => SetProperty(ref mainWindowHeight, value);
        }

        public double TranslateWindowHeight
        {
            get => translateWindowHeight;
            set => SetProperty(ref translateWindowHeight, value);
        }

        public double TranslateWindowWidth
        {
            get => translateWindowWidth;
            set => SetProperty(ref translateWindowWidth, value);
        }

        public double TranslateWindowLeft
        {
            get => translateWindowLeft;
            set => SetProperty(ref translateWindowLeft, value);
        }

        public double TranslateWindowTop
        {
            get => translateWindowTop;
            set => SetProperty(ref translateWindowTop, value);
        }

        public bool TranslateWindowTopmost
        {
            get => translateWindowTopmost;
            set => SetProperty(ref translateWindowTopmost, value);
        }

        public int TranslateWindowTransparency
        {
            get => translateWindowTransparency;
            set => SetProperty(ref translateWindowTransparency, value);
        }

        public Color SourceTextColor
        {
            get => sourceTextColor;
            set => SetProperty(ref sourceTextColor, value);
        }

        public Color TranslatedTextColor
        {
            get => translatedTextColor;
            set => SetProperty(ref translatedTextColor, value);
        }

        public bool SourceTextVisibility
        {
            get => sourceTextVisibility;
            set => SetProperty(ref sourceTextVisibility, value);
        }

        public bool TranslatorNameVisibility
        {
            get => translatorNameVisibility;
            set => SetProperty(ref translatorNameVisibility, value);
        }

        public string SourceTextFontFamily
        {
            get => sourceTextFontFamily;
            set => SetProperty(ref sourceTextFontFamily, value);
        }

        public string TranslatedTextFontFamily
        {
            get => translatedTextFontFamily;
            set => SetProperty(ref translatedTextFontFamily, value);
        }
    }
}

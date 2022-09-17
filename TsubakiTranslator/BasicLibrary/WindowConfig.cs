using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace TsubakiTranslator.BasicLibrary
{
    public class WindowConfig : ObservableObject
    {
        public WindowConfig()
        {
            MainWindowHeight = 400;
            MainWindowWidth = 800;

            TranslateWindowHeight = 400;
            TranslateWindowWidth = 800;
            TranslateWindowLeft = 400;
            TranslateWindowTop = 200;
            TranslateWindowTopmost = false;
            TranslateWindowTransparency = 165;
            SourceTextColor = Colors.BurlyWood;
            TranslatedTextColor = Colors.WhiteSmoke;
            SourceTextVisibility = true;
            TranslatorNameVisibility = true;
        }

        private double mainWindowWidth;
        private double mainWindowHeight;
        private double translateWindowHeight;
        private double translateWindowWidth;
        private double translateWindowLeft;
        private double translateWindowTop;
        private bool translateWindowTopmost;
        private int translateWindowTransparency;
        private Color sourceTextColor;
        private Color translatedTextColor;
        private bool sourceTextVisibility;
        private bool translatorNameVisibility;


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
    }
}

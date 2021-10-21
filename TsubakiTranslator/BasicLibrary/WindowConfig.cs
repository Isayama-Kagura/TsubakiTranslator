using CommunityToolkit.Mvvm.ComponentModel;

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
        }

        private double mainWindowWidth;
        private double mainWindowHeight;
        private double translateWindowHeight;
        private double translateWindowWidth;
        private double translateWindowLeft;
        private double translateWindowTop;
        private bool translateWindowTopmost;
        private int translateWindowTransparency;


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

    }
}

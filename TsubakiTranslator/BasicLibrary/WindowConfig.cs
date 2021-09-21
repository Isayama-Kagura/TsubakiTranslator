using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsubakiTranslator.BasicLibrary
{
    public class WindowConfig 
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
        }        

        public double MainWindowWidth
        {
            get;
            set;
        }
        public double MainWindowHeight
        {
            get;
            set;
        }

        public double TranslateWindowHeight
        {
            get;
            set;
        }

        public double TranslateWindowWidth
        {
            get;
            set;
        }

        public double TranslateWindowLeft
        {
            get;
            set;
        }

        public double TranslateWindowTop
        {
            get;
            set;
        }

        public bool TranslateWindowTopmost
        {
            get;
            set;
        }

    }
}

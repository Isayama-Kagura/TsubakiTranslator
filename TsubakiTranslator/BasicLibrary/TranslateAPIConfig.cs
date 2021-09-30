using CommunityToolkit.Mvvm.ComponentModel;

namespace TsubakiTranslator.BasicLibrary
{
    public class TranslateAPIConfig: ObservableObject
    {
        private bool baiduIsEnabled;
        private string baiduAppID;
        private string baiduSecretKey;

        private bool caiyunIsEnabled;
        private string caiyunToken;

        private bool deeplIsEnabled;
        private string deeplSecretKey;

        private bool ibmIsEnabled;

        private bool googleIsEnabled;

        //LE配置暂时先跟翻译API放在一起
        private bool leIsEnabled;
        private string lePath;

        private bool tencentIsEnabled;
        private string tencentSecretID;
        private string tencentSecretKey;

        private bool xiaoniuIsEnabled;

        private bool yandexIsEnabled;
        private string yandexAPIKey;

        private bool yeekitIsEnabled;

        private bool youdaoIsEnabled;

        public string LEPath
        {
            get => lePath;
            set => SetProperty(ref lePath, value);
        }

        public bool LEIsEnabled
        {
            get => leIsEnabled;
            set => SetProperty(ref leIsEnabled, value);
        }

        public bool IbmIsEnabled
        {
            get => ibmIsEnabled;
            set => SetProperty(ref ibmIsEnabled, value);
        }

        public bool GoogleIsEnabled
        {
            get => googleIsEnabled;
            set => SetProperty(ref googleIsEnabled, value);
        }

        public bool BaiduIsEnabled
        {
            get => baiduIsEnabled;
            set => SetProperty(ref baiduIsEnabled, value);
        }

        public string BaiduAppID
        {
            get => baiduAppID;
            set => SetProperty(ref baiduAppID, value);
        }

        public string BaiduSecretKey
        {
            get => baiduSecretKey;
            set => SetProperty(ref baiduSecretKey, value);
        }

        public bool CaiyunIsEnabled
        {
            get => caiyunIsEnabled;
            set => SetProperty(ref caiyunIsEnabled, value);
        }

        public string CaiyunToken
        {
            get => caiyunToken;
            set => SetProperty(ref caiyunToken, value);
        }

        public bool DeeplIsEnabled
        {
            get => deeplIsEnabled;
            set => SetProperty(ref deeplIsEnabled, value);
        }

        public string DeeplSecretKey
        {
            get => deeplSecretKey;
            set => SetProperty(ref deeplSecretKey, value);
        }

        public bool TencentIsEnabled
        {
            get => tencentIsEnabled;
            set => SetProperty(ref tencentIsEnabled, value);
        }

        public string TencentSecretID
        {
            get => tencentSecretID;
            set => SetProperty(ref tencentSecretID, value);
        }

        public string TencentSecretKey
        {
            get => tencentSecretKey;
            set => SetProperty(ref tencentSecretKey, value);
        }

        public bool XiaoniuIsEnabled
        {
            get => xiaoniuIsEnabled;
            set => SetProperty(ref xiaoniuIsEnabled, value);
        }


        public bool YandexIsEnabled
        {
            get => yandexIsEnabled;
            set => SetProperty(ref yandexIsEnabled, value);
        }

        public string YandexAPIKey
        {
            get => yandexAPIKey;
            set => SetProperty(ref yandexAPIKey, value);
        }

        public bool YeekitIsEnabled
        {
            get => yeekitIsEnabled;
            set => SetProperty(ref yeekitIsEnabled, value);
        }

        public bool YoudaoIsEnabled
        {
            get => youdaoIsEnabled;
            set => SetProperty(ref youdaoIsEnabled, value);
        }
    }
}

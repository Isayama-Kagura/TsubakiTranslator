using CommunityToolkit.Mvvm.ComponentModel;

namespace TsubakiTranslator.BasicLibrary
{
    public class TranslateAPIConfig : ObservableObject
    {
        private string sourceLanguage = "Japanese";

        private bool ttsIsEnabled;
        private string ttsRegion;
        private string ttsResourceKey;

        private bool aliIsEnabled;
        private string aliSecretId;
        private string aliSecretKey;

        private bool baiduIsEnabled;
        private string baiduAppID;
        private string baiduSecretKey;

        private bool bingIsEnabled;

        private bool caiyunIsEnabled;
        private string caiyunToken;

        private bool deeplIsEnabled;
        private bool deeplIsFreeApi;
        private string deeplSecretKey;

        private bool ibmIsEnabled;

        private bool iCiBaIsEnabled;

        private bool tencentIsEnabled;
        private string tencentSecretID;
        private string tencentSecretKey;

        private bool xiaoniuIsEnabled;
        private string xiaoniuApiKey;

        private bool volcengineIsEnabled;

        private bool yeekitIsEnabled;


        public string SourceLanguage
        {
            get => sourceLanguage;
            set => SetProperty(ref sourceLanguage, value);
        }

        public bool TTSIsEnabled
        {
            get => ttsIsEnabled;
            set => SetProperty(ref ttsIsEnabled, value);
        }

        public string TTSRegion
        {
            get => ttsRegion;
            set => SetProperty(ref ttsRegion, value);
        }

        public string TTSResourceKey
        {
            get => ttsResourceKey;
            set => SetProperty(ref ttsResourceKey, value);
        }

        public bool AliIsEnabled
        {
            get => aliIsEnabled;
            set => SetProperty(ref aliIsEnabled, value);
        }

        public string AliSecretId
        {
            get => aliSecretId;
            set => SetProperty(ref aliSecretId, value);
        }

        public string AliSecretKey
        {
            get => aliSecretKey;
            set => SetProperty(ref aliSecretKey, value);
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

        public bool BingIsEnabled
        {
            get => bingIsEnabled;
            set => SetProperty(ref bingIsEnabled, value);
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

        public bool DeeplIsFreeApi
        {
            get => deeplIsFreeApi;
            set => SetProperty(ref deeplIsFreeApi, value);
        }

        public string DeeplSecretKey
        {
            get => deeplSecretKey;
            set => SetProperty(ref deeplSecretKey, value);
        }

        public bool IbmIsEnabled
        {
            get => ibmIsEnabled;
            set => SetProperty(ref ibmIsEnabled, value);
        }

        public bool ICiBaIsEnabled
        {
            get => iCiBaIsEnabled;
            set => SetProperty(ref iCiBaIsEnabled, value);
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

        public string XiaoniuApiKey
        {
            get => xiaoniuApiKey;
            set => SetProperty(ref xiaoniuApiKey, value);
        }

        public bool VolcengineIsEnabled
        {
            get => volcengineIsEnabled;
            set => SetProperty(ref volcengineIsEnabled, value);
        }

        public bool YeekitIsEnabled
        {
            get => yeekitIsEnabled;
            set => SetProperty(ref yeekitIsEnabled, value);
        }

    }
}

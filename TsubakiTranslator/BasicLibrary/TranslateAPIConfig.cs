using CommunityToolkit.Mvvm.ComponentModel;

namespace TsubakiTranslator.BasicLibrary
{
    public partial class TranslateAPIConfig : ObservableObject
    {
        [ObservableProperty]
        private bool ttsIsEnabled;
        [ObservableProperty]
        private string ttsRegion;
        [ObservableProperty]
        private string ttsResourceKey;

        [ObservableProperty]
        private bool aliIsEnabled;
        [ObservableProperty]
        private string aliSecretId;
        [ObservableProperty]
        private string aliSecretKey;

        [ObservableProperty]
        private bool baiduIsEnabled;
        [ObservableProperty]
        private string baiduAppID;
        [ObservableProperty]
        private string baiduSecretKey;

        [ObservableProperty]
        private bool bingIsEnabled;

        [ObservableProperty]
        private bool caiyunIsEnabled;
        [ObservableProperty]
        private string caiyunToken;

        [ObservableProperty]
        private bool deeplIsEnabled;
        [ObservableProperty]
        private bool deeplIsFreeApi;
        [ObservableProperty]
        private string deeplSecretKey;

        [ObservableProperty]
        private bool ibmIsEnabled;

        [ObservableProperty]
        private bool iCiBaIsEnabled;

        [ObservableProperty]
        private bool tencentIsEnabled;
        [ObservableProperty]
        private string tencentSecretID;
        [ObservableProperty]
        private string tencentSecretKey;

        [ObservableProperty]
        private bool xiaoniuIsEnabled;
        [ObservableProperty]
        private string xiaoniuApiKey;

        [ObservableProperty]
        private bool volcengineIsEnabled;

        [ObservableProperty]
        private bool yeekitIsEnabled;

    }
}

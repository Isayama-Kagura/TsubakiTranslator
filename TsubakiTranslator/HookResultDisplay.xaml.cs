using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace TsubakiTranslator
{
    /// <summary>
    /// HookResultDisplay.xaml 的交互逻辑
    /// </summary>
    public partial class HookResultDisplay : UserControl
    {
        private Dictionary<string, HookData> HookItemDict;
        private TranslateWindow translateWindow;

        public HookResultDisplay(TranslateWindow translateWindow)
        {
            InitializeComponent();

            HookItemDict = new Dictionary<string, HookData>();
            this.translateWindow = translateWindow;

            HookDataSet = new ObservableCollection<HookData>();
            HookDataGrid.ItemsSource = HookDataSet;
        }

        public void UpdateHookResultItem(string hookcode, string content)
        {
            if (HookItemDict.ContainsKey(hookcode))
                HookItemDict[hookcode].HookText = content;
            else
            {
                //Dispatcher是一个线程控制器，要控制线程里跑的东西，就要经过它。
                //WPF里面，有个所谓UI线程，后台代码不能直接操作UI控件，需要控制，就要通过这个Dispatcher。
                App.Current.Dispatcher.Invoke((Action)(() =>
                {
                    /// start 你的逻辑代码
                    //HookResultItem item = new HookResultItem(hookcode, content, translateWindow);
                    //HookItemDict.Add(hookcode, item);
                    //DisplayStackPanel.Children.Add(item);
                    HookData item = new HookData(hookcode, content);
                    HookItemDict.Add(hookcode, item);
                    HookDataSet.Add(item);
                    ///  end
                }));
                

            }
        }

        public HashSet<HookData> GetSelectedHookData()
        {
            HashSet<HookData> result = new HashSet<HookData>();
            foreach(HookData data in HookDataSet)
            {
                if (data.IsSelected)
                {
                    result.Add(data);
                }
            }
            return result;
        }

        private ObservableCollection<HookData> HookDataSet { get; }

        
    }

    public class HookData : ObservableObject
    {

        private string hookCode;
        private string hookText;
        private bool isSelected;

        public HookData(string hookCode, string hookText)
        {
            this.hookCode = hookCode;
            this.hookText = hookText;
        }
        public string HookCode
        {
            get => hookCode;
            set => SetProperty(ref hookCode, value);
        }
        public string HookText
        {
            get => hookText;
            set => SetProperty(ref hookText, value);
        }

        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

    }
}

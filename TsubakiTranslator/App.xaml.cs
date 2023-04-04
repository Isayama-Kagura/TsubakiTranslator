using System.Diagnostics;
using System.Linq;
using System.Windows;
using TsubakiTranslator.BasicLibrary;

namespace TsubakiTranslator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;

        public static WindowConfig WindowConfig { get; private set; }

        public static GamesConfig GamesConfig { get; private set; }

        public static TranslateAPIConfig TranslateAPIConfig { get; private set; }

        public static OtherConfig OtherConfig { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            WindowConfig = FileHandler.DeserializeObject<WindowConfig>(
                baseDir + @"config/WindowConfig.json"
            ) ?? new WindowConfig();

            GamesConfig = FileHandler.DeserializeObject<GamesConfig>(
                baseDir + @"config/GamesData.json"
            ) ?? new GamesConfig();

            TranslateAPIConfig =
                FileHandler.DeserializeObject<TranslateAPIConfig>(baseDir +
                                                                  @"config/APIConfig.json") ??
                new TranslateAPIConfig();

            OtherConfig = FileHandler.DeserializeObject<OtherConfig>(
                baseDir + @"config/OtherConfig.json"
            ) ?? new OtherConfig();

            var processes = Process.GetProcessesByName("TsubakiTranslator")
                .Where(proc => proc.Id != System.Environment.ProcessId);
            foreach (var proc in processes)
            {
                try
                {
                    proc.Kill();
                }
                catch
                {
                    // ignored
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            FileHandler.SerializeObject<WindowConfig>(WindowConfig,
                baseDir + @"config/WindowConfig.json");
            FileHandler.SerializeObject<GamesConfig>(GamesConfig,
                baseDir + @"config/GamesData.json");
            FileHandler.SerializeObject<TranslateAPIConfig>(TranslateAPIConfig,
                baseDir + @"config/APIConfig.json");
            FileHandler.SerializeObject<OtherConfig>(OtherConfig,
                baseDir + @"config/OtherConfig.json");

            base.OnExit(e);
        }
    }
}
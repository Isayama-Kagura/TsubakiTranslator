using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace TsubakiTranslator.BasicLibrary
{
    public class TextHookHandler
    {
        /// <summary>
        /// Textractor进程
        /// </summary>
        private Process processTextractor;
        public Process ProcessTextractor { get => processTextractor; }

        private Process processGame;
        public Process ProcessGame { get => processGame; }


        public string SelectedHookCode { get; set; }

        public TextHookHandler(Process p, string hookCode)
        {
            Init(p, hookCode);
        }


        ~TextHookHandler()
        {
            CloseTextractor();
        }

        /// <summary>
        /// 初始化Textractor,建立CLI与本软件间的通信
        /// </summary>
        /// <returns>成功返回真，失败返回假</returns>
        public async void Init(Process gameProcess, string hookCode)
        {
            bool isX86 = ProcessHelper.IsWinX86(gameProcess);

            //当游戏退出时可以获得退出事件。
            processGame = gameProcess;
            processGame.EnableRaisingEvents = true;

            string path = Environment.CurrentDirectory + @"\Resources\Textractor\" + (isX86 ? "x86" : "x64") + @"\TextractorCLI.exe";//打开对应的Textractor路径

            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.Unicode,
                StandardInputEncoding = new UnicodeEncoding(false, false),
                FileName = path,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            processTextractor = Process.Start(processStartInfo);
            await AttachProcess();

            if (hookCode != null)
                await AttachProcessByHookCode(hookCode);

            ProcessTextractor.BeginOutputReadLine();

        }

        /// <summary>
        /// 向Textractor CLI写入命令
        /// 注入进程
        /// </summary>
        /// <param name="pid"></param>
        private async Task AttachProcess()
        {
            //ProcessTextractor.StandardInput.WriteLine("attach -P" + GamePID);
            //Console.Write("attach -P" + GamePID);

            //适用多个同名进程的情况，只在通过进程启动有效。
            Process[] processes = Process.GetProcessesByName(ProcessGame.ProcessName);

            foreach (Process process in processes)
            {
                await ProcessTextractor.StandardInput.WriteLineAsync("attach -P" + process.Id);
                await ProcessTextractor.StandardInput.FlushAsync();
            }

        }

        /// <summary>
        /// 向Textractor CLI写入命令
        /// 结束注入进程
        /// </summary>
        /// <param name="pid"></param>
        private async Task DetachProcess()
        {
            //适用多个同名进程的情况，只在通过进程启动有效。
            Process[] processes = Process.GetProcessesByName(ProcessGame.ProcessName);
            foreach (Process process in processes)
            {
                await ProcessTextractor.StandardInput.WriteLineAsync("detach -P" + process.Id);
                await ProcessTextractor.StandardInput.FlushAsync();
            }
        }

        /// <summary>
        /// 向Textractor CLI写入命令
        /// 给定特殊码注入，由Textractor作者指导方法
        /// </summary>
        /// <param name="pid"></param>
        private async Task AttachProcessByHookCode(string hookCode)
        {
            //解决有hookcode时莫名的报错。
            //await Task.Delay(10);

            Process[] processes = Process.GetProcessesByName(ProcessGame.ProcessName);
            foreach (Process process in processes)
            {
                await ProcessTextractor.StandardInput.WriteLineAsync(hookCode + " -P" + process.Id);
                await ProcessTextractor.StandardInput.FlushAsync();
            }
        }

        /// <summary>
        /// 关闭Textractor进程，关闭前Detach所有Hook
        /// </summary>
        public async void CloseTextractor()
        {
            /*
             * TODO:!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
             * 这里如果进程退出会出现异常
             */
            if (ProcessTextractor != null && ProcessTextractor.HasExited == false)
            {
                await DetachProcess();
                ProcessTextractor.Kill();

            }

            processTextractor = null;
        }

    }
}

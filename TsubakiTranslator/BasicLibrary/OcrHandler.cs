using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsubakiTranslator.BasicLibrary
{
    public class OcrHandler
    {
        /// <summary>
        /// WinOCR进程
        /// </summary>
        private Process ocrProcess;
        public Process OcrProcess { get => ocrProcess; }

        private string language;
        public OcrHandler(string srcLang)
        {
            if (srcLang.Equals("Japanese"))
            {
                language = "ja";
            }
            else if (srcLang.Equals("English"))
            {
                language = "en-US";
            }

            string path = Environment.CurrentDirectory + @"\Resources\WinOCR\WinOCR.exe";//打开对应的WinOCR路径

            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                //StandardOutputEncoding = Encoding.Unicode,
                //StandardInputEncoding = new UnicodeEncoding(false, false),
                FileName = path,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            ocrProcess = Process.Start(processStartInfo);

            //ProcessTextractor.OutputDataReceived += (S, E) => { MessageBox.Show(E.Data); };
            OcrProcess.BeginOutputReadLine();

        }


        /// <summary>
        /// 向WinOCR CLI写入命令
        /// 结束注入进程
        /// </summary>
        /// <param name="pid"></param>
        public async Task RecognizeText(string imagePath)
        {
            await OcrProcess.StandardInput.WriteLineAsync($"recognize {language} {imagePath}");
            await OcrProcess.StandardInput.FlushAsync();
            
        }

        /// <summary>
        /// 关闭WinOCR进程
        /// </summary>
        public void CloseWinOCR()
        {
            /*
             * TODO:!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
             * 这里如果进程退出会出现异常
             */
            if (OcrProcess != null && OcrProcess.HasExited == false)
            {
                OcrProcess.Kill();
            }

            ocrProcess = null;
        }

        ~OcrHandler()
        {
            CloseWinOCR();
        }
    }
}

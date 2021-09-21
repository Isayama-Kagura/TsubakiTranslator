using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace TsubakiTranslator.BasicLibrary
{
    class FileHandler
    {
        public static string SelectPath()
        {
            string path = string.Empty;
            var openFileDialog = new OpenFileDialog()
            {
                Filter = "可执行文件|*.exe|所有文件|*.*"//如果需要筛选txt文件（"Files (*.txt)|*.txt"）
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                path = openFileDialog.FileName;
            }
            return path;
        }

        public static void SerializeObject<T>(T value, string path)
        {
            var jsonString = JsonSerializer.SerializeToUtf8Bytes<T>(value);
            try
            {
                using(FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(jsonString, 0, jsonString.Length);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message); 
            }
        }

        public static T DeserializeObject<T>(string path, T defaultResult)
        {
            T result = defaultResult;
            if(CreateFileIfNotExist(path))
                SerializeObject<T>(result, path);

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    byte[] byteArray = new byte[fs.Length];
                    fs.Read(byteArray, 0, byteArray.Length);
                    var utf8Reader = new Utf8JsonReader(byteArray);
                    result = JsonSerializer.Deserialize<T>(ref utf8Reader);
                }

            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return result;
        }

        public static bool CreateFileIfNotExist(string path)
        {
            string directory = System.IO.Path.GetDirectoryName(path);
            bool flag = false;

            if (!Directory.Exists(directory))     // 返回bool类型，存在返回true，不存在返回false
            {
                Directory.CreateDirectory(directory);      //不存在则创建路径
            }

            if (!File.Exists(path))        // 返回bool类型，存在返回true，不存在返回false                                     
            {
                File.Create(path).Close();         //不存在则创建文件
                flag = true;
            }

            return flag;

        }

        /// <summary>
        /// 打开游戏并返回游戏的进程
        /// </summary>
        /// <param name="gamePath"></param>
        /// <param name="lePath"></param>
        /// <returns></returns>

        public static Process OpenGame(string gamePath, string lePath)
        {
            ProcessStartInfo p = new ProcessStartInfo();
            Process process;

            if (lePath != null && lePath.Length != 0)
            {
                
                p.FileName = lePath;
                p.Arguments = gamePath;
                p.UseShellExecute = false;
                p.WorkingDirectory = Path.GetDirectoryName(lePath);
                Process.Start(p).WaitForExit();
                process = ProcessHelper.GetProcessByPath(gamePath);
            }
            else
            {
                p.FileName = $"\"{gamePath}\"";
                p.UseShellExecute = false;
                p.WorkingDirectory = Path.GetDirectoryName(gamePath);
                process = Process.Start(p);
                
            }
            
            //MessageBox.Show($"进程ID:{ process.Id}\n进程名：{ process.ProcessName}\n进程路径：{ process.MainModule.FileName}");
            return process;

        }

    }
}

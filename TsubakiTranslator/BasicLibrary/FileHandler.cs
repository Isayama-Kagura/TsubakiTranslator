using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace TsubakiTranslator.BasicLibrary
{
    class FileHandler
    {
        public static string SelectFolderPath()
        {
            string path = "";
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件夹作为路径";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = dialog.SelectedPath; // "e:/go"
            }

            return path;

        }

        public static void SerializeObject<T>(T value, string path)
        {
            var jsonString = JsonSerializer.SerializeToUtf8Bytes<T>(value);
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(jsonString, 0, jsonString.Length);
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }

        public static T DeserializeObject<T>(string path)
        {
            T result = default(T);
            if (CreateFile(path))
                return result;

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
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }

            return result;
        }

        public static bool CreateFile(string path)
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

        public static void AppendTextToFile(string text, string path)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(text);
            }
        }

    }
}

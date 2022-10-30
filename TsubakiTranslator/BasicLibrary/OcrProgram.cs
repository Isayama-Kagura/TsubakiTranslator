using System;
using System.Drawing;
using System.IO;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage;
using Windows.Storage.Streams;

namespace TsubakiTranslator.BasicLibrary
{
    public class OcrProgram
    {
        private string language;


        public OcrProgram(string srcLang)
        {
            if (srcLang.Equals("Japanese"))
            {
                language = "ja";
            }
            else if (srcLang.Equals("English"))
            {
                language = "en-US";
            }
        }

        [SupportedOSPlatform("windows10.0.10240")]
        public async Task<string> RecognizeAsync(Bitmap CaptureBitmap)
        {
            //Bitmap to BitmapSource
            BitmapSource bitmapSource;
            try
            {
                bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(CaptureBitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            catch
            {
                bitmapSource = null;
            }

            //BitmapSource to SoftwareBitmap
            // 画像データをメモリストリームへ書き出し
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bitmapSource));
            MemoryStream temp_stream = new MemoryStream();
            encoder.Save(temp_stream);

            // メモリストリームを変換
            var converted_stream = WindowsRuntimeStreamExtensions.AsRandomAccessStream(temp_stream);

            // メモリストリームからOCR用画像データの生成
            var decorder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(converted_stream);
            SoftwareBitmap softwareBitmap = await decorder.GetSoftwareBitmapAsync();
            converted_stream.Dispose();
            temp_stream.Close();


            Language lang = new Language(language);
            string space = language.Contains("zh") || language.Contains("ja") ? "" : " ";
            string result = null;
            if (OcrEngine.IsLanguageSupported(lang))
            {
                OcrEngine engine = OcrEngine.TryCreateFromLanguage(lang);
                if (engine != null)
                {
                    OcrResult ocrResult = await engine.RecognizeAsync(softwareBitmap);
                    foreach (var tempLine in ocrResult.Lines)
                    {
                        string line = "";
                        foreach (var word in tempLine.Words)
                        {
                            line += word.Text + space;
                        }
                        //result += line + Environment.NewLine;
                        result += line;
                    }
                }
            }
            else
            {
                throw new Exception(string.Format("Language {0} is not supported", language));
            };
            softwareBitmap.Dispose();
            return await Task<string>.Run(() =>
            {
                return result;
            });
        }
    }
}

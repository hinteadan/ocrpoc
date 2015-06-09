using System;
using System.IO;
using Tesseract;

namespace OcrPoc
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var tesseract = new TesseractEngine(@".\tessdata", "eng", EngineMode.Default))
            {
                foreach (var file in Directory.EnumerateFiles("Samples"))
                {
                    using (var img = Pix.LoadFromFile(file))
                    {
                        using (var page = tesseract.Process(img))
                        {
                            var result = page.GetText();
                            File.WriteAllText(string.Format(@"{0}.txt", file), result);
                        }
                    }
                }
            }

            Console.WriteLine("Done @ {0}", DateTime.Now);
            Console.ReadLine();
        }
    }
}

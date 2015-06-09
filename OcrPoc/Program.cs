using System;
using System.IO;
using Tesseract;

namespace OcrPoc
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!Directory.Exists(@"Samples\..\Processed"))
            {
                Directory.CreateDirectory(@"Samples\..\Processed");
            }

            if (!Directory.Exists(@"Samples\..\Results"))
            {
                Directory.CreateDirectory(@"Samples\..\Results");
            }

            using (var tesseract = new TesseractEngine(@".\tessdata", "eng", EngineMode.Default))
            {
                foreach (var file in Directory.EnumerateFiles("Samples"))
                {
                    var fileInfo = new FileInfo(file);
                    using (var img = Pix.LoadFromFile(file))
                    {
                        using (var page = tesseract.Process(PreProcessAndStoreImage(img, PreProcessImageWithOtsuAdaptive, string.Format(@"{0}\..\Processed\{1}", fileInfo.DirectoryName, fileInfo.Name))))
                        {
                            var result = page.GetText();
                            File.WriteAllText(string.Format(@"{0}\..\Results\{1}.txt", fileInfo.DirectoryName, fileInfo.Name), result);
                        }
                    }
                }
            }

            Console.WriteLine("Done @ {0}", DateTime.Now);
            Console.ReadLine();
        }

        private static Pix PreProcessImageWithOtsuAdaptive(Pix img)
        {
            try
            {
                return img.ConvertRGBToGray(50, 50, 50).BinarizeOtsuAdaptiveThreshold(1, 1, 0, 0, 0.0f);
            }
            catch(Exception)
            {
                return img;
            }
        }

        private static Pix PreProcessAndStoreImage(Pix source, Func<Pix, Pix> processor, string filePath)
        {
            var img = processor(source);
            try
            {
                img.Save(filePath);
            }
            catch (Exception) { }
            return img;
        }
    }
}

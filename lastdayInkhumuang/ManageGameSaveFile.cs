using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public static class ManageGameSaveFile
    {
        public static List<string> listText = new List<string>();
        public static readonly string filePath = Path.Combine(@"Content\data.txt");

        public static void ReadFile()
        {
            listText.Clear();
            if (File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                while (!sr.EndOfStream)
                {
                    string tmpStr = sr.ReadLine();
                    listText.Add(tmpStr);
                }
                sr.Close();
            }                                     
        }
        public static void WriteFiles(string text)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(text);
            sw.Flush();
            sw.Close();
        }
        public static bool GetFileExists()
        {
            return File.Exists(filePath);
        }
    }
}

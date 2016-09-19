using System;
using System.Collections.Generic;
using System.IO;

namespace Telepromter_VS2010
{
    public class Save
    {
        public Dictionary<int, String> DataValue = new Dictionary<int, string>() { { 0, "Fullscreen" }, { 1, "Borderless" }, { 2, "Auto Download" } };
        private string userAppData = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Telepromter\";
        public Save()
        {
            if (!Directory.Exists(userAppData))
                Directory.CreateDirectory(userAppData);
            if (!File.Exists(userAppData + "Config.dat")) writeScore(50);
        }
        public int readScore()
        {
            int size = 0;
            using (BinaryReader reader = new BinaryReader(File.Open(userAppData + "Config.dat", FileMode.Open)))
            {
                size = reader.ReadInt32();
                reader.Dispose();
                reader.Close();
            }
            return size;
        }
        public void writeScore(int size)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(userAppData + "Config.dat", FileMode.Create)))
            {
                writer.Write(size);
                writer.Dispose();
                writer.Close();
            }
        }
    }
}
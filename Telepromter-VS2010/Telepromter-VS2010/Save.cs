using System;
using System.Collections.Generic;
using System.IO;

namespace Telepromter_VS2010
{
    public class Save
    {
        private string userAppData = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Telepromter\";
        public Save()
        {
            if (!Directory.Exists(userAppData))
                Directory.CreateDirectory(userAppData);
            if (!File.Exists(userAppData + "Config.dat")) writeValue(50);
        }
        public int readValue()
        {
            int size = 0;
            using (BinaryReader reader = new BinaryReader(File.Open(userAppData + "Config.dat", FileMode.Open)))
                size = reader.ReadInt32();
            return size;
        }
        public void writeValue(int size)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(userAppData + "Config.dat", FileMode.Create)))
                writer.Write(size);
        }
    }
}
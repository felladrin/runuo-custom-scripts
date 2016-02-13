// ClilocToTsv v1.0.1
// Author: Felladrin
// Started: 2016-02-10
// Updated: 2016-02-13
// Credits: Inspired by Turley's UO Fiddler Cliloc Module and Arthurasrossi's ClilocEditor

using System;
using System.IO;
using System.Text;

namespace Felladrin.ClilocToTsvConverter
{
    class ClilocToTsv
    {
        public static void Main()
        {
            const string input = "Cliloc.enu";
            const string output = "Cliloc.tsv";
            const char delimiter = '\t';

            var csv = new StringBuilder();
            csv.AppendLine(string.Concat("Number", delimiter, "Text"));

            var fi = new FileInfo(input);

            FileStream fs;

            try
            {
                fs = new FileStream(fi.FullName, FileMode.Open);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            var br = new BinaryReader(fs, Encoding.UTF8);
            br.BaseStream.Seek((long)6, SeekOrigin.Begin);

            while (br.BaseStream.Length != br.BaseStream.Position)
            {
                uint number = br.ReadUInt32();
                br.ReadByte();
                uint length = br.ReadUInt16();
                byte[] textBytes = br.ReadBytes(Convert.ToInt32(length));
                var text = Encoding.UTF8.GetString(textBytes);

                if (text.Contains(delimiter.ToString()))
                    text = text.Replace(delimiter.ToString(), " ");

                csv.AppendLine(string.Concat(number.ToString(), delimiter, text));
            }

            fs.Close();
            br.Close();

            File.WriteAllText(output, csv.ToString());
        }
    }
}

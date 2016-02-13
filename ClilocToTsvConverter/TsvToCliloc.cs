// TsvToCliloc v1.0.1
// Author: Felladrin
// Started: 2016-02-10
// Updated: 2016-02-13
// Credits: Inspired by Turley's UO Fiddler Cliloc Module and Arthurasrossi's ClilocEditor

using System;
using System.IO;
using System.Text;

namespace Felladrin.ClilocToTsvConverter
{
    class TsvToCliloc
    {
        public static void Main()
        {
            const string input = "Cliloc.tsv";
            const string output = "Cliloc.enu";
            const char delimiter = '\t';

            var fi = new FileInfo(output);
            FileStream fs;

            try
            {
                fs = new FileStream(fi.FullName, FileMode.Create);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            var bw = new BinaryWriter(fs);
            bw.Write(new byte[] { 2, 0, 0, 0, 1, 0 });

            using (var sr = new StreamReader(input))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if ((line = line.Trim()).Length == 0 || line.StartsWith("Number"))
                        continue;
                    
                    try
                    {
                        string[] split = line.Split(delimiter);
                        int number = int.Parse(split[0].Trim());
                        string text = (split.Length > 1) ? split[1].Trim() : "";
                        byte[] textBytes = Encoding.UTF8.GetBytes(text);

                        bw.Write(Convert.ToUInt32(number));
                        bw.Write((byte)0);
                        bw.Write(Convert.ToUInt16(textBytes.Length));
                        bw.Write(textBytes);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

            fs.Close();
            bw.Close();
        }
    }
}

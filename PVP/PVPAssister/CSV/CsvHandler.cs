using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PVPAssister.CSV
{
    public abstract class CsvHandler
    {
        public static List<List<string>> Read(string fileName)
        {
            var contents = new List<List<string>>();
            foreach (var line in File.ReadAllLines(fileName))
            {
                var row = new List<string>(line.Split(','));
                contents.Add(row);
            }

            return contents;
        }

        public static void Write(string fileName, List<List<string>> contents)
        {
            var sb = new StringBuilder();
            foreach (var row in contents)
            {
                sb.AppendLine(string.Join(",", row));
            }

            File.WriteAllText(fileName, sb.ToString());
        }
    }
}
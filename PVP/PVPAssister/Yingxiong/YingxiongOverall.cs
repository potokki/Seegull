using System;
using System.Collections.Generic;
using PVPAssister.CSV;

namespace PVPAssister.Yingxiong
{
    public class YingxiongOverall
    {
        private const string FileName = @"Yingxiong\Yingxiong.csv";

        public Dictionary<string, YingxiongInfo> Yingxiongs = new Dictionary<string, YingxiongInfo>();

        public YingxiongOverall()
        {
            var contents = CsvHandler.Read(FileName);
            List<string> titles = null;
            foreach (var row in contents)
            {
                if (null == titles)
                {
                    titles = row;
                    continue;
                }
                Add(titles, row);
            }
        }

        private void Add(List<string> titles, List<string> values)
        {
            string yingxiongName = values[0];
            if (string.IsNullOrEmpty(yingxiongName)) return;
            if (Yingxiongs.ContainsKey(yingxiongName))
                throw new ArgumentException($"Duplicated {yingxiongName}");
            var info = new YingxiongInfo { Name = yingxiongName };
            Yingxiongs[yingxiongName] = info;
            for (int i = 1; i < titles.Count && i < values.Count; i++)
            {
                string title = titles[i];
                if (string.IsNullOrEmpty(title)) continue;
                string valueString = values[i];
                info.AttributeDependency.Add(title, valueString);
            }
        }
    }
}
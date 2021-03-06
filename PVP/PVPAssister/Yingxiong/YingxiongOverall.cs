﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PVPAssister.CSV;
using PVPAssister.Mingwen;

namespace PVPAssister.Yingxiong
{
    public class YingxiongOverall
    {
        private const string FileName = @"Yingxiong\Yingxiong.csv";
        private static readonly Regex _regTitle = new Regex(@"(?<level>\d)\-(?<color>[BGR])");

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
            var yingxiongName = values[0];
            if (string.IsNullOrEmpty(yingxiongName)) return;
            if (Yingxiongs.ContainsKey(yingxiongName))
                throw new ArgumentException($"Duplicated {yingxiongName}");
            var info = new YingxiongInfo {Name = yingxiongName};
            Yingxiongs[yingxiongName] = info;
            for (var i = 1; i < titles.Count && i < values.Count; i++)
            {
                var title = titles[i];
                if (string.IsNullOrEmpty(title)) continue;
                var valueString = values[i];
                if (_regTitle.IsMatch(title))
                {
                    var mingwen = MingwenOverall.Get(valueString);
                    if (null != mingwen)
                        info.MingwensBySystem[mingwen.Level].Elements[mingwen.Color].Add(mingwen);
                }
                else
                {
                    info.AttributeDependency.Add(title, valueString);
                }
            }
        }
    }
}
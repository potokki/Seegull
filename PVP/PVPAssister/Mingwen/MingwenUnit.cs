using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PVPAssister.Mingwen
{
    public class MingwenUnit
    {
        public int Level { get; }

        public Dictionary<MingwenColor, List<MingwenInfo>> Elements = new Dictionary<MingwenColor, List<MingwenInfo>>()
            {
                {MingwenColor.蓝色, new List<MingwenInfo>()},
                {MingwenColor.绿色, new List<MingwenInfo>()},
                {MingwenColor.红色, new List<MingwenInfo>()},
            };

        public string Summary => GetSummary();

        public MingwenUnit(int level) { Level = level; }

        public override string ToString()
        {
            string str = Summary;
            str += "," + string.Join("|", Elements[MingwenColor.蓝色].Select(m => m.Name));
            str += "," + string.Join("|", Elements[MingwenColor.绿色].Select(m => m.Name));
            str += "," + string.Join("|", Elements[MingwenColor.红色].Select(m => m.Name));
            return str;
        }

        public string ToDetailedString()
        {
            string str = string.Empty;
            str += string.Join("|", Elements[MingwenColor.蓝色].Select(m => m.Name + m.Score.ToString("N1")));
            str += "," + string.Join("|", Elements[MingwenColor.绿色].Select(m => m.Name + m.Score.ToString("N1")));
            str += "," + string.Join("|", Elements[MingwenColor.红色].Select(m => m.Name + m.Score.ToString("N1")));
            return str;
        }

        private string GetSummary()
        {
            List<string> summaries = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                string summary = string.Empty;
                bool hasItemForI = false;
                foreach (var e in Elements)
                {
                    if (e.Value.Count > i)
                    {
                        summary += e.Value[i].Summary;
                        hasItemForI = true;
                    }
                    else if (e.Value.Count > 0)
                    {
                        summary += e.Value[0].Summary;
                    }
                }
                if (hasItemForI) summaries.Add(AttibuteSummary.Get(summary));
                else break;
            }
            string summarySum = string.Join("|", summaries);
            return summarySum;
        }
    }
}
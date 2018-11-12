using System;
using System.Collections.Generic;
using System.Linq;

namespace PVPAssister.Mingwen
{
    public class MingwenUnit : IComparable
    {
        public int Level { get; }

        public Dictionary<MingwenColor, List<MingwenInfo>> Elements = new Dictionary<MingwenColor, List<MingwenInfo>>
            {
                {MingwenColor.蓝色, new List<MingwenInfo>()},
                {MingwenColor.绿色, new List<MingwenInfo>()},
                {MingwenColor.红色, new List<MingwenInfo>()},
            };

        public string Summary => GetSummary();

        public MingwenUnit(int level) => Level = level;

        public override string ToString()
        {
            var str = string.Empty;
            str += string.Join("|", Elements[MingwenColor.蓝色].Select(m => m.Name));
            str += "," + string.Join("|", Elements[MingwenColor.绿色].Select(m => m.Name));
            str += "," + string.Join("|", Elements[MingwenColor.红色].Select(m => m.Name));
            return str;
        }

        public string ToDetailedString(bool needSummary = true)
        {
            var str = needSummary ? Summary + "," : string.Empty;
            str += string.Join("|", Elements[MingwenColor.蓝色].Select(m => m.Name + m.Score.ToString("N1")));
            str += "," + string.Join("|", Elements[MingwenColor.绿色].Select(m => m.Name + m.Score.ToString("N1")));
            str += "," + string.Join("|", Elements[MingwenColor.红色].Select(m => m.Name + m.Score.ToString("N1")));
            return str;
        }

        private string GetSummary()
        {
            var summaries = new List<string>();
            for (var i = 0; i < 5; i++)
            {
                var summary = string.Empty;
                var hasItemForI = false;
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

            var summarySum = string.Join("|", summaries);
            return summarySum;
        }

        public int CompareTo(object obj)
        {
            var other = obj as MingwenUnit;
            var diff = ComparePerColor(MingwenColor.蓝色, this, other)
                + ComparePerColor(MingwenColor.绿色, this, other)
                + ComparePerColor(MingwenColor.红色, this, other);
            return diff;
        }

        private int ComparePerColor(MingwenColor color, MingwenUnit current, MingwenUnit other) =>
            ComparePerColor(current.Elements[color], other.Elements[color]);

        public int ComparePerColor(IList<MingwenInfo> mingwens1, IList<MingwenInfo> mingwens2)
        {
            if (!mingwens1.Any())
                return 4;

            if (!mingwens2.Any())
                return 4;

            if (mingwens2.Contains(mingwens1.First()))
                return 0;

            if (mingwens1.Contains(mingwens2.First()))
                return 0;

            var diff = 0;
            var i = 0;
            var existedIn2 = false;
            foreach (var mingwen1 in mingwens1)
            {
                var j = 0;
                foreach (var mingwen2 in mingwens2)
                {
                    var jAdjust = -1;
                    if (mingwen2.Equals(mingwen1))
                        jAdjust = 0;
                    else if (mingwen1.HasSameAttribute(mingwen2))
                    {
                        jAdjust = 2;
                    }
                    if (jAdjust >= 0)
                    {
                        existedIn2 = true;
                        diff += i * 5 + (j + jAdjust) * 2;
                    }
                    j++;
                }

                i++;
            }

            if (!existedIn2)
                diff += 7;

            return diff;
        }
    }
}
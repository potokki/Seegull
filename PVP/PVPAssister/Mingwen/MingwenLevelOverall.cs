using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using PVPAssister.CSV;

namespace PVPAssister.Mingwen
{
    public class MingwenLevelOverall
    {
        private const int ScoreMin = 9;
        private const int ScoreAdvanced = 12;
        private const double ScoreDiff = 0.5;
        public int Level;
        public Dictionary<string, MingwenInfo> Elements =
            new Dictionary<string, MingwenInfo>();

        public AttributeOverall Attributes = new AttributeOverall();

        public MingwenLevelOverall(string fileName, int mingwenLevel)
        {
            Level = mingwenLevel;
            var contents = CsvHandler.Read(fileName);
            List<string> titles = null;
            foreach (var row in contents)
            {
                if (null == titles)
                {
                    titles = row;
                    continue;
                }
                Add(mingwenLevel, titles, row);
            }
            foreach (var mingwen in Elements.Values)
            {
                foreach (var attribute in mingwen.Attributes)
                    Attributes.UpdatePercentage(attribute);
            }
        }

        public MingwenUnit SuggestedMingwenUnit(AttributeDependency attributeDependency)
        {
            MingwenUnit unit = new MingwenUnit();
            foreach (var color in unit.Elements.Keys)
            {
                unit.Elements[color].AddRange(SuggestedMingwens(color, attributeDependency));
            }
            return unit;
        }

        private void Add(int mingwenLevel,
            List<string> titles,
            List<string> values)
        {
            var mingwenInfo = new MingwenInfo { Level = mingwenLevel };
            for (int i = 0; i < values.Count && i < titles.Count; i++)
            {
                string title = titles[i];
                if (string.IsNullOrEmpty(title)) continue;
                var color = GetMingwenColor(title);
                string valueString = values[i];
                if (string.IsNullOrEmpty(valueString)) continue;
                if (color == MingwenColor.Unknown)
                {
                    var attribute = Attributes.Add(title, valueString);
                    mingwenInfo.Attributes.Add(attribute);
                }
                else
                {
                    mingwenInfo.Name = valueString;
                    mingwenInfo.Color = color;
                }
            }
            Elements[mingwenInfo.Name] = mingwenInfo;
        }

        private static MingwenColor GetMingwenColor(string colorString)
        {
            if (!Enum.TryParse(colorString, false, out MingwenColor color))
            {
                color = MingwenColor.Unknown;
            }
            return color;
        }

        private List<MingwenInfo> SuggestedMingwens(MingwenColor color, AttributeDependency attributeDependency)
        {
            List<MingwenInfo> mingwens = new List<MingwenInfo>();
            var currentColorMingwens = Elements.Values.Where(m => color == m.Color).ToList();
            foreach (var mingwen in currentColorMingwens)
            {
                mingwen.Score = 0;
                foreach (var attibute in mingwen.Attributes)
                {
                    int rate = attributeDependency.GetDependencyRate(attibute.Name);
                    mingwen.Score += rate * attibute.Percentage;
                }
            }

            var orderedMingwens = currentColorMingwens.OrderByDescending(m => m.Score).ToList();
            MingwenInfo first = null;
            foreach (var mingwen in orderedMingwens)
            {
                if (null == first && mingwen.Score > ScoreMin ||
                    null != first && (mingwen.Score > ScoreAdvanced ||
                        mingwen.Score > ScoreMin && first.Score - mingwen.Score < ScoreDiff))
                {
                    var current = mingwen.Clone();
                    mingwens.Add(current);
                    if (null == first)
                        first = current;
                }
            }
            return mingwens;
        }
    }
}
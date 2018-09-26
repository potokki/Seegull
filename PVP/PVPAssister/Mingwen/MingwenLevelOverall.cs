using System;
using System.Collections.Generic;
using System.Linq;
using PVPAssister.CSV;

namespace PVPAssister.Mingwen
{
    public class MingwenLevelOverall
    {
        private const double ScoreMin = 9.8;
        private const int ScoreMid = 11;
        private const int ScoreAdvanced = 14;
        private const double ScoreDiffLess = 0.2;
        private const double ScoreDiffMore = 0.4;
        private const int MaxMingwenPerUnit = 2;
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
                    Attributes.UpdatePercentageAndRate(attribute);
            }
        }

        public MingwenUnit SuggestedMingwenUnit(AttributeDependency attributeDependency)
        {
            var unit = new MingwenUnit(Level);
            foreach (var color in unit.Elements.Keys)
            {
                unit.Elements[color].AddRange(SuggestedMingwens(color, attributeDependency));
            }

            return unit;
        }

        private void Add(
            int mingwenLevel,
            IReadOnlyList<string> titles,
            IReadOnlyList<string> values)
        {
            var mingwenInfo = new MingwenInfo {Level = mingwenLevel};
            for (var i = 0; i < values.Count && i < titles.Count; i++)
            {
                var title = titles[i];
                if (string.IsNullOrEmpty(title)) continue;
                var color = GetMingwenColor(title);
                var valueString = values[i];
                if (string.IsNullOrEmpty(valueString)) continue;
                if (color == MingwenColor.Unknown)
                {
                    var attribute = Attributes.Add(title, valueString);
                    mingwenInfo.AddAttribute(attribute);
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
            var mingwens = new List<MingwenInfo>();
            var currentColorMingwens = Elements.Values.Where(m => color == m.Color).ToList();
            foreach (var mingwen in currentColorMingwens)
            {
                mingwen.UpdateScore(attributeDependency);
            }

            var orderedMingwens = currentColorMingwens.OrderByDescending(m => m.Score).ToList();
            MingwenInfo first = null;
            foreach (var mingwen in orderedMingwens)
            {
                if (mingwens.Count < MaxMingwenPerUnit &&
                    (null == first && mingwen.Score > ScoreMin ||
                        null != first && (mingwen.Score > ScoreAdvanced ||
                            mingwen.Score > ScoreMin && first.Score - mingwen.Score < ScoreDiffLess ||
                            mingwen.Score > ScoreMid && first.Score - mingwen.Score < ScoreDiffMore)))
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
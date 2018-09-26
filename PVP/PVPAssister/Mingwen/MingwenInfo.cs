using System;
using System.Collections.Generic;
using System.Linq;

namespace PVPAssister.Mingwen
{
    public class MingwenInfo
    {
        public string Name;
        public int Level;
        public MingwenColor Color;
        public List<Attribute> Attributes { get; private set; } = new List<Attribute>();
        public string Summary => GetSummary();
        public double Score { get; private set; }

        private const int RateNormalMax = 10;

        public MingwenInfo Clone()
        {
            var cloned = new MingwenInfo
                {
                    Name = Name,
                    Level = Level,
                    Color = Color,
                    Attributes = Attributes,
                    Score = Score,
                };
            return cloned;
        }

        public void AddAttribute(Attribute attribute) { Attributes.Add(attribute); }

        public void UpdateScore(AttributeDependency attributeDependency)
        {
            Score = 0;
            var attibuteCount = 0;
            var preferSingleAttribute = false;
            foreach (var attibute in Attributes)
            {
                var rate = attributeDependency.GetDependencyRate(attibute.Name);
                Score += rate * attibute.Rate;
                preferSingleAttribute = rate > RateNormalMax;
                attibuteCount++;
            }

            if (Score > 0)
            {
                if (attibuteCount == 1)
                {
                    if (preferSingleAttribute) Score *= 1.3;
                }
                else
                {
                    var multipleAttibuteRateBase = 1 + 0.7 * (attibuteCount - 1) / attibuteCount;
                    var multipleAttibuteRate =
                        multipleAttibuteRateBase * (1 - (multipleAttibuteRateBase - 1) / 1.8);
                    if (multipleAttibuteRate > 1) Score *= multipleAttibuteRate;
                }
            }
        }

        private string GetSummary()
        {
            var query = (from e in Attributes
                orderby e.Rate descending
                select AttibuteSummary.Get(e.Name)).Take(3);
            var summary = string.Join("", query);
            summary = AttibuteSummary.Get(summary);
            return summary;
        }

        public override string ToString()
        {
            var val = $"{Name}-{Level}-{Color}-{Summary}";
            return val;
        }

        public override bool Equals(object obj)
        {
            var other = obj as MingwenInfo;
            return string.Equals(Name, other.Name, StringComparison.Ordinal);
        }

        public override int GetHashCode() => string.IsNullOrEmpty(Name) ? 0 : Name.GetHashCode();
    }
}
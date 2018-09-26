using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;

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
            MingwenInfo cloned = new MingwenInfo
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
            int attibuteCount = 0;
            bool preferSingleAttribute = false;
            foreach (var attibute in Attributes)
            {
                int rate = attributeDependency.GetDependencyRate(attibute.Name);
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
                    double multipleAttibuteRateBase = 1 + 0.7 * (attibuteCount - 1) / attibuteCount;
                    double multipleAttibuteRate =
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
            string summary = string.Join("", query);
            summary = AttibuteSummary.Get(summary);
            return summary;
        }

        public override string ToString()
        {
            string val = $"{Name}-{Level}-{Color}-{Summary}";
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
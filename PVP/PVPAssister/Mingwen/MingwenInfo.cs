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
            foreach (var attibute in Attributes)
            {
                int rate = attributeDependency.GetDependencyRate(attibute.Name);
                Score += rate * attibute.Rate;
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
    }
}
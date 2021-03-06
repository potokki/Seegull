﻿using System.Collections.Generic;
using System.Linq;

namespace PVPAssister.Mingwen
{
    public class AttributeDependency
    {
        public Dictionary<string, int> Elements = new Dictionary<string, int>();
        public string Summary => GetSummary();
        internal bool HasSet { get; private set; }

        public void Add(string attributeName, string dependencyRateString)
        {
            if (!int.TryParse(dependencyRateString, out var dependencyRate))
            {
                dependencyRate = 0;
            }
            else
            {
                HasSet = true;
            }

            Elements[attributeName] = dependencyRate;
        }

        public int GetDependencyRate(string attibuteName)
        {
            if (!Elements.TryGetValue(attibuteName, out var rate))
            {
                rate = 0;
            }

            return rate;
        }

        private string GetSummary()
        {
            var query = (from e in Elements
                orderby e.Value descending
                select AttibuteSummary.Get(e.Key)).Take(3);
            var summary = string.Join("", query);
            summary = AttibuteSummary.Get(summary);
            return summary;
        }
    }
}
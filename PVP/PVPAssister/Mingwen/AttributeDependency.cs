using System.Collections.Generic;

namespace PVPAssister.Mingwen
{
    public class AttributeDependency
    {
        public Dictionary<string, int> Elements = new Dictionary<string, int>();

        public void Add(string attributeName, string dependencyRateString)
        {
            if (!int.TryParse(dependencyRateString, out int dependencyRate))
            {
                dependencyRate = 1;
            }
            Elements[attributeName] = dependencyRate;
        }

        public int GetDependencyRate(string attibuteName)
        {
            if (!Elements.TryGetValue(attibuteName, out int rate))
            {
                rate = 1;
            }
            return rate;
        }
    }
}

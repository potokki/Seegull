using System.Collections.Generic;

namespace PVPAssister.Mingwen
{
    public abstract class MingwenOverall
    {
        private const string Level4FileName = @"Mingwen\MingwenLevel4.csv";
        private const string Level5FileName = @"Mingwen\MingwenLevel5.csv";

        public const int MaxMingwenLevel = 5;
        public static Dictionary<int, MingwenLevelOverall> Levles = new Dictionary<int, MingwenLevelOverall>();

        public static Dictionary<string, MingwenInfo> Elements =
            new Dictionary<string, MingwenInfo>();

        static MingwenOverall()
        {
            Levles[4] = new MingwenLevelOverall(Level4FileName, 4);
            Levles[5] = new MingwenLevelOverall(Level5FileName, 5);
            foreach (var levelOverall in Levles.Values)
            {
                foreach (var e in levelOverall.Elements)
                {
                    Elements[e.Key] = e.Value;
                }
            }
        }

        public static MingwenInfo Get(string mingwenName)
        {
            Elements.TryGetValue(mingwenName, out var mingwenInfo);
            return mingwenInfo?.Clone();
        }
    }
}
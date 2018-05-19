using System.Collections.Generic;

namespace PVPAssister.Mingwen
{
    public class MingwenOverall
    {
        private const string Level4FileName = @"Mingwen\MingwenLevel4.csv";
        private const string Level5FileName = @"Mingwen\MingwenLevel5.csv";

        public Dictionary<int, MingwenLevelOverall> Levles = new Dictionary<int, MingwenLevelOverall>();

        public MingwenOverall()
        {
            Levles[4] = new MingwenLevelOverall(Level4FileName, 4);
            Levles[5] = new MingwenLevelOverall(Level5FileName, 5);
        }

    }
}
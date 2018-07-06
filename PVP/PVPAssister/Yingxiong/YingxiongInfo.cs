using PVPAssister.Mingwen;

namespace PVPAssister.Yingxiong
{
    public class YingxiongInfo
    {
        public string Name;
        public AttributeDependency AttributeDependency = new AttributeDependency();
        public MingwenUnit[] Mingwens;
        public MingwenUnit[] MingwensBySystem;
        public const string OutputTitle = "Yingxiong,4-类型,4-B,4-G,4-R,5-类型,5-B,5-G,5-R,D4-B,D4-G,D4-R,D5-B,D5-G,D5-R,4-类型,4-B,4-G,4-R,5-类型,5-B,5-G,5-R";

        public YingxiongInfo()
        {
            const int max = MingwenOverall.MaxMingwenLevel + 1;
            Mingwens = new MingwenUnit[max];
            MingwensBySystem = new MingwenUnit[max];
            for (int i = 1; i < max; i++)
            {
                Mingwens[i] = new MingwenUnit(i);
                MingwensBySystem[i] = new MingwenUnit(i);
            }
        }

        public override string ToString()
        {
            string str =
                $"{Name},{Mingwens[4].ToDetailedString()},{Mingwens[5].ToDetailedString()},{Mingwens[4]},{Mingwens[5]},{MingwensBySystem[4].ToDetailedString()},{MingwensBySystem[5].ToDetailedString()}";

            return str;
        }
    }
}
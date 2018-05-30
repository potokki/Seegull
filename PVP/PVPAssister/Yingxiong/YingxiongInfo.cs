using PVPAssister.Mingwen;

namespace PVPAssister.Yingxiong
{
    public class YingxiongInfo
    {
        public string Name;
        public AttributeDependency AttributeDependency = new AttributeDependency();
        public MingwenUnit Level4Mingwens;
        public MingwenUnit Level5Mingwens;

        public override string ToString()
        {
            string str =
                $"{Name},{Level4Mingwens},{Level5Mingwens},{Level4Mingwens.ToDetailedString()},{Level5Mingwens.ToDetailedString()}";

            return str;
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace PVPAssister.Mingwen
{
    public class MingwenUnit
    {
        public Dictionary<MingwenColor, List<MingwenInfo>> Elements = new Dictionary<MingwenColor, List<MingwenInfo>>()
            {
                {MingwenColor.蓝色, new List<MingwenInfo>()},
                {MingwenColor.绿色, new List<MingwenInfo>()},
                {MingwenColor.红色, new List<MingwenInfo>()},
            };

        public override string ToString()
        {
            string str = string.Empty;
            //str += string.Join("|", Elements[MingwenColor.蓝色].Select(m => m.Name + m.Score)) + ",";
            //str += string.Join("|", Elements[MingwenColor.绿色].Select(m => m.Name + m.Score)) + ",";
            //str += string.Join("|", Elements[MingwenColor.红色].Select(m => m.Name + m.Score));
            str += string.Join("|", Elements[MingwenColor.蓝色].Select(m => m.Name)) + ",";
            str += string.Join("|", Elements[MingwenColor.绿色].Select(m => m.Name)) + ",";
            str += string.Join("|", Elements[MingwenColor.红色].Select(m => m.Name));

            return str;
        }

    }
}

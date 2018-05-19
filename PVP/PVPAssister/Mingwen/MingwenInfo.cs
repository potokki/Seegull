using System.Collections.Generic;

namespace PVPAssister.Mingwen
{
    public class MingwenInfo
    {
        public string Name;
        public int Level;
        public MingwenColor Color;
        public List<Attribute> Attributes = new List<Attribute>();
        public double Score;

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
    }
}

using System;

namespace PVPAssister.Mingwen
{
    public class Attribute
    {
        public string Name;
        public double Value;
        public double Percentage;
        public double Rate;

        /// <summary>
        ///     Overall Max Value, used for Overall only
        /// </summary>
        public double MaxValue;

        public override bool Equals(object obj)
        {
            var other = obj as Attribute;
            return string.Equals(Name, other?.Name, StringComparison.Ordinal);
        }

        public override int GetHashCode() => string.IsNullOrEmpty(Name) ? 0 : Name.GetHashCode();
    }
}
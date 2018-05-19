using System;
using System.Collections.Generic;

namespace PVPAssister.Mingwen
{
    public class AttributeOverall
    {
        public Dictionary<string, Attribute> Elements = new Dictionary<string, Attribute>();

        public Attribute Add(string attributeName, string valueString)
        {
            var attribute = new Attribute
            {
                Name = attributeName,
                Value = Convert.ToDouble(valueString),
            };

            if (!Elements.TryGetValue(attributeName, out Attribute overallAttribute))
            {
                overallAttribute = new Attribute
                {
                    Name = attribute.Name,
                    Value = attribute.Value,
                    MaxValue = attribute.Value,
                };
                Elements[attributeName] = overallAttribute;
            }
            else
            {
                overallAttribute.MaxValue = Math.Max(overallAttribute.MaxValue, attribute.Value);
            }
            return attribute;
        }

        public void UpdatePercentage(Attribute attribute)
        {
            if (Elements.TryGetValue(attribute.Name, out Attribute overallAttribute))
            {
                attribute.Percentage = attribute.Value / overallAttribute.MaxValue;
            }
            else
            {
                throw new ArgumentOutOfRangeException(attribute.Name);
            }
        }
    }
}
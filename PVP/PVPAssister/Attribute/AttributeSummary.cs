using System;
using System.Collections.Generic;
using System.Linq;
using PVPAssister.CSV;

namespace PVPAssister.Mingwen
{
    public abstract class AttibuteSummary
    {
        private const string AttributeSummaryFileName = @"Attribute\AttributeSummaryName.csv";
        public static Dictionary<string, string> Map = new Dictionary<string, string>();

        static AttibuteSummary()
        {
            var contents = CsvHandler.Read(AttributeSummaryFileName);
            foreach (var row in contents)
            {
                if (row.Count == 2)
                    Add(row[0], row[1], false);
            }

            var existingCombinations = Map.Keys.ToList();
            foreach (var existingCombination in existingCombinations)
            {
                Initialize(existingCombination, Map[existingCombination]);
            }
        }

        public static void Add(string original, string summary, bool overwrite = true)
        {
            if (string.IsNullOrEmpty(original))
                return;
            if (!overwrite && Map.ContainsKey(original))
                throw new ArgumentOutOfRangeException($"Duplicated attribute {original} in AttibuteSummary");
            Map[original] = summary;
        }

        public static string Get(string original)
        {
            if (string.IsNullOrEmpty(original)) return original;

            var summary = original;
            var temp = original;
            for (var i = 0; i < 6; i++)
            {
                if (Map.TryGetValue(temp, out summary))
                {
                    return summary;
                }

                summary = TryGet(temp);
                if (summary == temp) break;
                temp = summary;
            }

            summary = Sort(summary);
            //if (original != summary)
            //    Add(original, summary);
            return summary;
        }

        private static void Initialize(string combination, string existingSummary)
        {
            if (combination.Length < 4)
                return;

            var reversedCombination = string.Empty;
            for (var i = 0; i < combination.Length - 1; i += 2)
            {
                var item = combination.Substring(i, 2);
                Add(existingSummary + item, existingSummary);
                Add(item + existingSummary, existingSummary);
                reversedCombination = item + reversedCombination;
            }

            Add(reversedCombination, existingSummary);
        }

        private static string TryGet(string combination)
        {
            var hasDuplicatedItem = false;
            var items = new List<string>();
            for (var i = 0; i < combination.Length - 1; i += 2)
            {
                var item = combination.Substring(i, 2);
                if (!items.Contains(item)) items.Add(item);
                else hasDuplicatedItem = true;
            }

            if (hasDuplicatedItem) combination = string.Join("", items);

            var dimensionCount = items.Count;
            var offsetOfDimensions = new int[dimensionCount];
            var completed = false;
            while (!completed)
            {
                var indexes = new List<int>();
                var k = 0;
                for (; !completed && k < dimensionCount;)
                {
                    for (var index = offsetOfDimensions[k];;)
                    {
                        if (index >= dimensionCount)
                        {
                            for (var j = dimensionCount - 1; j >= k; j--)
                            {
                                offsetOfDimensions[j] = 0;
                                if (indexes.Count == j + 1) indexes.RemoveAt(j);
                            }

                            if (--k >= 0)
                            {
                                offsetOfDimensions[k]++;
                                if (indexes.Count == k + 1) indexes.RemoveAt(k);
                                else if (indexes.Count > k)
                                    throw new Exception($"indexes.Count {indexes.Count} shall not greater than {k}");
                            }
                            else
                            {
                                completed = true;
                            }

                            break;
                        }

                        if (indexes.Contains(index))
                        {
                            index = ++offsetOfDimensions[k];
                        }
                        else
                        {
                            indexes.Add(index);
                            k++;
                            break;
                        }
                    }
                }

                if (k == dimensionCount)
                {
                    ++offsetOfDimensions[k - 1];
                    var summary = TryGet(items, indexes);
                    if (!string.IsNullOrEmpty(summary))
                        return summary;
                }
            }

            return combination;
        }

        private static string TryGet(List<string> items, List<int> indexes)
        {
            var summary = string.Empty;
            for (var itemCount = indexes.Count; itemCount > 1; itemCount--)
            {
                var combination = string.Empty;
                for (var i = 0; i < itemCount; i++) combination += items[indexes[i]];
                if (Map.TryGetValue(combination, out summary))
                {
                    for (var i = itemCount; i < items.Count; i++) summary += items[indexes[i]];
                    break;
                }
            }

            return summary;
        }

        private static string Sort(string summary)
        {
            if (summary.Length < 4)
                return summary;
            var items = new List<string>();
            for (var i = 0; i < summary.Length - 1; i += 2)
            {
                items.Add(summary.Substring(i, 2));
            }

            items.Sort();
            var orderedSummary = string.Join("", items);
            return orderedSummary;
        }
    }
}
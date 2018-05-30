using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    Add(row[0], row[1]);
            }
            var existingCombinations = Map.Keys.ToList();
            foreach (var existingCombination in existingCombinations)
            {
                Initialize(existingCombination, Map[existingCombination]);
            }
        }

        public static void Add(string original, string summary)
        {
            if (string.IsNullOrEmpty(original))
                return;
            if (Map.ContainsKey(original))
                throw new ArgumentOutOfRangeException($"Duplicated attribute {original} in AttibuteSummary");
            Map[original] = summary;
        }

        public static string Get(string original)
        {
            if (string.IsNullOrEmpty(original)) return original;

            string summary = original;
            string temp = original;
            for (int i = 0; i < 5; i++)
            {
                if (Map.TryGetValue(temp, out summary))
                { return summary; }
                else
                {
                    summary = TryGet(temp);
                    if (summary == temp) break;
                    else temp = summary;
                }
            }
            summary = Sort(summary);
            if (original != summary)
                Map[original] = summary;
            return summary;
        }

        private static void Initialize(string combination, string existingSummary)
        {
            if (combination.Length < 4)
                return;

            string reversedCombination = string.Empty;
            for (int i = 0; i < combination.Length - 1; i += 2)
            {
                string item = combination.Substring(i, 2);
                Map[existingSummary + item] = existingSummary;
                reversedCombination = item + reversedCombination;
            }
            Map[reversedCombination] = existingSummary;
        }

        private static string TryGet(string combination)
        {
            bool hasDuplicatedItem = false;
            List<string> items = new List<string>();
            for (int i = 0; i < combination.Length - 1; i += 2)
            {
                string item = combination.Substring(i, 2);
                if (!items.Contains(item)) items.Add(item);
                else hasDuplicatedItem = true;
            }
            if (hasDuplicatedItem) combination = string.Join("", items);

            int dimensionCount = items.Count;
            int[] offsetOfDimensions = new int[dimensionCount];
            bool completed = false;
            while (!completed)
            {
                List<int> indexes = new List<int>();
                int k = 0;
                for (; !completed && k < dimensionCount;)
                {
                    for (int index = offsetOfDimensions[k]; ;)
                    {
                        if (index >= dimensionCount)
                        {
                            for (int j = dimensionCount - 1; j >= k; j--)
                            {
                                offsetOfDimensions[j] = 0;
                                if (indexes.Count == j + 1) indexes.RemoveAt(j);
                            }
                            if (--k >= 0)
                            {
                                offsetOfDimensions[k]++;
                                if (indexes.Count == k + 1) indexes.RemoveAt(k);
                                else if (indexes.Count > k) throw new Exception($"indexes.Count {indexes.Count} shall not greater than {k}");
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
                    string summary = TryGet(items, indexes);
                    if (!string.IsNullOrEmpty(summary))
                        return summary;
                }
            }
            return combination;
        }

        private static string TryGet(List<string> items, List<int> indexes)
        {
            string summary = string.Empty;
            for (int itemCount = indexes.Count; itemCount > 1; itemCount--)
            {
                string combination = string.Empty;
                for (int i = 0; i < itemCount; i++) combination += items[indexes[i]];
                if (Map.TryGetValue(combination, out summary))
                {
                    for (int i = itemCount; i < items.Count; i++) summary += items[indexes[i]];
                    break;
                }
            }
            return summary;
        }

        private static string Sort(string summary)
        {
            if (summary.Length < 4)
                return summary;
            List<string> items = new List<string>();
            for (int i = 0; i < summary.Length - 1; i += 2)
            {
                items.Add(summary.Substring(i, 2));
            }
            items.Sort();
            string orderedSummary = string.Join("", items);
            return orderedSummary;
        }
    }
}

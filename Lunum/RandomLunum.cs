using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;

namespace Lunum
{
    public class RandomLunum
    {
        public NumRangeInfo[] Parts { get; set; }

        public string Next
        {
            get
            {
                string result = string.Empty;
                foreach (var part in Parts)
                {
                    foreach (var num in part.MostPossibleNums)
                    {
                        result += $"{num:00} ";
                    }
                    result += "+ ";
                }
                result = result.TrimEnd().TrimEnd('+');
                return result;
            }
        }

        public void LoadHistory(string fileName)
        {
            int termIndex = 0;
            foreach (var line in File.ReadAllLines(fileName))
            {
                string lineStr = line.Trim();
                if (!string.IsNullOrEmpty(lineStr))
                {
                    if (AddHistory(lineStr, termIndex)) termIndex++;
                }
            }
        }

        public bool AddHistory(string winNumStr, int termIndex)
        {
            var winNum = new WinNumInfo(winNumStr, termIndex);
            bool valid = winNum.Valid;
            if (valid)
            {
                int partCount = Parts.Length;
                if (winNum.Parts.Count != partCount) throw new ArgumentOutOfRangeException($"{winNum.Parts.Count} != {partCount}");
                for (int i = 0; i < partCount; i++)
                {
                    Parts[i].AddHistory(winNum.Parts[i], winNum.TermIndex);
                }
            }
            return valid;
        }

    }

    class WinNumInfo
    {
        public List<int[]> Parts { get; private set; }

        public int TermIndex { get; }

        public bool Valid => null != Parts && Parts.Count > 0;

        static readonly char[] Seperator = { ' ', ',', '\t', '+' };
        public WinNumInfo(string winNumStr, int termIndex)
        {
            var numStrList = winNumStr.Split(Seperator, StringSplitOptions.RemoveEmptyEntries);
            int count = numStrList.Length;
            if (count > 0)
            {
                List<int> numList = new List<int>();
                int num;
                for (int i = 0; i < count; i++)
                {
                    if (int.TryParse(numStrList[i], out num)) numList.Add(num);
                }
                int[] countPerPartList = { numList.Count - 1, 1 };
                SetNumInfo(numList, countPerPartList);
                TermIndex = termIndex;
            }
        }

        void SetNumInfo(IEnumerable<int> numList, IEnumerable<int> countPerPartList)
        {
            Parts = new List<int[]>();
            int[] numArray = numList.ToArray();
            if (numArray.Length > 0)
            {
                int lastIndex = 0;
                foreach (int count in countPerPartList)
                {
                    int newIndex = lastIndex + count;
                    if (newIndex > numArray.Length) throw new ArgumentOutOfRangeException(nameof(newIndex), newIndex.ToString());
                    int[] part = new int[count];
                    for (int i = lastIndex, j = 0; i < newIndex; i++, j++) { part[j] = numArray[i]; }
                    Parts.Add(part);
                    lastIndex = newIndex;
                }
            }
        }
    }

}

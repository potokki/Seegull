using System;
using System.Collections.Generic;
using System.Linq;

namespace Lunum
{
    public class NumRangeInfo
    {
        public int Min { get; set; } = 1;
        public int Max { get; set; }
        public int NumCount { get; set; }
        public int TotalTermCount { get; private set; }
        Dictionary<int, AppearFactors> _appearRates;

        public List<int> MostPossibleNums
        {
            get
            {
                if (null != _appearRates && _appearRates.Count > 0)
                {
                    foreach (var ar in _appearRates) { ar.Value.CauculateFactor(TotalTermCount); }
                    var list = _appearRates.OrderBy(r => r.Value).Select(r => r.Key).ToList();
                    list.RemoveRange(NumCount, list.Count - NumCount);
                    list.Sort();
                    return list;
                }
                else { return new List<int>(); }
            }
        }

        public void AddHistory(int[] winNums, int termIndex)
        {
            TotalTermCount = Math.Max(TotalTermCount, termIndex);
            if (null == _appearRates)
            {
                _appearRates = new Dictionary<int, AppearFactors>();
                for (int i = Min; i <= Max; i++) _appearRates[i] = new AppearFactors();
            }
            foreach (int num in winNums)
            {
                _appearRates[num].Add(termIndex);
            }
        }
    }


    class AppearFactors : IComparable
    {
        public bool LastAppeared { get; set; }
        //public bool Appeared => AppearPercentage > 0;
        //public int AppearPercentage { get => _appearPercentage; set => _appearPercentage = value < 0 ? 0 : value > 100 ? 100 : value; }
        //int _appearPercentage;
        //public int Factor
        //{
        //    get
        //    {
        //        int factor = LastAppeared ? 0 : 100 - AppearPercentage;
        //        return factor;
        //    }
        //}
        public int Factor { get; private set; }

        List<int> _appearedTerms = new List<int>();
        static Random Rdm = new Random();
        const int RandomMax = 10000;

        public void Add(int termIndex)
        {
            _appearedTerms.Add(termIndex);
            _appearedTerms.Sort();
        }

        public void CauculateFactor(int totalTermCount)
        {
            if (totalTermCount <= 0) { Factor = -1; }
            else
            {
                double factor = 0;
                foreach (var index in _appearedTerms)
                {
                    int recentFactor = totalTermCount - index;
                    int power = 1;
                    if (recentFactor * 2 > totalTermCount) power = 3;
                    else if (recentFactor * 3 > totalTermCount) power = 2;

                    factor += Math.Pow(recentFactor, power);
                }
                Factor = (int)(factor * _appearedTerms.Count * RandomMax) + Rdm.Next(RandomMax);
            }
        }

        public int CompareTo(object obj)
        {
            int val = 0;
            if (!(obj is AppearFactors other)) { val = 1; }
            else { val = this.Factor - other.Factor; }
            return val;
        }
    }

}

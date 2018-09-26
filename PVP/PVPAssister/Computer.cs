using System;
using System.IO;
using System.Linq;
using System.Text;
using PVPAssister.Mingwen;
using PVPAssister.Yingxiong;

namespace PVPAssister
{
    public class Computer
    {
        private static readonly string _outputFileName =
            $"Result_{DateTime.Now.ToShortTimeString().Replace(":", "_").Replace(" ", "")}.csv";

        //private static readonly string OutputFileName =
        //    $"Result_{DateTime.Now.ToShortDateString().Replace("/", "_").Replace(" ", "")}.csv";

        private YingxiongOverall _yingxiong = new YingxiongOverall();

        public void ComputeMingwensForEachYingxiong()
        {
            var yingxiongs = _yingxiong.Yingxiongs.Values.Where(y => y.HasSet).ToList();
            foreach (var yingxiong in yingxiongs)
            {
                for (int level = 4; level < 6; level++)
                    yingxiong.Mingwens[level] = MingwenOverall.Levles[level].SuggestedMingwenUnit(yingxiong.AttributeDependency);
            }

            foreach (var yingxiong in yingxiongs)
            {
                var similarYingxiongs = yingxiongs.OrderBy(y => yingxiong.CompareTo(y));
                yingxiong.SimilarYingxiongs.AddRange(similarYingxiongs.TakeWhile(y => yingxiong.CompareTo(y) < 9));
            }
        }

        public void Print()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(YingxiongInfo.OutputTitle);
            foreach (var yingxiong in _yingxiong.Yingxiongs.Values)
            {
                sb.AppendLine(yingxiong.ToString());
            }

            File.WriteAllText(_outputFileName, sb.ToString(), Encoding.UTF8);
            //Process.Start(OutputFileName);
        }
    }
}
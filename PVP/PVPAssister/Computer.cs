using System;
using System.IO;
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
        private const string OutputTitle = "Yingxiong,4-类型,4-B,4-G,4-R,5-类型,5-B,5-G,5-R,D4-B,D4-G,D4-R,D5-B,D5-G,D5-R";

        private MingwenOverall _mingwen = new MingwenOverall();
        private YingxiongOverall _yingxiong = new YingxiongOverall();

        public void ComputeMingwensForEachYingxiong()
        {
            foreach (var yingxiong in _yingxiong.Yingxiongs.Values)
            {
                yingxiong.Level4Mingwens = _mingwen.Levles[4].SuggestedMingwenUnit(yingxiong.AttributeDependency);
                yingxiong.Level5Mingwens = _mingwen.Levles[5].SuggestedMingwenUnit(yingxiong.AttributeDependency);
            }
        }

        public void Print()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(OutputTitle);
            foreach (var yingxiong in _yingxiong.Yingxiongs.Values)
            {
                sb.AppendLine(yingxiong.ToString());
            }

            File.WriteAllText(_outputFileName, sb.ToString(), Encoding.UTF8);
            //Process.Start(OutputFileName);
        }
    }
}
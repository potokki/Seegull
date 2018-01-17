using System;

namespace Lunum
{
    class Program
    {
        static void Main(string[] args)
        {
            int cnt = 1;
            while (string.IsNullOrEmpty(Console.ReadLine()))
            {
                Invoke(cnt++);
            }
        }

        static void Invoke(int cnt)
        {
            Console.WriteLine(cnt);
            Game pb = new Game
            {
                HistoryFileName = "pb.txt",
                RandomHandler = new RandomLunum()
                {
                    Parts = new[]
                                {
                                    new NumRangeInfo() {Max = 69, NumCount = 5,},
                                    new NumRangeInfo() {Max = 26, NumCount = 1,},
                                },
                },
            };
            Console.WriteLine($"pb {pb.GetNext()}");

            Game mm = new Game
            {
                HistoryFileName = "mm.txt",
                RandomHandler = new RandomLunum()
                {
                    Parts = new[]
                                {
                                    new NumRangeInfo() {Max = 70, NumCount = 5,},
                                    new NumRangeInfo() {Max = 25, NumCount = 1,},
                                },
                },
            };
            Console.WriteLine($"mm {mm.GetNext()}");
            Console.WriteLine();
        }
    }
}
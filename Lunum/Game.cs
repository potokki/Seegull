using System;
using System.Collections.Generic;
using System.Text;

namespace Lunum
{
    public class Game
    {
        public string HistoryFileName { get; set; }
        public RandomLunum RandomHandler { get; set; }

        public string GetNext()
        {
            RandomHandler.LoadHistory(HistoryFileName);
            return RandomHandler.Next;
        }
    }
}

namespace PVPAssister
{
    class Program
    {
        private static void Main(string[] args)
        {
            Computer computer = new Computer();
            computer.ComputeMingwensForEachYingxiong();
            computer.Print();
        }
    }
}
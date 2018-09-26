namespace PVPAssister
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var computer = new Computer();
            computer.ComputeMingwensForEachYingxiong();
            computer.Print();
        }
    }
}
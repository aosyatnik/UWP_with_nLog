using System;

namespace Console_with_nLog
{
    class Program
    {
        static void Main(string[] args)
        {
            var vm = new Bootstrapper().MainViewModel;
            Console.WriteLine("All was ok.");
            Console.ReadLine();
        }
    }
}

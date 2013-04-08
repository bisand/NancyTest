using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Nancy.Hosting.Self;
using RedBranch.Hammock;

namespace NancyTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var uri = "http://localhost:8888";
            Console.WriteLine(uri);
            // initialize an instance of NancyHost (found in the Nancy.Hosting.Self package)
            var host = new NancyHost(new Uri(uri));
            host.Start();  // start hosting

            //Under mono if you deamonize a process a Console.ReadLine with cause an EOF 
            //so we need to block another way
            if (args.Any(s => s.Equals("-d", StringComparison.CurrentCultureIgnoreCase)))
            {
                while (true)
                    Thread.Sleep(10000000);
            }
            else
            {
                Console.ReadKey();
            }

            host.Stop();  // stop hosting
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public IList<string> Categories { get; set; }
    }

    internal class MyObject
    {
        public string Name { get; set; }
    }
}

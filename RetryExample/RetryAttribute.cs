using System;
using System.Collections.Generic;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace RetryExample
{
    [PSerializable]
    public class RetryAttribute : MethodInterceptionAspect
    {
        public int Retries { get; set; }

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            var counter = 0;
            while (counter < Retries)
                try
                {
                    base.OnInvoke(args);
                    return;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    counter++;
                }
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
           new MyService().Foo();

            Console.ReadLine();
        }
    }

    public class MyService
    {
        List<int> ints = new List<int>();

        [Retry(Retries = 5)]
        public void Foo()
        {
            ints.Add(0);
            Console.WriteLine(ints[3]);
        }

    }
}
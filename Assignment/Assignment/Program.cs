﻿using System.Threading;
using System.Threading.Tasks;

namespace Assignment
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var startup = new Startup();

            var worker = startup.GetWorker();

            await worker.RunAsync(CancellationToken.None); // todo: provide cancellation token.
        }
    }
}

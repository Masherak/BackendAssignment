using Assignment.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment
{
	public class StringConsoleWriter : IStringWriter
	{
		private BlockingCollection<string> _queue = new BlockingCollection<string>();
		private ManualResetEventSlim _endEvent = new ManualResetEventSlim(false);

		public StringConsoleWriter()
		{
			var thread = new Thread(
				() =>
				{
					while (!_queue.IsCompleted)
					{
						try
						{
							string message = _queue.Take();
							Console.WriteLine(message);
						}
						catch (InvalidOperationException)
						{
							// queue IsCompleted
						}
					}

					_endEvent.Set(); // signal queue completed
				});

			thread.IsBackground = true;
			thread.Start();
		}

		public async Task Write(string message)
		{
			await Task.Run(() => _queue.Add(message));
		}

		public async Task Finish()
		{
			_queue.CompleteAdding();
			while (!_queue.IsCompleted)
			{
				await Task.Run(() => _endEvent.Wait());
			}
		}
	}
}

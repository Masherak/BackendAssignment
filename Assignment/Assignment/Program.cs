using System.Threading.Tasks;

namespace Assignment
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var startup = new Startup();

			var worker = startup.GetWorker();
			var stringWriter = startup.GetStringWriter();

			await worker.RunAsync(stringWriter);
		}
	}
}

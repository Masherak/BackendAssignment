using System.Threading.Tasks;

namespace Assignment.Interfaces
{
	public interface IWorker
	{
		Task RunAsync(IStringWriter writer);
	}
}

using System.Threading.Tasks;

namespace Assignment.Interfaces
{
	public interface IStringWriter
	{
		// there is not needed async/await for this task, but, it can be useful in the future

		Task Write(string message);
		Task Finish();
	}
}

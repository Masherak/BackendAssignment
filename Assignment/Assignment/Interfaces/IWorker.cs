using System.Threading;
using System.Threading.Tasks;

namespace Assignment.Interfaces
{
    public interface IWorker
    {
        Task RunAsync(CancellationToken ct);
    }
}

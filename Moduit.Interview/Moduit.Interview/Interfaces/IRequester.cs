using System.Threading.Tasks;

namespace Moduit.Interview.Interfaces
{
    public interface IRequester
    {
        Task<T> GetRequest<T>(string path) where T : class;
    }
}
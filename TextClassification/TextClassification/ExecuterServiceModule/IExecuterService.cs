using System.Threading.Tasks;

namespace TextClassification.ExecuterServiceModule
{
    public interface IExecuterService<T>
    {
        Task<T> Execute();
    }
}

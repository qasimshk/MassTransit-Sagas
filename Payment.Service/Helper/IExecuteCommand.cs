using CQRS.Business.Utils;
using System.Threading.Tasks;

namespace Payment.Service.Helper
{
    public interface IExecuteCommand
    {
        Task<Results> ExecuteAsync(object command);
    }
}
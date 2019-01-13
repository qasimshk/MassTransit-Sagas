using CQRS.Business.Utils;
using System.Threading.Tasks;

namespace CQRS.Business
{
    public interface ICommandPatchHandler<TCommand> where TCommand : class, new()
    {
        Task<Results> Handle(TCommand command, dynamic Id);
    }
}
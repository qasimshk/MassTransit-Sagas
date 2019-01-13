using CQRS.Business.Utils;
using System.Threading.Tasks;

namespace CQRS.Business
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task<Results> Handle(TCommand command);
    }
}
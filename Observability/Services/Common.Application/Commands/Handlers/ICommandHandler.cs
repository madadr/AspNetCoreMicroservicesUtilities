using System.Threading.Tasks;

namespace Common.Application.Commands.Handlers
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task HandleAsync(T command);
    }
}
using System.Threading.Tasks;

namespace Tickets.Application.Commands.Handlers
{
    public interface ICommandHandler<T> where T : ICommand
    {
        public Task HandleAsync(T command);
    }
}
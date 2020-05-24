using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Common.Application.Commands.Handlers
{
    public abstract class CommandHandlerBase<T> : ICommandHandler<T> where T : ICommand
    {
        protected readonly ILogger<CommandHandlerBase<T>> Logger;

        protected CommandHandlerBase(ILogger<CommandHandlerBase<T>> logger)
        {
            Logger = logger;
        }

        public async Task LogHandleAsync(T command)
        {
            await Task.Run(() => Logger.LogInformation($"Handling {command}: {JsonConvert.SerializeObject(command)}"));
        }

        public abstract Task HandleAsync(T command);
    }
}
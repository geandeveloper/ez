using EzCommon.Commands;
using EzCommon.Models;
using MediatR;

namespace EzCommon.CommandHandlers
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, EventStream>
          where TCommand : ICommand
    {
    }
}


using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Commands;
using EzCommon.Models;

namespace EzGym.Accounts.CheckIn
{
    public record CompleteCheckInCommand(string AccountId, string GymId) : ICommand;

    public class CompleteCheckInCommandHandler : ICommandHandler<CompleteCheckInCommand>
    {
        public Task<EventStream> Handle(CompleteCheckInCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}

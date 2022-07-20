using EzCommon.Models;
using MediatR;

namespace EzCommon.Commands
{
    public interface ICommand : IRequest<EventStream>
    {
    }
}

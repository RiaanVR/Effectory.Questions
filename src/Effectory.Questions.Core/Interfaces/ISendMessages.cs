using Effectory.Questions.Contract;

namespace Effectory.Questions.Core.Interfaces
{
    public interface ISendMessages
    {
        Task PublishMessage(IEvent @event);
        Task PublishMessages(IEnumerable<IEvent> events);
    }
}

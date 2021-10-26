using Effectory.Questions.Contract;

namespace Effectory.Questions.Core
{
    public abstract class Entity<TIdentity> : IEquatable<Entity<TIdentity>> where TIdentity : ISingleValueObject
    {
        public TIdentity Id { get; }

        protected Entity(TIdentity id)
        {
            Id = id;
        }

        public bool Equals(Entity<TIdentity>? other) => Id.Equals(Id);

        private readonly List<IEvent> _uncommittedEvents = new List<IEvent>();

        public virtual IEnumerable<IEvent> GetUncommittedEvents() => _uncommittedEvents.AsEnumerable();
        public void RaiseEvent(IEvent @event) => _uncommittedEvents.Add(@event);

        public void ClearEvents() => _uncommittedEvents.Clear();
    }

    
}

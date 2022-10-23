namespace Competitions.SharedKernel.Common
{
    public abstract class BaseEntity<TId>
    {
        public TId Id { get; protected set; }

        public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();

        public override bool Equals ( object? obj )
        {
            var other = obj as BaseEntity<TId>;

            if ( ReferenceEquals(other , null) )
                return false;

            if ( ReferenceEquals(other , this) )
                return true;

            if ( GetType() != other.GetType() )
                return false;

            if ( Id.Equals(default) || other.Id.Equals(default) )
                return false;

            return Id.Equals(other.Id);
        }
    }
}

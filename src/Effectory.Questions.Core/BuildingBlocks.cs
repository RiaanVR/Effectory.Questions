using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Effectory.Questions.Core
{
    public interface ISingleValueObject
    {
        object GetValue();
    }
    public class SingleValueObject<T>
        : IComparable, ISingleValueObject, IEquatable<T>
        where T : IComparable
    {
        private static readonly Type Type = typeof(T);
        private static readonly TypeInfo TypeInfo = typeof(T).GetTypeInfo();

        public T Value { get; private set; }

        protected SingleValueObject(T value)
        {
            if (TypeInfo.IsEnum && !Enum.IsDefined(Type, value))
            {
                throw new ArgumentException($"The value '{value}' isn't defined in enum '{Type}'");
            }

            Value = value;
        }

        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (!(obj is SingleValueObject<T> other))
            {
                throw new ArgumentException($"Cannot compare '{GetType()}' and '{obj.GetType()}'");
            }

            return Value.CompareTo(other.Value);
        }

        // ReSharper disable once NonReadonlyMemberInGetHashCode
        // Non-readonly due to the serialization mechanism
        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        public override bool Equals(object obj) =>
            obj switch
            {
                SingleValueObject<T> singleValueObject => singleValueObject.Value.Equals(Value),
                T value => Value.Equals(value),
                _ => false
            };

        public override string ToString() => Value is null ? string.Empty : Value.ToString();

        public bool Equals(T other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other);
        }

        public static bool operator ==(SingleValueObject<T> left, T right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SingleValueObject<T> left, T right)
        {
            return !Equals(left, right);
        }

        public static implicit operator T([MaybeNull] SingleValueObject<T> singleValueObject)
            => singleValueObject is null ? default : singleValueObject.Value;

        public static implicit operator SingleValueObject<T>(T item)
            => new SingleValueObject<T>(item);


        public object GetValue()
        {
            return Value;
        }
    }
}

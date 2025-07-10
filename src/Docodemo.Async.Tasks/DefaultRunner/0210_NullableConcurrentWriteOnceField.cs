using System;
using System.Threading;

namespace Docodemo.Async.Tasks.DefaultRunner
{
    /// <summary>
    /// A thread-safe field that allows setting a value only once.
    /// </summary>
    internal class NullableConcurrentWriteOnceField<T>
        where T : notnull
    {
        private static readonly object _unset = new();
        private object? _boxedValue = _unset;

        /// <summary>
        /// Sets the value of the field. If the field is already set, an exception is thrown.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual void Set(T? value)
        {
            var boxed = (object?)value ?? DBNull.Value;

            if (Interlocked.CompareExchange(ref _boxedValue, boxed, _unset) != _unset)
                throw new InvalidOperationException("Value already set.");
        }

        /// <summary>
        /// Gets the value of the field. If the field is not set, an exception is thrown.
        /// </summary>
        public virtual T? Value
        {
            get
            {
                var current = _boxedValue;
                if (current == _unset)
                    throw new InvalidOperationException("Value is not set.");

                return ReferenceEquals(current, DBNull.Value) ? default : (T?)current;
            }
        }

        /// <summary>
        /// Whether the field has been set.
        /// </summary>
        public virtual bool IsSet => !ReferenceEquals(_boxedValue, _unset);
    }
}
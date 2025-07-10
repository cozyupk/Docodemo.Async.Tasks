using System;

namespace Docodemo.Async.Tasks.DefaultRunner
{
    internal class NullableDisposableWriteOnceField<T> : NullableConcurrentWriteOnceField<T>, IDisposable
        where T : notnull, IDisposable
    {
        /// <summary>
        /// Indicates whether the field has been disposed.
        /// </summary>
        private bool IsDisposed { get; set; } = false;

        /// <summary>
        /// Lock object to ensure thread safety when checking or setting the disposed state.
        /// </summary>
        private object DisposedLock { get; } = new();

        /// <summary>
        /// Disposes the field, allowing it to be reused.
        /// If the field has already been disposed, the call is silently ignored.
        /// </summary>
        public void Dispose()
        {
            lock (DisposedLock)
            {
                if (IsDisposed)
                {
                    // Already disposed, nothing to do.
                    return;
                }
                // If the field is not set, there's nothing to dispose.
                if (IsSet)
                {
                    // If the field has a value, dispose it.
                    Value?.Dispose();
                }
                IsDisposed = true;
            }
        }

        /// <summary>
        /// Gets the value of the field.
        /// If the field has been disposed, an exception is thrown.
        /// </summary>
        public override T? Value
        {
            get
            {
                lock (DisposedLock)
                {
                    if (IsDisposed)
                    {
                        throw new ObjectDisposedException(nameof(NullableDisposableWriteOnceField<T>), "The field has been disposed and cannot be accessed.");
                    }
                    return base.Value;
                }
            }
        }

        /// <summary>
        /// Sets the value of the field.
        /// If the field has already been disposed, an exception is thrown.
        /// </summary>
        public override void Set(T? value)
        {
            lock (DisposedLock)
            {
                if (IsDisposed)
                {
                    throw new ObjectDisposedException(nameof(NullableDisposableWriteOnceField<T>), "The field has been disposed and cannot be set.");
                }
                base.Set(value);
            }
        }

        /// <summary>
        /// Overrides the IsSet property to ensure thread safety when checking if the field is set.
        /// </summary>
        public override bool IsSet
        {
            get
            {
                lock (DisposedLock)
                {
                    return base.IsSet;
                }
            }
        }
    }
}

using System;
using Docodemo.Async.Tasks.DefaultRunner;
using Moq;
using Xunit;

namespace Docodemo.Async.Tasks.UnitTests.DefaultRunner
{
    public class DisposableWriteOnceFieldTests
    {
        [Fact]
        public void Set_Then_Get_Value_ShouldReturnValue()
        {
            var mock = new Mock<IDisposable>();
            var field = new NullableDisposableWriteOnceField<IDisposable>();

            field.Set(mock.Object);

            Assert.Equal(mock.Object, field.Value);
        }

        [Fact]
        public void Set_Twice_ShouldThrow()
        {
            var field = new NullableDisposableWriteOnceField<IDisposable>();
            field.Set(Mock.Of<IDisposable>());

            var ex = Assert.Throws<InvalidOperationException>(() =>
                field.Set(Mock.Of<IDisposable>()));

            Assert.Equal("Value already set.", ex.Message);
        }

        [Fact]
        public void Dispose_After_Set_ShouldCallDisposeOnInnerValue()
        {
            var mock = new Mock<IDisposable>();
            var field = new NullableDisposableWriteOnceField<IDisposable>();

            field.Set(mock.Object);
            field.Dispose();

            mock.Verify(x => x.Dispose(), Times.Once);
        }

        [Fact]
        public void Dispose_After_SetNull_ShouldNotThrow()
        {
            var field = new NullableDisposableWriteOnceField<IDisposable>();

            field.Set(null);
            field.Dispose();
        }

        [Fact]
        public void Dispose_Twice_ShouldBeIdempotent()
        {
            var mock = new Mock<IDisposable>();
            var field = new NullableDisposableWriteOnceField<IDisposable>();

            field.Set(mock.Object);
            field.Dispose();
            field.Dispose();

            mock.Verify(x => x.Dispose(), Times.Once);
        }

        [Fact]
        public void Value_After_Dispose_ShouldThrowObjectDisposedException()
        {
            var mock = new Mock<IDisposable>();
            var field = new NullableDisposableWriteOnceField<IDisposable>();

            field.Set(mock.Object);
            field.Dispose();

            var ex = Assert.Throws<ObjectDisposedException>(() => _ = field.Value);
            Assert.Contains(nameof(NullableDisposableWriteOnceField<IDisposable>), ex.ObjectName);
        }

        [Fact]
        public void Set_After_Dispose_ShouldThrowObjectDisposedException()
        {
            var field = new NullableDisposableWriteOnceField<IDisposable>();

            // Manually set IsSet to false to simulate a Dispose without Set
            field.Dispose();

            var ex = Assert.Throws<ObjectDisposedException>(() =>
                field.Set(Mock.Of<IDisposable>()));

            Assert.Contains(nameof(NullableDisposableWriteOnceField<IDisposable>), ex.ObjectName);
        }
    }
}

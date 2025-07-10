using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Docodemo.Async.Tasks.DefaultRunner;
using Xunit;

namespace Docodemo.Async.Tasks.UnitTests.DefaultRunner
{
    public partial class WriteOnceFieldTests
    {
        [Fact]
        public void IsSet_ShouldBeFalseInitially()
        {
            var field = new NullableConcurrentWriteOnceField<int>();
            Assert.False(field.IsSet);
        }

        [Fact]
        public void Set_SetsValue_WhenNotSetBefore()
        {
            var field = new NullableConcurrentWriteOnceField<string>();
            field.Set("Hello");

            Assert.True(field.IsSet);
            Assert.Equal("Hello", field.Value);
        }

        [Fact]
        public void Set_AllowsNullValue()
        {
            var field = new NullableConcurrentWriteOnceField<string>();
            field.Set(null);

            Assert.True(field.IsSet);
            Assert.Null(field.Value);
        }

        [Fact]
        public void Set_ThrowsIfSetTwice()
        {
            var field = new NullableConcurrentWriteOnceField<int>();
            field.Set(42);

            var ex = Assert.Throws<InvalidOperationException>(() => field.Set(99));
            Assert.Equal("Value already set.", ex.Message);
        }

        [Fact]
        public void Value_ThrowsIfNotSet()
        {
            var field = new NullableConcurrentWriteOnceField<string>();
            var ex = Assert.Throws<InvalidOperationException>(() => _ = field.Value);
            Assert.Equal("Value is not set.", ex.Message);
        }

        [Theory]
        [InlineData(123)]
        [InlineData(0)]
        [InlineData(-456)]
        public void Value_ReturnsCorrectValue(int input)
        {
            var field = new NullableConcurrentWriteOnceField<int>();
            field.Set(input);
            Assert.Equal(input, field.Value);
        }

        [Fact]
        public async Task Set_ShouldOnlyAllowOneSuccessfulCall_EvenWhenCalledFromMultipleThreads()
        {
            var field = new NullableConcurrentWriteOnceField<string>();
            var successValues = new ConcurrentBag<string>();
            var exceptions = new ConcurrentBag<Exception>();

            var valuesToTry = Enumerable.Range(0, 10).Select(i => $"Value-{i}").ToArray();

            var tasks = valuesToTry.Select(val =>
                Task.Run(() =>
                {
                    try
                    {
                        field.Set(val);
                        successValues.Add(val);
                    }
                    catch (InvalidOperationException ex)
                    {
                        exceptions.Add(ex);
                    }
                })
            );

            await Task.WhenAll(tasks);

            // 検証: 成功は1つだけ
            Assert.Single(successValues);

            // 検証: 例外が N-1 個発生している
            Assert.Equal(valuesToTry.Length - 1, exceptions.Count);

            // 成功した値は Value プロパティと一致するはず
            Assert.Equal(successValues.Single(), field.Value);
        }
    }
}

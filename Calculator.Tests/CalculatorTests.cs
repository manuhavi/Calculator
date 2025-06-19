using Calculator.Library;
using Xunit;

namespace Calculator.Tests
{
    public class CalculatorTests
    {
        private readonly Calculator.Library.Calculator _calculator;

        public CalculatorTests()
        {
            _calculator = new Calculator.Library.Calculator();
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(-1, 1, 0)]
        [InlineData(0, 0, 0)]
        [InlineData(double.MaxValue, 1, double.MaxValue)]  // Edge case: testing with maximum double value
        public void Add_ShouldReturnExpectedResult(double a, double b, double expected)
        {
            var result = _calculator.Add(a, b);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1, 1, 0)]
        [InlineData(-1, 1, -2)]
        [InlineData(0, 0, 0)]
        [InlineData(double.MinValue, -1, double.MinValue)]  // Edge case: testing with minimum double value
        public void Subtract_ShouldReturnExpectedResult(double a, double b, double expected)
        {
            var result = _calculator.Subtract(a, b);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2, 3, 6)]
        [InlineData(-2, 3, -6)]
        [InlineData(0, 5, 0)]
        [InlineData(1, 0, 0)]
        public void Multiply_ShouldReturnExpectedResult(double a, double b, double expected)
        {
            var result = _calculator.Multiply(a, b);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(6, 2, 3)]
        [InlineData(-6, 2, -3)]
        [InlineData(0, 5, 0)]
        public void Divide_ShouldReturnExpectedResult(double a, double b, double expected)
        {
            var result = _calculator.Divide(a, b);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Divide_ByZero_ShouldThrowException()
        {
            Assert.Throws<DivideByZeroException>(() => _calculator.Divide(1, 0));
        }

        [Fact]
        public void Add_LargeNumbers_ShouldHandleOverflow()
        {
            var result = _calculator.Add(double.MaxValue, double.MaxValue);
            Assert.Equal(double.PositiveInfinity, result);
        }

        [Fact]
        public void Multiply_LargeNumbers_ShouldHandleOverflow()
        {
            var result = _calculator.Multiply(double.MaxValue, 2);
            Assert.Equal(double.PositiveInfinity, result);
        }
    }
}

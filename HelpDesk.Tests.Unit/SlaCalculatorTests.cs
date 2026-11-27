using FluentAssertions;
using HelpDesk.Core.Enums;
using HelpDesk.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Tests.Unit
{
    public class SlaCalculatorTests
    {
        [Theory]
        [InlineData(Priority.Low, 72)]
        [InlineData(Priority.Normal, 24)]
        [InlineData(Priority.High, 8)]
        [InlineData(Priority.Critical, 4)]
        //прибавление нужного количества часов к дате создания заявки
        public void CalculateSlaDeadline_KnownPriority_ReturnsCreatedAtPlusExpectedHours(Priority priority, int expectedHours)
        {
            var createdAt = new DateTimeOffset(2026, 10, 20, 10, 0, 0, TimeSpan.Zero);
            var sut = new SlaCalculator();

            var deadline = sut.CalculateSlaDeadline(createdAt, priority);

            deadline.Should().Be(createdAt.AddHours(expectedHours));
        }

        [Fact]
        //вывод ошибке при неизвестном приоритете
        public void CalculateSlaDeadline_UnknownPriority_ThrowsArgumentOutOfRange()
        {
            var sut = new SlaCalculator();

            Action act = () => sut.CalculateSlaDeadline(DateTimeOffset.UtcNow, (Priority)999);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        //граничное значение для критического приоритета (4 часа)
        public void CalculateSlaDeadline_CriticalBoundary_ReturnsExactlyPlus4Hours()
        {
            var createdAt = new DateTimeOffset(2026, 10, 20, 10, 0, 0, TimeSpan.Zero);
            var sut = new SlaCalculator();

            var deadline = sut.CalculateSlaDeadline(createdAt, Priority.Critical);

            Assert.Equal(createdAt.AddHours(4), deadline);
        }
    }
}

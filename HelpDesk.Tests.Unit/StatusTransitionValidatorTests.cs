using HelpDesk.Core.Enums;
using HelpDesk.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Tests.Unit
{
    public class StatusTransitionValidatorTests
    {
        [Theory]
        //разрешенные переходы
        [InlineData(Status.New, Status.InProgress, true)]
        [InlineData(Status.New, Status.Cancelled, true)]
        [InlineData(Status.InProgress, Status.Resolved, true)]
        [InlineData(Status.InProgress, Status.Cancelled, true)]
        [InlineData(Status.Resolved, Status.Closed, true)]
        //запрещенные переходы
        [InlineData(Status.New, Status.Resolved, false)]
        [InlineData(Status.New, Status.Closed, false)]
        [InlineData(Status.InProgress, Status.New, false)]
        [InlineData(Status.Resolved, Status.InProgress, false)]
        [InlineData(Status.Resolved, Status.New, false)]
        [InlineData(Status.Closed, Status.New, false)]
        [InlineData(Status.Closed, Status.InProgress, false)]
        [InlineData(Status.Cancelled, Status.New, false)]
        public void IsTransitionAllowed_GivenPair_ReturnsExpected(Status from, Status to, bool expected)
        {
            var sut = new StatusTransitionValidator();

            var result = sut.IsTransitionAllowed(from, to);

            Assert.Equal(expected, result);
        }
    }
}

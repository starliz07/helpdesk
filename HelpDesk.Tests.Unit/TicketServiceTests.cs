using FluentAssertions;
using HelpDesk.Core.Enums;
using HelpDesk.Core.Exceptions;
using HelpDesk.Core.Models;
using HelpDesk.Core.Services;
using HelpDesk.Tests.Unit.Fakes;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HelpDesk.Tests.Unit
{
    public class TicketServiceTests
    {
        //использование fake-репозиториев, метод для того, чтобы не писать в кадом тесте
        private static TicketService CreateService(FakeTicketRepository? tickets = null, FakeEmployeeRepository? employees = null)
        {
            return new TicketService(
                tickets ?? new FakeTicketRepository(),
                employees ?? new FakeEmployeeRepository(),
                new StatusTransitionValidator());
        }

        [Fact]
        //при пустом заголовке DomainException и в сообщении есть слово Title
        public async Task CreateTicket_EmptyTitle_ThrowsDomainExceptionWithTitleMessage()
        {
            var sut = CreateService();
            var ticket = new Ticket { Title = "", Description = "ok" };

            var ex = await Assert.ThrowsAsync<DomainException>(() => sut.CreateTicketAsync(ticket));

            Assert.Contains("Title", ex.Message);
        }

        [Fact]
        //если описание больше 2000, то DomainException с 2000
        public async Task CreateTicket_DescriptionOver2000_ThrowsDomainExceptionWithDescriptionMessage()
        {
            var sut = CreateService();
            var ticket = new Ticket { Title = "ok", Description = new string('x', 2001) };

            var ex = await Assert.ThrowsAsync<DomainException>(() => sut.CreateTicketAsync(ticket));

            Assert.Contains("2000", ex.Message);
        }

        [Fact]
        //при передаче несуществующего приоритета DomainException с priority
        public async Task CreateTicket_UnknownPriority_ThrowsDomainExceptionWithPriorityMessage()
        {
            var sut = CreateService();
            var ticket = new Ticket { Title = "ok", Description = "ok", Priority = (Priority)999 };

            var ex = await Assert.ThrowsAsync<DomainException>(() => sut.CreateTicketAsync(ticket));

            Assert.Contains("priority", ex.Message);
        }

        [Fact]
        //нельзя сменить статус с закрыт на новый, DomainException с Недопустимый переход
        public async Task ChangeStatus_ClosedToNew_ThrowsDomainExceptionWithTransitionMessage()
        {
            var repo = new FakeTicketRepository();
            var id = await repo.CreateAsync(new Ticket { Status = Status.Closed });
            var sut = CreateService(repo);

            var ex = await Assert.ThrowsAsync<DomainException>(() => sut.ChangeStatusAsync(id, Status.New, 1));

            Assert.Contains("Недопустимый переход", ex.Message);
        }

        [Fact]
        //нельзя удалить сотрудника если на нем незакрытые заявкм, DomainException
        public async Task SoftDeleteEmployee_WithOpenTickets_ThrowsDomainException()
        {
            var tickets = new FakeTicketRepository();
            await tickets.CreateAsync(new Ticket { AssigneeId = 1, Status = Status.InProgress });
            var sut = CreateService(tickets);

            var ex = await Assert.ThrowsAsync<DomainException>(() => sut.SoftDeleteEmployeeAsync(1));

            Assert.Contains("open tickets", ex.Message);
        }
    }
}

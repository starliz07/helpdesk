using HelpDesk.Core.Interfaces;
using HelpDesk.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Tests.Unit.Fakes
{
    public class FakeEmployeeRepository : IEmployeeRepository
    {
        public List<Employee> Storage { get; } = new();

        public Task<Employee?> GetByIdAsync(int id) =>
            Task.FromResult(Storage.FirstOrDefault(e => e.Id == id && !e.IsDeleted));

        public Task SoftDeleteAsync(int id)
        {
            var e = Storage.FirstOrDefault(x => x.Id == id);
            if (e is not null) e.IsDeleted = true;
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<Employee>> GetActiveAsync() =>
            Task.FromResult<IReadOnlyList<Employee>>(
                Storage.Where(e => !e.IsDeleted).ToList());
    }
}

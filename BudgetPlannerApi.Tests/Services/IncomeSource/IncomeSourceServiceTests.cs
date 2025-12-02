using Xunit;
using FluentAssertions;
using BudgetPlannerApi.DB.Models;
using Bpst.API.DB;
using Bpst.API.Repositories;
using Bpst.API.Services.IncomeSources;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace BudgetPlannerApi.Tests.Services.IncomeSourceTests
{
    public class IncomeSourceServiceTests : IDisposable
    {
        private readonly AppDbContext _db;
        private readonly EfRepository<IncomeSource> _repo;
        private readonly IncomeSourceService _service;

        public IncomeSourceServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _db = new AppDbContext(options);
            _repo = new EfRepository<IncomeSource>(_db);
            _service = new IncomeSourceService(_repo, _db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        [Fact]
        public async Task CreateAsync_ShouldSetUserIdAndCreatedDate()
        {
            var model = new IncomeSource { SourceName = "Test", IncomeAmount = 100 };
            var result = await _service.CreateAsync(model, 1);
            result.UserId.Should().Be(1);
            result.CreatedDate.Should().NotBeNull();

            var persisted = await _db.IncomeSource.FindAsync(result.UniqueId);
            persisted.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAllForUserAsync_ReturnsOnlyUserItems()
        {
            _db.IncomeSource.Add(new IncomeSource { SourceName = "A", UserId = 1 });
            _db.IncomeSource.Add(new IncomeSource { SourceName = "B", UserId = 2 });
            await _db.SaveChangesAsync();

            var result = await _service.GetAllForUserAsync(1);
            result.Should().HaveCount(1);
            result.First().SourceName.Should().Be("A");
        }

        [Fact]
        public async Task GetByIdForUserAsync_ReturnsNullWhenMissing()
        {
            var result = await _service.GetByIdForUserAsync(999, 1);
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ThrowsIfNotFound()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(new IncomeSource { UniqueId = 999 }, 1));
        }

        [Fact]
        public async Task UpdateAsync_ThrowsIfNotOwned()
        {
            var entity = new IncomeSource { SourceName = "X", UserId = 2 };
            _db.IncomeSource.Add(entity);
            await _db.SaveChangesAsync();

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.UpdateAsync(new IncomeSource { UniqueId = entity.UniqueId }, 1));
        }

        [Fact]
        public async Task UpdateAsync_UpdatesAndSaves()
        {
            var entity = new IncomeSource { SourceName = "X", UserId = 1 };
            _db.IncomeSource.Add(entity);
            await _db.SaveChangesAsync();

            // Detach tracked instance to avoid EF conflict when updating with a different instance
            _db.Entry(entity).State = EntityState.Detached;

            var updatedModel = new IncomeSource { UniqueId = entity.UniqueId, SourceName = "Y", IncomeAmount = 200 };
            var result = await _service.UpdateAsync(updatedModel, 1);

            result.LastUpdatedDate.Should().NotBeNull();
            var persisted = await _db.IncomeSource.FindAsync(entity.UniqueId);
            persisted.SourceName.Should().Be("Y");
        }

        [Fact]
        public async Task DeleteAsync_RemovesAndSaves()
        {
            var entity = new IncomeSource { SourceName = "Del", UserId = 1 };
            _db.IncomeSource.Add(entity);
            await _db.SaveChangesAsync();

            await _service.DeleteAsync(entity.UniqueId, 1);

            (await _db.IncomeSource.FindAsync(entity.UniqueId)).Should().BeNull();
        }
    }
}

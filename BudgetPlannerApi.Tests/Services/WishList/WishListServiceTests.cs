using Xunit;
using FluentAssertions;
using BudgetPlannerApplication_2025.Models;
using Bpst.API.DB;
using Bpst.API.Repositories;
using Bpst.API.Services.WishLists;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace BudgetPlannerApi.Tests.Services.WishListTests
{
    public class WishListServiceTests : IDisposable
    {
        private readonly AppDbContext _db;
        private readonly EfRepository<WishList> _repo;
        private readonly WishListService _service;

        public WishListServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _db = new AppDbContext(options);
            _repo = new EfRepository<WishList>(_db);
            _service = new WishListService(_repo, _db);
        }

        public void Dispose() => _db.Dispose();

        [Fact]
        public async Task CreateAsync_PersistsAndReturns()
        {
            var model = new WishList { Item = "T1", Description = "" };
            var result = await _service.CreateAsync(model, 1);
            result.UserId.Should().Be(1);
            (await _db.WishLists.FindAsync(result.UniqueId)).Should().NotBeNull();
        }

        [Fact]
        public async Task GetAllForUserAsync_FiltersByUser()
        {
            _db.WishLists.Add(new WishList { Item = "A", UserId = 1, Description = "" });
            _db.WishLists.Add(new WishList { Item = "B", UserId = 2, Description = "" });
            await _db.SaveChangesAsync();

            var res = await _service.GetAllForUserAsync(1);
            res.Should().HaveCount(1);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsIfNotFound()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(new WishList { UniqueId = 999 }, 1));
        }

        [Fact]
        public async Task UpdateAsync_ThrowsIfNotOwned()
        {
            var entity = new WishList { Item = "X", UserId = 2, Description = "" };
            _db.WishLists.Add(entity);
            await _db.SaveChangesAsync();

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.UpdateAsync(new WishList { UniqueId = entity.UniqueId }, 1));
        }

        [Fact]
        public async Task DeleteAsync_Removes()
        {
            var entity = new WishList { Item = "Del", UserId = 1, Description = "" };
            _db.WishLists.Add(entity);
            await _db.SaveChangesAsync();

            await _service.DeleteAsync(entity.UniqueId, 1);
            (await _db.WishLists.FindAsync(entity.UniqueId)).Should().BeNull();
        }
    }
}

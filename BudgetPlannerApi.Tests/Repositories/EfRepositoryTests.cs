using Xunit;
using FluentAssertions;
using Bpst.API.Repositories;
using Bpst.API.DB;
using BudgetPlannerApplication_2025.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace BudgetPlannerApi.Tests.Repositories
{
    public class EfRepositoryTests : IDisposable
    {
        private readonly AppDbContext _db;
        private readonly EfRepository<WishList> _repo;

        public EfRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _db = new AppDbContext(options);
            _repo = new EfRepository<WishList>(_db);
        }

        public void Dispose() => _db.Dispose();

        [Fact]
        public async Task AddAndGetById_Works()
        {
            var entity = new WishList { Item = "R1", UserId = 1, Description = "" };
            await _repo.AddAsync(entity);
            await _db.SaveChangesAsync();

            var found = await _repo.GetByIdAsync(entity.UniqueId);
            found.Should().NotBeNull();
            found.Item.Should().Be("R1");
        }

        [Fact]
        public async Task GetAllAsync_WithPredicate_Works()
        {
            _db.WishLists.Add(new WishList { Item = "A", UserId = 1, Description = "" });
            _db.WishLists.Add(new WishList { Item = "B", UserId = 2, Description = "" });
            await _db.SaveChangesAsync();

            var list = await _repo.GetAllAsync(w => EF.Property<int?>(w, "UserId") == 1);
            list.Should().HaveCount(1);
        }

        [Fact]
        public async Task UpdateAndRemove_Works()
        {
            var entity = new WishList { Item = "U1", UserId = 1, Description = "" };
            await _repo.AddAsync(entity);
            await _db.SaveChangesAsync();

            entity.Item = "U2";
            await _repo.UpdateAsync(entity);
            await _db.SaveChangesAsync();

            (await _repo.GetByIdAsync(entity.UniqueId)).Item.Should().Be("U2");

            await _repo.RemoveAsync(entity);
            await _db.SaveChangesAsync();

            (await _repo.GetByIdAsync(entity.UniqueId)).Should().BeNull();
        }
    }
}

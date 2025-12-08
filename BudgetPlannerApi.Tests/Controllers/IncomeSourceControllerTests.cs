using Xunit;
using Moq;
using FluentAssertions;
using BudgetPlannerApi.Controllers;
using BudgetPlannerApi.DB.Models;
using Bpst.API.Services.IncomeSources;
using Bpst.API.Services.UserAccount;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace BudgetPlannerApi.Tests.Controllers
{
    public class IncomeSourceControllerTests
    {
        private readonly Mock<IIncomeSourceService> _serviceMock;
        private readonly Mock<IUserAccountService> _userMock;
        private readonly IncomeSourceController _controller;

        public IncomeSourceControllerTests()
        {
            _serviceMock = new Mock<IIncomeSourceService>();
            _userMock = new Mock<IUserAccountService>();
            _controller = new IncomeSourceController(_serviceMock.Object, _userMock.Object);
        }

        [Fact]
        public async Task GetIncomeSource_ReturnsUnauthorized_IfNoUser()
        {
            _userMock.Setup(u => u.GetLoggedInUserId()).Returns((int?)null);
            var result = await _controller.GetIncomeSource();
            result.Result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task GetIncomeSource_ReturnsOk()
        {
            _userMock.Setup(u => u.GetLoggedInUserId()).Returns(1);
            _serviceMock.Setup(s => s.GetAllForUserAsync(1)).ReturnsAsync(new List<IncomeSource> { new IncomeSource { UniqueId = 1 } });
            var result = await _controller.GetIncomeSource();
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CreateIncomeSource_ReturnsUnauthorized_IfNoUser()
        {
            _userMock.Setup(u => u.GetLoggedInUserId()).Returns((int?)null);
            var result = await _controller.CreateIncomeSource(new IncomeSource());
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task CreateIncomeSource_ReturnsBadRequest_IfNull()
        {
            _userMock.Setup(u => u.GetLoggedInUserId()).Returns(1);
            var result = await _controller.CreateIncomeSource(null);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateIncomeSource_ReturnsCreated()
        {
            _userMock.Setup(u => u.GetLoggedInUserId()).Returns(1);
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<IncomeSource>(), 1)).ReturnsAsync(new IncomeSource { UniqueId = 1 });
            var result = await _controller.CreateIncomeSource(new IncomeSource());
            result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task UpdateIncomeSource_ReturnsBadRequest_IfNullOrIdMismatch()
        {
            _userMock.Setup(u => u.GetLoggedInUserId()).Returns(1);
            var result = await _controller.UpdateIncomeSource(1, null);
            result.Should().BeOfType<BadRequestObjectResult>();
            result = await _controller.UpdateIncomeSource(2, new IncomeSource { UniqueId = 1 });
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateIncomeSource_ReturnsNotFound_IfMissing()
        {
            _userMock.Setup(u => u.GetLoggedInUserId()).Returns(1);
            _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<IncomeSource>(), 1)).ThrowsAsync(new KeyNotFoundException());
            var result = await _controller.UpdateIncomeSource(1, new IncomeSource { UniqueId = 1 });
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task UpdateIncomeSource_ReturnsForbid_IfNotOwned()
        {
            _userMock.Setup(u => u.GetLoggedInUserId()).Returns(1);
            _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<IncomeSource>(), 1)).ThrowsAsync(new UnauthorizedAccessException());
            var result = await _controller.UpdateIncomeSource(1, new IncomeSource { UniqueId = 1 });
            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task UpdateIncomeSource_ReturnsOk_IfUpdated()
        {
            _userMock.Setup(u => u.GetLoggedInUserId()).Returns(1);
            _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<IncomeSource>(), 1)).ReturnsAsync(new IncomeSource { UniqueId = 1 });
            var result = await _controller.UpdateIncomeSource(1, new IncomeSource { UniqueId = 1 });
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task DeleteIncomeSource_ReturnsUnauthorized_IfNoUser()
        {
            _userMock.Setup(u => u.GetLoggedInUserId()).Returns((int?)null);
            var result = await _controller.DeleteIncomeSource(1);
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task DeleteIncomeSource_ReturnsNotFound_IfMissing()
        {
            _userMock.Setup(u => u.GetLoggedInUserId()).Returns(1);
            _serviceMock.Setup(s => s.DeleteAsync(1, 1)).ThrowsAsync(new KeyNotFoundException());
            var result = await _controller.DeleteIncomeSource(1);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteIncomeSource_ReturnsNoContent_IfDeleted()
        {
            _userMock.Setup(u => u.GetLoggedInUserId()).Returns(1);
            _serviceMock.Setup(s => s.DeleteAsync(1, 1)).Returns(Task.CompletedTask);
            var result = await _controller.DeleteIncomeSource(1);
            result.Should().BeOfType<NoContentResult>();
        }
    }
}

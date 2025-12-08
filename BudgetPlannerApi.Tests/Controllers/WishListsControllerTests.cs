using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using BudgetPlannerApplication_2025.Models;
using Bpst.API.Services.WishLists;
using BudgetPlannerApplication_2025.Controllers;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BudgetPlannerApi.Tests.Controllers
{
    public class WishListsControllerTests
    {
        private readonly Mock<IWishListService> _serviceMock;
        private readonly WishListsController _controller;

        public WishListsControllerTests()
        {
            _serviceMock = new Mock<IWishListService>();
            _controller = new WishListsController(_serviceMock.Object);
        }

        private void SetUser(int? userId)
        {
            if (userId == null)
                _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            else
            {
                var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
                var identity = new ClaimsIdentity(claims, "TestAuth");
                var principal = new ClaimsPrincipal(identity);
                _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = principal } };
            }
        }

        [Fact]
        public async Task GetWishLists_Unauthorized_IfNoUser()
        {
            SetUser(null);
            var result = await _controller.GetWishLists();
            result.Result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task GetWishLists_ReturnsOk()
        {
            SetUser(1);
            _serviceMock.Setup(s => s.GetAllForUserAsync(1)).ReturnsAsync(new List<WishList> { new WishList { UniqueId = 1 } });
            var result = await _controller.GetWishLists();
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CreateWishList_ReturnsBadRequest_IfNull()
        {
            SetUser(1);
            var res = await _controller.CreateWishList(null);
            res.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateWishList_ReturnsCreated()
        {
            SetUser(1);
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<WishList>(), 1)).ReturnsAsync(new WishList { UniqueId = 1 });
            var res = await _controller.CreateWishList(new WishList());
            res.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task UpdateWishList_ReturnsBadRequest_IfMismatch()
        {
            SetUser(1);
            var res = await _controller.UpdateWishList(1, new WishList { UniqueId = 2 });
            res.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteWishList_ReturnsNoContent_WhenDeleted()
        {
            SetUser(1);
            _serviceMock.Setup(s => s.DeleteAsync(1, 1)).Returns(Task.CompletedTask);
            var res = await _controller.DeleteWishList(1);
            res.Should().BeOfType<NoContentResult>();
        }
    }
}

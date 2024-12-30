using fightnight.Server.Controllers;
using fightnight.Server.Models.Tables;
using Microsoft.AspNetCore.Identity;
using MetInProximityBack.Factories;
using Moq;
using fightnight.Server.Interfaces.IServices;
using fightnight.Server.Interfaces;
using MetInProximityBack.Interfaces;

namespace fightnight.Tests
{
    public class RegisterTests
    {
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly Mock<OAuthProviderFactory> _mockProviderFactory;
        private readonly Mock<IInviteService> _mockInviteService;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IOAuthService> _mockOAuthService;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<IEmailService> _mockEmailService;

        private readonly AccountController _controller;

        public RegisterTests()
        {
            _mockUserManager = new Mock<UserManager<AppUser>>();
            _mockProviderFactory = new Mock<OAuthProviderFactory>();
            _mockInviteService = new Mock<IInviteService>();
            _mockTokenService = new Mock<ITokenService>();
            _mockOAuthService = new Mock<IOAuthService>();
            _mockAuthService = new Mock<IAuthService>();
            _mockEmailService = new Mock<IEmailService>();

            // Create the controller instance with the mocked dependencies
            _controller = new AccountController(
                _mockUserManager.Object,
                _mockProviderFactory.Object,
                _mockInviteService.Object,
                _mockTokenService.Object,
                _mockOAuthService.Object,
                _mockAuthService.Object,
                _mockEmailService.Object
            );
        }

        [Fact]
        public void TestInput()
        {
            // Model State, 
            // Unique Email
            // When invite service works or not, TEST INDIVIDUALL INVITE SERVICE TO MAKE SURE IT CHANGES APP USER TO EMAIL CONFIRMED
        }

        [Fact]
        public void Test()
        {

        }
    }
}
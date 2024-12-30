using Azure;
using fightnight.Server.Interfaces;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Models.Tables;
using fightnight.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace fightnight.Tests
{
    
    public class InviteServiceTests
    {
        private readonly InviteService _inviteService;
        private readonly Mock<IInviteRepo> _mockInviteRepo;
        private readonly Mock<IMemberRepo> _mockMemberRepo;

        public InviteServiceTests()
        {
            _mockInviteRepo = new Mock<IInviteRepo>();
            _mockMemberRepo = new Mock<IMemberRepo>();

            _inviteService = new InviteService(
                _mockInviteRepo.Object,
                _mockMemberRepo.Object
            ); 
        }

        [Fact]
        //AppUser and Response should change 
        public async void TestChangeOfParams()
        {
            // Arrage
            AppUser appUser = new AppUser
            {
                UserName = "UserName",
                Email = "Email",
                EmailConfirmed = false
            };

            var httpContext = new DefaultHttpContext();

            var locationHeader = httpContext.Response.Headers.Location;
            string newRedirectUrl = "https://localhost:5173/4321/team";

            // SET up REPO Mock




            // Action
            // Invite Exists, Params should change
            await _inviteService.UpdateUserAsync(appUser, "1234", httpContext.Response);

            // Assess
            Assert.True(appUser.EmailConfirmed);

            Assert.NotEqual(locationHeader, httpContext.Response.Headers.Location);
            Assert.Equal(httpContext.Response.Headers.Location, newRedirectUrl);

            // Action
            // Invite Doesnt Exist, Params shouldnt change
            await _inviteService.UpdateUserAsync(appUser, null, httpContext.Response);

            // Assess
            Assert.False(appUser.EmailConfirmed);

            Assert.Equal(locationHeader, httpContext.Response.Headers.Location);


        }
    }


    
}

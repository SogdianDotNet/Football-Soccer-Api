using FL.Common.Base.Data;
using FL.Data.Domain.Entities.Authentication;
using FL.Services.DatabaseAccessors.ApiKeys;
using FL.Shared.ClientHttp.Requests;
using FL.Shared.DataContracts;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FL.Test.Services
{
    public class AuthenticationServiceTest
    {
        private ILogger<AuthenticationService> logger;
        private IRepository<ApiKeyEntity> repository;
        private AuthenticationService authService;

        [SetUp]
        public void SetUp()
        {
            logger = new Mock<ILogger<AuthenticationService>>().Object;
            repository = new Mock<IRepository<ApiKeyEntity>>().Object;
            authService = new AuthenticationService(logger, repository);
        }

        [Test]
        public void Request_With_NonExisting_Credentials_Should_FailAsync()
        {
            var authRequest = new AuthenticateRequest("nonexisting", "nonexisting");
            var response = authService.Authenticate(authRequest).Result;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.DoesExist, Is.False);
        }

        [Test]
        public void Authenticate_Throws_ArgumentNullException_WithNullParameter()
        {
            AuthenticateRequest request = null;

            Assert.That(async () => await authService.Authenticate(request), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Authenticate_Throws_ArgumentNullException_With_NullValues()
        {
            var authRequest = new AuthenticateRequest();

            Assert.That(async () => await authService.Authenticate(authRequest), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Request_Without_Username_Should_Throw_ArgumentNullException()
        {
            var authRequest = new AuthenticateRequest { ApiKey = "testvalue" };

            Assert.That(async () => await authService.Authenticate(authRequest), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Request_Without_ApiKey_Should_Throw_ArgumentNullException()
        {
            var authRequest = new AuthenticateRequest { Username = "usernametest" };

            Assert.That(async () => await authService.Authenticate(authRequest), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        [Ignore("Data in the database is not implemented yet")]
        public async Task Authentication_Should_Pass_With_Existing_CredentialsAsync()
        {
            var username = string.Empty;
            var apiKey = string.Empty;
            var authRequest = new AuthenticateRequest(username, apiKey);

            var response = await authService.Authenticate(authRequest);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Username.ToLower(), Is.EqualTo(username.ToLower()));
            Assert.That(response.ApiKey.ToLower(), Is.EqualTo(apiKey.ToLower()));
        }
    }
}

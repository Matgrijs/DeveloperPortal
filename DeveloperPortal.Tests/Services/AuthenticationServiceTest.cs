using DeveloperPortal.Services;
using JetBrains.Annotations;
using Xunit;
using Shouldly;

namespace DeveloperPortal.Tests.Services
{
    [TestSubject(typeof(AuthenticationService))]
    public class AuthenticationServiceTest
    {
        [Fact]
        public void Initialize_Should_Populate_Properties()
        {
            // Prepare
            var service = AuthenticationService.Instance;

            // Act
            service.Initialize("username", "auth0Id", "abcd1234");

            // Assert
            service.UserName.ShouldBe("username");
            service.Auth0Id.ShouldBe("auth0Id");
            service.Token.ShouldBe("abcd1234");
        }

        [Fact]
        public void Initialize_With_Null_Values_Should_Populate_Properties_As_Null()
        {
            // Prepare
            var service = AuthenticationService.Instance;

            // Act
            service.Initialize(null, null, null);

            // Assert
            service.UserName.ShouldBeNull();
            service.Auth0Id.ShouldBeNull();
            service.Token.ShouldBeNull();
        }

        [Fact]
        public void Initialize_With_Empty_Values_Should_Populate_Properties_As_Null()
        {
            // Prepare
            var service = AuthenticationService.Instance;

            // Act
            service.Initialize("", "", "");

            // Assert
            service.UserName.ShouldBe("");
            service.Auth0Id.ShouldBe("");
            service.Token.ShouldBe("");
        }

        [Fact]
        public void Instance_Should_Return_Same_Instance()
        {
            // Prepare
            var service1 = AuthenticationService.Instance;
            var service2 = AuthenticationService.Instance;

            // Assert
            service1.ShouldBe(service2);
        }
    }
}
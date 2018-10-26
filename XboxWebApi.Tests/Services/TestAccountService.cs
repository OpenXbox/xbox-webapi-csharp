using Moq;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using XboxWebApi.Authentication;
using XboxWebApi.Extensions;
using XboxWebApi.Services;
using XboxWebApi.Services.Api;
using XboxWebApi.Services.Model.Account;

namespace XboxWebApi.Tests.Services
{
    public class TestAccountService
    {
        [Test]
        [Category("unit")]
        public void GetFamilyAccount_Request()
        {
            //Assemble
            const ulong xuid = 1234567890;
            IRestRequest request = null;

            var parameters = new List<Parameter>();

            var mock = new Mock<IRestSharpEx>();
            mock.Setup(r => r.DefaultParameters).Returns(parameters);
            
            mock.SetupGenericExecute<AccountResponse>()
                .Callback((IRestRequest req) =>
                {
                    request = req;
                })
                .AddGenericExecuteResponse(new RestResponse<AccountResponse>());

            var token = new XToken
            {
                Jwt = string.Empty
            };

            var configMock = new Mock<IXblConfiguration>();
            configMock.Setup(c => c.Userhash).Returns(string.Empty);
            configMock.Setup(c => c.xToken).Returns(token);

            var sut = new AccountService(configMock.Object, mock.Object);

            //Act
            sut.GetFamilyAccount(xuid);

            //Assert
            Assert.IsNotNull(request);
            Assert.IsNotNull(request.Resource);
            Assert.IsNotEmpty(request.Resource);
            Assert.IsTrue(request.Resource.Contains(xuid.ToString()));
        }
    }
}

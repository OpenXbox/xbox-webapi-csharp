using Moq;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using XboxWebApi.Authentication;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Api
{
    public class TestAchievementService
    {
        [Test]
        [Category("unit")]
        public void GetAchievementGameprogress_Request()
        {
            //Assemble
            const ulong xuid = 1234567890;
            const ulong titleId = 10987654321;
            IRestRequest request = null;

            var parameters = new List<Parameter>();

            var mock = new Mock<IRestSharpEx>();
            mock.Setup(r => r.DefaultParameters).Returns(parameters);

            mock.Setup(r => r.Execute(It.IsAny<RestRequestEx>()))
                .Callback((IRestRequest req) => { request = req; })
                .Returns(new RestResponse());

            var token = new XToken
            {
                Jwt = string.Empty
            };

            var configMock = new Mock<IXblConfiguration>();
            configMock.Setup(c => c.Userhash).Returns(string.Empty);
            configMock.Setup(c => c.xToken).Returns(token);

            var sut = new AchievementService(configMock.Object, mock.Object);

            //Act
            sut.GetAchievementGameprogress(xuid, titleId);

            //Assert
            Assert.IsNotNull(request);
            Assert.IsNotNull(request.Resource);
            Assert.IsNotEmpty(request.Resource);
            Assert.IsTrue(request.Resource.Contains(xuid.ToString()));
        }
    }
}
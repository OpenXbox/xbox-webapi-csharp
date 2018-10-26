using Moq;
using Moq.Language.Flow;
using RestSharp;
using XboxWebApi.Extensions;

namespace XboxWebApi.Tests
{
    public static class TestExtensions
    {
        public static ISetup<IRestSharpEx, IRestResponse<T>> SetupGenericExecute<T>(this Mock<IRestSharpEx> client) 
            where T : class, new() =>
                client.Setup(r => r.Execute<T>(It.IsAny<RestRequestEx>()));

        public static void AddGenericExecuteResponse<T>(
            this IReturnsThrows<IRestSharpEx, IRestResponse<T>> setup,
            RestResponse<T> response) 
                where T : class, new() => 
                    setup.Returns(response);
    }
}

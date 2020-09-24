using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace ProductApiExample.Api.Test.Helpers
{
    internal static class ServiceMockHelper
    {
        public static ILogger<T> CreateLoggerMock<T>() => new Mock<ILogger<T>>().Object;
        public static IMapper CreateMapperMock() => new Mock<IMapper>().Object;
    }
}

using System;
using AutoMapper;

namespace ProductApiExample.Api.Utils
{
    internal class MappingHelper
    {
        /// <summary>
        /// Creates mapper for this object classes
        /// </summary>
        public static IMapper CreateMapper()
        {
            return new MapperConfiguration(MappingConfiguration).CreateMapper();
        }

        /// <summary>
        /// Sets up configuration for this object classes mapper
        /// </summary>
        public static Action<IMapperConfigurationExpression> MappingConfiguration = cfg =>
        {
            cfg.CreateMap<DataLayer.Entities.Product, Dto.Product>();
        };
    }
}

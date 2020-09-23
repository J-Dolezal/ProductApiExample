using System;
using AutoMapper;
using ProductApiExample.Api.Dto.v1_0;

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
            cfg.CreateMap<DataLayer.Entities.Product, Product>();
        };
    }
}

// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.
using GenericServices.Configuration;
using GenericServices.Helpers.GenericServices.Helpers;
using GenericServices.PublicButHidden;
using GenericServices.Setup.Internal;
using Mapster;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Tests.Helpers
{
    public static class MapsterTestHelpers
    {
        public static TypeAdapterSetter<TEntity, TDto> CreateReadConfig<TEntity, TDto>(
            PerDtoConfig<TDto, TEntity> alterMapping, ILoggerFactory loggerFactory = null)
            where TDto : class where TEntity : class
        {
            loggerFactory ??= NullLoggerFactory.Instance;

            TypeAdapterSetter<TEntity, TDto> readConfig;

            var readProfile = new MappingProfile(false);
            if (!(alterMapping?.ConfigureReadMapping(out readConfig) ?? false)) // if true, read mapping is configured and no need to reconfigure 
            {
                // Create a default read mapping using Mapster's conventions
                readConfig = TypeAdapterConfig<TEntity, TDto>.NewConfig()
                    .SetIgnoreReadOnly(readProfile.IgnoreReadOnlyAttributes);  // Automatically apply ReadOnly attribute information based on profile
            }


            /*
            alterMapping(readProfile.CreateMap<TEntity, TDto>());
            var readConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(readProfile);
            },
            loggerFactory);
            */

            return readConfig;
        }

        public static WrappedAndMapper CreateWrapperMapper<TDto, TEntity>(ILoggerFactory loggerFactory = null)
        {
            loggerFactory ??= NullLoggerFactory.Instance;

            var readProfile = new MappingProfile(false);

            TypeAdapterConfig<TEntity, TDto>.NewConfig();
            /*
            readProfile.CreateMap<TEntity, TDto>();
            var readConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(readProfile);
            }, loggerFactory);
            */

            var saveProfile = new MappingProfile(true);
            TypeAdapterConfig<TDto, TEntity>.NewConfig()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .SetIgnoreReadOnly(true);
            /*
            saveProfile.CreateMap<TDto, TEntity>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            var saveConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(saveProfile);
            }, loggerFactory);
            */

            return new WrappedAndMapper(new GenericServicesConfig(), null, null); // no need for specific config
        }
    }
}
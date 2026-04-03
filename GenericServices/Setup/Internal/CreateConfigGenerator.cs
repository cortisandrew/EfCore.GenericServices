// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using GenericServices.Configuration;
using GenericServices.Helpers.GenericServices.Helpers;
using GenericServices.Internal.Decoders;
using Mapster;
using System;

namespace GenericServices.Setup.Internal
{
    internal class CreateConfigGenerator
    {
        public CreateConfigGenerator(Type dtoType, DecodedEntityClass entityInfo, object configInfo)
        {
            var myGeneric = typeof(ConfigGenerator<,>);
            var copierType = myGeneric.MakeGenericType(dtoType, entityInfo.EntityType);
            Accessor = Activator.CreateInstance(copierType, new object[] { configInfo });
        }

        public dynamic Accessor { get; }

        public class ConfigGenerator<TDto, TEntity>
            where TDto : class
            where TEntity : class
        {
            private readonly PerDtoConfig<TDto, TEntity> _config;

            public ConfigGenerator(PerDtoConfig<TDto, TEntity> config)
            {
                _config = config;
            }

            /// <summary>
            /// </summary>
            /// <param name="profile"></param>
            /// <returns></returns>
            /// <remarks>
            /// This is within the internal namespace - we are assuming that the scan and configuration only happens once, otherwise the mappings will be compiled multiple times
            /// </remarks>
            public TypeAdapterSetter<TEntity, TDto> ConfigureReadMapping(MappingProfile profile)
            {
                // Remarks:
                // TODO: Optional, potential improvement: customise behaviour globally, e.g.:
                // config.Default.PreserveReference(true); // for circular references
                // config.Default.IgnoreNullValues(true);  // ignore nulls
                // config.NameMatchingStrategy(NameMatchingStrategy.Flexible); // ignore case, underscores, etc.
                // config.IgnoreNullValues(true);
                // This allows for users to setup global customisation for uninitialised typeAdapterSetter. Possibly we can keep a separate default for read and save
                // The idea is to allow users to configure the default behaviour without requiring to modify the code

                TypeAdapterSetter<TEntity, TDto> typeAdapterSetter;

                if (!(_config?.ConfigureReadMapping(out typeAdapterSetter) ?? false)) // if true, read mapping is configured and no need to reconfigure 
                {
                    // Create a default read mapping using Mapster's conventions
                    typeAdapterSetter = TypeAdapterConfig<TEntity, TDto>.NewConfig()
                        .SetIgnoreReadOnly(profile.IgnoreReadOnlyAttributes);  // Automatically apply ReadOnly attribute information based on profile
                }

                // typeAdapterSetter.Compile(); // Compiling here causes issues later...

                return typeAdapterSetter;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="profile"></param>
            /// <returns></returns>
            /// <remarks>
            /// This is within the internal namespace - we are assuming that the scan and configuration only happens once, otherwise the mappings will be compiled multiple times
            /// </remarks>
            public TypeAdapterSetter<TDto, TEntity> ConfigureSaveMapping(MappingProfile profile)
            {
                TypeAdapterSetter<TDto, TEntity> typeAdapterSetter;

                // Same as before - if no config exists, create default
                if (!(_config?.ConfigureSaveMapping(out typeAdapterSetter) ?? false))
                {
                    typeAdapterSetter = TypeAdapterConfig<TDto, TEntity>.NewConfig()
                        .SetIgnoreReadOnly(profile.IgnoreReadOnlyAttributes); // Automatically apply ReadOnly attribute information based on profile
                }

                // typeAdapterSetter.Compile(); // Compiling here causes issues later...

                return typeAdapterSetter;
            }
        }
    }
}
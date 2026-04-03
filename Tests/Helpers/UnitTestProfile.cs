// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using GenericServices.Configuration;
using GenericServices.Helpers.GenericServices.Helpers;
using GenericServices.Setup.Internal;
using Mapster;

namespace Tests.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// In programs, you should only configure the mapping once at startup
    /// </remarks>
    public class UnitTestProfile
    {
        MappingProfile _mappingProfile;

        public UnitTestProfile(bool addIgnoreParts)
        {
            _mappingProfile = new MappingProfile(addIgnoreParts);
        }

        public void AddReadMap<TIn, TOut>(PerDtoConfig<TIn, TOut> alterMapping = null)
            where TIn : class where TOut : class
        {
            // TypeAdapterSetter<TOut, TIn>? setter;

            if (!(alterMapping?.ConfigureReadMapping(out _) ?? false)) // if true, read mapping is configured and no need to reconfigure 
            {
                // Create a default read mapping using Mapster's conventions
                TypeAdapterConfig<TIn, TOut>.NewConfig();
            }
        }

        public void AddWriteMap<TIn, TOut>(PerDtoConfig<TIn, TOut> alterMapping = null)
            where TIn : class where TOut : class
        {
            if (!(alterMapping?.ConfigureSaveMapping(out _) ?? false)) // if true, read mapping is configured and no need to reconfigure 
            {
                // Create a default write mapping using Mapster's conventions
                TypeAdapterConfig<TIn, TOut>.NewConfig()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter()
                    .SetIgnoreReadOnly(_mappingProfile.IgnoreReadOnlyAttributes);  // Automatically apply ReadOnly attribute information based on profile
            }
        }
    }
}
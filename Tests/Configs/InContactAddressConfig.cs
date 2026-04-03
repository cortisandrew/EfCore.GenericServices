// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using GenericServices.Configuration;
using Mapster;
using Tests.Dtos;
using Tests.EfClasses;

namespace Tests.Configs
{
    public class InContactAddressConfig : PerDtoConfig<InContactAddressDto, ContactAddress>
    {
        public override bool ConfigureSaveMapping(out TypeAdapterSetter<InContactAddressDto, ContactAddress> typeAdapterSetter)
        {
            typeAdapterSetter =
                TypeAdapterConfig<InContactAddressDto, ContactAddress>
                    .NewConfig()
                    .Map(dest => dest.Address, src => src.Addess);

            return true;
        }
    }
}
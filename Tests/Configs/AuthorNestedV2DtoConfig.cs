// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.


using DataLayer.EfClasses;
using GenericServices.Configuration;
using GenericServices.Helpers.GenericServices.Helpers;
using Mapster;
using Tests.Dtos;

namespace Tests.Configs
{
    public class AuthorNestedV2DtoConfig : PerDtoConfig<AuthorNestedV2Dto, BookAuthor>
    {
        public override bool ConfigureReadMapping(out TypeAdapterSetter<BookAuthor, AuthorNestedV2Dto> typeAdapterSetter)
        {
            typeAdapterSetter = TypeAdapterConfig<BookAuthor, AuthorNestedV2Dto>
                .NewConfig()
                .Map(
                    dest => dest.AStringToHoldAuthorName,
                    src => src.Author.Name
                )
                .SetIgnoreReadOnly(false); // Not strictly required since we are passing false here, but we want to test the change

            return true;
        }
    }
}
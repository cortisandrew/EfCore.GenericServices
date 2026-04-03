// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using DataLayer.EfClasses;
using GenericServices.Configuration;
using Mapster;
using System.Linq;
using Tests.Dtos;

namespace Tests.Configs
{
    public class BookWithTagsConfig : PerDtoConfig<BookWithTags, Book>
    {
        public override bool ConfigureReadMapping(out TypeAdapterSetter<Book, BookWithTags> typeAdapterSetter)
        {
            typeAdapterSetter = TypeAdapterConfig<Book, BookWithTags>
                .NewConfig()
                .Map
                (
                    dest => dest.TagIds,
                    src => src.Tags.Select(y => y.TagId).ToList()
                );

            return true;
        }
    }
}
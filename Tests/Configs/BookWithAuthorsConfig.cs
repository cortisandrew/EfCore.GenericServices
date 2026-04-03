// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.


using DataLayer.EfClasses;
using GenericServices.Configuration;
using Mapster;
using System.Linq;
using Tests.Dtos;

namespace Tests.Configs
{
    public class BookWithAuthorsConfig : PerDtoConfig<BookWithAuthors, Book>
    {
        public override bool ConfigureReadMapping(out TypeAdapterSetter<Book, BookWithAuthors> typeAdapterSetter)
        {
            typeAdapterSetter = TypeAdapterConfig<Book, BookWithAuthors>
                .NewConfig()
                .Map(
                    dest => dest.Authors,
                    src => src.AuthorsLink.OrderBy(y => y.Order).Select(z => z.Author.Name).ToList()
                );

            return true; ;
        }
    }
}
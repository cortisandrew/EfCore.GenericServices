// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using DataLayer.EfClasses;
using GenericServices.Configuration;
using GenericServices.Helpers.GenericServices.Helpers;
using Mapster;
using System.Linq;

namespace ServiceLayer.HomeController.Dtos
{
    class DeleteBookDtoConfig : PerDtoConfig<DeleteBookDto, Book>
    {
        public override bool ConfigureReadMapping(out TypeAdapterSetter<Book, DeleteBookDto> typeAdapterSetter)
        {
            typeAdapterSetter = TypeAdapterConfig<Book, DeleteBookDto>
                .NewConfig()
                .Map(
                    dest => dest.AuthorsOrdered,
                    src => string.Join(", ",
                            src.AuthorsLink.OrderBy(q => q.Order).Select(q => q.Author.Name).ToList())
                )
                .SetIgnoreReadOnly(false); // Not strictly required since we are passing false here, but we want to test the change

            return true;
        }
    }
}
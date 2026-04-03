// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using DataLayer.EfClasses;
using GenericServices.Configuration;
using Mapster;
using System;
using System.Linq;
using Tests.Dtos;

namespace Tests.Configs
{
    public class BookTitleAndCountConfig : PerDtoConfig<BookTitleAndCount, Book>
    {
        public override bool ConfigureReadMapping(out TypeAdapterSetter<Book, BookTitleAndCount> typeAdapterSetter)
        {
            typeAdapterSetter = TypeAdapterConfig<Book, BookTitleAndCount>
                .NewConfig()
                .Map(
                    dest => dest.ReviewsCount,
                    src => src.Reviews.Count()
                );

            /*
                .MapWith(book => new BookTitleAndCount()
                {
                    ReviewsCount = book.Reviews.Count()
                });
            */

            // .SetIgnoreReadOnly(false); - not required, unless we want to set this to true


            return true;
        }
    }
}
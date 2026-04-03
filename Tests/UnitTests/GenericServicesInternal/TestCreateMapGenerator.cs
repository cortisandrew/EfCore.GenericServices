// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using DataLayer.EfClasses;
using DataLayer.EfCode;
using GenericServices.Internal.Decoders;
using GenericServices.Setup.Internal;
using Mapster;
using System.Linq;
using Tests.Configs;
using Tests.Dtos;
using Tests.Helpers;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Tests.UnitTests.GenericServicesInternal
{
    public class TestCreateMapGenerator
    {
        private readonly DecodedEntityClass _authorInfo;
        private readonly DecodedEntityClass _bookInfo;

        public TestCreateMapGenerator()
        {
            var options = SqliteInMemory.CreateOptions<EfCoreContext>();
            using (var context = new EfCoreContext(options))
            {
                _bookInfo = new DecodedEntityClass(typeof(Book), context);
                _authorInfo = new DecodedEntityClass(typeof(Author), context);
            }
        }

        [Fact]
        public void TestAuthorReadMappings()
        {
            //SETUP
            // var maps = new MapperConfigurationExpression();
            MappingProfile mappingProfile = new MappingProfile(false);

            //ATTEMPT
            var mapCreator = new CreateConfigGenerator(typeof(AuthorNameDto), _authorInfo, null);
            mapCreator.Accessor.ConfigureReadMapping(mappingProfile);

            //mapCreator.Accessor.AddReadMappingToProfile(maps);
            //var config = new MapperConfiguration(maps, NullLoggerFactory.Instance);

            //VERIFY
            var entity = new Author { AuthorId = 1, Name = "Author", Email = "me@nospam.com" };
            var dto = entity.Adapt<AuthorNameDto>(); // config.CreateMapper().Map<AuthorNameDto>(entity);
            dto.Name.ShouldEqual("Author");
        }

        [Fact]
        public void TestBookReadMappingsWithConfig()
        {
            //SETUP
            // var maps = new MapperConfigurationExpression();
            MappingProfile mappingProfile = new MappingProfile(false);


            //ATTEMPT
            var mapCreator = new CreateConfigGenerator(typeof(BookTitleAndCount), _bookInfo, new BookTitleAndCountConfig());
            mapCreator.Accessor.ConfigureReadMapping(mappingProfile);

            //VERIFY
            // var config = new MapperConfiguration(maps, NullLoggerFactory.Instance);
            var entity = DddEfTestData.CreateFourBooks().Last();
            var dto = entity.Adapt<BookTitleAndCount>(); // config.CreateMapper().Map<BookTitleAndCount>(entity);
            dto.Title.ShouldEqual("Quantum Networking");
            dto.ReviewsCount.ShouldEqual(2);
        }
    }
}
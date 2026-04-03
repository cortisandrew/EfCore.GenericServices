// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using DataLayer.EfClasses;
using GenericServices.Helpers.GenericServices.Helpers;
using Mapster;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Tests.Configs;
using Tests.Dtos;
using Tests.Helpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static Tests.UnitTests.TestIssues.TestIssue60;

namespace Tests.UnitTests.Libraries
{
    public class TestAutoMapper
    {
        [Fact]
        public void TestDirectMappingBookTitle()
        {
            //SETUP
            var wrappedMapper = MapsterTestHelpers.CreateWrapperMapper<Book, BookTitle>();

            //ATTEMPT
            var input = DddEfTestData.CreateFourBooks().First();

            var data = input.Adapt<BookTitle>(); //.Adapt(data); // wrappedMapper. dto.Adapt(entity);

            // var data = wrappedMapper.MapsterSaveConfig.MapperSaveConfig.CreateMapper().Map<BookTitle>(input);

            //VERIFY
            data.Title.ShouldEqual("Refactoring");
        }

        [Fact]
        public void TestProjectionMappingBookTitle()
        {
            //SETUP
            var wrappedMapper = MapsterTestHelpers.CreateWrapperMapper<BookTitle, Book>();

            //ATTEMPT
            var input = DddEfTestData.CreateFourBooks().AsQueryable();

            if (wrappedMapper != null && wrappedMapper.MapsterReadConfig != null)
            {
                TypeAdapterConfig<Book, BookTitle>.NewConfig().SetIgnoreReadOnly(wrappedMapper.MapsterReadConfig.IgnoreReadOnlyAttributes);
            }

            var list = input.ProjectToType<BookTitle>().ToList();

            //VERIFY
            list.First().Title.ShouldEqual("Refactoring");
        }

        [Fact]
        public void TestDirectMappingBookTitleAndCount()
        {
            //SETUP
            var genSerConfig = new BookTitleAndCountConfig();
            var mapperConfig = MapsterTestHelpers.CreateReadConfig(genSerConfig);

            //ATTEMPT
            var input = DddEfTestData.CreateFourBooks().Last();
            var data = input.Adapt<BookTitleAndCount>(); // mapperConfig.CreateMapper().Map<BookTitleAndCount>(input);

            //VERIFY
            data.Title.ShouldEqual("Quantum Networking");
            data.ReviewsCount.ShouldEqual(2);
        }

        [Fact]
        public void TestProjectionMappingBookTitleAndCount()
        {
            //SETUP
            var genSerConfig = new BookTitleAndCountConfig();
            var mapperConfig = MapsterTestHelpers.CreateReadConfig(genSerConfig);

            //ATTEMPT
            var input = DddEfTestData.CreateFourBooks().AsQueryable();
            // var list = input.ProjectToType<BookTitleAndCount>(mapperConfig).ToList();
            var list = input.ProjectToType<BookTitleAndCount>(mapperConfig.Config).ToList();

            //VERIFY
            list.Last().Title.ShouldEqual("Quantum Networking");
            list.Last().ReviewsCount.ShouldEqual(2);
        }

        /// <summary>
        /// The test behaves differently now: on formatting issue, you expect a System.FormatException to be thrown
        /// </summary>
        [Fact]
        public void TestProjectionMappingBookTitleBadType()
        {
            //SETUP
            var wrappedMapper = MapsterTestHelpers.CreateWrapperMapper<Book, BookTitleBadType>();

            //ATTEMPT

            // No Direct equivalent
            // wrappedMapper.MapperSaveConfig.AssertConfigurationIsValid();

            // wrappedMapper..Create.MapsterSaveConfig.Compile(true);
            var input = DddEfTestData.CreateFourBooks().First();

            Assert.Throws<FormatException>(() =>
                {
                    BookTitleBadType dto = input.Adapt<BookTitleBadType>();
                });

            //VERIFY
            //Doesn't error on name fit but different 

            //Test now fails and throws System.FormatException
        }

        [Fact]
        public void TestDirectMappingToBookNotSetPrivateSetter()
        {
            //SETUP
            var wrappedMapper = MapsterTestHelpers.CreateWrapperMapper<BookTitle, Book>();
            var entity = DddEfTestData.CreateFourBooks().First();

            //ATTEMPT
            var dto = new BookTitle { Title = "New Title" };

            var data = dto.Adapt(entity);

            // var data = wrappedMapper.MapperSaveConfig.CreateMapper().Map(dto, entity);

            //VERIFY
            data.Title.ShouldEqual("Refactoring");
            entity.Title.ShouldEqual("Refactoring"); // just checking that the entity is also populated
        }

        private bool Filter(MemberInfo member)
        {
            var readOnlyAttr = member.GetCustomAttribute<ReadOnlyAttribute>();
            var isReadOnly = readOnlyAttr?.IsReadOnly ?? false;
            return isReadOnly;
        }

        [Fact]
        public void TestIgnoreReadOnlyProperties()
        {
            //SETUP
            var entity = new Author { AuthorId = 1, Name = "Start Name", Email = "me@nospam.com" };

            TypeAdapterConfig<WriteAuthorReadOnlyDto, Author>
                .NewConfig()
                .SetIgnoreReadOnly(true);

            /*
            var config = new MapperConfiguration(cfg =>
            {
                //see https://github.com/AutoMapper/AutoMapper/issues/2571#issuecomment-374159340
                cfg.Internal().ForAllPropertyMaps(pm => Filter(pm.SourceMember), (pm, opt) => opt.Ignore());
                cfg.CreateMap<WriteAuthorReadOnlyDto, Author>();
            }, NullLoggerFactory.Instance);
            */

            //ATTEMPT
            var dto = new WriteAuthorReadOnlyDto { AuthorId = 123, Name = "New Name", Email = "youhavechanged@gmail.com" };
            // var mapper = config.CreateMapper();
            var data = dto.Adapt(entity); // mapper.Map(dto, entity);

            //VERIFY
            data.Name.ShouldEqual("New Name");       //changed
            data.AuthorId.ShouldEqual(123);          //changed
            data.Email.ShouldEqual("me@nospam.com"); //not changed - ReadOnly
        }

        [Fact]
        public void TestUsingProfileBasic()
        {
            //SETUP
            var entity = new Author { AuthorId = 1, Name = "Start Name", Email = "me@nospam.com" };
            var profile = new UnitTestProfile(false);

            TypeAdapterConfig<WriteAuthorReadOnlyDto, Author>
                .NewConfig();

            // profile.AddWriteMap<WriteAuthorReadOnlyDto, Author>();
            // var config = new MapperConfiguration(cfg => cfg.AddProfile(profile), NullLoggerFactory.Instance);

            //ATTEMPT
            var dto = new WriteAuthorReadOnlyDto { AuthorId = 123, Name = "New Name", Email = "youhavechanged@gmail.com" };
            // var mapper = config.CreateMapper();
            var data = dto.Adapt<Author>(); // mapper.Map(dto, entity);

            //VERIFY
            data.Name.ShouldEqual("New Name");
            data.AuthorId.ShouldEqual(123);
            data.Email.ShouldEqual("youhavechanged@gmail.com");
        }

        [Fact]
        public void TestUsingProfileWithIgnore()
        {
            //SETUP
            var entity = new Author { AuthorId = 1, Name = "Start Name", Email = "me@nospam.com" };
            var profile = new UnitTestProfile(true);
            profile.AddWriteMap<WriteAuthorReadOnlyDto, Author>();


            // Profile should already have setup what we need...
            // var config = new MapperConfiguration(cfg => cfg.AddProfile(profile), NullLoggerFactory.Instance);

            //ATTEMPT
            var dto = new WriteAuthorReadOnlyDto { AuthorId = 123, Name = "New Name", Email = "youhavechanged@gmail.com" };
            // var mapper = config.CreateMapper();
            var data = dto.Adapt(entity); //  mapper.Map(dto, entity);

            //VERIFY
            data.Name.ShouldEqual("New Name");       //changed
            data.AuthorId.ShouldEqual(123);          //changed
            data.Email.ShouldEqual("me@nospam.com"); //not changed - ReadOnly
        }


        /// <summary>
        /// This test will permanently fail because the last profile will override the older profile and therefore they are not independent
        /// </summary>
        [Fact]
        public void TestUsingProfileWithIgnoreDoesntAffectOtherProfiles()
        {
            //SETUP
            var profile1 = new UnitTestProfile(true);
            profile1.AddWriteMap<WriteAuthorReadOnlyDto, Author>();
            //var config1 = new MapperConfiguration(cfg => cfg.AddProfile(profile1), NullLoggerFactory.Instance);

            var profile2 = new UnitTestProfile(false);
            profile2.AddWriteMap<WriteAuthorReadOnlyDto, Author>();
            //var config2 = new MapperConfiguration(cfg => cfg.AddProfile(profile2), NullLoggerFactory.Instance);

            //ATTEMPT
            var dto = new WriteAuthorReadOnlyDto { AuthorId = 123, Name = "New Name", Email = "youhavechanged@gmail.com" };
            var entity1 = new Author { AuthorId = 1, Name = "Start Name", Email = "me@nospam.com" };

            dto.Adapt(entity1);
            // config1.CreateMapper().Map(dto, entity1);
            var entity2 = new Author { AuthorId = 1, Name = "Start Name", Email = "me@nospam.com" };
            dto.Adapt(entity2);
            // config2.CreateMapper().Map(dto, entity2);


            // CONFIRMED TEST FAILURE DUE TO DIFFERENCES IN Mapster compared to AutoMapper
            //VERIFY
            entity1.Email.ShouldEqual("me@nospam.com"); //not changed - ReadOnly
            entity2.Email.ShouldEqual("youhavechanged@gmail.com"); //changes
        }


        /// <summary>
        /// IMPORTANT WARNING! SUB-CLASSES INHERIT PROFILE SETTINGS SUCH AS READ-ONLY!
        /// </summary>
        [Fact]
        public void TestUsingProfileWithIgnoreForRelatedClasses()
        {
            //SETUP
            var profile1 = new UnitTestProfile(false);
            profile1.AddWriteMap<WriteAuthorReadOnlyDto, Author>();
            
            var profile2 = new UnitTestProfile(true);
            profile2.AddWriteMap<WriteAuthorReadOnlyTwoDto, Author>();
            
            //ATTEMPT
            var dto = new WriteAuthorReadOnlyDto { AuthorId = 123, Name = "New Name", Email = "youhavechanged@gmail.com" };
            var entity1 = new Author { AuthorId = 1, Name = "Start Name", Email = "me@nospam.com" };
            dto.Adapt(entity1);
            
            var dto2 = new WriteAuthorReadOnlyTwoDto { AuthorId = 123, Name = "New Name", Email = "youhavechanged@gmail.com" };
            var entity2 = new Author { AuthorId = 1, Name = "Start Name", Email = "me@nospam.com" };
            dto2.Adapt(entity2);
            
            //VERIFY
            entity2.Email.ShouldEqual("me@nospam.com"); //not changed - ReadOnly
            entity1.Email.ShouldEqual("youhavechanged@gmail.com"); //changes
        }

        [Fact]
        public void TestUsingProfileWithIgnoreForRelatedClassesTwo()
        {
            //SETUP
            var profile1 = new UnitTestProfile(true);
            profile1.AddWriteMap<WriteAuthorReadOnlyDto, Author>();

            var profile2 = new UnitTestProfile(false);
            profile2.AddWriteMap<WriteAuthorReadOnlyTwoDto, Author>();

            //ATTEMPT
            var dto = new WriteAuthorReadOnlyDto { AuthorId = 123, Name = "New Name", Email = "youhavechanged@gmail.com" };
            var entity1 = new Author { AuthorId = 1, Name = "Start Name", Email = "me@nospam.com" };
            dto.Adapt(entity1);

            var dto2 = new WriteAuthorReadOnlyTwoDto { AuthorId = 123, Name = "New Name", Email = "youhavechanged@gmail.com" };
            var entity2 = new Author { AuthorId = 1, Name = "Start Name", Email = "me@nospam.com" };
            dto2.Adapt(entity2);

            //VERIFY
            entity1.Email.ShouldEqual("me@nospam.com"); //not changed - ReadOnly
            entity2.Email.ShouldEqual("youhavechanged@gmail.com"); //changes
        }
    }
}
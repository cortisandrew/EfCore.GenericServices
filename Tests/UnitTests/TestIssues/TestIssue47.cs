// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using GenericServices;
using GenericServices.Configuration;
using GenericServices.PublicButHidden;
using GenericServices.Setup;
using Mapster;
using System;
using System.Linq;
using Tests.EfClasses;
using Tests.EfCode;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Tests.UnitTests.TestIssues
{
    public class TestIssue47
    {
        [Fact]
        public void TestDtoWithNoPropertiesForLinkedEntity()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<TestDbContext>();
            using (var context = new TestDbContext(options))
            {
                context.Database.EnsureCreated();
                context.Add(new ParentOneToOne {OneToOne = new Child {MyString = "Test"}});
                context.SaveChanges();

                var utData = context.SetupSingleDtoAndEntities<Issue47Dto>();
                var service = new CrudServices(context, utData.ConfigAndMapper);

                //ATTEMPT
                var list = service.ReadManyNoTracked<Issue47Dto>().ToList();

                //VERIFY
                list.Count.ShouldEqual(1);
                list[0].OneToOneMyString.ShouldEqual("Test");
            }
        }

        public class Issue47Dto : ILinkToEntity<ParentOneToOne>
        {
            //public int OneToOneChildId { get; set; }
            public string OneToOneMyString { get; set; }
        }

        public class Issue47DtoConfig : PerDtoConfig<Issue47Dto, ParentOneToOne>
        {
            public override bool ConfigureReadMapping(out TypeAdapterSetter<ParentOneToOne, Issue47Dto> typeAdapterSetter)
            {
                typeAdapterSetter = TypeAdapterConfig<ParentOneToOne, Issue47Dto>
                .NewConfig()
                .Map(
                    dest => dest.OneToOneMyString,
                    src => src.OneToOne.MyString
                );

                return true;
            }
        }
    }
}
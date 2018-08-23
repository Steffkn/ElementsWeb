namespace Elements.Tests.Mocks
{
    using AutoMapper;
    using Elements.Web.Mapping;

    public static class MockAutoMapper
    {
        static MockAutoMapper()
        {
            Mapper.Initialize(configuration => configuration.AddProfile<AutoMapperProfile>());
        }

        public static IMapper Get()
        {
            return Mapper.Instance;
        }
    }
}

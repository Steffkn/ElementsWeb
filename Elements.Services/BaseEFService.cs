namespace Elements.Services
{
    using AutoMapper;
    using Elements.Data;

    public abstract class BaseEFService
    {
        protected BaseEFService(ElementsContext context,
            IMapper mapper)
        {
            this.Context = context;
            this.Mapper = mapper;
        }

        protected ElementsContext Context { get; private set; }
        protected IMapper Mapper { get; private set; }
    }
}

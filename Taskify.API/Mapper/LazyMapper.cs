using AutoMapper;
using Taskify.API.Mapper.MappingProfiles;

namespace Taskify.API.Mapper
{
    public static class LazyMapper
    {
        private readonly static Lazy<IMapper> _lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod?.IsPublic == true || p.GetMethod?.IsAssembly == true;

                cfg.AddProfile<UserMappingProfile>();
                cfg.AddProfile<ProjectMappingProfile>();
                cfg.AddProfile<UserProjectMappingProfiles>();
                cfg.AddProfile<TaskMappingProfiles>();
            });

            return config.CreateMapper();
        });

        public static IMapper Mapper => _lazy.Value;
    }
}

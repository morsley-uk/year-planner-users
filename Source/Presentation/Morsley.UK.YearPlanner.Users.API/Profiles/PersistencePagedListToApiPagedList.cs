using AutoMapper;
using Morsley.UK.YearPlanner.Users.API.Models.v1;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;

namespace Morsley.UK.YearPlanner.Users.API.Profiles
{
    public class PersistencePagedListToApiPagedList : Profile
    {
        public PersistencePagedListToApiPagedList()
        {
            CreateMap<Domain.Models.User, API.Models.v1.Response.UserResponse>();

            CreateMap<IPagedList<Domain.Models.User>, IPagedList<API.Models.v1.Response.UserResponse>>()
                .ConvertUsing<PagedListTypeConverter>();

        }
    }

    public class PagedListTypeConverter : ITypeConverter<IPagedList<Domain.Models.User>,
                                                         IPagedList<API.Models.v1.Response.UserResponse>>
    {
        public IPagedList<API.Models.v1.Response.UserResponse> Convert(
            IPagedList<Domain.Models.User> source,
            IPagedList<API.Models.v1.Response.UserResponse> destination,
            ResolutionContext context)
        {
            var conversion = new PagedList<API.Models.v1.Response.UserResponse>();

            foreach (var user in source)
            {
                var response = context.Mapper.Map<API.Models.v1.Response.UserResponse>(user);
                conversion.Add(response);
            }
            
            conversion.CurrentPage = source.CurrentPage;
            conversion.TotalPages = source.TotalPages;
            conversion.PageSize = source.PageSize;
            conversion.TotalCount = source.TotalCount;

            return conversion;
        }
    }
}
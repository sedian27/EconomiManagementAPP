using AutoMapper;
using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Accounts, AccountsViewModel>();
            CreateMap<Categories, CategoriesViewModel>();
        }
    }
}

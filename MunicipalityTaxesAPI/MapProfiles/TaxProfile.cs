using AutoMapper;
using MunicipalityTaxesAPI.Entities;
using MunicipalityTaxesAPI.Models.Requests;
using MunicipalityTaxesAPI.Models.Responses;

namespace MunicipalityTaxesAPI.MapProfiles
{
    public class TaxProfile : Profile
    {
        public TaxProfile()
        {
            CreateMap<TaxCreateRequest, TaxEntity>();
            CreateMap<TaxEntity, TaxResponse>();
            CreateMap<TaxEntity, TaxByMunicipalityandDateResponse>();
            CreateMap<TaxUpdateRequest, TaxEntity>()
                .ForMember(x => x.Municipality, opt =>
                {
                    opt.Condition(e => e.Municipality != null && e.Municipality != "");
                })
                .ForMember(x => x.Type, opt =>
                {
                    opt.Condition(e => e.Type != null);
                })
                .ForMember(x => x.TaxRate, opt =>
                {
                    opt.Condition(e => e.TaxRate != null && e.TaxRate != 0);
                })
                .ForMember(x => x.TaxSchedule, opt =>
                {
                    opt.Condition(e => e.TaxSchedule != null);
                });
        }
    }
}
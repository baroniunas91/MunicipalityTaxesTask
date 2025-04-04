using AutoMapper;
using MunicipalityTaxesAPI.Entities;
using MunicipalityTaxesAPI.Models.Requests;
using MunicipalityTaxesAPI.Models.Responses;

namespace MunicipalityTaxesAPI.MapProfiles
{
    public class TaxScheduleProfile : Profile
    {
        public TaxScheduleProfile()
        {
            CreateMap<TaxScheduleCreateRequest, TaxScheduleEntity>();
            CreateMap<TaxScheduleEntity, TaxScheduleResponse>();
        }
    }
}

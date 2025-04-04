﻿using AutoMapper;
using MunicipalityTaxesAPI.Entities;
using MunicipalityTaxesAPI.Extensions;
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
        }
    }
}

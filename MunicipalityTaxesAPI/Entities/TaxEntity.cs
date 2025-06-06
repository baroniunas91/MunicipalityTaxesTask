﻿using MunicipalityTaxesAPI.Enums;
using MunicipalityTaxesAPI.Interfaces;

namespace MunicipalityTaxesAPI.Entities
{
    public class TaxEntity : IEntity
    {
        public int Id { get; set; }
        public string Municipality { get; set; }
        public TaxType Type { get; set; }
        public double TaxRate { get; set; }
        public virtual TaxScheduleEntity TaxSchedule { get; set; }
    }
}

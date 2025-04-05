using Microsoft.EntityFrameworkCore;
using MunicipalityTaxesAPI.Entities;
using MunicipalityTaxesAPI.Interfaces;
using MunicipalityTaxesAPI.Models.Requests;

namespace MunicipalityTaxesAPI.Filters
{
    public class TaxEntityFilter : IEntityFilter<TaxEntity>
    {
        public IQueryable<TaxEntity> ApplyFilter(IQueryable<TaxEntity> queryable, IBaseGetRequest request)
        {
            if (request is not TaxGetRequest taxRequest)
            {
                return queryable;
            }

            if (taxRequest.Municipality != null && taxRequest.Municipality != string.Empty)
            {
                queryable = queryable.Where(x => taxRequest.Municipality.Contains(x.Municipality));
            }

            if (taxRequest.Date is not null)
            {
                queryable = queryable.Where(x => x.TaxSchedule.PeriodStart <= taxRequest.Date && x.TaxSchedule.PeriodEnd > taxRequest.Date);
            }

            return queryable;
        }

        public IQueryable<TaxEntity> ApplyRelations(IQueryable<TaxEntity> queryable)
        {
            return queryable.Include(x => x.TaxSchedule);
        }

        public IQueryable<TaxEntity> ApplyOrdering(IQueryable<TaxEntity> queryable)
        {
            return queryable.OrderBy(x => x.Type);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Domain.Aggregates.Orders.Services
{
    public interface IGoodsTotalPriceValidator
    {
        Task<bool> IsTotalPriceValid(IEnumerable<Guid> goodIds);
    }
}
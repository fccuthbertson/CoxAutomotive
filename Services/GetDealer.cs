using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoxAutomotive.Models.Domain;

namespace CoxAutomotive.Services
{
    public class GetDealer : IGetDealer
    {
        public Task<Dealer> Get(DataSetId @in)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoxAutomotive.Models.Domain;

namespace CoxAutomotive.Services
{
    public class GetVehicle : IGetVehicle
    {
        public Task<Vehicle> Get(DataSetId @in)
        {
            throw new NotImplementedException();
        }
    }
}

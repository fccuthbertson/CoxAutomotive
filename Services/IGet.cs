using CoxAutomotive.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoxAutomotive.Services
{
    public interface IGet<T>
    {
        Task<T> Get();
    }
    public interface IGet<TIn, TOut>
    {
        Task<TOut> Get(TIn @in);
    }

    public interface IGetDataSet : IGet<DataSetId>
    {

    }

    public interface IGetDealer : IGet<DataSetId, Dealer>
    {

    }

    public interface IGetVehicle : IGet<DataSetId,Vehicle>
    {

    }
}

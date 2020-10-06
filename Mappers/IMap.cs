using CoxAutomotive.Models.Domain;
using CoxAutomotive.Models.Response;

namespace CoxAutomotive.Mappers
{
    public interface IMap<TIn, TOut>
    {
        TOut Map(TIn @in);
    }

    public interface IDataSetMapper : IMap<DataSetResponse, DataSetId> { }
    public class DataSetMapper : IDataSetMapper
    {
        public DataSetId Map(DataSetResponse @in)
        {
            var model = new DataSetId
            {
                Value = @in.DataSetId
            };
            return model;
        }
    }

    public interface IDealerMapper : IMap<DealerResponse, Dealer> { }
    public class DealerMapper : IDealerMapper
    {
        public Dealer Map(DealerResponse @in)
        {
            throw new System.NotImplementedException();
        }
    }
}

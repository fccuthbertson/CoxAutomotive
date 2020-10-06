namespace CoxAutomotive.Models.Domain
{
    public abstract class Id<T>
    {
        public T Value { get; set; }
    }

    public class DataSetId : Id<string>
    {        
    }

    public class DealerId : Id<int>
    {

    }

    public class VehicleId : Id<int>
    {

    }
}

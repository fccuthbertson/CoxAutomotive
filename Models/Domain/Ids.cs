using System;

namespace CoxAutomotive.Models.Domain
{
    public abstract class Id<T>
    {
        protected Id(T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            Value = value;
        }
        public T Value { get; set; }
    }

    public class DataSetId : Id<string>
    {
        public DataSetId(string value) : base(value)
        {
        }
    }

    public class DealerId : Id<int>
    {
        public DealerId(int value) : base(value)
        {
        }
    }

    public class VehicleId : Id<int>
    {
        public VehicleId(int value) : base(value)
        {
        }
    }
}

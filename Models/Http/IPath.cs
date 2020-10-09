using CoxAutomotive.Models.Domain;
using System;

namespace CoxAutomotive.Models.Http
{
    public interface IPath
    {
        Type Type { get; }
        string Template { get; }
    }

    public class DataSetIdPath : IPath
    {
        public Type Type => typeof(DataSetId);

        public string Template => $"api/datasetId";
    }    

    public class DealerIdPath : IPath
    {
        public Type Type => typeof(DealerId);

        public string Template => "api/{0}/dealers/{1}";
    }    

    public class VehicleIdPath : IPath
    {
        public Type Type => typeof(VehicleId);

        public string Template => "api/{0}/vehicles/{1}";
    }

    public class VehiclesPath : IPath
    {
        public Type Type => typeof(Vehicle);

        public string Template => "api/{0}/vehicles";
    }

    public class CheatPath : IPath
    {
        public Type Type => typeof(DataSetIdCheat);

        public string Template => "api/{0}/cheat";
    }
}

﻿using CoxAutomotive.Models.Domain;
using System;
using System.Threading.Tasks;

namespace CoxAutomotive.Services
{
    public class GetInventory : IGetInvenory
    {
        private readonly IGetAutomotiveData _data;

        public GetInventory(IGetAutomotiveData getAutomotiveData)
        {
            _data = getAutomotiveData;
        }

        public async Task<Inventory> Get(DataSetId @in)
        {
            try
            {
                var inventory = await _data.GetDataSetDealersVehicle(@in);
                if (inventory is null) return null;
                return inventory;
            }
            catch (Exception)
            {
                //log 
                return null;
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketSelling.Services
{
    public class DistanceService : IDistanceService
    {
        public Task<CityDistance> GetDistance(string fromCity, string toCity)
        {
            return Task.FromResult(new CityDistance
            {
                FromCity = fromCity,
                ToCity = toCity,
                Distance = (double)Math.Abs(fromCity.Length - toCity.Length))
            };
        }
    }
}

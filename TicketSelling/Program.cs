using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TicketSelling.Services;

namespace TicketSelling
{
    public class Program
    {
        public async static void Main(string[] args)
        {
            IEmailService emailSerice = new EmailService();
            IDistanceService distanceService = new DistanceService();

            List<Event> events = new List<Event>()
            {
                new Event { Name = "Phantom of the Opera", City = "New York", Price = 10 },
                new Event{ Name = "Metallica", City = "Los Angeles", Price = 20 },
                new Event{ Name = "Metallica", City = "New York", Price = 12 },
                new Event{ Name = "Metallica", City = "Boston", Price = 15 },
                new Event{ Name = "LadyGaGa", City = "New York", Price = 3 },
                new Event{ Name = "LadyGaGa", City = "Boston", Price = 100 },
                new Event{ Name = "LadyGaGa", City = "Chicago", Price = 1 },
                new Event{ Name = "LadyGaGa", City = "San Francisco", Price = 22 },
                new Event{ Name = "LadyGaGa", City = "Washington", Price = 35 },
            };

            Customer customer = new Customer { Name = "John Smith", City = "New York" };

            #region Question 1
            List<Event> sameCityEvents = events.Where(e => e.City == customer.City).ToList();

            sameCityEvents.ForEach(e => emailSerice.AddToEmail(customer, e));
            Console.WriteLine($"Question 1 - Email sent to {customer.Name} for {sameCityEvents.Count} events");
            #endregion



            #region Question 2
            List<Event> closestCityEvents = events.Select(e => new
            {
                evt = e,
                distance = distanceService.GetDistance(customer.City, e.City)
            }).OrderBy(e => e.distance).Select(e => e.evt).Take(5).ToList();

            closestCityEvents.ForEach(e => emailSerice.AddToEmail(customer, e));
            Console.WriteLine($"Question 2 - Email sent to {customer.Name} for {string.Join(", ", closestCityEvents.Select(c => c.Name))} cities");
            #endregion



            #region Question 3 and 4
            List<string> distinctCities = events.Select(e => e.City).Distinct().ToList();
            Dictionary<string, double> cityDistance = new Dictionary<string, double>();
            List<Task<CityDistance>> getDistanceTasks = distinctCities.Select(c => distanceService.GetDistance(customer.City, c)).ToList();
            while (getDistanceTasks.Count > 0)
            {
                Task<CityDistance> finishedTask = await Task.WhenAny(getDistanceTasks);
                if (finishedTask.Result != null) cityDistance[finishedTask.Result.ToCity] = finishedTask.Result.Distance;
                getDistanceTasks.Remove(finishedTask);
            }

            List<string> closestCities = cityDistance.OrderBy(c => c.Value).Select(c => c.Key).Take(5).ToList();

            List<Event> closestCityEvents2 = events.Where(e => closestCities.Contains(e.City)).Take(5).ToList();

            closestCityEvents2.ForEach(e => emailSerice.AddToEmail(customer, e));
            Console.WriteLine($"Question 3,4 - API call improved by adding a try/catch to handle failing calls");
            #endregion


            #region Question 5
            List<Event> closestCityEvents3 = events.Select(e => new
            {
                evt = e,
                distancePrice = GetDistancePriceWeight(cityDistance[e.City], e.Price)
            }).OrderBy(e => e.distancePrice).Select(e => e.evt).Take(5).ToList();

            closestCityEvents3.ForEach(e => emailSerice.AddToEmail(customer, e));
            Console.WriteLine($"Question 5 - Email sent to {customer.Name} for {string.Join(", ", closestCityEvents3.Select(c => c.Name))} cities considering price and distance");
            #endregion


            Console.ReadLine();
        }


        private static decimal GetDistancePriceWeight(double distance, decimal price) => (decimal)(distance * (0.5)) + price * (decimal)(0.5);

    }
}

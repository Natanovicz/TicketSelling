namespace TicketSelling
{
    public class Event
    {
        public string Name { get; set; }
        public string City { get; set; }
        public decimal Price { get; set; }
    }

    public class CityDistance
    { 
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public double Distance { get; set; }
    }
}

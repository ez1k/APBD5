namespace WebApplication1.Models.DTOs
{
    public class TripAllData
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }
        public ICollection<TripAllDataCountry> Countries { get; set; }
        public ICollection<TripAllDataClient> Clients { get; set; }
    }
    public class TripAllDataClient
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
    public class TripAllDataCountry
    {
        public string Name { get; set; }
    }


}

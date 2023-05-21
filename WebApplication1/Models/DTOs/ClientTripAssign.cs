namespace WebApplication1.Models.DTOs
{
    public class ClientTripAssign
    {
        public string firstName { get; set; } = null!;

        public string lastName { get; set; } = null!;

        public string email { get; set; } = null!;

        public string telephone { get; set; } = null!;

        public string pesel { get; set; } = null!;

        public int tripID { get; set; }
        public string tripName { get; set; } = null!;
        public DateTime? PaymentDate { get; set; } 


    }
}

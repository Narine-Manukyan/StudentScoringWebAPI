namespace ScoringPortal.Core
{
    public class Account
    {
        public int ID { get; set; }
        public AccountType AccountType { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}

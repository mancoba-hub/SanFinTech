namespace Mbiza.Bank
{
    public class Accounts
    {
        public long Id { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime UpdatedDateTime { get; set; } = DateTime.MinValue;

        public string UpdatedBy { get; set; } = string.Empty;
    }
}

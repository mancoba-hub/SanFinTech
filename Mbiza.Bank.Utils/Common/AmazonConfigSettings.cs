namespace Mbiza.Bank
{
    public class AmazonConfigSettings
    {
        public bool Enabled { get; set; }

        public required string Region { get; set; }

        public required string TopicOwner { get; set; }

        public required string TopicName { get; set; }
    }
}

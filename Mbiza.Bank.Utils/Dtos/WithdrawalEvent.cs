namespace Mbiza.Bank
{
    public class WithdrawalEvent
    {
        #region Properties

        public decimal Amount { get; }

        public long AccountId { get; }

        public string Status { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WithdrawalEvent"/> class.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="accountId"></param>
        /// <param name="status"></param>
        public WithdrawalEvent(decimal amount, long accountId, string status)
        {
            Amount = amount;
            AccountId = accountId;
            Status = status;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Formats the class results to Json
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return $"{{\"amount\":\"{Amount}\",\"accountId\":{AccountId},\"status\":\"{Status}\"}}";
        }

        #endregion
    }
}

namespace Mbiza.Bank
{
    public interface IAccountService
    {
        Task<decimal> GetAccountBalanceAsync(long accountId);

        Task<string> WithdrawAsync(long accountId, decimal amount);
    }
}

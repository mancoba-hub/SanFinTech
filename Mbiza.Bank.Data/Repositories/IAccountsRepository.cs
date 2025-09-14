namespace Mbiza.Bank
{
    public interface IAccountsRepository
    {
        Task<Accounts> GetAccountByIdAsync(long accountId);

        Task<bool> WithdrawAsync(Accounts account);
    }
}

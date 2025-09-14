using Microsoft.Extensions.Logging;

namespace Mbiza.Bank
{
    public class AccountService : IAccountService
    {
        #region Properties

        private readonly IAccountsRepository _accountsRepository;
        private readonly ILogger<AccountService> _accountServiceLogger;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="_accountServiceLogger"></param>
        /// <param name="accountsRepository"></param>
        public AccountService(ILogger<AccountService> accountServiceLogger,
                              IAccountsRepository accountsRepository)
        {
            _accountServiceLogger = accountServiceLogger;
            _accountsRepository = accountsRepository;
        }

        #endregion

        #region Implemented Members

        /// <summary>
        /// Gets the account balance asynchronously
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<decimal> GetAccountBalanceAsync(long accountId)
        {
            try
            {
                Accounts account = await getAccountsAsync(accountId);
                if (account == default)
                    return 0;

                return account.Balance;
            }
            catch (Exception ex)
            {
                _accountServiceLogger.LogError(ex, "An error occurred while getting the account balance");
                throw;
            }
        }

        /// <summary>
        /// Withdraws from account asynchronously
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> WithdrawAsync(long accountId, decimal amount)
        {
            try
            {
                Accounts account = await getAccountsAsync(accountId);
                if (account == default)
                    return Constants.CONST_ACCOUNT_NOT_EXIST;

                account.UpdatedDateTime = DateTime.UtcNow;
                account.UpdatedBy = Constants.CONST_USER;
                account.Amount = amount;
                account.Balance -= amount;

                bool successWithdrawal = await _accountsRepository.WithdrawAsync(account);
                if (successWithdrawal)
                    return Constants.CONST_WITHDRAWAL_SUCCESS;
                else
                    return Constants.CONST_INSUFFICIENT_FUNDS;
            }
            catch (Exception ex)
            {
                _accountServiceLogger.LogError(ex, "An error occurred while withdrawing from the account");
                throw;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the account details asynchronously
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private async Task<Accounts> getAccountsAsync(long accountId)
        {
            return await _accountsRepository.GetAccountByIdAsync(accountId);
        }

        #endregion
    }
}

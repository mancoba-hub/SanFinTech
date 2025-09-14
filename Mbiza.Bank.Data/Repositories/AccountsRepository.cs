
namespace Mbiza.Bank
{
    public class AccountsRepository : BaseRepository<Accounts>, IAccountsRepository
    {
        #region Properties

        private readonly MbizaContext _mbizaContext;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mbizaContext"></param>
        public AccountsRepository(MbizaContext mbizaContext) : base(mbizaContext)
        {
            _mbizaContext = mbizaContext;
        }

        #endregion

        #region Implemented Members

        /// <summary>
        /// Withdraws from accounts
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Accounts> GetAccountByIdAsync(long accountId)
        {
            return await GetByQueryAsync(x => x.Id == accountId);
        }

        /// <summary>
        /// Withdraws from account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<bool> WithdrawAsync(Accounts account)
        {
            bool success = await UpdateAsync(account);
            if (success)
                await _mbizaContext.SaveChangesAsync();

            return success;
        }

        #endregion
    }
}

using Mbiza.Bank.Aws;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Mbiza.Bank.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml, MediaTypeNames.Text.Xml)]
    [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml, MediaTypeNames.Text.Xml)]
    public class BankAccountController : ControllerBase
    {
        #region Properties

        private readonly IAccountService _accountService;
        private readonly IAmazonPublishService _amazonPublishService;
        private readonly ILogger<BankAccountController> _bankAccountLogger;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BankAccountController"/> class.
        /// </summary>
        /// <param name="bankAccountLogger"></param>
        /// <param name="accountService"></param>
        /// <param name="amazonPublishService"></param>
        public BankAccountController(ILogger<BankAccountController> bankAccountLogger,
                                     IAccountService accountService,
                                     IAmazonPublishService amazonPublishService)
        {
            _bankAccountLogger = bankAccountLogger;
            _accountService = accountService;
            _amazonPublishService = amazonPublishService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Withdraws from account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [HttpPost("withdraw")]
        public async Task<ActionResult<string>> Withdraw([FromQuery] long accountId, [FromQuery] decimal amount)
        {
            _bankAccountLogger.LogInformation("Starting the withdrawal process...");
            try
            {
                _bankAccountLogger.LogInformation("Check current balance");
                decimal accountBalance = await _accountService.GetAccountBalanceAsync(accountId);
                if (accountBalance >= amount)
                {
                    _bankAccountLogger.LogInformation("Update balance");
                    string withdrawalResults = await _accountService.WithdrawAsync(accountId, amount);
                    if (withdrawalResults.Equals(Constants.CONST_WITHDRAWAL_SUCCESS))
                    {
                        _bankAccountLogger.LogInformation("After a successful withdrawal, publish a withdrawal event to SNS");
                        _ = await _amazonPublishService.PublishWithdrawalAsync(accountId, amount);
                    }
                    return new JsonResult(withdrawalResults);
                }
                else
                {
                    _bankAccountLogger.LogInformation(Constants.CONST_INSUFFICIENT_FUNDS);
                    return Constants.CONST_INSUFFICIENT_FUNDS;
                }
            }
            catch (Exception exc)
            {
                _bankAccountLogger.LogError(exc, "An error occurred while withdrawing");
                _bankAccountLogger.LogInformation("In case the update fails for reasons other than a balance check");
                return Constants.CONST_WITHDRAWAL_FAILURE;
                throw;
            }
        }

        #endregion
    }
}

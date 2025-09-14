using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mbiza.Bank.Aws
{
    public class AmazonPublishService : IAmazonPublishService
    {
        #region Properties

        private readonly IAmazonSimpleNotificationService _snsClient;
        private readonly IOptions<AmazonConfigSettings> _amazonSettings;
        private readonly ILogger<AmazonPublishService> _amazonPublishServiceLogger;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonPublishService"/> class.
        /// </summary>
        /// <param name="snsClient"></param>
        /// <param name="amazonSettings"></param>
        /// <param name="amazonPublishServiceLogger"></param>
        public AmazonPublishService(IAmazonSimpleNotificationService snsClient,
                                    IOptions<AmazonConfigSettings> amazonSettings,
                                    ILogger<AmazonPublishService> amazonPublishServiceLogger)
        {
            _snsClient = snsClient;
            _amazonSettings = amazonSettings;
            _amazonPublishServiceLogger = amazonPublishServiceLogger;
        }

        #endregion

        #region Implemented Members

        /// <summary>
        /// Publishes the withdrawal asynchronously
        /// </summary>
        /// <param name="withdrawalEvent"></param>
        /// <returns></returns>
        public async Task<bool> PublishWithdrawalAsync(long accountId, decimal amount)
        {
            try
            {
                WithdrawalEvent withdrawalEvent = new(amount, accountId, Constants.CONST_SUCCESS);
                string withdrawalEventJson = withdrawalEvent.ToJson();
                string snsTopicArn = $"arn:aws:sns:{_amazonSettings.Value.Region}:{_amazonSettings.Value.TopicOwner}:{_amazonSettings.Value.TopicName}";
                PublishRequest publishRequest = new PublishRequest
                {
                    Message = withdrawalEventJson,
                    TopicArn = snsTopicArn
                };
                PublishResponse publishResponse = await _snsClient.PublishAsync(publishRequest);
                if (publishResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                _amazonPublishServiceLogger.LogError(ex, "An error occurred while publishing the withdrawal");
                throw;
            }
        }

        #endregion
    }
}

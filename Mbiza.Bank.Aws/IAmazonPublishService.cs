namespace Mbiza.Bank.Aws
{
    public interface IAmazonPublishService
    {
        Task<bool> PublishWithdrawalAsync(long accountId, decimal amount);
    }
}

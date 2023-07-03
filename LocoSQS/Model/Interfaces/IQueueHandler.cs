using LocoSQS.Handler;
using LocoSQS.Model.ActionResults;
using LocoSQS.Model.Utils;

namespace LocoSQS.Model.Interfaces;

public interface IQueueHandler
{
    event Action<IQueue> OnNewQueue;

    ActionResult<CreateQueueResponse> CreateQueue(Dictionary<string, string> attributes, string queueName, Dictionary<string, string> tags);
    ActionResult<DeleteQueueResponse> DeleteQueue(string queueName);
    ActionResult<GetQueueUrlResponse> GetQueueUrl(string queueName, string? queueOwnerAWSAccountId);
    ActionResult<ListDeadLetterSourceQueuesResponse> ListDeadLetterSourceQueues(IQueue queue, int? maxResults, string? nextToken);
    ActionResult<ListQueuesResponse> ListQueues(int? maxResults, string? nextToken, string? queueNamePrefix);
    IQueue? GetQueue(string queueName);
    IQueue? GetQueueViaArn(string queueArn);
}
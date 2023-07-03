using LocoSQS.Exeptions;
using LocoSQS.Model.ActionResults;
using LocoSQS.Model.ActionResults.Properties;
using LocoSQS.Model.Utils;
using LocoSQS.Queue;

namespace LocoSQS.Model.Interfaces;

public interface IQueue
{
    long Id { get; }
    string Name { get; }
    string Arn => $"arn:aws:sqs:us-east-1:0:{Name}";
    string Url => $"{ServerConfiguration.BASEURL}/0/{Name}";
    public RedrivePolicy? RedrivePolicy { get; }
    event Action<IQueue, IMessage> OnNewRegisteredMessage;
    event Action<IQueue> OnUpdate;


    ActionResult<AddPermissionResponse> AddPermission(string label, AddPermissionData newPermissions);
    ActionResult<ChangeMessageVisibilityResponse> ChangeMessageVisibility(string receiptHandle, TimeSpan visibilityTimeout);
    ActionResult<ChangeMessageVisibilityBatchResponse> ChangeMessageVisibilityBatch(List<ChangeMessageVisibilityBatchRequestEntry> items)
    {
        if (items.Count > 10)
            throw new SQSException("The batch request contains more than 10 entries", 400, "AWS.SimpleQueueService.TooManyEntriesInBatchRequest");

        if (!items.All(x => items.Count(y => x.Id == y.Id) == 1))
            throw new SQSException("Two or more batch entries in the request have the same Id", 400, "AWS.SimpleQueueService.BatchEntryIdsNotDistinct");

        List<string> successful = new();
        List<BatchResultErrorEntry> errors = new();

        foreach (var x in items)
        {
            try
            {
                ChangeMessageVisibility(x.ReceiptHandle, TimeSpan.FromSeconds(x.VisibilityTimeout));
                successful.Add(x.Id);
            }
            catch (SQSException e)
            {
                errors.Add(new(x.Id, e.Code, e.Message, true));
            }
            catch (Exception e)
            {
                errors.Add(new(x.Id, "InternalFailure", e.Message, false));
            }
        }

        return new(200, new(successful, errors));
    }
    ActionResult<DeleteMessageResponse> DeleteMessage(string receiptHandle);
    ActionResult<DeleteMessageBatchResponse> DeleteMessageBatch(List<DeleteMessageBatchRequestEntry> items)
    {
        if (items.Count > 10)
            throw new SQSException("The batch request contains more than 10 entries", 400, "AWS.SimpleQueueService.TooManyEntriesInBatchRequest");

        if (!items.All(x => items.Count(y => x.Id == y.Id) == 1))
            throw new SQSException("Two or more batch entries in the request have the same Id", 400, "AWS.SimpleQueueService.BatchEntryIdsNotDistinct");

        List<string> successful = new();
        List<BatchResultErrorEntry> errors = new();

        foreach (var x in items)
        {
            try
            {
                DeleteMessage(x.ReceiptHandle);
                successful.Add(x.Id);
            }
            catch (SQSException e)
            {
                errors.Add(new(x.Id, e.Code, e.Message, true));
            }
            catch (Exception e)
            {
                errors.Add(new(x.Id, "InternalFailure", e.Message, false));
            }
        }

        return new(200, new(successful, errors));
    }

    ActionResult<GetQueueAttributesResponse> GetQueueAttributes(List<string> attributes);
    ActionResult<ListQueueTagsResponse> ListQueueTags();
    ActionResult<PurgeQueueResponse> PurgeQueue(bool ignoreTime);
    ActionResult<ReceiveMessageResponse> ReceiveMessage(List<string>? attributes, int? maxNumberOfMessages, List<string>? messageAttributeNames, string? receiveRequestAttemptId, int? visibilityTimeout, int? waitTimeSeconds, bool? silent);
    ActionResult<RemovePermissionResponse> RemovePermission(string label);
    ActionResult<SendMessageResponse> SendMessage(int? delaySeconds, List<MessageUserAttribute> attributes, string messageBody, string? messageDeduplicationId, string? messageGroupId);
    ActionResult<SendMessageBatchResponse> SendMessageBatch(List<SendMessageBatchRequestEntry> items)
    {
        if (items.Count > 10)
            throw new SQSException("The batch request contains more than 10 entries", 400, "AWS.SimpleQueueService.TooManyEntriesInBatchRequest");

        if (!items.All(x => items.Count(y => x.Id == y.Id) == 1))
            throw new SQSException("Two or more batch entries in the request have the same Id", 400, "AWS.SimpleQueueService.BatchEntryIdsNotDistinct");

        List<SendMessageBatchResultEntry> successful = new();
        List<BatchResultErrorEntry> errors = new();

        foreach (var x in items)
        {
            try
            {
                var result = SendMessage(x.DelaySeconds, x.Attributes.Select(x => x.ToMessageUserAttribute()).ToList(), x.MessageBody, x.MessageDeduplicationId, x.MessageGroupId);
                successful.Add(new(x.Id, result.Value.SendMessageResult.MessageId, result.Value.SendMessageResult.MD5OfBody, null)); // TODO: Implement MessageAttributes
            }
            catch (SQSException e)
            {
                errors.Add(new(x.Id, e.Code, e.Message, true));
            }
            catch (Exception e)
            {
                errors.Add(new(x.Id, "InternalFailure", e.Message, false));
            }
        }

        return new(200, new(successful, errors));
    }

    ActionResult<SetQueueAttributesResponse> SetQueueAttributes(Dictionary<string, string> attributes);
    ActionResult<TagQueueResponse> TagQueue(Dictionary<string, string> tags);
    ActionResult<UntagQueueResponse> UntagQueue(List<string> tags);

    long GetNumberOfMessagesDelayed();
    long GetNumberOfMessagesNotVisible();
    long GetNumberOfMessagesVisible();
    TimeSpan? GetAgeOfOldestMessage();
    void AddMessage(Message message);
}
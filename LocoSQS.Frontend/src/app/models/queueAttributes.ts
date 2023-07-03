export interface QueueAttributes {
    delaySeconds : number; // Range 0 to 900
    maximumMessageSize : number; // Range 1024 to 262144
    messageRetentionPeriod : number; // Range 60 to 1209600
    receiveMessageWaitTimeSeconds : number; // Range 0 to 20
    visibilityTimeout : number; // Range 0 to 43200
    deadLetterTargetArn : string|null;
    maxReceiveCount : number|null;
    queueArn : string;
}

export function ConstructQueueAttributesFromAttributes(attributes : {[key: string]: string}) : QueueAttributes {
    const delaySeconds = parseInt(attributes["DelaySeconds"])
    const maximumMessageSize = parseInt(attributes["MaximumMessageSize"])
    const messageRetentionPeriod = parseInt(attributes["MessageRetentionPeriod"])
    const receiveMessageWaitTimeSeconds = parseInt(attributes["ReceiveMessageWaitTimeSeconds"])
    const visibilityTimeout = parseInt(attributes["VisibilityTimeout"])
    const queueArn = attributes["QueueArn"]

    let redrivePolicy = null;

    if ("RedrivePolicy" in attributes){
        redrivePolicy = JSON.parse(attributes["RedrivePolicy"])
    }

    const response : QueueAttributes = {
        queueArn: queueArn,
        delaySeconds: delaySeconds,
        maximumMessageSize: maximumMessageSize,
        messageRetentionPeriod: messageRetentionPeriod,
        receiveMessageWaitTimeSeconds: receiveMessageWaitTimeSeconds,
        visibilityTimeout: visibilityTimeout,
        deadLetterTargetArn: redrivePolicy?.["deadLetterTargetArn"] ?? null,
        maxReceiveCount: redrivePolicy?.["maxReceiveCount"] ?? null,
    } 

    if (response.maxReceiveCount === null)
        response.maxReceiveCount = 10;

    console.log(attributes)
    console.log(response)
    return response;
}

export function DeconstructQueueAttributes(queueAttributes : QueueAttributes) : {[key: string]: string} {
    let redrivePolicy = null;
    queueAttributes.deadLetterTargetArn = queueAttributes.deadLetterTargetArn?.trim() ?? null;

    if (queueAttributes.deadLetterTargetArn === "")
        queueAttributes.deadLetterTargetArn = null;

    if (queueAttributes.deadLetterTargetArn !== null && queueAttributes.maxReceiveCount === null){
        queueAttributes.maxReceiveCount = 10;
    }

    if (queueAttributes.deadLetterTargetArn !== null){
        redrivePolicy = JSON.stringify({
            "deadLetterTargetArn": queueAttributes.deadLetterTargetArn,
            "maxReceiveCount": queueAttributes.maxReceiveCount,
        });
    }

    const items : {[key: string]: string} = {
        "DelaySeconds": queueAttributes.delaySeconds.toString(),
        "MaximumMessageSize": queueAttributes.maximumMessageSize.toString(),
        "MessageRetentionPeriod": queueAttributes.messageRetentionPeriod.toString(),
        "ReceiveMessageWaitTimeSeconds": queueAttributes.receiveMessageWaitTimeSeconds.toString(),
        "VisibilityTimeout": queueAttributes.visibilityTimeout.toString()
    }

    if (redrivePolicy !== null)
        items["RedrivePolicy"] = redrivePolicy

    console.log(items)
    return items;
}
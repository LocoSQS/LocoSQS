export interface Queue {
    name : string;
    url : string;
    arn : string
}

export function createQueue(queueUrl : string) : Queue {
    let split = queueUrl.split("/");
    return {
        url: queueUrl,
        name: split[split.length - 1],
        arn: `arn:aws:sqs:us-east-1:0:${split[split.length - 1]}`
    }
}
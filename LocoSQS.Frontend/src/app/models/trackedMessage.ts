import { IncomingMessage } from "./incomingMessage";

export interface TrackedMessageHistoryEntry {
    at: Date;
    event: "OnMessageDeleted" | "OnMessageInvisible" | "OnMessageReady"
}

export interface TrackedMessage {
    id : string;
    body : string;
    firstReceived? : Date;
    receiveCount : number;
    receiptHandle : string;
    sentTimestamp : Date;
    history: TrackedMessageHistoryEntry[];
}

export function newTrackedMessage(data : IncomingMessage) {
    let receiveStr = ('SentTimestamp' in data.Attributes) ? data.Attributes["SentTimestamp"] : null;

    if (receiveStr === null)
        throw new Error("Attribute SentTimestamp not found")

    const message : TrackedMessage = {
        id: data.MessageId,
        body: data.Body,
        receiveCount: 0,
        receiptHandle: data.ReceiptHandle,
        sentTimestamp: new Date(parseInt(receiveStr) * 1000),
        history: []
    }

    return message;
}

export function updateTrackedMessage(message : TrackedMessage, data : IncomingMessage){
    let receieveCount = ("ApproximateReceiveCount" in data.Attributes) ? data.Attributes["ApproximateReceiveCount"] : null;
    let firstReceived = ("ApproximateFirstReceiveTimestamp" in data.Attributes) ? data.Attributes["ApproximateFirstReceiveTimestamp"] : null;

    if (receieveCount !== null)
        message.receiveCount = parseInt(receieveCount)

    if (firstReceived !== null && message.firstReceived === undefined)
        message.firstReceived = new Date(parseInt(firstReceived) * 1000)

    message.receiptHandle = data.ReceiptHandle;
}
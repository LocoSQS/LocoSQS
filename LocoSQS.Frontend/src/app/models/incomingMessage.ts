export interface IncomingMessage {
    Attributes : { [key: string] : string }
    Body : string,
    MessageAttributes: {
        [key: string] : {
            BinaryValue? : string
            StringValue? : string
            DataType : string
        }
    }
    MessageId : string
    ReceiptHandle : string
}
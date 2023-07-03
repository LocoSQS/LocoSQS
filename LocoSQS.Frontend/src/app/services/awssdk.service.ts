import { Injectable } from "@angular/core";
import { SQSClient, ListQueuesCommand, PurgeQueueCommand, CreateQueueCommand, DeleteQueueCommand, DeleteMessageCommand, SendMessageCommand, GetQueueAttributesCommand, SetQueueAttributesCommand } from "@aws-sdk/client-sqs";
import { Queue, createQueue } from "../models/queue";
import { Observable, Observer, defer } from "rxjs";
import { ConstructQueueAttributesFromAttributes, DeconstructQueueAttributes, QueueAttributes } from "../models/queueAttributes";
import { isDevMode } from '@angular/core';

@Injectable({
    providedIn: 'root'
  })
export class AwsSdkService {
    public client : SQSClient;
    private endpoint;

    public constructor(){
        this.endpoint = (isDevMode() ? "http://localhost:8080" : window.location.origin)
        this.client = new SQSClient({
            endpoint: this.endpoint,
            credentials: {
                accessKeyId: "x",
                secretAccessKey: "x"
            },
            region: "us-east-2"
        })
    }

    public async getQueues() : Promise<Queue[]> {
        const request  = new ListQueuesCommand({});
        const response = await this.client.send(request);
        const queues : Queue[] = []

        response.QueueUrls?.forEach(element => {
            queues.push(createQueue(element))
        });

        return queues;
    }

    public GetQueue$() : Observable<Queue[]> {
        return defer(() => this.getQueues())
    }

    public async purgeQueue(queue : Queue, ignoreTimer = false) : Promise<void> {
        if (!ignoreTimer){
            const request = new PurgeQueueCommand({
                QueueUrl: queue.url
            });

            await this.client.send(request);
        }
        else {
            // LocoSQS only!
            await fetch(this.endpoint, {
                method: "POST",
                body: JSON.stringify({
                    QueueUrl: queue.url,
                    IgnoreTime: 1
                }),
                headers: {
                    "X-Amz-Target": "AmazonSQS.PurgeQueue",
                    "Content-Type": "application/x-amz-json-1.0"
                }
            });
        }
    }

    public async deleteQueue(queue : Queue) {
        const request = new DeleteQueueCommand({
            QueueUrl: queue.url
        });
    
        await this.client.send(request);
    }

    public async createQueue(queueName : string){
        queueName = queueName.trim();
        if (queueName.length <= 0)
            throw new Error("Queue name is empty!");


        const request = new CreateQueueCommand({
            QueueName: queueName
        });

        await this.client.send(request);
    }

    public async deleteMessage(queue : Queue, receiptHandle : string){
        const request = new DeleteMessageCommand({
            ReceiptHandle: receiptHandle,
            QueueUrl: queue.url
        });

        await this.client.send(request);
    }

    public async sendMessage(queue : Queue, body : string){
        const request = new SendMessageCommand({
            QueueUrl: queue.url,
            MessageBody: body
        })

        await this.client.send(request)
    }

    public async GetQueueAttributes(queue : Queue){
        const request = new GetQueueAttributesCommand({
            QueueUrl: queue.url,
            AttributeNames: ["All"]
        });

        const response = await this.client.send(request);

        return ConstructQueueAttributesFromAttributes(response.Attributes!);
    }

    public GetQueueAttribute$(queue : Queue){
        return defer(() => this.GetQueueAttributes(queue))
    }

    public async SetQueueAttributes(queue : Queue, queueAttributes : QueueAttributes){
        const request = new SetQueueAttributesCommand({
            QueueUrl: queue.url,
            Attributes: DeconstructQueueAttributes(queueAttributes)
        });

        await this.client.send(request);
    }
}
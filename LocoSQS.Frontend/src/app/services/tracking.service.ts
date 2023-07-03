import { Injectable } from "@angular/core";
import { SQSClient, ListQueuesCommand, PurgeQueueCommand, CreateQueueCommand, DeleteQueueCommand } from "@aws-sdk/client-sqs";
import { Queue, createQueue } from "../models/queue";
import { BehaviorSubject, Observable, Observer, defer } from "rxjs";
import { webSocket, WebSocketSubject } from "rxjs/webSocket";
import { AwsSdkService } from "./awssdk.service";
import { QueueService } from "./queue.service";
import { TrackedMessage, newTrackedMessage, updateTrackedMessage } from "../models/trackedMessage";
import { ObserverLog } from "../models/observerLog";
import { IncomingMessage } from "../models/incomingMessage";

@Injectable({
    providedIn: 'root'
  })
export class TrackingService {
    private MessageSubject = new BehaviorSubject<TrackedMessage[]>([]);
    public Message$ = this.MessageSubject.asObservable();
    private socket : WebSocketSubject<ObserverLog[]> | null = null;

    constructor(private awssdk: AwsSdkService, private queueService : QueueService) {
        queueService.selectedQueue$.subscribe(x => {
            if (x === null)
                this.disconnect()
            else
                (this.connect(x))
        })
    }

    private connect(queue : Queue){
        console.log("Connecting...")
        this.disconnect();
        this.socket = webSocket(queue.url.replace("http", "ws"));
        this.socket.subscribe(x => x.forEach(y => this.onNewMessage(y)))
    }

    private disconnect(){
        this.MessageSubject.next([])
        if (this.socket !== null){
            this.socket.complete()
            this.socket = null;
        }
    }

    private onNewMessage(message : ObserverLog){
        console.log(message);
        const current = this.MessageSubject.getValue();
        const existingMessageIndex = current.findIndex(x => x.id === message.MessageId);
        let existingMessage = (existingMessageIndex >= 0) ? current[existingMessageIndex] : null;
        let incomingMessage : IncomingMessage = JSON.parse(message.MessageJson);

        if (existingMessage !== null){
            current.splice(existingMessageIndex, 1);
        }
        else {
            existingMessage = newTrackedMessage(incomingMessage)
        }

        if (!(message.Event === "OnMessageDeleted" || message.Event === "OnMessageReady" || message.Event == "OnMessageInvisible"))
            throw new Error("Invalid event type")

        updateTrackedMessage(existingMessage, incomingMessage)
        if (existingMessage.history.length <= 0 || existingMessage.history[existingMessage.history.length - 1].event != message.Event) {
            existingMessage.history.push({
                at: new Date(message.At),
                event: message.Event
            })
        }

        current.push(existingMessage)
        this.MessageSubject.next(current)
    }
}
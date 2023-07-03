import { Component } from '@angular/core';
import { QueueService } from '../services/queue.service';
import { AwsSdkService } from '../services/awssdk.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'loco-message-sender',
  templateUrl: './message-sender.component.html',
  styleUrls: ['./message-sender.component.scss']
})
export class MessageSenderComponent {
    ready : boolean = false;
    text : string = "";
    queue : string | undefined;

    constructor(private queueService : QueueService, private awssdk : AwsSdkService, private snackBar : MatSnackBar){
      queueService.selectedQueue$.subscribe(x => {
        this.ready = x !== null
        this.queue = x?.name;
      });
    }

    send(){
      this.awssdk.sendMessage(this.queueService.getSelectedQueue()!, this.text).then(x => {
        this.snackBar.open("Message sent", undefined, {
          duration: 5000
        });
      }).catch(x => {
        this.snackBar.open(`Message sending unsuccessful! ${x}`, undefined, {
          duration: 5000
        });
      })
    }

    updateValue(event : any){
      this.text = event.target.value;

      if (event.key === "Enter")
        this.send();
    }
}

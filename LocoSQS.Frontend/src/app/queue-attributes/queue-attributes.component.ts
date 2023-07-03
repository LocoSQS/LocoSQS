import { Component } from '@angular/core';
import { QueueService } from '../services/queue.service';
import { QueueAttributes } from '../models/queueAttributes';
import { Queue } from '../models/queue';
import { AwsSdkService } from '../services/awssdk.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'loco-queue-attributes',
  templateUrl: './queue-attributes.component.html',
  styleUrls: ['./queue-attributes.component.scss']
})
export class QueueAttributesComponent {
  attributes : QueueAttributes | null = null;
  availableQueues: Queue[] = []

  constructor(private queueService : QueueService, private awssdk : AwsSdkService, private snackBar : MatSnackBar){}

  async ngOnInit(){
    this.queueService.selectedQueueAttribute$.subscribe(x => {
      this.attributes = Object.assign({}, x)
      this.availableQueues = this.queueService.getCachedQueues().filter(y => y.arn !== x?.queueArn)
    })
  }

  save(){
    this.awssdk.SetQueueAttributes(this.queueService.getSelectedQueue()!, this.attributes!).then(x => {
      this.snackBar.open("Queue edited", undefined, {
        duration: 5000
      });
    }).catch(x => {
      this.snackBar.open(`Queue edit unsuccessful! ${x}`, undefined, {
        duration: 5000
      });
    })
  }
}

import { Component } from '@angular/core';
import { QueueService } from '../services/queue.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Queue } from '../models/queue';

@Component({
  selector: 'loco-purge-queue-modal',
  templateUrl: './purge-queue-modal.component.html',
  styleUrls: ['./purge-queue-modal.component.scss']
})
export class PurgeQueueModalComponent {
  public selectedQueue;
  private selectedQueueObject : Queue | null = null;

  constructor(private queueService : QueueService, private snackBar : MatSnackBar){
    this.selectedQueue = queueService.selectedQueue$;
    this.selectedQueue.subscribe(x => this.selectedQueueObject = x);
  }

  purge(){
    if (this.selectedQueueObject != null){
      this.queueService.purgeQueue(this.selectedQueueObject).then(x => {
        this.snackBar.open("Purged queue successfully", undefined, {
          duration: 5000
        });
      }).catch(x => {
        this.snackBar.open(`Purged queue unsuccessful! ${x}`, undefined, {
          duration: 5000
        });
      })
    }
  }
}

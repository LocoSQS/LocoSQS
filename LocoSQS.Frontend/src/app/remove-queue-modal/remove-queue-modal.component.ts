import { Component } from '@angular/core';
import { Queue } from '../models/queue';
import { QueueService } from '../services/queue.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'loco-remove-queue-modal',
  templateUrl: './remove-queue-modal.component.html',
  styleUrls: ['./remove-queue-modal.component.scss']
})
export class RemoveQueueModalComponent {
  public selectedQueue;
  private selectedQueueObject : Queue | null = null;

  constructor(private queueService : QueueService, private snackBar : MatSnackBar){
    this.selectedQueue = queueService.selectedQueue$;
    this.selectedQueue.subscribe(x => this.selectedQueueObject = x);
  }

  delete(){
    if (this.selectedQueueObject != null){
      this.queueService.deleteQueue(this.selectedQueueObject).then(x => {
        this.snackBar.open("Deleted queue successfully", undefined, {
          duration: 5000
        });
      }).catch(x => {
        this.snackBar.open(`Delete queue unsuccessful! ${x}`, undefined, {
          duration: 5000
        });
      })
    }
  }
}

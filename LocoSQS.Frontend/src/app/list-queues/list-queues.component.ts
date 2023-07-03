import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { Queue } from '../models/queue';
import { QueueService } from '../services/queue.service';
import { MatDialog } from '@angular/material/dialog';
import { AddQueueModalComponent } from '../add-queue-modal/add-queue-modal.component';
import { RemoveQueueModalComponent } from '../remove-queue-modal/remove-queue-modal.component';
import { PurgeQueueModalComponent } from '../purge-queue-modal/purge-queue-modal.component';
import { QueueAttributes } from '../models/queueAttributes';

@Component({
  selector: 'loco-list-queues',
  templateUrl: './list-queues.component.html',
  styleUrls: ['./list-queues.component.scss']
})
export class ListQueuesComponent {
  data? : Observable<Queue[]>
  current? : Observable<Queue|null>
  attributes : QueueAttributes | null = null;

  constructor(private queueService : QueueService, public dialog: MatDialog){}

  async ngOnInit(){
    this.data = this.queueService.queue$;
    this.current = this.queueService.selectedQueue$;
  }

  openQueueModal(){
    this.dialog.open(AddQueueModalComponent, {})
  }

  openDeleteModal(){
    this.dialog.open(RemoveQueueModalComponent, {})
  }

  openPurgeModal(){
    this.dialog.open(PurgeQueueModalComponent, {})
  }

  selected(queue : Queue){
    this.queueService.setSelectedQueue(queue);
  }

  reloadQueues(){
    this.queueService.reloadQueues();
  }
}

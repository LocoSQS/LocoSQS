import { Component } from '@angular/core';
import { TrackingService } from '../services/tracking.service';
import { TrackedMessage, TrackedMessageHistoryEntry } from '../models/trackedMessage';
import { Observable } from 'rxjs';
import { AwsSdkService } from '../services/awssdk.service';
import { QueueService } from '../services/queue.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
	selector: 'loco-queue-tracker',
	templateUrl: './queue-tracker.component.html',
	styleUrls: ['./queue-tracker.component.scss']
})
export class QueueTrackerComponent {
	data: Observable<TrackedMessage[]>;
	onQueueVisible: boolean = true;
	inFlightVisible: boolean = true;
	deletedVisible: boolean = true;

	constructor(private trackerService: TrackingService, private awssdk: AwsSdkService, private queueService : QueueService, private snackBar : MatSnackBar) {
		this.data = trackerService.Message$;
	}

	filterMessages(messages : TrackedMessage[]){
		return messages.filter(x => {
			let lastHistory = x.history[x.history.length - 1]
			if (lastHistory.event === "OnMessageReady")
				return this.onQueueVisible;
			
			if (lastHistory.event === "OnMessageInvisible")
				return this.inFlightVisible;

			if (lastHistory.event === "OnMessageDeleted")
				return this.deletedVisible;

			return false;
		})
	}
	
	deleteMessage(message : TrackedMessage){
		this.awssdk.deleteMessage(this.queueService.getSelectedQueue()!, message.receiptHandle).then(x => {
			this.snackBar.open("Message Deleted", undefined, {
			  duration: 5000
			});
		  }).catch(x => {
			this.snackBar.open(`Message deletion unsuccessful! ${x}`, undefined, {
			  duration: 5000
			});
		  })

	}
}

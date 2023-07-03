import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, defer, from } from 'rxjs';
import { ListQueuesResult } from '@aws-sdk/client-sqs';
import { Queue } from '../models/queue';
import { AwsSdkService } from './awssdk.service';
import { QueueAttributes } from '../models/queueAttributes';

@Injectable({
	providedIn: 'root'
})
export class QueueService {
	private queueSubject = new BehaviorSubject<Queue[]>([]);
	public queue$ = this.queueSubject.asObservable();

	private selectedQueueSubject = new BehaviorSubject<Queue | null>(null);
	public selectedQueue$ = this.selectedQueueSubject.asObservable();

	private selectedQueueAttributesSubject = new BehaviorSubject<QueueAttributes | null>(null);
	public selectedQueueAttribute$ = this.selectedQueueAttributesSubject.asObservable();

	constructor(private awssdk: AwsSdkService) {
		this.reloadQueues();

		this.selectedQueue$.subscribe(x => {
			if (x !== null){
				this.selectedQueueAttributesSubject.next(null);
				awssdk.GetQueueAttribute$(x).subscribe(y => this.selectedQueueAttributesSubject.next(y));
			}
		})
	}

	public getCachedQueues(): Queue[] {
		return this.queueSubject.getValue();
	}

	public async createQueue(name: string) {
		await this.awssdk.createQueue(name);
		this.reloadQueues();
	}

	public setSelectedQueue(queue: Queue) {
		this.selectedQueueSubject.next(queue);
	}

	public getSelectedQueue() {
		return this.selectedQueueSubject.getValue();
	}

	public async purgeQueue(queue: Queue) {
		await this.awssdk.purgeQueue(queue, true);
	}

	public async deleteQueue(queue: Queue) {
		await this.awssdk.deleteQueue(queue);
		this.reloadQueues();
	}

	public reloadQueues() {
		this.awssdk.GetQueue$().subscribe((result) => {
			this.queueSubject.next(result)
			if (this.getCachedQueues().find(x => x == this.selectedQueueSubject.getValue()) === undefined)
				this.selectedQueueSubject.next(null);
		})
	}
}

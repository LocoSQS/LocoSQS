import { Pipe, PipeTransform } from '@angular/core';
import { TrackedMessageHistoryEntry } from '../models/trackedMessage';

@Pipe({
	name: 'iconHistory'
})
export class IconHistoryPipe implements PipeTransform {

	transform(history: TrackedMessageHistoryEntry): string {
		if (history.event === "OnMessageReady")
			return "mail"

		if (history.event === "OnMessageInvisible")
			return "drafts"

		return "delete_forever"
	}

}

import { Pipe, PipeTransform } from '@angular/core';
import { TrackedMessage, TrackedMessageHistoryEntry } from '../models/trackedMessage';

@Pipe({
    name: 'readableHistory'
})
export class ReadableHistoryPipe implements PipeTransform {

    transform(history: TrackedMessageHistoryEntry): string {
        if (history.event === "OnMessageReady")
            return "On Queue"

        if (history.event === "OnMessageInvisible")
            return "In Flight"

        return "Deleted"
    }

}

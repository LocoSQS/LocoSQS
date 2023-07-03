import { Component } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { QueueService } from '../services/queue.service';
import { AsyncValidatorFn } from '../models/asyncValidatorFn';
import { map } from 'rxjs';
import { ValidatorFn } from '../models/validatorFn';
import { ShowOnDirtyErrorStateMatcher } from '@angular/material/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'loco-add-queue-modal',
  templateUrl: './add-queue-modal.component.html',
  styleUrls: ['./add-queue-modal.component.scss']
})
export class AddQueueModalComponent {
  public matcher = new ShowOnDirtyErrorStateMatcher();

  public addQueueForm = new FormGroup({
    queueName: new FormControl('', {
      validators: [Validators.required, uniqueQueueNameValidator(this.queueService)],
      nonNullable: true,
    })
  })

  constructor(private queueService : QueueService, private snackBar : MatSnackBar){}

  public add(){
      this.queueService.createQueue(this.addQueueForm.controls.queueName.value).then(x => {
        this.snackBar.open("Queue created", undefined, {
          duration: 5000
        });
      }).catch(x => {
        this.snackBar.open(`Queue creation unsuccessful! ${x}`, undefined, {
          duration: 5000
        });
      })
  }
}

export function uniqueQueueNameValidator(queueService : QueueService) : ValidatorFn {
  return (control : AbstractControl) => {
    const value: string = control.value;
    const find = queueService.getCachedQueues().find(y => y.name == value);
    return find !== undefined
    ? { uniqueQueueNameValidator: { value }}
    : null;
  }
}
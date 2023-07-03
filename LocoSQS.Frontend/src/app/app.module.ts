import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatIconModule, MatIconRegistry } from '@angular/material/icon'
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import {MatButtonModule} from '@angular/material/button';
import {MatDividerModule} from '@angular/material/divider';
import {MatListModule} from '@angular/material/list'
import {MatCardModule} from '@angular/material/card';
import {MatDialogModule} from '@angular/material/dialog';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatSnackBarModule} from '@angular/material/snack-bar';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ListQueuesComponent } from './list-queues/list-queues.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AddQueueModalComponent } from './add-queue-modal/add-queue-modal.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ErrorStateMatcher, ShowOnDirtyErrorStateMatcher } from '@angular/material/core';
import { RemoveQueueModalComponent } from './remove-queue-modal/remove-queue-modal.component';
import { PurgeQueueModalComponent } from './purge-queue-modal/purge-queue-modal.component';
import { QueueTrackerComponent } from './queue-tracker/queue-tracker.component';
import { ReadableHistoryPipe } from './pipes/readable-history.pipe';
import { IconHistoryPipe } from './pipes/icon-history.pipe';
import { TruncatePipe } from './pipes/truncate.pipe';
import {MatChipsModule} from '@angular/material/chips';
import { MessageSenderComponent } from './message-sender/message-sender.component';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import { FormsModule } from '@angular/forms';
import { QueueAttributesComponent } from './queue-attributes/queue-attributes.component';
import {MatToolbarModule} from '@angular/material/toolbar';

@NgModule({
  declarations: [
    AppComponent,
    ListQueuesComponent,
    AddQueueModalComponent,
    RemoveQueueModalComponent,
    PurgeQueueModalComponent,
    QueueTrackerComponent,
    ReadableHistoryPipe,
    IconHistoryPipe,
    TruncatePipe,
    MessageSenderComponent,
    QueueAttributesComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatIconModule,
    BrowserAnimationsModule,
    MatSlideToggleModule,
    MatButtonModule,
    MatDividerModule,
    MatListModule,
    MatCardModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatExpansionModule,
    MatSnackBarModule,
    MatChipsModule,
    MatAutocompleteModule,
    FormsModule,
    MatToolbarModule
  ],
  providers: [
    {provide: ErrorStateMatcher, useClass: ShowOnDirtyErrorStateMatcher}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

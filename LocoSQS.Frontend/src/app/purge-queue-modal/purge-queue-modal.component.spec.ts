import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurgeQueueModalComponent } from './purge-queue-modal.component';

describe('PurgeQueueModalComponent', () => {
  let component: PurgeQueueModalComponent;
  let fixture: ComponentFixture<PurgeQueueModalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PurgeQueueModalComponent]
    });
    fixture = TestBed.createComponent(PurgeQueueModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
